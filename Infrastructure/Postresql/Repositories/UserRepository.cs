using Domain.Entities;
using Domain.IRepositories;
using Domain.Specification;
using Infrastructure.Postresql.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Postresql.Repositories
{
    public class UserRepository(PostreSqlDbContext context) : IUserRepository
    {

        protected readonly DbSet<User> DbSet = context.Set<User>();

        public async Task<ICollection<User>> GetAllAsync() => await DbSet.ToListAsync();

        public async Task<User> GetByIdAsync(Guid id) => await DbSet.Where(e => e.Id.Equals(id)).FirstOrDefaultAsync();

        public async Task CommitChangesAsync() => await context.SaveChangesAsync();

        public virtual async Task CreateAsync(IEnumerable<User> items)
        {
            ArgumentNullException.ThrowIfNull(items);
            await DbSet.AddRangeAsync(items);
        }

        public async Task<User> CreateAsync(User item)
        {
            ArgumentNullException.ThrowIfNull(item);
            await DbSet.AddAsync(item);
            return item;
        }

        public User UpdateAsync(User item)
        {
            ArgumentNullException.ThrowIfNull(item);
            DbSet.Attach(item);
            context.Entry(item).State = EntityState.Modified;
            return item;
        }

        public async Task<User> DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException($"Entity with id {id} not found.");
            DbSet.Remove(entity);
            return entity;
        }

        public async Task<ICollection<User>> FindAsync(Func<User, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);
            return await Task.FromResult(DbSet.AsEnumerable().Where(predicate).ToList());
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            return entity != null;
        }

        public async Task<int> CountAsync() => await DbSet.CountAsync();

        public async Task<PagedResult<User>> GetPagedAsync(SqlSpecification<User> spec)
        {
            IQueryable<User> query = context.Set<User>();

            // Apply criteria
            if (spec.Criteria != null) query = query.Where(spec.Criteria);

            // Apply search fields
            if (spec.SearchFields != null && spec.SearchFields.Any())
            {
                foreach (var searchField in spec.SearchFields)
                {
                    if (!string.IsNullOrWhiteSpace(searchField.SearchTerm))
                    {
                        var searchTerm = searchField.SearchTerm.ToLower();
                        query = query.Where(entity =>
                            EF.Functions.Like(EF.Property<string>(entity, GetPropertyName(searchField.Field)), $"%{searchTerm}%"));
                    }
                }
            }

            // Apply ordering
            if (spec.OrderFields != null && spec.OrderFields.Any())
            {
                var isFirstOrder = true;
                foreach (var orderField in spec.OrderFields)
                {
                    if (isFirstOrder)
                    {
                        query = orderField.Direction == SortDirection.Ascending
                            ? query.OrderBy(orderField.Field)
                            : query.OrderByDescending(orderField.Field);

                        isFirstOrder = false;
                    }
                    else
                        query = orderField.Direction == SortDirection.Ascending
                            ? ((IOrderedQueryable<User>)query).ThenBy(orderField.Field)
                            : ((IOrderedQueryable<User>)query).ThenByDescending(orderField.Field);
                }
            }

            // Apply pagination
            var totalItems = await query.CountAsync();

            query = query.Skip(spec.Skip).Take(spec.Take);

            var items = await query.ToListAsync();

            return new PagedResult<User>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = (spec.Skip / spec.Take) + 1,
                PageSize = spec.Take
            };
        }

        private string GetPropertyName(Expression<Func<User, object>> expression)
        {
            if (expression.Body is MemberExpression memberExpression) return memberExpression.Member.Name;

            else if (expression.Body is UnaryExpression unaryExpression
                && unaryExpression.Operand is MemberExpression innerMemberExpression)
                return innerMemberExpression.Member.Name;

            throw new ArgumentException("Invalid expression");
        }

    }
}
