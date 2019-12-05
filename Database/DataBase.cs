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
                connection.Execute("insert into contributors (FirstName, LastName) values (@FirstName, @LastName)", contributor);
                connection.Dispose();
            }
            
        }

        public static void InsertProject (IProject project)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Execute("insert into projects (Title, Description, Date, DueDate, ProjectPath, Priority) " +
                                   $"values (@Title, @Description, @Date, @DueDate, @ProjectPath, @Priority)", project);
                connection.Dispose();
            }
        }

        public static List<Contributor> GetContributors(long projectID)
        {

            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                var output = connection.Query<Contributor>(
                    "SELECT c.Id, c.FirstName, c.LastName " +
                    "FROM contributors c " +
                    "INNER JOIN project_contributors p ON p.contributorID = c.Id " +
                    $"INNER JOIN projects pr on pr.ID = p.projectID WHERE pr.Id = {projectID}", new DynamicParameters());
                connection.Dispose();
                return output.ToList();
            }

           
        }

        public static List<Project> GetProjects()
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                var output = connection.Query<Project>("select * from projects", new DynamicParameters());
                foreach (var project in output)
                {
                    project.AddContributors(GetContributors(project.ID));
                }
                connection.Dispose();
                return output.ToList();
            }
        }

        private static string GetConnectionString (string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}

