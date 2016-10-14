using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SoloTournamentCreator.Helper
{
    public class Converters
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

        public class SummonerLeagueToValue : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (value == null) return null;
                return new List<object>() { value };
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
