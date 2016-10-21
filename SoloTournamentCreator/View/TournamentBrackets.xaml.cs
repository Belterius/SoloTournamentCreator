using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using SoloTournamentCreator.Model;
using SoloTournamentCreator.Helper;

namespace SoloTournamentCreator.View
{
    /// <summary>
    /// Interaction logic for TournamentBrackets.xaml
    /// </summary>
    public partial class TournamentBrackets : UserControl
    {
        public Tournament SelectedTournament
        {
            get {
                return (Tournament)GetValue(SelectedTournamentProperty);
            }
            set {
                SetValue(SelectedTournamentProperty, value);
            }
        }
        public int VerticalSize
        {
            get
            {
                if(SelectedTournament != null)
                    return 30 * SelectedTournament.NbTeam;
                return 30 * 16;
            }
        }
        public int HorizontalSize
        {
            get
            {
                if (SelectedTournament != null)
                    return Convert.ToInt32((Math.Log(SelectedTournament.NbTeam, 2) + 1) * bracketwidth + 50);
                return Convert.ToInt32((Math.Log(16, 2) + 1) * bracketwidth + 50);
            }
        }
        public RelayCommand SelectWinner
        {
            get {
                return (RelayCommand)GetValue(SelectWinnerProperty);
            }
            set {
                SetValue(SelectWinnerProperty, value);
            }
        }
        public static readonly DependencyProperty SelectedTournamentProperty =
            DependencyProperty.Register(
                "SelectedTournament",
                typeof(Tournament),
                typeof(TournamentBrackets),
                new UIPropertyMetadata(null, new PropertyChangedCallback(OnSelectedTournamentUpdated))
                );

        private static void OnSelectedTournamentUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue != null) //cf TournamentBracketViewModel.ConfirmMatchResult
                ((TournamentBrackets)d).UpdateBrackets();
        }

        public static readonly DependencyProperty SelectWinnerProperty =
           DependencyProperty.Register(
               "SelectWinner",
               typeof(RelayCommand),
               typeof(TournamentBrackets),
               new UIPropertyMetadata(null)
               );

        public TournamentBrackets()
        {
            InitializeComponent();
        }

        private void UpdateBrackets()
        {
            Brackets.Children.Clear();
            if (SelectedTournament != null)
            {
                AddBracket(SelectedTournament.MyTournamentTree.MyTournamentTree, (Math.Log(SelectedTournament.NbTeam, 2) + 1) * bracketwidth, 0, Colors.Silver);
            }
        }


        const double bracketwidth = 110;
        struct BracketLocation
        {
            public double Height { get; set; }
            public double Slotloc { get; set; }
        }
        private BracketLocation AddBracket(Match slot, double right, double top, Color color)
        {
            Brush brush, border;
            if (slot.Winner != null)
            {
                brush = new SolidColorBrush(Color.FromArgb(255, color.R, color.G, color.B));
                border = Brushes.Black;
            }
            else
            {
                brush = new SolidColorBrush(Color.FromArgb(48, color.R, color.G, color.B)); ;
                border = null;
            }

            if (slot.RightContendant != null && slot.LeftContendant != null)
            {
                BracketLocation above = AddBracket(slot.LeftContendant, right - bracketwidth, top, Color.FromArgb(255, 255, 128, 128));
                BracketLocation below = AddBracket(slot.RightContendant, right - bracketwidth, top + above.Height + 10, Color.FromArgb(255, 96, 128, 255));
                double loc = (above.Slotloc + below.Slotloc + above.Height + 10) / 2;

                AddBracketSlot(slot, right, top + loc - 10, brush, border);
                Brackets.Children.Add(new Line
                {
                    X1 = right - bracketwidth,
                    X2 = right - bracketwidth,
                    Y1 = top + above.Slotloc - 10,
                    Y2 = top + above.Height + below.Slotloc + 20,
                    Stroke = Brushes.Black
                });

                return new BracketLocation { Height = above.Height + below.Height + 10, Slotloc = loc };
            }
            else
            {
                AddBracketSlot(slot, right, top, brush, border);
                return new BracketLocation { Height = 20, Slotloc = 10 };
            }
        }
        private double AddBracketSlot(Match slot, double right, double top, Brush color, Brush border)
        {
            ContentPresenter presenter = new ContentPresenter
            {
                Content = slot,
                ContentTemplate = (DataTemplate)Resources["MatchTemplate"],
                Width = bracketwidth,
                Height = 20,
            };
            presenter.SetValue(Canvas.LeftProperty, right - bracketwidth);
            presenter.SetValue(Canvas.TopProperty, top);
            Brackets.Children.Add(presenter);

            return presenter.Height;
        }

    }
}
