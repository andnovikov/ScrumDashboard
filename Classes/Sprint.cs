using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumDashboard.Classes
{
    public class Sprint
    {
        public Sprint() { }

        public Sprint(int ID, int TeamID, DateTime DateStart, DateTime DateEnd) {
            this.ID = ID;
            this.TeamID = TeamID;
            this.DateStart = DateStart;
            this.DateEnd = DateEnd;
        }

        public int ID { get; set; }
        public int TeamID { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

    }
}
