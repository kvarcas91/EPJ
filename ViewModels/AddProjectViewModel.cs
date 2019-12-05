using Caliburn.Micro;
using EPJ.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ.ViewModels
{
    public class AddProjectViewModel : Screen
    {

        public bool CanAddProject()
        {
            return true;
        }

        public void AddProject ()
        {
            Project project = new Project();
            project.AddContributors(DataBase.GetContributors(0));
            project.Priority = Priority;
            project.Title = "trecias projektas";
            project.ProjectPath = "somewhere in C";
            project.Date = DateTime.Now;
            project.Description = "test priority";
            DataBase.InsertProject(project);
            ProjectListViewModel lg = new ProjectListViewModel();
            var parentConductor = (Conductor<object>)(this.Parent);
            parentConductor.ActivateItem(lg);
        }

        public Priority Priority { get; set; }

        public IEnumerable<Priority> Priorities
        {
            get
            {
                return Enum.GetValues(typeof(Priority))
                    .Cast<Priority>();
            }
        }

    }
}
