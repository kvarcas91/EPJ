using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace EPJ
{
    public class DataBase
    {

        public static void InsertContributor (IContributor contributor)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Execute("insert into contributors (FirstName, LastName) values (@FirstName, @LastName)", contributor);
                connection.Dispose();
            }
            
        }

        public static void AssignContributors (long pID, long cID)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Execute("INSERT INTO project_contributors (projectID, contributorID) values (@projectID, @contributorID)", 
                    new { projectID = pID, contributorID = cID});
                connection.Dispose();
            }
        }
    
        private static int GetLastRowID()
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                var count = connection.ExecuteScalar<int>("select seq from sqlite_sequence where name='projects'");
                //var count = connection.Query("SELECT COUNT(*) FROM projects;");
                connection.Dispose();
                return count;
            }
        }

        public static void InsertProject (IProject project)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                
                connection.Execute("insert into projects (Title, Description, Date, DueDate, ProjectPath, Priority) " +
                                   $"values (@Title, @Description, @Date, @DueDate, @ProjectPath, @Priority)", project);
                project.ID = GetLastRowID();
                Console.WriteLine($"ID: {project.ID }");
                connection.Dispose();

                foreach (var contributor in project.Contributors)
                {
                    AssignContributors(project.ID, contributor.Id);
                }
            }
        }

        public static List<Contributor> GetContributors()
        {

            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                var output = connection.Query<Contributor>("SELECT * FROM contributors;");
                connection.Dispose();
                return output.ToList();
            }
        }

        public static List<Contributor> GetProjectContributors(long projectID)
        {

            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                var output = connection.Query<Contributor>(
                    "SELECT c.Id, c.FirstName, c.LastName " +
                    "FROM contributors c " +
                    "INNER JOIN project_contributors p ON p.contributorID = c.Id " +
                    $"INNER JOIN projects pr on pr.ID = p.projectID WHERE pr.Id = {projectID}");
                connection.Dispose();
                return output.ToList();
            }    
        }

        public static List<Project> GetProjects()
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                var output = connection.Query<Project>("select * from projects");
                foreach (var project in output)
                {
                    project.AddContributors(GetProjectContributors(project.ID));
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

