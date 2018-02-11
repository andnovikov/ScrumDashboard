using System;
using System.Linq;
using System.Data.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

using Devart.Data.SQLite;
using Syncfusion.UI.Xaml.Kanban;
using ScrumDashboard.Classes;

namespace ScrumDashboard.ViewModel
{
    
    class KanbanDeskViewModel : INotifyPropertyChanged
    {
        public KanbanDeskViewModel ()
        {
            KanbanTasks = new ObservableCollection<KanbanModel>();

            LoadKanban();

            _inventoryView = CollectionViewSource.GetDefaultView(KanbanTasks);
        }

        private ObservableCollection<KanbanModel> _kanbanTasks;

        public ObservableCollection<KanbanModel> KanbanTasks
    {
            get {
                return _kanbanTasks;
            }
            set {
                _kanbanTasks = value;
            }
        }

        private ICollectionView _inventoryView;

        public ICollectionView InventoryView { get { return _inventoryView; } }

        private string _filterstring;


        public string FilterString {
            get {
                return _filterstring;
            }
            set {
                _filterstring = value;
                OnPropertyChanged("Filter");
            }
        }

        private void OnPropertyChanged(string v)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(v));
            }
        }

        private void LoadKanban () {
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

            foreach (var task in query)
            {
                KanbanModel kbTask = new KanbanModel();
                kbTask.ID = task.ID.ToString();
                kbTask.Title = task.Title;
                kbTask.Description = task.Description;
                kbTask.Category = task.Category;
                kbTask.ColorKey = "High";
                kbTask.Tags = new string[] { task.ExternalID.ToString() };
                if (!task.Nickname.Equals(""))
                {
                    kbTask.ImageURL = new Uri(@"Images/" + task.Nickname + ".png", UriKind.RelativeOrAbsolute);
                }
                KanbanTasks.Add(kbTask);
            }
        }

        private bool TagFilter(object item)
        {
            IKanbanModel task = item as IKanbanModel;
            bool result = false;

            if (FilterString.Equals(""))
                result = true;
            else
            {
                foreach (string s in task.Tags)
                {
                    if (s.Contains(FilterString))
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        public bool CanFilter()
        {
            return true;
        }

        public void ExecuteFilter(string e)
        {
            this.FilterString = e;
            this._inventoryView.Filter = TagFilter;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
