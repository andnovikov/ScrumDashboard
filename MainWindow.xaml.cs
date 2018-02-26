using System;
using System.Data.Linq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

using Devart.Data.SQLite;
using Syncfusion.UI.Xaml.Kanban;
using Syncfusion.Windows.SampleLayout;
using ScrumDashboard.Classes;
using ScrumDashboard.ViewModel;

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
                try
                {
                    ComboBoxItem cbItm = new ComboBoxItem();
                    cbItm.Tag = tm;
                    cbItm.Content = tm.Name;
                    TaskTeamMember.Items.Add(cbItm);
                }
                catch { }

                try
                {
                    ComboBoxItem cbItm = new ComboBoxItem();
                    cbItm.Tag = tm;
                    cbItm.Content = tm.Name;
                    CardAuthorFilter.Items.Add(cbItm);
                }
                catch { }
            }

            Table<Sprint> Sprints = db.GetTable<Sprint>();
            IQueryable<Sprint> sprts = (from sp in Sprints
                                        select sp);

            foreach (Sprint sp in Sprints)
            {
                ComboBoxItem spItm = new ComboBoxItem();
                spItm.Tag = sp;
                spItm.Content = "Sprint " + sp.ID.ToString() + " (" + sp.DateStart.ToString() + "-" + sp.DateEnd.ToString() + ")";
                SprintFilter.Items.Add(spItm);
            }
        }

        private void SprintFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Load sprint tasks
            Kanban.ItemsSource = null;
            (this.Kanban.DataContext as KanbanDeskViewModel).SprintID = (((sender as ComboBox).SelectedItem as ComboBoxItem).Tag as Sprint).ID;
            (this.Kanban.DataContext as KanbanDeskViewModel).ReloadKanban();
            Kanban.ItemsSource = (Kanban.DataContext as KanbanDeskViewModel).InventoryView;
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

            if (e.TargetKey.ToString().Equals("Closed") ||
                e.TargetKey.ToString().Equals("Review")) {

                int bp = 0;
                switch (e.TargetKey.ToString()) {
                    case "Review":
                        bp = Convert.ToInt32(Math.Round(tsk.Cost*0.8));
                        break;
                    case "Closed":
                        bp = Convert.ToInt32(Math.Round(tsk.Cost*0.2));
                        break;
                };
                    

                Table<SprintBurnData> SprintBrnDt = db.GetTable<SprintBurnData>();
                SprintBurnData brnDat = new SprintBurnData
                {
                    SprintID = 3,
                    BurnDate = DateTime.Today,
                    BurnPoint = bp,
                    SprintTaskID = SprintTaskID
                };


                SprintBrnDt.InsertOnSubmit(brnDat);
            }
            

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

                Kanban.ItemsSource = null;
                Kanban.ItemsSource = (Kanban.DataContext as KanbanDeskViewModel).InventoryView;
            }
        }

        #region CardTagFilter
        private void CardAuthorFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex != -1)
            {
                TeamMember tm = (((sender as ComboBox).SelectedItem as ComboBoxItem).Tag as TeamMember);
                Kanban.ItemsSource = null;
                (this.Kanban.DataContext as KanbanDeskViewModel).FilterByAuthor(tm.Nickname);
                Kanban.ItemsSource = (Kanban.DataContext as KanbanDeskViewModel).InventoryView;
            }
        }

        private void CardAuthorFilterClear_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CardAuthorFilter.SelectedIndex = -1;
            Kanban.ItemsSource = null;
            (this.Kanban.DataContext as KanbanDeskViewModel).FilterByAuthor("");
            Kanban.ItemsSource = (Kanban.DataContext as KanbanDeskViewModel).InventoryView;
        }
        #endregion

        #region CardTagFilter
        private void CardTagFilterApply_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Kanban.ItemsSource = null;
            (this.Kanban.DataContext as KanbanDeskViewModel).FilterByTag(CardTagFilter.Text);
            Kanban.ItemsSource = (Kanban.DataContext as KanbanDeskViewModel).InventoryView;
        }

        private void CardTagFilterClear_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CardTagFilter.Text = "";
            Kanban.ItemsSource = null;
            (this.Kanban.DataContext as KanbanDeskViewModel).FilterByTag(CardTagFilter.Text);
            Kanban.ItemsSource = (Kanban.DataContext as KanbanDeskViewModel).InventoryView;
        }

        private void CardTagFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            Kanban.ItemsSource = null;
            (this.Kanban.DataContext as KanbanDeskViewModel).FilterByTag(CardTagFilter.Text);
            Kanban.ItemsSource = (Kanban.DataContext as KanbanDeskViewModel).InventoryView;
        }
        #endregion
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