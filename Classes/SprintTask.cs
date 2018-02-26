using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Text;
using System.Threading.Tasks;

namespace ScrumDashboard.Classes
{
    [Table(Name = "SprintTask")]
    public class SprintTask
    {
        public SprintTask() { }

        public SprintTask(int ID, int SprintID, int ScrumTaskID, string State, int TeamMemberID) {
            this.ID = ID;
            this.SprintID = SprintID;
            this.ScrumTaskID = ScrumTaskID;
            this.State = State;
            this.TeamMemberID = TeamMemberID;
        }

        [Column(IsPrimaryKey = true)]
        public int ID { get; set; }
        [Column]
        public int SprintID { get; set; }
        [Column]
        public int ScrumTaskID { get; set; }
        [Column]
        public string State { get; set; }
        [Column]
        public int TeamMemberID { get; set; }
        [Column]
        public int Cost { get; set; }
    }
}
