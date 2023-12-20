using AutoMapper;
using OfferPrice.Auction.Api.Models.Comment;
using OfferPrice.Auction.Domain.Models;

namespace OfferPrice.Auction.Api.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<Lot, Models.Lot>();

            CreateMap<Bet, Models.Bet>();

            CreateMap<CommentRequest, Domain.Models.Comment>();

            CreateMap<CommentRequest, Domain.Models.SubComment>();
        }
    }
}
