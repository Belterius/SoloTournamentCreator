using SoloTournamentCreator.Helper;
using SoloTournamentCreator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloTournamentCreator.ViewModel
{
    public class TournamentBracketViewModel : BaseViewModel
    {
        Tournament _MyTournament;
        SavingContext MyDataContext;

        public Tournament SelectedTournament
        {
            get
            {
                return _MyTournament;
            }

            set
            {
                _MyTournament = value;
                RaisePropertyChanged("MyTournament");
            }
        }
        public RelayCommand ClosingCommand { get; set; }
        public RelayCommand SelectWinner { get; set; }
        public TournamentBracketViewModel()
        {
            MyDataContext = new SavingContext();
            InitCommand();
        }
        public TournamentBracketViewModel(SavingContext dataContext, Tournament selectedTournament)
        {
            MyDataContext = dataContext;
            //TODO : Check if it works or if I need to get the tournament again from DataContext
            SelectedTournament = selectedTournament;
            InitCommand();
        }
        private void InitCommand()
        {
            ClosingCommand = new RelayCommand(Closing);
            SelectWinner = new RelayCommand(SelectionWinner);
        }
        private void Closing(object obj)
        {

        }
        private void SelectionWinner(object obj)
        {

        }
    }
}
