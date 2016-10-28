using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using SoloTournamentCreator.Model;

namespace SoloTournamentCreator.Data
{
    //TODO:Check the impact of changing Database on my Local DbSet
    public static class UpdateConnection
    {
        public static bool CheckConnection(this SavingContext source)
        {
            try
            {
                source.Database.Connection.Open();
                source.Database.Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool CheckWriteRight(this SavingContext source)
        {
            Match dummyMatch = new Match(0, true);
            try
            {
                //TODO : Find a better way to check if I have write access on the database ...
                //Note : it doesn't matter if I have a dummyMatch created and can't remove it before throwing an Exception
                //because I won't be able to save it, and it won't show up anywhere as it is not linked to any tournament
                source.MyMatchs.Add(dummyMatch);
                source.SaveChanges();
                source.MyMatchs.Remove(dummyMatch);
                source.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                source.MyMatchs.Remove(dummyMatch);
                return false;
            }
        }
        // all params are optional
        public static void ChangeConnectionString(
            this SavingContext source,
            string server = "",
            string port = "",
            string database = "",
            string userId = "",
            string password = "",
            string configConnectionStringName = "")
        {
            try
            {
                if(configConnectionStringName != "")
                {
                    source.Database.Connection.ConnectionString = configConnectionStringName;
                    return;
                }
                string[] connectionFields = source.Database.Connection.ConnectionString.Split(';');
                string newConnectionString = "";
                bool passwordIsMissing = true;
                foreach(var field in connectionFields)
                {
                    switch (field.Split('=')[0])
                    {
                        case "server":
                            if(server != "")
                            {
                                newConnectionString += field.Split('=')[0] + "=" + server + ";";
                            }else
                            {
                                newConnectionString += field + ";";
                            }
                            break;
                        case "port":
                            if (port != "")
                            {
                                newConnectionString += field.Split('=')[0] + "=" + port + ";";
                            }
                            else
                            {
                                newConnectionString += field + ";";
                            }
                            break;
                        case "database":
                            if (database != "")
                            {
                                newConnectionString += field.Split('=')[0] + "=" + database + ";";
                            }
                            else
                            {
                                newConnectionString += field + ";";
                            }
                            break;
                        case "user id":
                            if (userId != "")
                            {
                                newConnectionString += field.Split('=')[0] + "=" + userId + ";";
                            }
                            else
                            {
                                newConnectionString += field + ";";
                            }
                            break;
                        case "password":
                            passwordIsMissing = false;
                            if (password != "")
                            {
                                newConnectionString += field.Split('=')[0] + "=" + password + ";";
                            }
                            else
                            {
                                newConnectionString += field + ";";
                            }
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }
                if (passwordIsMissing)
                {
                    if(password == "")
                    {
                        password = Properties.Settings.Default.Password;
                    }
                    newConnectionString += "password=" + password + ";";
                }
                source.Database.Connection.ConnectionString = newConnectionString;
            }
            catch (Exception ex)
            {
                // set log item if required
            }
        }
    }
}
