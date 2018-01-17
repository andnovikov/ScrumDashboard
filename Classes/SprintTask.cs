using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumDashboard.Classes
{
    public class SprintTask
    {
        public SprintTask() { }

        public SprintTask(int ID, int SprintID, ScrumTask Task) {
            this.ID = ID;
            this.SprintID = SprintID;
            this.Task = Task;
        }

        public int ID { get; set; }
        public int SprintID { get; set; }
        public ScrumTask Task { get; set; }
    }
}
