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
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Load sprint tasks
        }

        private void Kanban_CardDragEnd(object sender, KanbanDragEndEventArgs e)
        {

            SQLiteConnection Connection = new SQLiteConnection(Properties.Settings.Default.connectionString);
            Connection.Open();
            DataContext db = new DataContext(Connection);

            // Get a typed table to run queries.
            Table<SprintTask> SprintTsks = db.GetTable<SprintTask>();

            Int32.TryParse((e.SelectedCard.Content as KanbanModel).ID, out int SprintTaskID);
            SprintTask tsk = (from sp in SprintTsks
                              where sp.ID == SprintTaskID
                              select sp).SingleOrDefault();
            tsk.State = e.TargetKey.ToString();

            db.SubmitChanges();
        }
    }

    public class TaskDetails
    {
        public TaskDetails()
        {
            // Get connection with connection string from Properties
            SQLiteConnection Connection = new SQLiteConnection(Properties.Settings.Default.connectionString);
            Connection.Open();

            DataContext db = new DataContext(Connection);

            // Get a typed table to run queries.
            Table<ScrumTask> ScrumTsks = db.GetTable<ScrumTask>();
            Table<SprintTask> SprintTsks = db.GetTable<SprintTask>();
            
            var query =
                from spt in SprintTsks
                join sct in ScrumTsks on spt.ScrumTaskID equals sct.ID
                where spt.SprintID == 1
                select new { ID = spt.ID, Title = sct.Title, Category = spt.State, Description = sct.Description };
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
                kbTask.Tags = new string[] { "NewFeature" };
                kbTask.ImageURL = new Uri(@"Images/Image10.png", UriKind.RelativeOrAbsolute);
                KanbanTasks.Add(kbTask);
            }

            /*

            KanbanModel task = new KanbanModel();
            task.ColorKey = "High";
            task.Tags = new string[] { "Bug Fixing" };
            task.ImageURL = new Uri(@"Images/Image10.png", UriKind.RelativeOrAbsolute);
            Tasks.Add(task);

            task = new KanbanModel();
            task.ColorKey = "Low";
            task.Tags = new string[] { "GanttControl UWP" };
            task.ImageURL = new Uri(@"Images/Image10.png", UriKind.RelativeOrAbsolute);
            Tasks.Add(task);

            task = new KanbanModel();
            task.ColorKey = "Normal";
            task.Tags = new string[] { "Post processing" };
            task.ImageURL = new Uri(@"Images/Image10.png", UriKind.RelativeOrAbsolute);
            Tasks.Add(task);

            task = new KanbanModel();
            task.ColorKey = "High";
            task.Tags = new string[] { "Bug Fixing" };
            task.ImageURL = new Uri(@"Images/Image10.png", UriKind.RelativeOrAbsolute);
            Tasks.Add(task);

            task = new KanbanModel();
            task.ColorKey = "Low";
            task.Tags = new string[] { "Bug Fixing" };
            task.ImageURL = new Uri(@"Images/Image10.png", UriKind.RelativeOrAbsolute);
            Tasks.Add(task);

            task = new KanbanModel();
            task.ColorKey = "High";
            task.Tags = new string[] { "Bug Fixing" };
            task.ImageURL = new Uri(@"Images/Image10.png", UriKind.RelativeOrAbsolute);
            Tasks.Add(task);

            task = new KanbanModel();
            task.Tags = new string[] { "New control" };
            task.ImageURL = new Uri(@"Images/Image10.png", UriKind.RelativeOrAbsolute);
            Tasks.Add(task);

            task = new KanbanModel();
            task.ColorKey = "High";
            task.Tags = new string[] { "Bug fixing" };
            task.ImageURL = new Uri(@"Images/Image10.png", UriKind.RelativeOrAbsolute);
            Tasks.Add(task);

            task = new KanbanModel();
            task.ColorKey = "Normal";
            task.Tags = new string[] { "Bug Fixing" };
            task.ImageURL = new Uri(@"Images/Image10.png", UriKind.RelativeOrAbsolute);
            Tasks.Add(task);

            task = new KanbanModel();
            task.ColorKey = "Normal";
            task.Tags = new string[] { "Bug Fixing" };
            task.ImageURL = new Uri(@"Images/Image10.png", UriKind.RelativeOrAbsolute);
            Tasks.Add(task);

            task = new KanbanModel();
            task.Tags = new string[] { "Bug Fixing" };
            task.ImageURL = new Uri(@"Images/Image10.png", UriKind.RelativeOrAbsolute);
            Tasks.Add(task);

            task = new KanbanModel();
            task.ColorKey = "High";
            task.Tags = new string[] { "New Control" };
            task.ImageURL = new Uri(@"Images/Image10.png", UriKind.RelativeOrAbsolute);
            Tasks.Add(task);

            task = new KanbanModel();
            task.Title = "New Feature";
            task.ID = "29574";
            task.Description = "";
            task.Category = "";
            task.ColorKey = "Normal";
            task.Tags = new string[] { "New Control" };
            task.ImageURL = new Uri(@"Images/Image10.png", UriKind.RelativeOrAbsolute);
            Tasks.Add(task);

            task = new KanbanModel();
            task.ColorKey = "Low";
            task.Tags = new string[] { "New Control" };
            task.ImageURL = new Uri(@"Images/Image10.png", UriKind.RelativeOrAbsolute);
            Tasks.Add(task);
             */
        }

        public ObservableCollection<ScrumTask> ScrumTasks { get; set; }

        public ObservableCollection<KanbanModel> KanbanTasks { get; set; }
    }

    public class SprintDetails
    {
        public SprintDetails()
        {
            //берем из конфига строку подключения и подключаемся к БД
            SQLiteConnection Connection = new SQLiteConnection(Properties.Settings.Default.connectionString);
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
