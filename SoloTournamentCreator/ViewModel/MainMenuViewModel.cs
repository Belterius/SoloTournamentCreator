using MySql.Data.MySqlClient;
using SoloTournamentCreator.Helper;
using SoloTournamentCreator.Model;
using SoloTournamentCreator.View;
using SoloTournamentCreator.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SoloTournamentCreator.RiotToEntity;
using RiotApi.Net.RestClient.Configuration;
using RiotSharp;

namespace SoloTournamentCreator.ViewModel
{
    public class MainMenuViewModel : BaseViewModel
    {
        SavingContext MyDatabaseContext;
        Tournament _SelectedOpenTournament;
        Tournament _SelectedStartedTournament;
        Tournament _SelectedCompletedTournament;
        Team _SelectedStartedTournamentTeam;
        Team _SelectedCompletedTournamentTeam;
        Student _SelectedTeamSelectedPlayer;
        Student _SavedSwapPlayer;
        Team _SavedSwapTeam;
        string _SummonerFilter;
        string _StartedTournamentSummonerFilter;
        public ObservableCollection<Tournament> MyTournaments
        {
            get
            {
                return MyDatabaseContext.MyTournaments.Local;
            }
        }
        public IEnumerable<Student> MyPlayers
        {
            get
            {
                return MyDatabaseContext.MyStudents.Local.Where(x => x.Pseudo.ToLower().Contains(SummonerFilter ?? ""));
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

        public Tournament SelectedOpenTournament
        {
            get
            {
                return _SelectedOpenTournament;
            }

            set
            {
                _SelectedOpenTournament = value;
                RaisePropertyChanged("SelectedOpenTournament");
            }
        }
        public List<Team> MyStartedTournamentTeams
        {
            get
            {
                if (SelectedStartedTournament != null)
                {
                    return SelectedStartedTournament.Teams.Where(x => x.TeamMember.Where(y => y.Pseudo.ToLower().Contains(StartedTournamentSummonerFilter ?? "")).Count() != 0).ToList();
                }
                return null;
            }
        }
        public List<Team> MyCompletedTournamentTeams
        {
            get
            {
                if (SelectedCompletedTournament != null)
                    return SelectedCompletedTournament.Teams.ToList();
                return null;
            }
        }
        public Tournament SelectedStartedTournament
        {
            get
            {
                return _SelectedStartedTournament;
            }

            set
            {
                _SelectedStartedTournament = value;
                RaisePropertyChanged("SelectedStartedTournament");
                RaisePropertyChanged("MyStartedTournamentTeams");
                RaisePropertyChanged("SelectedTeamAdditionnalPlayers");
            }
        }
        public Team SelectedStartedTournamentTeam
        {
            get
            {
                return _SelectedStartedTournamentTeam;
            }

            set
            {
                _SelectedStartedTournamentTeam = value;
                if (_SelectedStartedTournamentTeam != null)//If I'm selecting a team, I need to reset my SelectedPlayer, but if my value is null, it means I'm either reseting the team because I'm chosing an additional player (so I don't want to reset my SelectedPlayer) or I'm changing tournament (and then I already reset my SelectedPlayer)
                    _SelectedTeamSelectedPlayer = null;
                RaisePropertyChanged("SelectedStartedTournamentTeam");
                RaisePropertyChanged("SelectedStartedTournamentTeamPlayers");
            }
        }
        public List<Student> SelectedStartedTournamentTeamPlayers
        {
            get
            {
                if (SelectedStartedTournamentTeam != null)
                    return SelectedStartedTournamentTeam.TeamMember.ToList();
                return null;
            }
        }
        public List<Student> SelectedTeamAdditionnalPlayers
        {
            get
            {
                if (SelectedStartedTournament != null)
                    return SelectedStartedTournament.Participants.ToList();
                return null;
            }
        }

        public Student SelectedTeamSelectedPlayer
        {
            get
            {
                return _SelectedTeamSelectedPlayer;
            }

            set
            {
                _SelectedTeamSelectedPlayer = value;
                RaisePropertyChanged("SelectedTeamSelectedPlayer");
            }
        }
        public Student SelectedStartedTournamentAdditionnalPlayer
        {
            get
            {
                return _SelectedTeamSelectedPlayer;
            }

            set
            {
                SelectedTeamSelectedPlayer = value;
                SelectedStartedTournamentTeam = null; //If I select an additionnal player I must unselect any Team, because he belong to none.
            }
        }

        public System.Windows.Media.SolidColorBrush IsSwapInProgress
        {
            get
            {
                if (SavedSwapPlayer != null)
                {
                    return System.Windows.Media.Brushes.DarkSalmon;
                }
                else
                {
                    return System.Windows.Media.Brushes.Transparent;
                }
            }
        }

        public Student SavedSwapPlayer
        {
            get
            {
                return _SavedSwapPlayer;
            }

            set
            {
                _SavedSwapPlayer = value;
                if (_SavedSwapPlayer != null)
                    SavedSwapTeam = SelectedStartedTournamentTeam;
                else
                    SavedSwapTeam = null;
                RaisePropertyChanged("IsSwapInProgress");
            }
        }
        public Team SavedSwapTeam
        {
            get
            {
                return _SavedSwapTeam;
            }

            set
            {
                _SavedSwapTeam = value;
            }
        }

        public Tournament SelectedCompletedTournament
        {
            get
            {
                return _SelectedCompletedTournament;
            }

            set
            {
                _SelectedCompletedTournament = value;
                RaisePropertyChanged("SelectedCompletedTournament");
                RaisePropertyChanged("MyCompletedTournamentTeams");
            }
        }
        public Team SelectedCompletedTournamentTeam
        {
            get
            {
                return _SelectedCompletedTournamentTeam;
            }

            set
            {
                _SelectedCompletedTournamentTeam = value;
                if (_SelectedCompletedTournamentTeam != null)//If I'm selecting a team, I need to reset my SelectedPlayer, but if my value is null, it means I'm either reseting the team because I'm chosing an additional player (so I don't want to reset my SelectedPlayer) or I'm changing tournament (and then I already reset my SelectedPlayer)
                    _SelectedTeamSelectedPlayer = null;
                RaisePropertyChanged("SelectedCompletedTournamentTeam");
                RaisePropertyChanged("SelectedCompletedTournamentTeamPlayers");
            }
        }
        public List<Student> SelectedCompletedTournamentTeamPlayers
        {
            get
            {
                if (SelectedCompletedTournamentTeam != null)
                    return SelectedCompletedTournamentTeam.TeamMember.ToList();
                return null;
            }
        }

        public string SummonerFilter
        {
            get
            {
                return _SummonerFilter;
            }
            set
            {
                _SummonerFilter = value.ToLower();
                RaisePropertyChanged("MyPlayers");
            }
        }
        public string StartedTournamentSummonerFilter
        {
            get
            {
                return _StartedTournamentSummonerFilter;
            }
            set
            {
                _StartedTournamentSummonerFilter = value.ToLower();
                RaisePropertyChanged("MyStartedTournamentTeams");
            }
        }
        public Visibility AdminOnlyVisible
        {
            get
            {
                if (Properties.Settings.Default.AdminRight)
                {
                    return Visibility.Visible;
                }else
                {
                    return Visibility.Hidden;
                }
            }
        }
        public RelayCommand CreateTournamentCommand { get; set; }
        public RelayCommand CreatePlayerCommand { get; set; }
        public RelayCommand PlayerCheckedCommand { get; set; }
        public RelayCommand PlayerUncheckedCommand { get; set; }
        public RelayCommand StartTournamentCommand { get; set; }
        public RelayCommand SwapPlayerCommand { get; set; }
        public RelayCommand InternalListBoxItemClickCommand { get; set; }
        public RelayCommand RenameTeamCommand { get; set; }
        public RelayCommand SeeBracketCommand { get; set; }
        public RelayCommand ArchiveTournamentCommand { get; set; }
        public RelayCommand ClosingCommand { get; set; }
        public RelayCommand OpenSettingsCommand { get; set; }


        public MainMenuViewModel()
        {
            this.PropertyChanged += CustomPropertyChanged;
            TestRiotSharp();
            InitDatabaseContext();
            //PopulateDatabase();
            try
            {
                //cf http://stackoverflow.com/questions/3356541/entity-framework-linq-query-include-multiple-children-entities

                MyDatabaseContext.MyStudents.Include(x => x.MySummonerData).Include(x => x.MyLeagues).Include(x => x.MySummonerData).Load();
                MyDatabaseContext.MyMatchs.Load();
                MyDatabaseContext.MyTeams.Include(x => x.TeamMember).Load();
                MyDatabaseContext.MyTournamentTrees.Load();
                MyDatabaseContext.MyTournaments.Include(x => x.Participants).Include(x => x.Teams).Load();
            }
            catch (Exception ex)
            {
                throw;
            }
            if (MyDatabaseContext.MyTournaments.Where(x => x.Status == Tournament.TournamentStage.Open).Count() != 0)
            {
                SelectedOpenTournament = MyOpenTournaments.First();
            }
            ClosingCommand = new RelayCommand(Closing);
            CreateTournamentCommand = new RelayCommand(CreateTournament);
            CreatePlayerCommand = new RelayCommand(CreatePlayer);
            PlayerCheckedCommand = new RelayCommand(PlayerChecked);
            PlayerUncheckedCommand = new RelayCommand(PlayerUnchecked);
            StartTournamentCommand = new RelayCommand(StartTournament);
            SwapPlayerCommand = new RelayCommand(SwapPlayer);
            RenameTeamCommand = new RelayCommand(RenameTeam);
            InternalListBoxItemClickCommand = new RelayCommand(InternalListBoxItemClick);
            SeeBracketCommand = new RelayCommand(SeeBracket);
            ArchiveTournamentCommand = new RelayCommand(ArchiveTournament);
            OpenSettingsCommand = new RelayCommand(OpenSettings);
        }
        
        private void InitDatabaseContext()
        {
            Properties.Settings.Default.Server = "coniface.no-ip.org";
            Properties.Settings.Default.Port ="19172";
            Properties.Settings.Default.Password = "NOxNQmCZ1D0i4gDdISlIixmrRrKGECYV0cIW0BRXgBeFI2y9m9";
            Properties.Settings.Default.Database = "paintfusion";
            Properties.Settings.Default.UserId = "paintfusion";
            string connectionString = $"server={Properties.Settings.Default.Server};port={Properties.Settings.Default.Port};database={Properties.Settings.Default.Database};uid={Properties.Settings.Default.UserId};password='{Properties.Settings.Default.Password}'";
            MyDatabaseContext = new SavingContext(connectionString);
            if (!MyDatabaseContext.CheckConnection())
            {
                MyDatabaseContext = new SavingContext();
            }
            if (!MyDatabaseContext.CheckWriteRight())
            {
                Properties.Settings.Default.AdminRight = false;
            }

        }

        private void OpenSettings(object obj)
        {
            MenuSettings MS = new MenuSettings() { DataContext = new MenuSettingsViewModel(MyDatabaseContext)};
            MS.ShowDialog();
            RaisePropertyChanged("AdminOnlyVisible");
        }

        [Conditional("DEBUG")]
        private void TestRiotSharp()
        {
            
        }

        private void InternalListBoxItemClick(object obj)
        {
            ((ListBox)((object[])obj)[0]).SelectedItem = ((ListBoxItem)((object[])obj)[1]).DataContext;
            ((ListBoxItem)((object[])obj)[1]).Focus();
        }
        private void Closing(object obj)
        {
            MyDatabaseContext.Dispose();
            Application.Current.Shutdown();
        }
        [Conditional("DEBUG")]
        private void ClearDatabase()
        {
            MyDatabaseContext.Database.Delete();
            MyDatabaseContext.SaveChanges();
        }
        [Conditional("DEBUG")]
        private void PopulateDatabase()// retrieve the Challengers players and add them as sample data
        {
            IEnumerable<RiotApi.Net.RestClient.Dto.League.LeagueDto.LeagueEntryDto> pgm = RiotToEntity.ApiRequest.GetSampleChallenger();
            pgm.OrderBy(x => x.LeaguePoints);
            Thread.Sleep(10000);
            int i = 190;
            foreach (RiotApi.Net.RestClient.Dto.League.LeagueDto.LeagueEntryDto challenjour in pgm)
            {
                try
                {
                    CreatePlayerIfNotExist(challenjour);
                }
                catch (Exception)
                {

                }
                i--;
                Console.WriteLine($"{i} : {challenjour.PlayerOrTeamName}");
                if (i % 5 == 0)
                {
                    MyDatabaseContext.SaveChanges();
                    Thread.Sleep(10000);
                }
                if (i <= 0)
                    break;
            }
            MyDatabaseContext.SaveChanges();
        }
        private void CreatePlayerIfNotExist(RiotApi.Net.RestClient.Dto.League.LeagueDto.LeagueEntryDto challenjour)
        {
            try
            {
                Student newPlayer = new Student(challenjour.PlayerOrTeamName + "@.", challenjour.PlayerOrTeamName, challenjour.PlayerOrTeamName, challenjour.PlayerOrTeamName, 2016);
                if (MyDatabaseContext.MyStudents.Where(x => x.SummonerID == newPlayer.SummonerID).Any())
                {
                    Student currentPlayer = MyDatabaseContext.MyStudents.Where(x => x.SummonerID == newPlayer.SummonerID).Single();
                    MessageBox.Show($"This summoner is already linked to an account, you should not create a new profile if a player rename ! Name : {currentPlayer.FirstName} {currentPlayer.LastName} old Pseudo : {currentPlayer.Pseudo}");
                    return;
                }
                if (MyDatabaseContext.MyStudents.Where(x => x.FirstName == newPlayer.FirstName && x.LastName == newPlayer.LastName).Any())
                {

                    if (MessageBox.Show($"There is already a Player with the same First and Last Name with the Pseudo {MyDatabaseContext.MyStudents.Where(x => x.FirstName == newPlayer.FirstName && x.LastName == newPlayer.LastName).First().Pseudo}, are you sure you want to create a new one ?", "Warning", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                    {
                        MessageBox.Show("Creation cancelled");
                        return;
                    }
                }
                MyDatabaseContext.MyStudents.Add(newPlayer);
            }
            catch (RiotApi.Net.RestClient.Helpers.RiotExceptionRaiser.RiotApiException ex)
            {
                if (ex.RiotErrorCode == RiotApi.Net.RestClient.Helpers.RiotExceptionRaiser.RiotErrorCode.DATA_NOT_FOUND)
                {
                    return;
                }
                if (ex.RiotErrorCode == RiotApi.Net.RestClient.Helpers.RiotExceptionRaiser.RiotErrorCode.SERVER_ERROR)
                {
                    return;
                }
                if (ex.RiotErrorCode == RiotApi.Net.RestClient.Helpers.RiotExceptionRaiser.RiotErrorCode.RATE_LIMITED)
                {
                    return;
                }
            }
            catch (Exception)
            {
            }
        }
        private void StartTournament(object obj)
        {
            if (SelectedOpenTournament == null)
            {
                MessageBox.Show("Please Select a Tournament");
                return;
            }
            if (MessageBox.Show("Are you sure you want to start the Tournament?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                SelectedOpenTournament.Start();
                MyDatabaseContext.SaveChanges();
                RaisePropertyChanged("MyTournaments");
            }

        }
        private void ArchiveTournament(object obj)
        {
            if (SelectedStartedTournament == null)
            {
                MessageBox.Show("Please Select a Tournament");
                return;
            }
            if (MessageBox.Show("Are you sure you want to archive the Tournament ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (SelectedStartedTournament.TournamentWinner == null)
                {
                    if (MessageBox.Show("This tournament doesn't have a Winner yet, are you really sure you want to Archive it ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                    {
                        return;
                    }
                }
                SelectedStartedTournament.Archive();
                MyDatabaseContext.SaveChanges();
                RaisePropertyChanged("MyTournaments");
            }

        }

        private void SwapPlayer(object obj)
        {
            if (SelectedTeamSelectedPlayer == null)
                return;
            if (SavedSwapPlayer == null)
            {
                SavedSwapPlayer = SelectedTeamSelectedPlayer;
                RaisePropertyChanged("MyStartedTournamentTeams");
                RaisePropertyChanged("SelectedStartedTournamentTeamPlayers");
                RaisePropertyChanged("SelectedTeamAdditionnalPlayers");
                return;
            }
            if (SelectedStartedTournamentTeam != null)
            {
                SelectedStartedTournamentTeam.RemoveMember(SelectedTeamSelectedPlayer);
                SelectedStartedTournamentTeam.AddMember(SavedSwapPlayer);
            }
            else
            {
                //to additionnal players
                SelectedStartedTournament.Participants.Remove(SelectedTeamSelectedPlayer);
                SelectedStartedTournament.Participants.Add(SavedSwapPlayer);
            }
            if (SavedSwapTeam != null)
            {
                SavedSwapTeam.RemoveMember(SavedSwapPlayer);
                SavedSwapTeam.AddMember(SelectedTeamSelectedPlayer);
            }
            else
            {
                //from additionnal players
                SelectedStartedTournament.Participants.Remove(SavedSwapPlayer);
                SelectedStartedTournament.Participants.Add(SelectedTeamSelectedPlayer);
            }

            MyDatabaseContext.SaveChanges();
            SavedSwapPlayer = null;
            RaisePropertyChanged("MyStartedTournamentTeams");
            RaisePropertyChanged("SelectedStartedTournamentTeamPlayers");
            RaisePropertyChanged("SelectedTeamAdditionnalPlayers");
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
                case "SelectedStartedTournament":
                    SavedSwapPlayer = null;
                    break;
                default:
                    break;
            }
        }
        private void PlayerChecked(object obj)
        {
            Student selectedPlayer = (Student)obj;
            if (SelectedOpenTournament == null)
            {
                return;
            }
            if (SelectedOpenTournament.Participants.Contains(selectedPlayer))
            {
                return;
            }
            MyDatabaseContext.MyTournaments.Where(x => x.TournamentId == SelectedOpenTournament.TournamentId).Single().Register(selectedPlayer);
            MyDatabaseContext.SaveChanges();
            RaisePropertyChanged("MyTournaments");

        }
        private void PlayerUnchecked(object obj)
        {
            Student selectedPlayer = (Student)obj;
            if (SelectedOpenTournament == null)
            {
                return;
            }
            if (!SelectedOpenTournament.Participants.Contains(selectedPlayer))
            {
                return;
            }
            MyDatabaseContext.MyTournaments.Where(x => x.TournamentId == SelectedOpenTournament.TournamentId).Single().Deregister(selectedPlayer);
            MyDatabaseContext.SaveChanges();
            RaisePropertyChanged("MyTournaments");
        }
        private void CreateTournament(object obj)
        {
            CreateTournamentMenu CTM = new CreateTournamentMenu() { DataContext = new CreateTournamentViewModel(MyDatabaseContext) };
            CTM.ShowDialog();
            RaisePropertyChanged("MyOpenTournaments");
        }
        private void CreatePlayer(object obj)
        {
            CreatePlayerMenu CPM = new CreatePlayerMenu() { DataContext = new CreatePlayerViewModel(MyDatabaseContext) };
            CPM.ShowDialog();
            RaisePropertyChanged("MyPlayers");
        }
        private void RenameTeam(object obj)
        {
            if (SelectedStartedTournamentTeam != null)
            {
                DialogBox teamName = new DialogBox("Rename Team", "Please enter a new Team Name");
                teamName.ShowDialog();
                if (teamName.DialogResult == true)
                {
                    if (!SelectedStartedTournamentTeam.Rename(teamName.GetInput))
                    {
                        MessageBox.Show("Invalid Team Name");
                        return;
                    }
                    MyDatabaseContext.SaveChanges();
                    RaisePropertyChanged("MyStartedTournamentTeams");
                }
            }
        }
        private void SeeBracket(object obj)
        {
            if (obj is Tournament)
            {
                TournamentBracket tb = new TournamentBracket() { DataContext = new TournamentBracketViewModel(MyDatabaseContext, (Tournament)obj) };
                tb.ShowDialog();
            }
        }
    }
}
