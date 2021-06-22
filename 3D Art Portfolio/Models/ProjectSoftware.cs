using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3D_Art_Portfolio.Models
{
    public class ProjectSoftware
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int SoftwareId { get; set; }
        public ProjectSoftware() { }
        public ProjectSoftware(int ProjectId, int SoftwareId)
        {
            this.ProjectId = ProjectId;
            this.SoftwareId = SoftwareId;
        }
    }
}