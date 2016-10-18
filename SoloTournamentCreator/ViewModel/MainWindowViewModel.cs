using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotApi.Net.RestClient;
using RiotApi.Net.RestClient.Configuration;
using SoloTournamentCreator.Helper;
using SoloTournamentCreator.Model;
using System.Data.Entity;
using System.Collections.ObjectModel;
using SoloTournamentCreator.View;

namespace SoloTournamentCreator.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        SavingContext MyDatabaseContext;

        Tournament _MyTournament;

        public RelayCommand CreatePlayerCommand { get; set; }
        public RelayCommand ClosingCommand { get; set; }

        public ObservableCollection<Student> MyPlayers
        {
            get
            {
                return MyDatabaseContext.MyStudents.Local;
            }
        }

        public Tournament MyTournament
        {
            get
            {
                return _MyTournament;
            }

            set
            {
                _MyTournament = value;
            }
        }

        public MainWindowViewModel()
        {
            MyTournament = new Tournament();
            MyDatabaseContext = new SavingContext();
            MyDatabaseContext.MyStudents
                          .Include(b => b.SummonerData)
                          .Load();
            var teest = (from students in MyDatabaseContext.MyStudents select students).ToList();
            
            //CleanDatabase();
            CreatePlayerCommand = new RelayCommand(CreatePlayer);
            ClosingCommand = new RelayCommand(Closing);
            //LoadPlayers();
        }
        private void Closing(object obj)
        {
            MyDatabaseContext.Dispose();
        }
        private void CleanDatabase()
        {
            lock(MyDatabaseContext)
            {
                MyDatabaseContext.Database.Delete();
                MyDatabaseContext.SaveChanges();
            }
        }
        private void LoadPlayers()
        {
            lock(MyDatabaseContext)
            {
                try
                {

                    MyDatabaseContext.MyStudents.Load();
                    var ListPlayers = (from hero in MyDatabaseContext.MyStudents select hero).ToList();
                    foreach(Student std in ListPlayers)
                    {
                        MyPlayers.Add(std);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        public void CreatePlayer(object obj)
        {
            DialogBox TestDialog = new DialogBox("Player Name", "Enter a Player Name");

            if (TestDialog.ShowDialog() == true)
            {
                if (TestDialog.GetInput != "")
                {
                    lock(MyDatabaseContext)
                    {
                        Student std = new Student("test", "test", "test", TestDialog.GetInput, 1998);
                        MyDatabaseContext.MyStudents.Add(std);
                        MyDatabaseContext.SaveChanges();
                        MyDatabaseContext.MyStudents.Load();
                        var teest = (from students in MyDatabaseContext.MyStudents select students).ToList();
                    }
                }
            }
            else
            {
            }
        }
    }
}
