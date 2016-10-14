using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SoloTournamentCreator.RiotToEntity;
using SoloTournamentCreator.Model;

namespace SoloTournamentCreator.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                Team team = new Team(teamName:"1");
                Team team2 = new Team(teamName:"2");
                Team team3 = new Team(teamName:"3");
                Team team4 = new Team(teamName:"4");
                Team team5 = new Team(teamName: "5");
                Team team6 = new Team(teamName: "6");
                Team team7 = new Team(teamName: "7");
                Team team8 = new Team(teamName: "8");
                Tournament t = new Tournament();
                t.Teams.Add(team);
                t.Teams.Add(team2);
                t.Teams.Add(team3);
                t.Teams.Add(team4);
                t.Teams.Add(team5);
                t.Teams.Add(team6);
                t.Teams.Add(team7);
                t.Teams.Add(team8);
                TournamentTree tt = new TournamentTree(t);
                //Student std = new Student("mail", "fname", "lname", "lörth", 1998);
                //t.Register(std);
                //Student std2 = new Student("mail", "fname", "lname", "Belterius", 1998);
                //t.Register(std2);
                //Student std3 = new Student("mail", "fname", "lname", "Kindermoumoute", 1998);
                //t.Register(std3);
                //Student std4 = new Student("mail", "fname", "lname", "Ninochin", 1998);
                //t.Register(std4);
                //t.Start();
            }
            catch (Exception)
            {

                MessageBox.Show("WrongSumName");
            }
        }
    }
}
