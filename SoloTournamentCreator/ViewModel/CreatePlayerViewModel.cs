using SoloTournamentCreator.Helper;
using SoloTournamentCreator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SoloTournamentCreator.ViewModel
{
    public class CreatePlayerViewModel : BaseViewModel
    {
        SavingContext MyDatabaseContext;
        string _LastName;
        string _FirstName;
        string _Pseudo;
        string _Mail;
        string _GraduationYear;

        public string LastName
        {
            get
            {
                return _LastName;
            }

            set
            {
                _LastName = value;
                RaisePropertyChanged("LastName");
            }
        }

        public string FirstName
        {
            get
            {
                return _FirstName;
            }

            set
            {
                _FirstName = value;
                RaisePropertyChanged("FirstName");
            }
        }

        public string Pseudo
        {
            get
            {
                return _Pseudo;
            }

            set
            {
                _Pseudo = value;
                RaisePropertyChanged("Pseudo");
            }
        }

        public string Mail
        {
            get
            {
                return _Mail;
            }

            set
            {
                _Mail = value;
                RaisePropertyChanged("Mail");
            }
        }

        public string GraduationYear
        {
            get
            {
                return _GraduationYear;
            }

            set
            {
                _GraduationYear = value;
                RaisePropertyChanged("GraduationYear");
            }
        }
        
        public RelayCommand CreatePlayerCommand { get; set; }
        public RelayCommand ClosingCommand { get; set; }

        public CreatePlayerViewModel()
        {
            MyDatabaseContext = new SavingContext();
            InitCommands();
        }
        public CreatePlayerViewModel(SavingContext savingContext)
        {
            MyDatabaseContext = savingContext;
            InitCommands();
        }
        private void InitCommands()
        {
            CreatePlayerCommand = new RelayCommand(CreatePlayer);
            ClosingCommand = new RelayCommand(Closing);
        }
        private void CreatePlayer(object obj)
        {
            if (CheckParameters())
            {
                try
                {
                    Student newPlayer = new Student(Mail, FirstName, LastName, Pseudo, Convert.ToInt32(GraduationYear));
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
                    MyDatabaseContext.SaveChanges();
                    MessageBox.Show("Player successfully created !");
                    CleanFields();
                }
                catch (RiotApi.Net.RestClient.Helpers.RiotExceptionRaiser.RiotApiException ex)
                {
                    if (ex.RiotErrorCode == RiotApi.Net.RestClient.Helpers.RiotExceptionRaiser.RiotErrorCode.DATA_NOT_FOUND)
                    {
                        MessageBox.Show("Data not found, please make sure the Pseudo exist");
                        return;
                    }
                    if (ex.RiotErrorCode == RiotApi.Net.RestClient.Helpers.RiotExceptionRaiser.RiotErrorCode.SERVER_ERROR)
                    {
                        MessageBox.Show("Riot Server Error, their API servers may be down, please check on their status and try again later");
                        return;
                    }
                    if (ex.RiotErrorCode == RiotApi.Net.RestClient.Helpers.RiotExceptionRaiser.RiotErrorCode.RATE_LIMITED)
                    {
                        MessageBox.Show("You have reached the limit of your API key rate, slow down on the usage !");
                        return;
                    }
                    MessageBox.Show($"Unknown error from Riot Server : {ex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while saving the data : {ex.Message}");
                }
            }

        }
        private bool CheckParameters()
        {
            bool isValid = true;
            if (!Mail.Contains('@') || !Mail.Contains('.'))
            {
                MessageBox.Show("Invalid Mail");
                isValid = false;
            }
            try
            {
                int gradYear = Convert.ToInt32(GraduationYear);
                //if(gradYear < 2010 || gradYear > 2100)
                //{
                //    isValid = false;
                //    MessageBox.Show("Invalid Graduation Year");
                //}
            }
            catch (Exception)
            {
                isValid = false;
                MessageBox.Show("Invalid Graduation Year");
            }
            return isValid;
        }
        private void CleanFields()
        {
            Mail = "";
            Pseudo = "";
            GraduationYear = "";
            FirstName = "";
            LastName = "";
        }
        private void Closing(object obj)
        {

        }
    }
}
