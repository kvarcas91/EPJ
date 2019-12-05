using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ.ViewModels
{
    public class AddProjectViewModel : Screen
    {
        public void AddProject ()
        {
            Project project = new Project();
            project.AddContributors(DataBase.GetContributors(0));
            project.Priority = Priority.MEDIUM;
            project.Title = "trecias projektas";
            project.ProjectPath = "somewhere in C";
            project.Date = DateTime.Now;
            project.Description = "test priority";
            DataBase.InsertProject(project);
            //ActivateItem(new ProjectListViewModel(project));
            ProjectListViewModel lg = new ProjectListViewModel();
            var parentConductor = (Conductor<object>)(this.Parent);
            parentConductor.ActivateItem(lg);
        }
    }
}
