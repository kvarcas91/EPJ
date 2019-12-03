using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
    public class DataBase
    {

        public static void InsertContributor(IContributor contributor)
        {
           
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Execute("insert into contributor (FirstName, LastName) values (@FirstName, @LastName)", contributor);
            }
            
        }

        public static List<Contributor> GetContributors()
        {

            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                var output = connection.Query<Contributor>("select * from contributor", new DynamicParameters());
                connection.Dispose();
                return output.ToList();
            }

           
        }

        public static void GetProjects ()
        {
            List<IProject> projects = new List<IProject>();
               
        }

        private static string GetConnectionString (string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}

