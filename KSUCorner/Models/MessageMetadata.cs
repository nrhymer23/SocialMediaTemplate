using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KSUCorner.Models
{
    [MetadataType(typeof(MessageMetadata))]
    public partial class Message
    {
    }

    public class MessageMetadata
    {
        public int MessageID { get; set; }

        public string SentBy { get; set; }

        public string SentTo { get; set; }

        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(80, MinimumLength = 1,
                      ErrorMessage = "Must be between 1 and 80 characters")]
        public string Subject { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(1000, ErrorMessage = "Maximum length of Message is 1000 characters")]
        public string Body { get; set; }

        public string MessageType { get; set; }

        public string MessageStatus { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime OpenedDate { get; set; }
    }
}