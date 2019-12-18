﻿using Dapper;
using Dapper.Contrib.Extensions;
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
        #region Project

        public static void InsertProject(IProject project)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {

                connection.Execute("insert into projects (Title, Description, Date, DueDate, ProjectPath, Priority) " +
                                   $"values (@Title, @Description, @Date, @DueDate, @ProjectPath, @Priority)", project);
                project.ID = GetLastRowID("projects");
                Console.WriteLine($"ID: {project.ID }");
                connection.Dispose();

                foreach (var contributor in project.Contributors)
                {
                    AssignContributors(project.ID, contributor.Id);
                }
            }
        }

        public static void UpdateProject(Project project)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Update(project);
                connection.Dispose();
            }
        }


        public static List<Project> GetProjects()
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                var output = connection.Query<Project>("select * from projects");
                foreach (var project in output)
                {
                    project.Progress = GetProjectprogress(project.ID);
                    project.AddContributors(GetProjectContributors(project.ID));
                }
                connection.Dispose();
                return output.ToList();
            }
        }

        private static double GetProjectprogress(long projectID)
        {
            var ProjectTasks = GetProjectTasks(projectID);
            var totalTaskCount = ProjectTasks.Count;
            if (totalTaskCount == 0) return 0;
            var completedTaskCount = 0;
            foreach (var task in ProjectTasks)
            {
                if (task.IsCompleted)
                {
                    completedTaskCount++;
                }
            }
            return (completedTaskCount * 100) / totalTaskCount;
        }

        #endregion

        #region Contributors

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

        public static void InsertContributor(IContributor contributor)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                var sql = @"insert into contributors (FirstName, LastName) values (@FirstName, @LastName)";
                connection.Execute(sql,
                                new
                                {
                                    contributor.FirstName,
                                    contributor.LastName,

                                });

                connection.Dispose();
                //return newContributor;
            }
        }

        public static void AssignContributors(long pID, long cID)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Execute("INSERT INTO project_contributors (projectID, contributorID) values (@projectID, @contributorID)",
                    new { projectID = pID, contributorID = cID });
                connection.Dispose();
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

        #endregion

        #region Tasks

        public static List<Task> GetProjectTasks(long projectID)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                var output = connection.Query<Task>(
                    "SELECT t.ID, t.Content, t.IsCompleted, t.Priority, t.DueDate " +
                    "FROM tasks t " +
                    "INNER JOIN project_tasks p ON p.taskID = t.ID " +
                    $"INNER JOIN projects pr on pr.ID = p.projectID WHERE pr.Id = {projectID}");
                connection.Dispose();
                return output.ToList();
            }
        }
        public static void InsertTask(ITask task, long projectID)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                var sql = @"insert into tasks (Content, Priority, IsCompleted, DueDate) 
                            values (@Content, @Priority, @IsCompleted, @DueDate)";
                connection.Execute(sql,
                                new
                                {
                                    task.Content,
                                    task.Priority,
                                    task.IsCompleted,
                                    task.DueDate
                                });

                connection.Dispose();

            }
            AssignTaskToTheProject(projectID, GetLastRowID("tasks"));
        }

        private static void AssignTaskToTheProject(long projectID, long taskID)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Execute("INSERT INTO project_tasks (projectID, taskID) values (@projectID, @taskID)",
                    new { projectID, taskID });
                connection.Dispose();
            }
        }

        public static void UpdateTask(Task task)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Update(task);
                connection.Dispose();
            }
        }

        #endregion

        #region Comments

        public static List<Note> GetProjectComments (long projectID)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                var sql = $" SELECT c.ID, c.Content, c.SubmitionDate " +
                    "FROM comments c " +
                    "INNER JOIN project_comments p ON p.commentID = c.ID " +
                    $"INNER JOIN projects pr on pr.ID = p.projectID WHERE pr.Id = {projectID}";
                var output = connection.Query<Note>(sql).ToList();
                connection.Dispose();
                return output;
            }
        }


        #endregion

        private static int GetLastRowID(string table)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                var count = connection.ExecuteScalar<int>($"select seq from sqlite_sequence where name='{table}'");
                //var count = connection.Query("SELECT COUNT(*) FROM projects;");
                connection.Dispose();
                return count;
            }
        }

        public static int GetCount (string table, string rowName, string param)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                var count = connection.ExecuteScalar<int>($"SELECT COUNT(*) FROM {table} WHERE {rowName} = '{param}'");
                //var count = connection.Query("SELECT COUNT(*) FROM projects;");
                connection.Dispose();
                return count;
            }
        }

      

    

      

        private static string GetConnectionString (string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}

