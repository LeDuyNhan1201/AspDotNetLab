using Application.Contracts;
using Domain.Entities;

namespace Presentation.Mappers;

public class AppMappers : AutoMapper.Profile
{
    public AppMappers()
    {
        CreateMap<CartItemRequest, CartDetail>();
        CreateMap<CreateCartRequest, Cart>();
    }
}
