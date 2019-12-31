using EPJ.Models.Components;
using EPJ.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EPJ.ViewModels
{
    public class FileViewModel : BaseScreen
        
    {

        #region Constructors

        private FileViewModel(string path) 
        {
            CurrentPath = path;
            Initialize();
        }

        #endregion // Constructors

        #region Static Factory

        public static FileViewModel Build (string path)
        {
            return new FileViewModel(path);
        }

        #endregion // Static Factory

        #region ICommand

        public ICommand ShowNewFolderPanelCommand { get; private set; }

        #endregion

        #region Private Properties

        private string path = string.Empty;


        #endregion // Private Properties

        #region Public Properties

        public string CurrentPath
        {
            get
            {
                return path;
            }
            set
            {
                if (path != value)
                {
                    path = value;
                    NotifyOfPropertyChange(() => CurrentPath);
                }
            }
        }

        public IList<IData> RelatedFiles { get; set; } = new ObservableCollection<IData>();

        #endregion // Public Properties

        #region Private Methods

        private void Initialize ()
        {
            GetComponents();
            SetCommands();
        }

        private void GetComponents()
        {
            CurrentPath = path;

            RelatedFiles.Clear();

            foreach (var item in FileHelper.GetFolderContent(path))
            {
                RelatedFiles.Add(item);
            }
        }

        private void SetCommands ()
        {
            ShowNewFolderPanelCommand = new FuncRelayCommand<string>(CreateNewFolder);
        }

        private void CreateNewFolder (string folderName)
        {
            Debug.WriteLine("CreateNewFolder");
        }

        #endregion // Private Methods

    }
}
