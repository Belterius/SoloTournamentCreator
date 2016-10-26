using SoloTournamentCreator.Data;
using SoloTournamentCreator.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SoloTournamentCreator.ViewModel
{
    public class MenuSettingsViewModel : BaseViewModel
    {
        SavingContext MyDatabaseContext;
        string _Server;
        string _Port;
        string _Database;
        string _UserId;
        string _Password;

        
        public string Server
        {
            get
            {
                return _Server;
            }

            set
            {
                _Server = value;
            }
        }

        public string Port
        {
            get
            {
                return _Port;
            }

            set
            {
                _Port = value;
            }
        }

        public string Database
        {
            get
            {
                return _Database;
            }

            set
            {
                _Database = value;
            }
        }

        public string UserId
        {
            get
            {
                return _UserId;
            }

            set
            {
                _UserId = value;
            }
        }

        public string Password
        {
            get
            {
                return _Password;
            }

            set
            {
                _Password = value;
            }
        }
        
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand ClosingCommand { get; set; }
        public MenuSettingsViewModel()
        {
        }
        public MenuSettingsViewModel(SavingContext myDatabaseContext)
        {
            MyDatabaseContext = myDatabaseContext;
            Server = Properties.Settings.Default.Server;
            Port = Properties.Settings.Default.Port;
            Database = Properties.Settings.Default.Database;
            UserId = Properties.Settings.Default.UserId;
            Password = Properties.Settings.Default.Password;
            SaveCommand = new RelayCommand(Save);
            ClosingCommand = new RelayCommand(Closing);
        }

        private void Closing(object obj)
        {
            if (!MyDatabaseContext.CheckConnection())
            {
                MyDatabaseContext.ChangeConnectionString(
                    Properties.Settings.Default.Server,
                    Properties.Settings.Default.Port,
                    Properties.Settings.Default.Database,
                    Properties.Settings.Default.UserId,
                    Properties.Settings.Default.Password
                    );
            }
        }

        private void Save(object obj)
        {
            MyDatabaseContext.ChangeConnectionString(Server,Port,Database,UserId,Password);
            if (!MyDatabaseContext.CheckConnection())
            {
                MessageBox.Show("Error, couldn't connect to the database ! All change will be reverted to the previous working config on exit");
            }
            else
            {
                Properties.Settings.Default.Server = Server;
                Properties.Settings.Default.Port = Port;
                Properties.Settings.Default.Database = Database;
                Properties.Settings.Default.UserId = UserId;
                Properties.Settings.Default.Password = Password;
                Properties.Settings.Default.AdminRight = MyDatabaseContext.CheckWriteRight();
                Properties.Settings.Default.Save();
                MessageBox.Show("Change Saved ! Admin mode : " + Properties.Settings.Default.AdminRight.ToString());
                this.CloseWindow();
            }
        }
    }
}
