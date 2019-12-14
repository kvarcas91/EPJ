using Caliburn.Micro;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private IProject _project;
        private string _projectTitle;
        private string _submitionDate;
        private string _description;

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
        public ObservableCollection<ITask> ProjectTasks { get; set; } = new ObservableCollection<ITask>();

        #endregion

        #region Private Methods

        private void InitializeProject()
        {
            ProjectTitle = _project.Title;
            Description = _project.Description;
            SubmitionDate = _project.Date.ToString();
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
    }
}
