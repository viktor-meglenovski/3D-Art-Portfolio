using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3D_Art_Portfolio.Models
{
    public class Like
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProjectId { get; set; }
        public Like() { }
        public Like(string UserId, int ProjectId)
        {
            this.UserId = UserId;
            this.ProjectId = ProjectId;
        }
    }
}