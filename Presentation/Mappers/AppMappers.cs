using Application.Contracts;
using Domain.Entities;
using System.Linq;
namespace Presentation.Mappers;

public class AppMappers : AutoMapper.Profile
{
    public AppMappers()
    {
        CreateMap<User, UserResponse>();

        CreateMap<Book, BookResponse>()
            .AfterMap((src, dest) =>
            {
                dest.Catalogues = src.BookCatalogues.Select(bookCatalogue => bookCatalogue.Catalogue.Name).ToArray();
            });

        CreateMap<Book, PreviewBookResponse>();

        CreateMap<CartDetail, CartItemResponse>();

        CreateMap<Cart, CartResponse>()
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.User = src.User.Email;
            });

        // DTO -> Entity
        CreateMap<CartItemRequest, CartDetail>();
        CreateMap<CreateCartRequest, Cart>();

    }
}
