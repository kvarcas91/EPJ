using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EPJ.ViewModels
{
    public class MainViewModel : Conductor<object>
    {

        public MainViewModel()

        {
            ProjectList();
        }

        #region Public 

        public void NewProject()
        {
            ActivateItem(new AddProjectViewModel());
        }

        public void ProjectList()
        {
            ActivateItem(new ProjectListViewModel());
        }


        public void ExportProject ()
        {
            MessageBox.Show("Soon...");
        }

        public void ImportProject ()
        {
            MessageBox.Show("Soon...");
        }

        public void Settings()
        {
            MessageBox.Show("Soon...");
        }

        public void About()
        {
            MessageBox.Show("Soon...");
        }

        #endregion //Public 

    }
}
