using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KSUCorner.Models
{
    [MetadataType(typeof(FileFolderMetadata))]
    public partial class FileFolder
    {
    }

    public class FileFolderMetadata
    {
        public int FileFolderID { get; set; }

        [Required(ErrorMessage = "The Name Field Is Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "The Name must be between 1 and 50 characters")]
        public string Name { get; set; }

        public int AccountID { get; set; }

        public string FolderType { get; set; }

        [StringLength(100, ErrorMessage = "The Description cannot be more than 100 characters")]
        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdateDate { get; set; }
    }
}