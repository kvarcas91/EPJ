using Caliburn.Micro;
using EPJ.Utilities;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ.ViewModels
{
    class ProjectViewModel : Screen, IDropTarget
    {

        public ProjectViewModel(IProject project)
        {
            _project = project;
            InitializeProject();
        }


        #region Private Properties

        private string _projectPath;
        private readonly IProject _project;
        private string _projectTitle;
        private string _submitionDate;
        private string _description;
        private DateTime _dueDate;
        private DateTime _taskDueDate  = (DateTime.Now).AddDays(7);
        private bool _isAddTaskPanelVisible = false;

        #endregion

        #region Public Properties

        public string ProjectTitle { 
            get
            {
                return _projectTitle;
            }
            set
            {
                _projectTitle = value;
                NotifyOfPropertyChange(() => ProjectTitle);
            }
        }

        public string SubmitionDate { get
            {
                return _submitionDate;
            }
            set
            {
                _submitionDate = value;
                NotifyOfPropertyChange(() => SubmitionDate);
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

        public DateTime TaskDueDate
        {
            get
            {
                return _taskDueDate;
            }
            set
            {
                _taskDueDate = value;
                NotifyOfPropertyChange(() => TaskDueDate);
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

        public IEnumerable<Priority> Priorities
        {
            get
            {
                return Enum.GetValues(typeof(Priority))
                    .Cast<Priority>();
            }
        }

        public Priority Priority { get; set; }

        public Priority TaskPriority { get; set; }

        public bool IsAddTaskPanelVisible
        {
            get
            {
                return _isAddTaskPanelVisible;
            }
            set
            {
                _isAddTaskPanelVisible = value;
                NotifyOfPropertyChange(() => IsAddTaskPanelVisible);
            }
        }

        public ObservableCollection<ITask> ProjectTasks { get; set; } = new ObservableCollection<ITask>();

        #endregion

        #region Private Methods

        private void InitializeProject()
        {
            ProjectTitle = _project.Title;
            Description = _project.Description;
            SubmitionDate = _project.Date.ToString();
            DueDate = _project.DueDate;
            _projectPath = $".{Path.DirectorySeparatorChar}Projects{Path.DirectorySeparatorChar}";
            Priority = _project.Priority;
            
        }

        private void ResetNewTaskProperties()
        {
            TaskPriority = Priority.Low;
            TaskDueDate = DateTime.Now.AddDays(7);
        }

        #endregion

        #region Public methods

        public void ShowAddTaskPanel() => IsAddTaskPanelVisible = !IsAddTaskPanelVisible;


        public bool CanSaveTask(string taskContent)
        {
            return ValidateUserInput.IsNullOrWhiteSpace(taskContent);
        }
        public void SaveTask (string taskContent)
        {
            var task = new Task
            {
                Description = taskContent,
                DueDate = TaskDueDate,
                Priority = TaskPriority
            };

            ProjectTasks.Add(task);
            ShowAddTaskPanel();

            ResetNewTaskProperties();
        }

       


        #endregion

        #region Drag and Drop

        public void DragOver(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }

        public void Drop(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Navigation

        public void BackToProjectList ()
        {
            ProjectListViewModel lg = new ProjectListViewModel();
            var parentConductor = (Conductor<object>)(this.Parent);
            parentConductor.ActivateItem(lg);
        }

        #endregion
    }
}
