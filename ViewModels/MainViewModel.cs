using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        

        #endregion //Public 

    }
}
