using System;
using System.Data.Linq.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumDashboard.Classes
{
    [Table(Name = "TeamMember")]
    public class TeamMember
    {
        public TeamMember() { }

        public TeamMember(int ID, int TeamID, string Nickname, string Name, string Role = "")
        {
            this.ID = ID;
            this.TeamID = TeamID;
            this.Nickname = Nickname;
            this.Name = Name;
            this.Role = Role;
        }

        [Column(IsPrimaryKey = true)]
        public int ID { get; set; }
        [Column]
        public int TeamID { get; set; }
        [Column]
        public string Nickname { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public string Role { get; set; }
    }
}
