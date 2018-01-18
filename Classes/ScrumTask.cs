using System;
using System.Data.Linq.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumDashboard.Classes
{
    [Table(Name = "ScrumTask")]
    public class ScrumTask
    {
        public ScrumTask() { }

        public ScrumTask(int ID, int ExternalID, string Title, string Description, string Category = "") {
            this.ID = ID;
            this.ExternalID = ExternalID;
            this.Title = Title;
            this.Description = Description;
            this.Category = Category;
        }

        [Column(IsPrimaryKey = true)]
        public int ID { get; set; }
        [Column]
        public int ExternalID { get; set; }
        [Column]
        public string Title { get; set; }
        [Column]
        public string Description { get; set; }
        // public string[] Tags { get; set; }
        [Column]
        public object Category { get; set; }
        /*[Column]
        public Uri ImageURL { get; set; }
        [Column]
        public object ColorKey { get; set; }*/
    }
}