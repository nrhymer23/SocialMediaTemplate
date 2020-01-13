using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KSUCorner.Models
{
    [MetadataType(typeof(ForumMetadata))]
    public partial class Forum
    {
    }

    public class ForumMetadata
    {
        public int ForumID { get; set; }

        [Required(ErrorMessage = "The Title Field Is Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "The Title must be between 1 and 50 characters")]
        public string Title { get; set; }

        public int AccountID { get; set; }

        public int CategoryID { get; set; }

        public int GroupID { get; set; }

        [StringLength(100, ErrorMessage = "The Description cannot be more than 100 characters")]
        public string Description { get; set; }

        [StringLength(1000, ErrorMessage = "The Body of your Forum cannot be more than 1000 characters")]
        public string Body { get; set; }

        public int ViewCount { get; set; }

        public bool PublicBlog { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdateDate { get; set; }
    }
}