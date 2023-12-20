using System.Collections.Generic;
using System;
using System.Linq;

namespace OfferPrice.Auction.Api.Models.Comment
{
    public class Comment
    {
        public Comment(Domain.Models.Comment comment)
        {
            Id = comment.Id;
            UserId = comment.UserId;
            LotId = comment.LotId;
            Text = comment.Text;
            CreationDate = comment.CreationDate;
            SubComments = comment.SubComments is null ? 
                new List<SubComment>() :
                comment.SubComments.Select(x => new SubComment(x)).ToList() ;
        }

        public string Id { get; set; }

        public string UserId { get; set; }

        public string LotId { get; set; }

        public string Text { get; set; }

        public DateTime CreationDate { get; set; }

        public List<SubComment> SubComments { get; set; }
    }
}
