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

        public bool CanAddProject(string title, string description)
        {
            //return true;
            return ValidateUserInput.IsNullOrWhiteSpace(title, description);
        }

        public void AddProject (string title, string description)
        {
            Project project = new Project();
            project.AddContributors(DataBase.GetContributors());
            project.Priority = Priority;
            project.Title = Title;
            project.ProjectPath = "somewhere in C";
            project.Date = DateTime.Now;
            project.Description = Description;
            Console.WriteLine(project.ToString());
            DataBase.InsertProject(project);
            ProjectListViewModel lg = new ProjectListViewModel();
            var parentConductor = (Conductor<object>)(this.Parent);
            parentConductor.ActivateItem(lg);
           
        }

        private string _title;
        private string _description;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                NotifyOfPropertyChange(() => Description);
            }
        }

        public DateTime DueDate { get; set; }

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
