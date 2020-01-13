using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KSUCorner.Models
{
    [MetadataType(typeof(MediaFileMetadata))]
    public partial class MediaFile
    {
    }

    public class MediaFileMetadata
    {
        public int FileID { get; set; }

        [Required(ErrorMessage = "The Name Field Is Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "The Name must be between 1 and 50 characters")]
        public string Name { get; set; }

        public int AccountID { get; set; }

        public string FilePath { get; set; }

        public string FileType { get; set; }

        public int FileFolderID { get; set; }

        public int GroupID { get; set; }

        [StringLength(100, ErrorMessage = "The Description cannot be more than 100 characters")]
        public string Description { get; set; }

        [StringLength(300, ErrorMessage = "The Detailed Description cannot be more than 300 characters")]
        public string MoreInfo { get; set; }

        public long Size { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdateDate { get; set; }
    }
}