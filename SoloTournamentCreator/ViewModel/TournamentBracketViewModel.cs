using SoloTournamentCreator.Helper;
using SoloTournamentCreator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SoloTournamentCreator.ViewModel
{
    public class TournamentBracketViewModel : BaseViewModel
    {
        Tournament _SelectedTournament;
        Match _SelectedMatch;
        SavingContext MyDataContext;

        public System.Windows.Visibility DisplaySelector
        {
            get
            {
                if(SelectedMatch != null)
                    return System.Windows.Visibility.Visible;
                return System.Windows.Visibility.Hidden;

            }
        }
        public System.Windows.Visibility FinalStageLaunch
        {
            get
            {
                if (SelectedTournament!= null && CanStartFinalStage())
                    return System.Windows.Visibility.Visible;
                return System.Windows.Visibility.Hidden;

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
                RaisePropertyChanged("FinalStageLaunch");
            }
        }
        public Match SelectedMatch
        {
            get
            {
                return _SelectedMatch;
            }

            set
            {
                _SelectedMatch = value;
                RaisePropertyChanged("SelectedMatch");
                RaisePropertyChanged("TeamOneName");
                RaisePropertyChanged("TeamTwoName");
                RaisePropertyChanged("PossibleWinners");
                RaisePropertyChanged("DisplaySelector");
            }
        }
        public string TeamOneName
        {
            get
            {
                if (SelectedMatch != null)
                    return SelectedMatch.LeftContendant.Winner?.TeamName;
                return String.Empty;
            }
        }
        public string TeamTwoName
        {
            get
            {
                if(SelectedMatch != null)
                    return SelectedMatch.RightContendant.Winner?.TeamName;
                return String.Empty;
            }
        }
        public List<Team> PossibleWinners
        {
            get
            {
                if (SelectedMatch != null)
                {
                    var list = new List<Team> { SelectedMatch.LeftContendant.Winner, SelectedMatch.RightContendant.Winner};
                    list.RemoveAll(item => item == null);
                    return list;
                }
                return new List<Team>();
            }
        }

        private int _FirstScore;
        private int _SecondScore;
        public int FirstScore
        {
            get
            {
                return _FirstScore;
            }

            set
            {
                _FirstScore = value;
                RaisePropertyChanged("FirstScore");
            }
        }

        public int SecondScore
        {
            get
            {
                return _SecondScore;
            }

            set
            {
                _SecondScore = value;
                RaisePropertyChanged("SecondScore");
            }
        }
        public System.Windows.Visibility AdminOnlyVisible
        {
            get
            {
                if (Properties.Settings.Default.AdminRight)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Hidden;
                }
            }
        }
        public RelayCommand ClosingCommand { get; set; }
        public RelayCommand SelectWinnerCommand { get; set; }
        public RelayCommand ConfirmMatchResultCommand { get; set; }
        public RelayCommand StartFinalStageCommand { get; set; }
        public TournamentBracketViewModel()
        {
            MyDataContext = new SavingContext();
            InitCommand();
        }
        public TournamentBracketViewModel(SavingContext dataContext, Tournament selectedTournament)
        {
            MyDataContext = dataContext;
            SelectedTournament = selectedTournament;
            InitCommand();
        }
        private void InitCommand()
        {
            ClosingCommand = new RelayCommand(Closing);
            SelectWinnerCommand = new RelayCommand(SelectWinner);
            ConfirmMatchResultCommand = new RelayCommand(ConfirmMatchResult);
            StartFinalStageCommand = new RelayCommand(StartFinalStage);
        }
        private bool CanStartFinalStage()
        {
            if (!Properties.Settings.Default.AdminRight)
                return false;
            if (!SelectedTournament.HasLoserBracket)
                return false;
            if (SelectedTournament.MyTournamentTree.MySecondaryTournamentTree == null)
                return false;
            if (SelectedTournament.MyTournamentTree.MyMainTournamentTree.Winner == null)
                return false;
            if (SelectedTournament.MyTournamentTree.MySecondaryTournamentTree.Winner == null)
                return false;
            return true;
        }
        private void StartFinalStage(object obj)
        {
            
            if(MessageBox.Show("Once the final stage has started, you won't be able to do any modification to the previous result, are you sure you want to continue ?", "WARNING", MessageBoxButton.OKCancel) == MessageBoxResult.OK && CanStartFinalStage())
            {
                SelectedTournament.StartFinalStage();
                var tempo = SelectedTournament;
                SelectedTournament = null;
                SelectedTournament = tempo;
                RefreshUserControl();
                RaisePropertyChanged("SelectedTournament");
            }
            
        }

        private void Closing(object obj)
        {

        }
        private void SelectWinner(object obj)
        {
            SelectedMatch = (Match)obj;
            FirstScore = SelectedMatch.WinnerScore;
            SecondScore = SelectedMatch.LoserScore;
        }
        private void ConfirmMatchResult(object obj)
        {
            if(obj != null)
            {
                SelectedMatch.Winner = (Team)obj;
                if(SelectedMatch.LeftContendant.Winner == SelectedMatch.Winner)
                {
                    SelectedMatch.LeftContendant.WinNext = true;
                    SelectedMatch.RightContendant.WinNext = false;
                }else
                {
                    SelectedMatch.LeftContendant.WinNext = false;
                    SelectedMatch.RightContendant.WinNext = true;
                }
                SelectedMatch.WinnerScore = Math.Max(FirstScore, SecondScore);
                SelectedMatch.LoserScore = Math.Min(FirstScore, SecondScore);
                SelectedTournament.MyTournamentTree.UpdateSecondaryBracket(SelectedMatch);
                FirstScore = 0;
                SecondScore = 0;
                SelectedMatch = null;
                MyDataContext.SaveChanges();
                RefreshUserControl();
            }
        }

        private void RefreshUserControl()
        {
            /*
                 * I could NOT figure why raising PropertyChanged SelectedTournament does update the SelectedTournament Property from my User Control (when debuging I see the new values if I raiseProperty, I'm not if I don't)
                 * BUT it would NOT fire my OnSelectedTournamentUpdated PropertyChangedCallBack
                 * after lots of try and try, decided to do the dirty trick, I set it to null and back, to fire my event
                 * In the event I handdle that I do nothing if my DependencyPropertyChangedEventArgs is null
                 * It's ugly but it works
                 * Edit : After more reading it seems that it doesn't fire because even if I raise my PropertyChanged, there's an additional check on my property, and as my SelectedTournament is still the same, it doesn't trigger
                 * Cf : http://stackoverflow.com/questions/31990339/propertychangedcallback-not-called for more information
                 * TODO : Remove the trick and implement the full proper way
                 */
            var tempo = SelectedTournament;
            SelectedTournament = null;
            SelectedTournament = tempo;
            RaisePropertyChanged("SelectedTournament");
        }
    }
}
