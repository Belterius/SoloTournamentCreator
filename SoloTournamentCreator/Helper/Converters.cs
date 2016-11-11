using SoloTournamentCreator.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    public class ValueNotNull : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    public class TournamentInformation : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Tournament myTournament = (Tournament) value;
            return $"{myTournament.Name} ({myTournament.nbParticipant}/{myTournament.nbParticipantMax})";
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        
    }
    public class TournamentTeamInformation : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Tournament myTournament = (Tournament)value;
            return $"{myTournament.Name} ({myTournament.Teams.Count}/{myTournament.NbTeam})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class PlayerDisplayLevel : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Student mySummoner = (Student)value;
            if (mySummoner.SummonerSoloQueueData != null)
                return $"{mySummoner.Pseudo} ({mySummoner.SummonerSoloQueueData.Tier} {mySummoner.DetailSoloQueueData.Division} {mySummoner.DetailSoloQueueData.LeaguePoints})";
            else
                return $"{mySummoner.Pseudo} (Unranked)";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class IsTournamentWinner : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is Match) || !(values[1] is Tournament))
            {
                return false;
            }
            Match myMatch = (Match)values[0];
            Tournament myTournament = (Tournament)values[1];
            if (myTournament.TournamentWinner == null)
                return false;
            if (myTournament.TournamentWinner != myMatch.Winner)
                return false;
            if (myTournament.HasLoserBracket && myTournament.MyTournamentTree.MySecondaryTournamentTree != null)
                return false;
            return true;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class WinnerSelectionAvailable : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is Match))
            {
                return false;
            }
            if(!(values[1] is Tournament))
            {
                return false;
            }
            Tournament myTournament = (Tournament)values[1];
            Match myMatch = (Match)values[0];
            if (myTournament.Status != Tournament.TournamentStage.Started)
            {
                return false;
            }
            if (myTournament.MyTournamentTree.MySecondaryTournamentTree == null && myMatch != myTournament.MyTournamentTree.MyMainTournamentTree)//myTournament.MyTournamentTree.MySecondaryTournamentTree == null means I'm on the final stage, at that point I only allow result on the last match (the finale)
                return false;
            if (myMatch.LeftContendant?.Winner == null && myMatch.RightContendant?.Winner == null)
            {
                return false;
            }
            //I can't win against noone unless it's the first round, because there's only during the first round that there may be a lack of opponent, if the tournament wasn't full
            //If it's the mainTournament, I never allow to win against noone, because I give the "free win" on tournament creation, if it doesn't have a loserBracket, same, I don't allow a free win (that would mean I only have 3 team ..)
            if ((myMatch.RightContendant?.Winner == null || myMatch.LeftContendant?.Winner == null) && (myMatch.IsMainMatch || !myTournament.HasLoserBracket || (myMatch.LeftContendant?.LeftContendant?.Winner != null || myMatch.LeftContendant?.RightContendant?.Winner != null)))
                return false;
            return true;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class IsTournamentWinningTeam : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is Tournament) || !(values[1] is Team))
            {
                return System.Windows.Media.Brushes.Transparent;
            }
            Tournament myTournament = (Tournament)values[0];
            Team myTeam = (Team)values[1];
            if (myTournament.TournamentWinner == myTeam)
                return System.Windows.Media.Brushes.Gold;
            return System.Windows.Media.Brushes.Transparent;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class PlayerSwapSelectedDisplay : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is Student) || !(values[1] is Student))
            {
                return System.Windows.Media.Brushes.Transparent;
            }
            Student swapingPlayer = (Student)values[0];
            Student myPlayer = (Student)values[1];
            if (swapingPlayer == myPlayer)
                return System.Windows.Media.Brushes.DarkSalmon;
            return System.Windows.Media.Brushes.Transparent;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class TeamSwapSelectedDisplay : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is Team) || !(values[1] is Team))
            {
                return System.Windows.Media.Brushes.Transparent;
            }
            Team swapingTeam = (Team)values[0];
            Team myTeam = (Team)values[1];
            if (swapingTeam == myTeam)
                return System.Windows.Media.Brushes.DarkSalmon;
            return System.Windows.Media.Brushes.Transparent;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class InternalListBoxItemClick : IMultiValueConverter
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

        public static int RankingToPoint(RiotApi.Net.RestClient.Helpers.Enums.Tier tier, string division, int leaguePoints)
        {
            int strenght = 0;
            switch (tier)
            {
                case RiotApi.Net.RestClient.Helpers.Enums.Tier.BRONZE:
                    break;
                case RiotApi.Net.RestClient.Helpers.Enums.Tier.SILVER:
                    strenght += 50;
                    break;
                case RiotApi.Net.RestClient.Helpers.Enums.Tier.GOLD:
                    strenght += 100;
                    break;
                case RiotApi.Net.RestClient.Helpers.Enums.Tier.PLATINUM:
                    strenght += 150;
                    break;
                case RiotApi.Net.RestClient.Helpers.Enums.Tier.DIAMOND:
                    strenght += 200;
                    break;
                case RiotApi.Net.RestClient.Helpers.Enums.Tier.MASTER:
                    strenght += 250;
                    break;
                case RiotApi.Net.RestClient.Helpers.Enums.Tier.CHALLENGER:
                    strenght += 300;
                    break;
                default:
                    throw new NotSupportedException("This Tier is not supported, Rito added a new one :(");
            }
            switch (division)
            {
                case "V":
                    break;
                case "IV":
                    strenght += 10;
                    break;
                case "III":
                    strenght += 20;
                    break;
                case "II":
                    strenght += 30;
                    break;
                case "I":
                    strenght += 40;
                    break;
                default:
                    throw new NotSupportedException("This Division is not supported, Rito added a new one :(");

            }
            strenght += Convert.ToInt32(leaguePoints/10);
            return strenght;

        }
    }
}
