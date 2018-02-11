using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Devart.Data.SQLite;

namespace ScrumDashboard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private SQLiteConnection _MainConnection;

        public SQLiteConnection MainConnection
        {
            get { return _MainConnection; }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _MainConnection = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + ScrumDashboard.Properties.Settings.Default.DBPath);
            _MainConnection.Open();
        }
    }
}
