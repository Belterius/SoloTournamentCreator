using SoloTournamentCreator.Helper;
using SoloTournamentCreator.Model;
using SoloTournamentCreator.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SoloTournamentCreator.ViewModel
{
    public class MainMenuViewModel : BaseViewModel
    {
        SavingContext MyDatabaseContext;
        Tournament _SelectedTournament;
        public ObservableCollection<Tournament> MyTournaments
        {
            get
            {
                return MyDatabaseContext.MyTournaments.Local;
            }
        }
        public ObservableCollection<Student> MyPlayers
        {
            get
            {
                return MyDatabaseContext.MyStudents.Local;
            }
        }
        public IEnumerable<Tournament> MyOpenTournaments
        {
            get
            {
                return MyTournaments.Where(x => x.Status == Tournament.TournamentStage.Open);
            }
        }
        public IEnumerable<Tournament> MyStartedTournaments
        {
            get
            {
                return MyTournaments.Where(x => x.Status == Tournament.TournamentStage.Started);
            }
        }
        public IEnumerable<Tournament> MyCompletedTournaments
        {
            get
            {
                return MyTournaments.Where(x => x.Status == Tournament.TournamentStage.Completed);
            }
        }

        public Tournament SelectedTournament
        {
            get
            {
                return _SelectedTournament;
            }

            set
            {
                _SelectedTournament = value;
                RaisePropertyChanged("SelectedTournament");
            }
        }
        
        public RelayCommand CreateTournamentCommand { get; set; }
        public RelayCommand CreatePlayerCommand { get; set; }
        public RelayCommand PlayerCheckedCommand { get; set; }
        public RelayCommand PlayerUncheckedCommand { get; set; }
        public RelayCommand StartTournamentCommand { get; set; }

        public MainMenuViewModel()
        {
            this.PropertyChanged += CustomPropertyChanged;
            MyDatabaseContext = new SavingContext();
            //ClearDatabase();
            //PopulateDatabase();
            try
            {
                //cf http://stackoverflow.com/questions/3356541/entity-framework-linq-query-include-multiple-children-entities

                MyDatabaseContext.MyStudents.Include(x => x.SummonerData).Include(x => x.SummonerSoloQueueData.Entries).Load();
                MyDatabaseContext.MyMatchs.Load();
                MyDatabaseContext.MyTeams.Load();
                MyDatabaseContext.MyTournamentTrees.Load();
                MyDatabaseContext.MyTournaments.Include(x => x.Participants).Include(x => x.Teams).Load();
            }
            catch (Exception ex)
            {

                throw;
            }
            if(MyDatabaseContext.MyTournaments.Where(x => x.Status == Tournament.TournamentStage.Open).Count() != 0)
            {
                SelectedTournament = MyOpenTournaments.First();
            }
            CreateTournamentCommand = new RelayCommand(CreateTournament);
            CreatePlayerCommand = new RelayCommand(CreatePlayer);
            PlayerCheckedCommand = new RelayCommand(PlayerChecked);
            PlayerUncheckedCommand = new RelayCommand(PlayerUnchecked);
            StartTournamentCommand = new RelayCommand(StartTournament);
        }
        private void ClearDatabase()
        {
            lock (MyDatabaseContext)
            {
                MyDatabaseContext.Database.Delete();
                MyDatabaseContext.SaveChanges();
            }
        }
        private void PopulateDatabase()
        {
            IEnumerable<RiotApi.Net.RestClient.Dto.League.LeagueDto.LeagueEntryDto> pgm = RiotToEntity.ApiRequest.GetSampleChallenger();
            lock (MyDatabaseContext)
            {
                int i = 200;
                foreach (RiotApi.Net.RestClient.Dto.League.LeagueDto.LeagueEntryDto challenjour in pgm)
                {
                    try
                    {
                        Student std = new Student(challenjour.PlayerOrTeamName + "@", challenjour.PlayerOrTeamName, challenjour.PlayerOrTeamName, challenjour.PlayerOrTeamName, 2016);
                        MyDatabaseContext.MyStudents.Add(std);
                    }
                    catch (Exception ex)
                    {

                    }
                    i--;
                    Console.WriteLine(i + " : " + challenjour.PlayerOrTeamName);
                    if(i % 5 == 0)
                    {
                        Thread.Sleep(10000);
                    }
                }
                MyDatabaseContext.SaveChanges();
            }
        }
        private void StartTournament(object obj)
        {
            MessageBoxResult result = MessageBox.Show("Do you wanna do something?", "Warning",MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                //code for Yes
                SelectedTournament.Start();
            }
            else if (result == MessageBoxResult.No)
            {
                //code for No
            }
            else if (result == MessageBoxResult.Cancel)
            {
                //code for Cancel
            }

        }
        private void CustomPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "MyTournaments":
                    RaisePropertyChanged("MyOpenTournaments");
                    RaisePropertyChanged("MyCompletedTournaments");
                    RaisePropertyChanged("MyStartedTournaments");
                    break;
                default:
                    break;
            }
        }
        private void PlayerChecked(object obj)
        {
            Student selectedPlayer = (Student)obj;
            if(SelectedTournament == null)
            {
                return;
            }
            if (SelectedTournament.Participants.Contains(selectedPlayer))
            {
                return;
            }
            //MyDatabaseContext.MyTournaments.Where(x => x.TournamentId == SelectedTournament.TournamentId).Single().Participants.Add(selectedPlayer);
            lock (MyDatabaseContext)
            {
                MyDatabaseContext.MyTournaments.Where(x => x.TournamentId == SelectedTournament.TournamentId).Single().Register(selectedPlayer);
                MyDatabaseContext.SaveChanges();
            }
            RaisePropertyChanged("MyTournaments");

        }
        private void PlayerUnchecked(object obj)
        {
            Student selectedPlayer = (Student)obj;
            if (SelectedTournament == null)
            {
                return;
            }
            if (!SelectedTournament.Participants.Contains(selectedPlayer))
            {
                return;
            }
            lock (MyDatabaseContext)
            {
                MyDatabaseContext.MyTournaments.Where(x => x.TournamentId == SelectedTournament.TournamentId).Single().Deregister(selectedPlayer);
                MyDatabaseContext.SaveChanges();
            }
            RaisePropertyChanged("MyTournaments");
        }
        private void CreateTournament(object obj)
        {
            CreateTournamentMenu CTM = new CreateTournamentMenu(){DataContext = new CreateTournamentViewModel(MyDatabaseContext)};
            CTM.ShowDialog();
            RaisePropertyChanged("MyOpenTournaments");
        }
        private void CreatePlayer(object obj)
        {
            CreatePlayerMenu CPM = new CreatePlayerMenu() { DataContext = new CreatePlayerViewModel(MyDatabaseContext) };
            CPM.ShowDialog();
        }
    }
}
