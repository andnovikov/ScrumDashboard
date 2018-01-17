using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumDashboard.Classes
{
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

        public int ID { get; set; }
        public int ExternalID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        // public string[] Tags { get; set; }
        public object Category { get; set; }
        public Uri ImageURL { get; set; }
        public object ColorKey { get; set; }
    }
}