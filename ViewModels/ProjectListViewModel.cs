using Caliburn.Micro;
using EPJ.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace EPJ.ViewModels
{
    public class ProjectListViewModel : Screen
    {

        #region Constructors
        
        public ProjectListViewModel()
        {
            //for (int i = 0; i < 100; i++)
            //{
                Getprojects();
            //}

            //mAddProjectCommand = new RelayCommand(ShowMessage);
            ShowListItemSettingsCommand = new RelayCommand(ShowListItemSettings);
            ShowProjectCommand = new RelayCommand(ShowProject);
            DeleteProjectCommand = new RelayCommand(Deleteproject);
            ArchiveProjectCommand = new RelayCommand(ArchiveProject);
        }


        #endregion

        #region Properties

       // public ICommand mAddProjectCommand { get; set; }

       public ICommand ShowListItemSettingsCommand { get; set; }

        public ICommand ShowProjectCommand { get; set; }
        public ICommand DeleteProjectCommand { get; set; }
        public ICommand ArchiveProjectCommand { get; set; }


        /// <summary>
        /// List of all projects
        /// </summary>
        public ObservableCollection<Project> Projects
        {
            get
            {
                return _projects;
            }
            set
            {
                _projects = value;
                NotifyOfPropertyChange(() => Projects);
            }
        }

        private ObservableCollection<Project> _projects;

        private ViewType _selectedViewType = ViewType.Ongoing;
        public ViewType SelectedViewType
        {
            get
            {
                return _selectedViewType;
            }
            set
            {
                _selectedViewType = value;
                Getprojects();
                NotifyOfPropertyChange(() => SelectedViewType);
            }
        }

        public List<string> SortBy
        {
            get
            {
                return EnumHelper.SortByEnumToString;
            }
        }

        #endregion

       public void LoadAddProjectPage ()
        {
            //MessageBox.Show("test");
            AddProjectViewModel lg = new AddProjectViewModel();
            var parentConductor = (Conductor<object>)(this.Parent);
            parentConductor.ActivateItem(lg);
            
        }

        public void ShowListItemSettings (object param)
        {
            MessageBox.Show("Labas");
            var project = (IProject)param;
        }

        public void ShowProject (object param)
        {
            var project = (IProject)param;
            ProjectViewModel lg = new ProjectViewModel(project);
            var parentConductor = (Conductor<object>)(this.Parent);
            parentConductor.ActivateItem(lg);
        }

        public void Getprojects ()
        {
            Projects = new ObservableCollection<Project>(DataBase.GetProjects(SelectedViewType));
        }

        private void Deleteproject (object param)
        {
            Project project = (Project)param;
            DataBase.DeleteProject(project);
            Projects.Remove(project);
        }

        private void ArchiveProject (object param)
        {
            var project = (Project)param;
            project.IsArchived = true;
            DataBase.UpdateProject(project);
            Getprojects();
            //Projects.Remove(project);
        }

    }
}
