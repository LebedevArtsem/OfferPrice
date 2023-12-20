using AutoMapper;
using OfferPrice.Catalog.Api.Models;
using OfferPrice.Catalog.Domain;

namespace OfferPrice.Catalog.Api.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<InsertProductRequest, Domain.Product>();
        CreateMap<UpdateProductRequest, Domain.Product>();
        CreateMap<Domain.Product, Models.Product>();

        CreateMap<LikeRequest, Domain.Like>();
        CreateMap<Domain.Like, Models.Like>();
    }
}

