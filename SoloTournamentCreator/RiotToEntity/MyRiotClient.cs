namespace SoloTournamentCreator.RiotToEntity
{
    public sealed class MyRiotClient
    {
        public RiotSharp.RiotApi riotSharpClient = RiotSharp.RiotApi.GetInstance(Properties.Settings.Default.RiotApiKey, 200, 500);
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
