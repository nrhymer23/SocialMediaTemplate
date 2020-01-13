using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSUCorner.Models
{
    public class ForumListItem
    {
        public int id { get; set; }

        public bool isMain { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public string createdBy { get; set; }

        public DateTime createdOn { get; set; }

        public DateTime lastPost { get; set; }

        public int count { get; set; }

        public int count2 { get; set; }
    }
}