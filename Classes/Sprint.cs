using System;
using System.Data.Linq.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumDashboard.Classes
{
    [Table(Name = "Sprint")]
    public class Sprint
    {
        public Sprint() { }

        public Sprint(int ID, int TeamID, DateTime DateStart, DateTime DateEnd) {
            this.ID = ID;
            this.TeamID = TeamID;
            this.DateStart = DateStart;
            this.DateEnd = DateEnd;
        }

        [Column(IsPrimaryKey = true)]
        public int ID { get; set; }
        [Column]
        public int TeamID { get; set; }
        [Column]
        public DateTime DateStart { get; set; }
        [Column]
        public DateTime DateEnd { get; set; }

    }
}
