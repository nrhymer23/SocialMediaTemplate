using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSUCorner.Models
{
    public class GalleryListItem
    {
        public int id { get; set; }

        public bool isGallery { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public string type { get; set; }

        public string path { get; set; }

        public int width { get; set; }

        public int height { get; set; }

        public string dateString { get; set; }

        public int count { get; set; }
    }
}