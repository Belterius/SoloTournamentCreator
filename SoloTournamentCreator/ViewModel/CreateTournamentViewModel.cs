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
        public RelayCommand CreateTournamentCommand { get; set; }

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

        public CreateTournamentViewModel()
        {
            MyDatabaseContext = new SavingContext();
            CreateTournamentCommand = new RelayCommand(CreateTournament);
        }
        public CreateTournamentViewModel(SavingContext savingContext)
        {
            MyDatabaseContext = savingContext;
            CreateTournamentCommand = new RelayCommand(CreateTournament);
        }
        private void CreateTournament(object obj)
        {
            Tournament newTournament = new Tournament(TournamentName, Convert.ToInt32(((ComboBoxItem)obj).Content));
            MyDatabaseContext.MyTournaments.Add(newTournament);
            MyDatabaseContext.SaveChanges();
            this.CloseWindow();

        }
    }
}
