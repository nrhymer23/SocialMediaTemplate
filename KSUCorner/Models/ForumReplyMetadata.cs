using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KSUCorner.Models
{
    [MetadataType(typeof(ForumReplyMetadata))]
    public partial class ForumReply
    {
    }

    public class ForumReplyMetadata
    {
        public int ReplyID { get; set; }

        [Required(ErrorMessage = "The Subject Field Is Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "The Subject must be between 1 and 50 characters")]
        public string Subject { get; set; }

        public int AccountID { get; set; }

        public int ForumID { get; set; }

        [StringLength(1000, ErrorMessage = "The Body of your Reply cannot be more than 1000 characters")]
        public string Body { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdateDate { get; set; }
    }
}