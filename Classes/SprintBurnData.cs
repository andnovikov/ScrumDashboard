using System;
using System.Data.Linq.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumDashboard.Classes
{
    [Table(Name = "SprintBurnData")]
    public class SprintBurnData
    {
        public SprintBurnData() { }

        public SprintBurnData(int SprintID, DateTime BurnDate, int BurnPoint, int SprintTaskID) {
            this.SprintID = SprintID;
            this.BurnDate = BurnDate;
            this.BurnPoint = BurnPoint;
            this.SprintTaskID = SprintTaskID;
        }

        [Column(IsPrimaryKey = true)]
        public int SprintID { get; set; }
        [Column(IsPrimaryKey = true)]
        public DateTime BurnDate { get; set; }
        [Column]
        public int BurnPoint { get; set; }
        [Column(IsPrimaryKey = true)]
        public int SprintTaskID { get; set; }
    }
}