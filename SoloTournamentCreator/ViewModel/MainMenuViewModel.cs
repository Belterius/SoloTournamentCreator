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
using System.Windows.Controls;

namespace SoloTournamentCreator.ViewModel
{
    public class MainMenuViewModel : BaseViewModel
    {
        SavingContext MyDatabaseContext;
        Tournament _SelectedOpenTournament;
        Tournament _SelectedStartedTournament;
        Team _SelectedStartedTournamentTeam;
        Student _SelectedTeamSelectedPlayer;
        Student _SavedSwapPlayer;
        Team _SavedSwapTeam;
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
        public List<Team> MyTeams
        {
            get
            {
                if (SelectedStartedTournament != null)
                    return SelectedStartedTournament.Teams.ToList();
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
                RaisePropertyChanged("MyTeams");
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
                if(_SelectedStartedTournamentTeam != null)//If I'm selecting a team, I need to reset my SelectedPlayer, but if my value is null, it means I'm either reseting the team because I'm chosing an additional player (so I don't want to reset my SelectedPlayer) or I'm changing tournament (and then I already reset my SelectedPlayer)
                    _SelectedTeamSelectedPlayer = null;
                RaisePropertyChanged("SelectedStartedTournamentTeam");
                RaisePropertyChanged("SelectedTeamPlayers");
            }
        }
        public List<Student> SelectedTeamPlayers
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
                if(SavedSwapPlayer != null)
                {
                    return System.Windows.Media.Brushes.DarkSalmon;
                }else
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


        public MainMenuViewModel()
        {
            this.PropertyChanged += CustomPropertyChanged;
            MyDatabaseContext = new SavingContext();
            //ClearDatabase();
            //PopulateDatabase();
            try
            {
                //cf http://stackoverflow.com/questions/3356541/entity-framework-linq-query-include-multiple-children-entities

                MyDatabaseContext.MyStudents.Include(x => x.SummonerData).Include(x => x.DetailSoloQueueData.MiniSeries).Load();
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
        }
        private void InternalListBoxItemClick(object obj)
        {
            ((ListBox)((object[])obj)[0]).SelectedItem = ((ListBoxItem)((object[])obj)[1]).DataContext;
            ((ListBoxItem)((object[])obj)[1]).Focus();
        }
        private void Closing(object obj)
        {
            MyDatabaseContext.Dispose();
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
            Thread.Sleep(10000);
            lock (MyDatabaseContext)
            {
                int i = 50;
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
                    if (i <= 0)
                        break;
                }
                MyDatabaseContext.SaveChanges();
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
                //SelectedOpenTournament.Start();

                SelectedOpenTournament.Clean();
                SelectedOpenTournament.CreateTeam();
                SelectedOpenTournament.BalanceTeam();
                SelectedOpenTournament.CreateTournamentTree();
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
                if(SelectedStartedTournament.TournamentWinner == null)
                {
                    if (MessageBox.Show("This tournament doesn't have a Winner yet, are you really sure you want to Archive it ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                    {
                        return;
                    }
                }
                SelectedStartedTournament.Archive();
                //MyDatabaseContext.SaveChanges();
                RaisePropertyChanged("MyTournaments");
            }

        }

        private void SwapPlayer(object obj)
        {
            if (SelectedTeamSelectedPlayer == null)
                return;
            if(SavedSwapPlayer == null)
            {
                SavedSwapPlayer = SelectedTeamSelectedPlayer;
                RaisePropertyChanged("MyTeams");
                RaisePropertyChanged("SelectedTeamPlayers"); 
                RaisePropertyChanged("SelectedTeamAdditionnalPlayers");
                return;
            }
            //var teamOne = MyDatabaseContext.MyTournaments.Local.Where(x => x.TournamentId == SelectedStartedTournament.TournamentId).Select(x => x.Teams.Where(y => y.TeamMember.Contains(MyDatabaseContext.MyStudents.Where(std => std.StudentId == SavedSwapPlayer.StudentId).FirstOrDefault()))).Single();
            //var teamTwo = MyDatabaseContext.MyTournaments.Local.Where(x => x.TournamentId == SelectedStartedTournament.TournamentId).Select(x => x.Teams.Where(y => y.TeamMember.Contains(MyDatabaseContext.MyStudents.Where(std => std.StudentId == SelectedTeamSelectedPlayer.StudentId).FirstOrDefault()))).Single();
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
            RaisePropertyChanged("MyTeams");
            RaisePropertyChanged("SelectedTeamPlayers");
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
            if(SelectedOpenTournament == null)
            {
                return;
            }
            if (SelectedOpenTournament.Participants.Contains(selectedPlayer))
            {
                return;
            }
            lock (MyDatabaseContext)
            {
                MyDatabaseContext.MyTournaments.Where(x => x.TournamentId == SelectedOpenTournament.TournamentId).Single().Register(selectedPlayer);
                MyDatabaseContext.SaveChanges();
            }
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
            lock (MyDatabaseContext)
            {
                MyDatabaseContext.MyTournaments.Where(x => x.TournamentId == SelectedOpenTournament.TournamentId).Single().Deregister(selectedPlayer);
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
        private void RenameTeam(object obj) {
            if(SelectedStartedTournamentTeam != null)
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
                    RaisePropertyChanged("MyTeams");
                }
            }
        }
        private void SeeBracket(object obj)
        {
            if(SelectedStartedTournament != null)
            {
                TournamentBracket tb = new TournamentBracket() { DataContext = new TournamentBracketViewModel(MyDatabaseContext, SelectedStartedTournament) };
                tb.ShowDialog();
            }
        }
    }
}
