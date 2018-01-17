using Syncfusion.UI.Xaml.Kanban;
using Syncfusion.Windows.SampleLayout;
using ScrumDashboard.Classes;
using Syncfusion.Windows.Shared;
using System;
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
    }

    public class TaskDetails
    {
        public TaskDetails()
        {
            ScrumTasks = new ObservableCollection<ScrumTask>();

            {
                ScrumTask task = new ScrumTask(6593, 0, "UWP Issue", "Sorting is not working properly in DateTimeAxis", "Postponed");
                ScrumTasks.Add(task);
                task = new ScrumTask(6593, 0, "New Feature", "Need to create code base for Gantt control", "Postponed");
                ScrumTasks.Add(task);
                task = new ScrumTask(6593, 0, "UG", "Sorting is not working properly in DateTimeAxis", "Postponed");
                ScrumTasks.Add(task);
                task = new ScrumTask(6516, 0, "UWP Issue", "Need to do post processing work for closed incidents", "Postponed");
                ScrumTasks.Add(task);
                task = new ScrumTask(651, 0, "UWP Issue", "Crosshair label template not visible in UWP.", "Open");
                ScrumTasks.Add(task);
                task = new ScrumTask(646, 0, "UWP Issue", "AxisLabel cropped when rotate the axis label.", "Open");
                ScrumTasks.Add(task);
                task = new ScrumTask(23822, 0, "WPF Issue", "Need to implement tooltip support for histogram series.", "Open");
                ScrumTasks.Add(task);
                task = new ScrumTask(25678, 0, "Kanban Feature", "Need to prepare SampleBrowser sample", "InProgress");
                ScrumTasks.Add(task);
                task = new ScrumTask(1254, 0, "WP Issue", "HorizontalAlignment for tooltip is not working", "InProgress");
                ScrumTasks.Add(task);
                task = new ScrumTask(28066, 0, "WPF Issue", "In minimized state, first and last segment have incorrect spacing", "Review");
                ScrumTasks.Add(task);
                task = new ScrumTask(29477, 0, "WPF Issue", "Null reference exception thrown in line chart", "Review");
                ScrumTasks.Add(task);
                task = new ScrumTask(29574, 0, "WPF Issue", "Minimum and maximum property are not working in dynamic update", "Review");
                ScrumTasks.Add(task);
                task = new ScrumTask(25678, 0, "Kanban Feature", "Need to implement tooltip support for SfKanban", "Review");
                ScrumTasks.Add(task);
                task = new ScrumTask(29574, 0, "New Feature", "Dragging events support for SfKanban", "Closed");
                ScrumTasks.Add(task);
                task = new ScrumTask(29574, 0, "loooooooooooooooooooooooooooooooooong name", "Dragging events support for SfKanban", "Closed");
                ScrumTasks.Add(task);
                task = new ScrumTask(29574, 0, "UWP Issue", "Swimlane support for SfKanban", "Open");
                ScrumTasks.Add(task);
            }

            KanbanTasks = new ObservableCollection<KanbanModel>();
            foreach (ScrumTask task in ScrumTasks)
            {
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
}
