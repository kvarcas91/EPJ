using Caliburn.Micro;
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
        }


        #endregion

        #region Properties

       // public ICommand mAddProjectCommand { get; set; }

       public ICommand ShowListItemSettingsCommand { get; set; }

        public ICommand ShowProjectCommand { get; set; }


        /// <summary>
        /// List of all projects
        /// </summary>
        public BindableCollection<Project> Projects { get; private set; } 


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
            Projects = new BindableCollection<Project>(DataBase.GetProjects());
        }

    }
}
