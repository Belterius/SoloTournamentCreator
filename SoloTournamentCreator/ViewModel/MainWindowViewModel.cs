using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotApi.Net.RestClient;
using RiotApi.Net.RestClient.Configuration;
using SoloTournamentCreator.Helper;
using SoloTournamentCreator.Model;

namespace SoloTournamentCreator.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {

        public RelayCommand CreatePlayerCommand { get; set; }
        public MainWindowViewModel()
        {
            CreatePlayerCommand = new RelayCommand(CreatePlayer);
        }

        public void CreatePlayer(object obj)
        {
            using (var db = new SavingContext())
            {
                Tournament t = new Tournament();
                Student std = new Student("mail", "fname", "lname", "Belterius", 1998);
                t.Register(std);
                db.MyTournaments.Add(new Tournament());
                db.SaveChanges();
            }
        }
    }
}
