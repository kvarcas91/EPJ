using Caliburn.Micro;
using EPJ.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EPJ.ViewModels
{
    public class AddProjectViewModel : Screen
    {

        public AddProjectViewModel()
        {
            SelectionChanged = new RelayCommand(ContributorSelected);
        }

        private Project _project = new Project();

        private string _title;
        private string _description;
        private bool _isContributorListVisible = false;
        private DateTime _dueDate = DateTime.Now;

        
        public ICommand SelectionChanged { get; set; }

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

        public bool IsContributorListVisible
        {
            get
            {
                return _isContributorListVisible;
            }
            set
            {
                _isContributorListVisible = value;
                NotifyOfPropertyChange(() => IsContributorListVisible);
            }
        }

        public DateTime DueDate
        {
            get
            {
                return _dueDate;
            }
            set
            {
                _dueDate = value;
                NotifyOfPropertyChange(() => DueDate);
            }
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

        public List<Contributor> AllContributors { get; } = new List<Contributor>(DataBase.GetContributors());

        public BindableCollection<Contributor> AddedContributors = new BindableCollection<Contributor>();


        public bool CanAddProject(string title, string description)
        {
            //return true;

            return ValidateUserInput.IsNullOrWhiteSpace(title, description);
        }

        public void AddProject(string title, string description)
        {

            _project.Priority = Priority;
            _project.Title = Title;
            _project.ProjectPath = "somewhere in C";
            _project.Date = DateTime.Now;
            _project.DueDate = DueDate;
            _project.Description = Description;
            Console.WriteLine(_project.ToString());
            // DataBase.InsertProject(project);

            //ProjectListViewModel lg = new ProjectListViewModel();
            //var parentConductor = (Conductor<object>)(this.Parent);
            //parentConductor.ActivateItem(lg);

        }

        public void ShowContributorList()
        {
            IsContributorListVisible = true;
        }

        private void ContributorSelected (object param)
        {
            var contributor = (Contributor)param;
            Console.WriteLine(contributor.ToString());
        }

        public void BackToProjectList()
        {
            ProjectListViewModel lg = new ProjectListViewModel();
            var parentConductor = (Conductor<object>)(this.Parent);
            parentConductor.ActivateItem(lg);
        }
    }
}
