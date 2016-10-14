using RiotApi.Net.RestClient;
using RiotApi.Net.RestClient.Configuration;
using SoloTournamentCreator.RiotToEntity;
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
using System.Windows.Shapes;

namespace SoloTournamentCreator.View
{
    /// <summary>
    /// Interaction logic for DialogBox.xaml
    /// </summary>
    public partial class DialogBox : Window
    {
        public DialogBox()
        {
            InitializeComponent();
        }
        public DialogBox(string title, string content)
        {
            InitializeComponent();
            this.Title = title;
            labelInformation.Content = content;
        }
        private void buttonConfirm_Click(object sender, RoutedEventArgs e)
        {
                DialogResult = true;
        }
        
        public string GetInput
        {
            get { return textBoxInput.Text; }
        }
    }
}
