using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KSUCorner.Models
{
    [MetadataType(typeof(GroupMetadata))]
    public partial class Group
    {
    }

    public class GroupMetadata
    {
        public int GroupID { get; set; }

        [Required(ErrorMessage = "The Name Field Is Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "The Name must be between 1 and 50 characters")]
        public string Name { get; set; }

        public int AccountID { get; set; }

        public string ImagePath { get; set; }

        public string ImageLinkType { get; set; }

        public bool IsPublic { get; set; }

        [StringLength(100, ErrorMessage = "The Description cannot be more than 100 characters")]
        public string Description { get; set; }

        [StringLength(1000, ErrorMessage = "The Mission Statement cannot be more than 1000 characters")]
        public string Mission { get; set; }

        public long Size { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdateDate { get; set; }
    }
}