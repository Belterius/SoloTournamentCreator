﻿using SoloTournamentCreator.Helper;
using SoloTournamentCreator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    return SelectedMatch.LeftContendant.Winner.TeamName;
                return String.Empty;
            }
        }
        public string TeamTwoName
        {
            get
            {
                if(SelectedMatch != null)
                    return SelectedMatch.RightContendant.Winner.TeamName;
                return String.Empty;
            }
        }
        public List<Team> PossibleWinners
        {
            get
            {
                if (SelectedMatch != null)
                    return new List<Team>{SelectedMatch.LeftContendant.Winner, SelectedMatch.RightContendant.Winner};
                return new List<Team>();
            }
        }
        private int _FirstScore;
        private int _SecondScore;
        public RelayCommand ClosingCommand { get; set; }
        public RelayCommand SelectWinnerCommand { get; set; }
        public RelayCommand ConfirmMatchResultCommand { get; set; }

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
                FirstScore = 0;
                SecondScore = 0;
                SelectedMatch = null;
                MyDataContext.SaveChanges();
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
}
