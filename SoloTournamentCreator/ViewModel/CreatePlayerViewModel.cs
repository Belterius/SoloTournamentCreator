using Microsoft.Win32;
using SoloTournamentCreator.Helper;
using SoloTournamentCreator.Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
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
        public RelayCommand ImportPlayerFromCVSCommand { get; set; }

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
            ImportPlayerFromCVSCommand = new RelayCommand(ImportPlayerFromCVS);
        }
        private void ImportPlayerFromCVS(object obj)
        {
            String sourcePath = "";
            OpenFileDialog excelPlayerData = new OpenFileDialog();
            excelPlayerData.Filter = "Excel Files (*.xlsx)|*.xlsx";
            excelPlayerData.FilterIndex = 1;
            excelPlayerData.Multiselect = false;
            if (excelPlayerData.ShowDialog() == true)
            {
                sourcePath = excelPlayerData.FileName;
            }


            string con =
                  $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={sourcePath};" +
                  @"Extended Properties='Excel 8.0;HDR=Yes;'";
            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("select * from [PlayerData$]", connection);
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                var mail = reader.GetString(1);
                                var nom = reader.GetString(2);
                                var prenom = reader.GetString(3);
                                var promotion = reader.GetString(4);
                                int promoYear;
                                var pseudo = reader.GetString(5);
                                switch (promotion)
                                {
                                    case "L1":
                                        promoYear = 2021;
                                        break;
                                    case "L2":
                                        promoYear = 2020;
                                        break;
                                    case "L3":
                                        promoYear = 2019;
                                        break;
                                    case "L4":
                                        promoYear = 2018;
                                        break;
                                    case "L5":
                                        promoYear = 2017;
                                        break;
                                    default:
                                        promoYear = 1999;
                                        break;
                                }
                                DatabaseCreatePlayer(mail, prenom, nom, pseudo, promoYear);
                            }
                            catch (Exception e)
                            {
                                break;
                            }
                            
                        }
                    }
                }
            }
        }
        private void CreatePlayer(object obj)
        {
            if (CheckParameters())
            {
                DatabaseCreatePlayer(Mail, FirstName, LastName, Pseudo, Convert.ToInt32(GraduationYear));
                
            }

        }
        private void DatabaseCreatePlayer(string mail, string firstName, string lastName, string pseudo, int gradYear)
        {
            try
            {
                Student newPlayer = new Student(mail, firstName, lastName, pseudo, gradYear);
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error while saving the data : {ex.Message}");
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
