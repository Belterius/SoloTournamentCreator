using SoloTournamentCreator.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SoloTournamentCreator.Helper
{
    public class MultipleValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class TournamentInformation : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Tournament myTournament = (Tournament) value;
            return myTournament.Name + " (" + myTournament.nbParticipant + "/" + myTournament.nbParticipantMax +")";
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        
    }
    public class PlayerToTournamentRegistered : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(values[0] is Student) || !(values[1] is Tournament))
            {
                return false;
            }
            Tournament myTournament = (Tournament)values[1];
            Student myPlayer = (Student)values[0];
            if (myTournament.Participants.Contains(myPlayer))
                return true;
            return false;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public static class GlobalConverters
    {

        public static int RankingToPoint(RiotApi.Net.RestClient.Helpers.Enums.Tier tier, string division)
        {
            int strenght = 0;
            switch (tier)
            {
                case RiotApi.Net.RestClient.Helpers.Enums.Tier.BRONZE:
                    break;
                case RiotApi.Net.RestClient.Helpers.Enums.Tier.SILVER:
                    strenght += 5;
                    break;
                case RiotApi.Net.RestClient.Helpers.Enums.Tier.GOLD:
                    strenght += 10;
                    break;
                case RiotApi.Net.RestClient.Helpers.Enums.Tier.PLATINUM:
                    strenght += 15;
                    break;
                case RiotApi.Net.RestClient.Helpers.Enums.Tier.DIAMOND:
                    strenght += 20;
                    break;
                case RiotApi.Net.RestClient.Helpers.Enums.Tier.MASTER:
                    strenght += 25;
                    break;
                case RiotApi.Net.RestClient.Helpers.Enums.Tier.CHALLENGER:
                    strenght += 30;
                    break;
                default:
                    throw new NotSupportedException("This Tier is not supported, Rito added a new one :(");
            }
            switch (division)
            {
                case "V":
                    break;
                case "IV":
                    strenght += 1;
                    break;
                case "III":
                    strenght += 2;
                    break;
                case "II":
                    strenght += 3;
                    break;
                case "I":
                    strenght += 4;
                    break;
                default:
                    throw new NotSupportedException("This Division is not supported, Rito added a new one :(");

            }
            return strenght;

        }
    }
}
