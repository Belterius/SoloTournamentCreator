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

        public CreatePlayerViewModel()
        {
            MyDatabaseContext = new SavingContext();
            CreatePlayerCommand = new RelayCommand(CreatePlayer);
        }
        public CreatePlayerViewModel(SavingContext savingContext)
        {
            MyDatabaseContext = savingContext;
            CreatePlayerCommand = new RelayCommand(CreatePlayer);
        }

        private void CreatePlayer(object obj)
        {
            if (CheckParameters())
            {
                try
                {
                    Student newPlayer = new Student(Mail, FirstName, LastName, Pseudo, Convert.ToInt32(GraduationYear));
                    MyDatabaseContext.MyStudents.Add(newPlayer);
                    MyDatabaseContext.SaveChanges();
                    MessageBox.Show("Player successfully created !");
                    CleanFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving Summoner informations, please check Pseudo");
                }
            }

        }
        private bool CheckParameters()
        {
            bool isValid = true;
            if (!Mail.Contains('@'))
            {
                MessageBox.Show("Invalid Mail");
                isValid = false;
            }
            try
            {
                int gradYear = Convert.ToInt32(GraduationYear);
                if(gradYear < 2010 || gradYear > 2100)
                {
                    isValid = false;
                    MessageBox.Show("Invalid Graduation Year");
                }
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
    }
}
