using Dapper;
using Dapper.Contrib.Extensions;
using EPJ.Models.Comments;
using EPJ.Models.Interfaces;
using EPJ.Models.Person;
using EPJ.Models.Project;
using EPJ.Models.Task;
using EPJ.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            connection.Execute("insert into projects (Header, Content, SubmitionDate, DueDate, Path, Priority, IsArchived) " +
                               $"values (@Header, @Content, @SubmitionDate, @DueDate, @Path, @Priority, @IsArchived)", project);
            project.ID = GetLastRowID("projects");
            Console.WriteLine($"ID: {project.ID }");
            connection.Dispose();

            foreach (var contributor in project.Contributors)
            {
                AssignContributors(project.ID, contributor.ID);
            }
        }

        public static void UpdateProject(Project project)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            connection.Update(project);
            connection.Dispose();
        }

        public static void DeleteProject(Project project)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            foreach (var task in project.Tasks)
            {
                DeleteTask(task);
            }

            foreach (var contributor in project.Contributors)
            {
                RemoveContributor(contributor.ID, project.ID);
            }

            connection.Delete(project);
            connection.Dispose();
        }


        public static IList<IProject> GetProjects(ViewType viewType)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            var sql = "select * from projects";
            switch (viewType)
            {
                case ViewType.Ongoing:
                    sql = $"{sql} where IsArchived = '0'";
                    break;
                case ViewType.Archived:
                    sql = $"{sql} where IsArchived = '1'";
                    break;
            }
            sql = $"{sql} order by OrderNumber";
            List<Project> output = connection.Query<Project>(sql).ToList();

            foreach (var project in output)
            {
                project.Progress = GetProjectprogress(project.ID);
                project.AddPersons(GetProjectContributors(project.ID));
                Console.WriteLine(project.ToString());
            }
            connection.Dispose();
            return output.ConvertAll(o => (IProject)o);
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

        public static IList<IPerson> GetProjectContributors(long projectID)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            List<Contributor> contributors = connection.Query<Contributor>(
                "SELECT c.ID, c.FirstName, c.LastName " +
                "FROM contributors c " +
                "INNER JOIN project_contributors p ON p.contributorID = c.ID " +
                $"INNER JOIN projects pr on pr.ID = p.projectID WHERE pr.ID = {projectID}").ToList();
            connection.Dispose();
            var output = new List<IPerson>();
            foreach (var contributor in contributors)
            {
                output.Add(contributor);
            }
            return output;
        }

        public static void InsertContributor(IContributor contributor)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            var sql = @"insert into contributors (FirstName, LastName) values (@FirstName, @LastName)";
            connection.Execute(sql,
                            new
                            {
                                contributor.FirstName,
                                contributor.LastName,
                            });

            connection.Dispose();
        }

        public static void AssignContributors(long pID, long cID)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            connection.Execute("INSERT INTO project_contributors (projectID, contributorID) values (@projectID, @contributorID)",
                new { projectID = pID, contributorID = cID });
            connection.Dispose();
        }

        public static void RemoveContributor (long projectID, long contributorID)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            string query = $"DELETE FROM project_contributors WHERE projectID = '{projectID}' AND contributorID = '{contributorID}'";
            int test = connection.Execute(query);
            connection.Dispose();
        }

        public static IList<IContributor> GetContributors()
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            List<Contributor> output = connection.Query<Contributor>("SELECT * FROM contributors;").ToList();
            connection.Dispose();
            return output.ConvertAll(o => (IContributor)o);
        }

        #endregion

        #region Tasks

        public static IList<ITask> GetProjectTasks(long projectID)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            List<Task> output = connection.Query<Task>(
                "SELECT t.ID, t.Content, t.IsCompleted, t.Priority, t.DueDate, t.OrderNumber " +
                "FROM tasks t " +
                "INNER JOIN project_tasks p ON p.taskID = t.ID " +
                $"INNER JOIN projects pr on pr.ID = p.projectID WHERE pr.ID = {projectID} order by t.OrderNumber").ToList();
            foreach (var task in output)
            {
                task.AddElements(GetSubTasks(task.ID));
                task.Progress = GetTaskProgress(task.ID);
                //task.AddSubTasks(GetSubTasks(task.ID));
            }
            connection.Dispose();
            return output.ConvertAll(o => (ITask)o);
        }

        private static double GetTaskProgress(long taskID)
        {
            var SubTasks = GetSubTasks(taskID);
            var totalSubTaskCount = SubTasks.Count;
            if (totalSubTaskCount == 0) return 0;
            var completedTaskCount = 0;
            foreach (var task in SubTasks)
            {
                if (((SubTask)task).IsCompleted)
                {
                    completedTaskCount++;
                }
            }
            return (completedTaskCount * 100) / totalSubTaskCount;
        }

        private static IList<IElement> GetSubTasks(long taskID)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            List<SubTask> tasks = connection.Query<SubTask>(
                "SELECT s.ID, s.Content, s.IsCompleted, s.Priority, s.DueDate, s.OrderNumber " +
                "FROM subtasks s " +
                "INNER JOIN task_subtasks p ON p.subtaskID = s.ID " +
                $"INNER JOIN tasks ts on ts.ID = p.taskID WHERE ts.ID = {taskID} order by s.OrderNumber").ToList();
            connection.Dispose();
            var output = new List<IElement>();
            foreach (var task in tasks)
            {
                output.Add(task);
            }
            return output;
        }

        public static void InsertTask(ITask task, long projectID)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                var order = (uint)GetCount("tasks");
                task.OrderNumber = order;
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

                task.ID = GetLastRowID("tasks");
                connection.Dispose();

            }
            AssignTaskToTheProject(projectID, GetLastRowID("tasks"));
        }

        public static void InsertSubTask(ISubTask subTask, long taskID)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                var order = (uint)GetCount("subtasks");
                subTask.OrderNumber = order;
               
                var sql = @"insert into subtasks (Content, Priority, IsCompleted, DueDate) 
                            values (@Content, @Priority, @IsCompleted, @DueDate)";
                connection.Execute(sql,
                                new
                                {
                                    subTask.Content,
                                    subTask.Priority,
                                    subTask.IsCompleted,
                                    subTask.DueDate
                                });

                subTask.ID = GetLastRowID("subtasks");
                connection.Dispose();

            }
            AssignSubTaskToTheTask(taskID, GetLastRowID("subtasks"));
        }

        private static void AssignSubTaskToTheTask (long taskID, long subtaskID)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            connection.Execute("INSERT INTO task_subtasks (taskID, subtaskID) values (@taskID, @subtaskID)",
                new { taskID, subtaskID });
            connection.Dispose();
        }

        private static void AssignTaskToTheProject(long projectID, long taskID)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            connection.Execute("INSERT INTO project_tasks (projectID, taskID) values (@projectID, @taskID)",
                new { projectID, taskID });
            connection.Dispose();
        }

        public static void UpdateTask(IElement param)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            if (param is Task task)
                connection.Update(task);

            if (param is SubTask subTask)
                connection.Update(subTask);

            Console.WriteLine(param.ToString());
            connection.Dispose();
        }

        public static void DeleteTask(IElement element)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());

            var task = (ITask)element;
           
            foreach (var subtask in task.SubTasks)
            {
                string subtaskQuery = $"DELETE FROM task_subtasks WHERE subtaskID = '{subtask.ID}'";
                connection.Execute(subtaskQuery);
            }

            string taskQuery = $"DELETE FROM project_tasks WHERE taskID = '{task.ID}'";

            connection.Execute(taskQuery);

            connection.Delete((Task)task);
            connection.Dispose();
        }

        public static void DeleteSubTask(ISubTask task)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            connection.Delete((SubTask)task);
            connection.Dispose();
        }

        #endregion

        #region Comments

        public static List<IComment> GetProjectComments (long projectID)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            var sql = $" SELECT c.ID, c.Content, c.SubmitionDate " +
                        "FROM comments c " +
                        "INNER JOIN project_comments p ON p.commentID = c.ID " +
                        $"INNER JOIN projects pr on pr.ID = p.projectID WHERE pr.Id = {projectID}";
            List<Comment> output = connection.Query<Comment>(sql).ToList();
            connection.Dispose();
            return output.ConvertAll(o => (IComment)o); ;
        }

        public static void InsertComment(IComment comment, long projectID)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                var sql = @"insert into comments (Content, SubmitionDate, Header) 
                            values (@Content, @SubmitionDate, @Header)";
                connection.Execute(sql,
                                new
                                {
                                    comment.Content,
                                    comment.SubmitionDate,
                                    comment.Header
                                });
                comment.ID = GetLastRowID("comments");
                connection.Dispose();

            }
            AssigCommentToTheProject(projectID, GetLastRowID("comments"));
        }

        private static void AssigCommentToTheProject(long projectID, long commentID)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            connection.Execute("INSERT INTO project_comments (projectID, commentID) values (@projectID, @commentID)",
                new { projectID, commentID });
            connection.Dispose();
        }


        public static void DeleteComment(IComment comment)
        {
            string query = $"DELETE FROM project_comments WHERE commentID = '{comment.ID}'";
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            connection.Execute(query);
            connection.Delete((Comment)comment);
            connection.Dispose();
        }

        public static void UpdateComment (IComment comment)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            connection.Update(comment);
            connection.Dispose();
        }

        #endregion

        private static int GetLastRowID(string table)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            var count = connection.ExecuteScalar<int>($"select seq from sqlite_sequence where name='{table}'");
            //var count = connection.Query("SELECT COUNT(*) FROM projects;");
            connection.Dispose();
            return count;
        }

        public static int GetCount (string table)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            var count = connection.ExecuteScalar<int>($"SELECT COUNT(*) FROM {table}");
            //var count = connection.Query("SELECT COUNT(*) FROM projects;");
            connection.Dispose();
            return count;
        }
        public static int GetCount (string table, string rowName, string param)
        {
            using IDbConnection connection = new SQLiteConnection(GetConnectionString());
            var count = connection.ExecuteScalar<int>($"SELECT COUNT(*) FROM {table} WHERE {rowName} = '{param}'");
            //var count = connection.Query("SELECT COUNT(*) FROM projects;");
            connection.Dispose();
            return count;
        }

        private static string GetConnectionString (string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}

