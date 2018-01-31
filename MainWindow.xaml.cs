using Syncfusion.UI.Xaml.Kanban;
using Syncfusion.Windows.SampleLayout;
using ScrumDashboard.Classes;
using Syncfusion.Windows.Shared;
using System;
using System.Data.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Devart.Data.SQLite;

namespace ScrumDashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : SampleLayoutWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var workflow = new WorkflowCollection();
            workflow.Add(new KanbanWorkflow() { Category = "Open", AllowedTransitions = { "InProgress", "Closed", "Closed NoChanges", "Won't Fix" } });
            workflow.Add(new KanbanWorkflow() { Category = "Postponed", AllowedTransitions = { "Open", "InProgress", "Closed", "Closed NoChanges", "Won't Fix" } });
            workflow.Add(new KanbanWorkflow() { Category = "Review", AllowedTransitions = { "InProgress", "Closed", "Postponed" } });
            workflow.Add(new KanbanWorkflow() { Category = "InProgress", AllowedTransitions = { "Review", "Postponed" } });

            Kanban.Workflows = workflow;

            SQLiteConnection Connection = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + Properties.Settings.Default.DBPath);
            Connection.Open();
            DataContext db = new DataContext(Connection);
            Table<TeamMember> TeamMembers = db.GetTable<TeamMember>();

            IQueryable<TeamMember> tmmb = (from tm in TeamMembers
                                           where tm.TeamID == 1
                                           select tm);

            foreach (TeamMember tm in tmmb)
            {
                ComboBoxItem cbItm = new ComboBoxItem();
                cbItm.Tag = tm;
                cbItm.Content = tm.Name;
                TaskTeamMember.Items.Add(cbItm);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Load sprint tasks
        }

        private void Kanban_CardDragEnd(object sender, KanbanDragEndEventArgs e)
        {

            SQLiteConnection Connection = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + Properties.Settings.Default.DBPath);
            Connection.Open();
            DataContext db = new DataContext(Connection);

            // Get a typed table to run queries.
            Table<SprintTask> SprintTsks = db.GetTable<SprintTask>();

            int SprintTaskID;
            Int32.TryParse((e.SelectedCard.Content as KanbanModel).ID, out SprintTaskID);
            SprintTask tsk = (from sp in SprintTsks
                              where sp.ID == SprintTaskID
                              select sp).SingleOrDefault();
            tsk.State = e.TargetKey.ToString();

            db.SubmitChanges();
        }

        private void Kanban_CardTapped(object sender, KanbanTappedEventArgs e)
        {
            SQLiteConnection Connection = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + Properties.Settings.Default.DBPath);
            Connection.Open();
            DataContext db = new DataContext(Connection);

            // Get a typed table to run queries.
            Table<SprintTask> SprintTsks = db.GetTable<SprintTask>();
            Table<ScrumTask> ScrumTsks = db.GetTable<ScrumTask>();

            int SprintTaskID;
            Int32.TryParse((e.SelectedCard.Content as KanbanModel).ID, out SprintTaskID);
 
            ScrumTask tsk = (from sp in SprintTsks
                             join sc in ScrumTsks on sp.ScrumTaskID equals sc.ID
                             where sp.ID == SprintTaskID
                             select sc).SingleOrDefault();

            TaskExternalID.Text = tsk.ExternalID.ToString();
            TaskExternalID.Tag = e.SelectedCard;
            TaskTitle.Text = tsk.Title;
            TaskDescription.Text = tsk.Description;
            TaskTeamMember.SelectedIndex = -1;
            // e.SelectedCard.;
        }

        private void TaskTeamMember_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex != -1)
            {
                SQLiteConnection Connection = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + Properties.Settings.Default.DBPath);
                Connection.Open();
                DataContext db = new DataContext(Connection);

                // Get a typed table to run queries.
                Table<ScrumTask> ScrumTsks = db.GetTable<ScrumTask>();
                Table<SprintTask> SprintTsks = db.GetTable<SprintTask>();

                TeamMember tm = (((sender as ComboBox).SelectedItem as ComboBoxItem).Tag as TeamMember);
                int SprintTaskID;
                Int32.TryParse(((TaskExternalID.Tag as KanbanCardItem).Content as KanbanModel).ID, out SprintTaskID);

                SprintTask tsk = (from spt in SprintTsks
                                  where spt.ID == SprintTaskID
                                  select spt).SingleOrDefault();
                tsk.TeamMemberID = tm.ID;
                db.SubmitChanges();

                ((TaskExternalID.Tag as KanbanCardItem).Content as KanbanModel).ImageURL = new Uri(@"Images/" + tm.Nickname + ".png", UriKind.RelativeOrAbsolute);
                TaskDetails td = new TaskDetails();
            }
        }
    }

    public class TaskDetails
    {
        public TaskDetails()
        {
            // Get connection with connection string from Properties
            SQLiteConnection Connection = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + Properties.Settings.Default.DBPath);
            Connection.Open();

            DataContext db = new DataContext(Connection);

            // Get a typed table to run queries.
            Table<ScrumTask> ScrumTsks = db.GetTable<ScrumTask>();
            Table<SprintTask> SprintTsks = db.GetTable<SprintTask>();
            Table<TeamMember> TeamMembers = db.GetTable<TeamMember>();

            var query =
                from spt in SprintTsks
                join sct in ScrumTsks on spt.ScrumTaskID equals sct.ID
                join tmm in TeamMembers on spt.TeamMemberID equals tmm.ID
                where spt.SprintID == 2
                select new { ID = spt.ID, Title = sct.Title, Category = spt.State, Description = sct.Description, ExternalID = sct.ExternalID, Nickname = tmm.Nickname };
            /*
             * select spt.ID, sct.Title, spt.State
             *   from SprintTask spt
             *    inner join ScrumTask sct
             *            on sct.ID = spt.ScrumTaskID
             *  where spt.SprintID = 1
             */

            KanbanTasks = new ObservableCollection<KanbanModel>();
            foreach (var task in query) {
                KanbanModel kbTask = new KanbanModel();
                kbTask.ID = task.ID.ToString();
                kbTask.Title = task.Title;
                kbTask.Description = task.Description;
                kbTask.Category = task.Category;
                kbTask.ColorKey = "High";
                kbTask.Tags = new string[] { task.ExternalID.ToString() };
                if (!task.Nickname.Equals("")) {
                    kbTask.ImageURL = new Uri(@"Images/" + task.Nickname + ".png", UriKind.RelativeOrAbsolute);
                }
                KanbanTasks.Add(kbTask);
            }

        }

        public ObservableCollection<ScrumTask> ScrumTasks { get; set; }

        public ObservableCollection<KanbanModel> KanbanTasks { get; set; }
    }

    public class SprintDetails
    {
        public SprintDetails()
        {
            //берем из конфига строку подключения и подключаемся к БД
            SQLiteConnection Connection = new SQLiteConnection(Properties.Settings.Default.DBPath);
            Connection.Open();

            DataContext db = new DataContext(Connection);

            // Get a typed table to run queries.
            Table<Sprint> Sprints = db.GetTable<Sprint>();

            /*
             * select spt.ID, sct.Title, spt.State
             *   from SprintTask spt
             *    inner join ScrumTask sct
             *            on sct.ID = spt.ScrumTaskID
             *  where spt.SprintID = 1
             */

            var ComboSprint =
                from s in Sprints
                select new { ID = s.ID };
        }

        public ObservableCollection<Sprint> ComboSprint { get; set; }
        // Content
    }
}
