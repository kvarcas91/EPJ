using Caliburn.Micro;
using EPJ.Models.Project;
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
           
            Getprojects();
          
            ShowProjectCommand = new RelayCommand(ShowProject);
            DeleteProjectCommand = new RelayCommand(Deleteproject);
            ArchiveProjectCommand = new RelayCommand(ArchiveProject);
        }


        #endregion //Constructors

        #region Properties

        #region Private

        private ObservableCollection<IProject> _projects;

        private ViewType _selectedViewType = ViewType.Ongoing;

        #endregion //Private

        #region Public

        public ObservableCollection<IProject> Projects
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


        #endregion //Public

        #region ICommand

        public ICommand ShowProjectCommand { get; set; }
        public ICommand DeleteProjectCommand { get; set; }
        public ICommand ArchiveProjectCommand { get; set; }

        #endregion //ICommand

        #endregion //Properties

        #region Methods

        #region ICommand

        private void Deleteproject(object param)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to delete this project?", 
                                                        "Warning", 
                                                        MessageBoxButton.YesNo, 
                                                        MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var project = (Project)param;
                DataBase.DeleteProject(project);
                Projects.Remove(project);
            }
        }

        private void ArchiveProject(object param)
        {
            var project = (Project)param;
            project.IsArchived = !project.IsArchived;
            DataBase.UpdateProject(project);
            Getprojects();
            //Projects.Remove(project);
        }

        public void ShowProject(object param)
        {
            var project = (IProject)param;
            ProjectViewModel lg = new ProjectViewModel(project);
            var parentConductor = (Conductor<object>)(this.Parent);
            parentConductor.ActivateItem(lg);
        }

        #endregion //ICommand

        #endregion //Methods

        #region Private

        private void Getprojects()
        {
            Projects = new ObservableCollection<IProject>(DataBase.GetProjects(SelectedViewType));
        }

        #endregion //Private

    }
}
