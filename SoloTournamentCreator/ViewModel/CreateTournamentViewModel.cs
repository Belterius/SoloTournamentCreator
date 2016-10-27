using SoloTournamentCreator.Helper;
using SoloTournamentCreator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SoloTournamentCreator.ViewModel
{
    public class CreateTournamentViewModel : BaseViewModel
    {
        SavingContext MyDatabaseContext;
        string _TournamentName;
        bool _HasLoserBracket;
        public RelayCommand CreateTournamentCommand { get; set; }
        public RelayCommand ClosingCommand { get; set; }

        public string TournamentName
        {
            get
            {
                return _TournamentName;
            }

            set
            {
                _TournamentName = value;
                RaisePropertyChanged("TournamentName");
            }
        }

        public bool HasLoserBracket
        {
            get
            {
                return _HasLoserBracket;
            }

            set
            {
                _HasLoserBracket = value;
                RaisePropertyChanged("HasLoserBracket");
            }
        }

        public CreateTournamentViewModel()
        {
            MyDatabaseContext = new SavingContext();
            InitCommands();
        }
        public CreateTournamentViewModel(SavingContext savingContext)
        {
            MyDatabaseContext = savingContext;
            InitCommands();
        }
        private void InitCommands()
        {
            CreateTournamentCommand = new RelayCommand(CreateTournament);
            ClosingCommand = new RelayCommand(Closing);

        }
        private void CreateTournament(object obj)
        {
            Tournament newTournament = new Tournament(TournamentName, Convert.ToInt32(((ComboBoxItem)obj).Content), HasLoserBracket);
            lock (MyDatabaseContext)
            {
                MyDatabaseContext.MyTournaments.Add(newTournament);
                MyDatabaseContext.SaveChanges();
            }
            this.CloseWindow();

        }
        private void Closing(object obj)
        {

        }
    }
}
