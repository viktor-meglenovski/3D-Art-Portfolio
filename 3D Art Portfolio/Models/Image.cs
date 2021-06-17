using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3D_Art_Portfolio.Models
{
    public class Image
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Url { get; set; }
        public Image() { }
        public Image(int ProjectId, string Url)
        {
            this.ProjectId = ProjectId;
            this.Url = Url;
        }
    }
}