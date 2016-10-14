using RiotApi.Net.RestClient;
using RiotApi.Net.RestClient.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloTournamentCreator.RiotToEntity
{
    public sealed class MyRiotClient
    {
        public IRiotClient riotClient = new RiotClient(Properties.Settings.Default.RiotApiKey);
        static readonly MyRiotClient INSTANCE = new MyRiotClient();

        private MyRiotClient()
        {

        }

        public static MyRiotClient Instance
        {
            get
            {
                return INSTANCE;
            }
        }
    }
    
}
