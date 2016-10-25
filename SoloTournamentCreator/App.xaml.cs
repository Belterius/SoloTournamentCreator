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

            if (!ApiRequest.ApiKeyIsValid(SoloTournamentCreator.Properties.Settings.Default.RiotApiKey))
            {
                while(ShowMyDialogBox() == "")
                {
                }
                MainMenu MW = new MainMenu();
                MW.Show();
            }else
            {
                this.StartupUri = new Uri("View/MainMenu.xaml", UriKind.Relative);
                //this.StartupUri = new Uri("View/MainWindow.xaml", UriKind.Relative);
            }
        }

        private string ShowMyDialogBox()
        {
            DialogBox TestDialog = new DialogBox("API Key Required", "Please enter a Valid Riot API Key");

            if (TestDialog.ShowDialog() == true)
            {
                if (ApiRequest.ApiKeyIsValid(TestDialog.GetInput))
                {
                    SoloTournamentCreator.Properties.Settings.Default.RiotApiKey = TestDialog.GetInput;
                    SoloTournamentCreator.Properties.Settings.Default.Save();
                    return TestDialog.GetInput;
                }
                else
                {
                    MessageBox.Show("Invalid Api Key");
                    return "";
                }
            }
            else
            {
                return "";
            }

        }
    }
}

