using RiotApi.Net.RestClient;
using RiotApi.Net.RestClient.Configuration;
using SoloTournamentCreator.View;
using SoloTournamentCreator.RiotToEntity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SoloTournamentCreator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            if ( SoloTournamentCreator.Properties.Settings.Default.AdminRight && !ApiRequest.ApiKeyIsValid(SoloTournamentCreator.Properties.Settings.Default.RiotApiKey))
            {
                SetKeyAPI();
            }
                this.StartupUri = new Uri("View/MainMenu.xaml", UriKind.Relative);
        }

        private string SetKeyAPI()
        {
            DialogBox APIKeyDialog = new DialogBox("API Key Required", "Please enter a Valid Riot API Key");

            if (APIKeyDialog.ShowDialog() == true)
            {
                if (ApiRequest.ApiKeyIsValid(APIKeyDialog.GetInput))
                {
                    SoloTournamentCreator.Properties.Settings.Default.RiotApiKey = APIKeyDialog.GetInput;
                    SoloTournamentCreator.Properties.Settings.Default.Save();
                    return APIKeyDialog.GetInput;
                }
                else
                {
                    MessageBox.Show("Invalid Api Key");
                    return SetKeyAPI();
                }
            }
            else
            {
                return "";
            }

        }
    }
}

