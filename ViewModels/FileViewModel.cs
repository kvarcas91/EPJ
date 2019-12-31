using EPJ.Models.Comments;
using EPJ.Models.Components;
using EPJ.Models.Interfaces;
using EPJ.Models.Project;
using EPJ.Utilities;
using EPJ.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public ICommand FileListItemClickCommand { get; set; }
     
        public ICommand ShowInExplorerCommand { get; set; }
        public ICommand EditFileCommand { get; set; }
        public ICommand DeleteFileCommand { get; set; }
        public ICommand ShowAddFolderPanelCommand { get; set; }
      

        #endregion

        #region Private Properties

        private bool _isSubTask = false;
        private string _currentPath;
        private string _projectPath;
        private readonly IProject _project;
        private IData _editableComponent = null;
        private string _projectTitle;
        private string _description;
        private DateTime _dueDate;
        private DateTime _taskDueDate = (DateTime.Now).AddDays(7);
        private bool _isAddTaskPanelVisible = false;
        private bool _isAddFilePanelVisible = false;
        private bool _isAddCommentPanelVisible = false;
        private bool _canNavigateBack = false;
        private bool _isDropping = false;
        private bool _isFileListVisible = true;
        private string _newFolderName;
        private string _taskContent;
        private IElement _updateableTask = null;
        private IComment _editableComment = null;
        private Priority _taskPriority = Priority.Default;
        private Priority _priority = Priority.Default;
        private string _commentContent;
        private string _commentHeader;
        private bool _canEdit = false;
        private bool _isProjectVisible = true;
        private bool _isExpandedCommentVisible = false;
        private DateTime _previewCommentSubmitionDate;
        private string _previewCommentContent;
        private string _previewCommentHeader;
        private bool _isProjectInfoPanelVisible = true;
        private bool _isAddContributorPanelVisible = false;
        private bool _isAddNewContributorPanelVisible = false;
        private bool _isAddedContributorListVisible = true;
        private bool _isAllContributorListVisible = false;
        private bool _isContributorPanelVisible = true;
        private bool _isCommentPanelVisible = true;


        #endregion // Private Properties

        #region Public Properties

        public bool IsAddFilePanelVisible
        {
            get
            {
                return _isAddFilePanelVisible;
            }
            set
            {
                _isAddFilePanelVisible = value;
                NotifyOfPropertyChange(() => IsAddFilePanelVisible);
            }
        }

        public bool CanNavigateBack
        {
            get
            {
                return _canNavigateBack;
            }
            set
            {
                _canNavigateBack = value;
                NotifyOfPropertyChange(() => CanNavigateBack);
            }
        }

        public bool IsDropping
        {
            get
            {
                return _isDropping;
            }
            set
            {
                _isDropping = value;
                NotifyOfPropertyChange(() => IsDropping);
            }
        }

        public bool IsFileListVisible
        {
            get
            {
                return _isFileListVisible;
            }
            set
            {
                _isFileListVisible = value;
                NotifyOfPropertyChange(() => IsFileListVisible);
            }
        }

        public string NewFolderName
        {
            get
            {
                return _newFolderName;
            }
            set
            {
                _newFolderName = value;
                NotifyOfPropertyChange(() => NewFolderName);
            }
        }


        public string CurrentPath
        {
            get
            {
                return _currentPath;
            }
            set
            {
                _currentPath = value;
                NotifyOfPropertyChange(() => CurrentPath);
            }
        }

        public bool CanEdit
        {
            get
            {
                return _canEdit;
            }
            set
            {
                _canEdit = value;
                NotifyOfPropertyChange(() => CanEdit);
            }
        }


        public bool CanAcceptChildren { get; set; }

        public ObservableCollection<IData> RelatedFiles { get; set; } = new ObservableCollection<IData>();

        #endregion

        #region Private Methods

        private void Initialize ()
        {
            ShowFolderContent(CurrentPath);
            SetCommands();
        }

       
        private void SetCommands ()
        {
            FileListItemClickCommand = new RelayCommand(FileListItemClick);
           
            EditFileCommand = new RelayCommand(EditFile);
            DeleteFileCommand = new RelayCommand(DeleteFile);
            ShowInExplorerCommand = new RelayCommand(ShowInExplorer);
            ShowAddFolderPanelCommand = new RelayCommand(ShowAddFolderPanel);
            
        }

        #region File


        private void ShowFolderContent(string path)
        {
            CurrentPath = path;

            //RelatedFiles.Clear();

            foreach (var item in FileHelper.GetFolderContent(path))
            {
                RelatedFiles.Add(item);
            }

        }

        public void ShowAddFolderPanel(object parram)
        {
            Console.WriteLine("Show Add Folder Panel");
            IsAddFilePanelVisible = !IsAddFilePanelVisible;
        }

        public bool CanAddNewFolder(string newFolderName)
        {
            return !string.IsNullOrWhiteSpace(newFolderName);
        }

        public void ShowProjectDirectory()
        {
            ShowFolderContent(_projectPath);
        }

        public void AddNewFolder(string newFolderName)
        {
            if (_editableComponent == null)
            {
                Directory.CreateDirectory($"{_currentPath}/{NewFolderName}");
                ShowFolderContent(_currentPath);
            }
            else
            {
                //var index = RelatedFiles.IndexOf(_editableComponent);
                //_editableComponent.Rename(newFolderName);
                //RelatedFiles.Insert(index, _editableComponent);
                //RelatedFiles.RemoveAt(index + 1);
                //_editableComponent = null;
            }
            NewFolderName = "";
            IsAddFilePanelVisible = false;
        }

        public void FileListItemClick(object param)
        {
            var component = (IData)param;
            if (component is IFolder folder)
            {
                CanNavigateBack = true;
                _currentPath = $"{_currentPath}{Path.DirectorySeparatorChar}{folder.Name}";
                ShowFolderContent(folder.Path);
                return;
            }

            CanNavigateBack = false;
            Process.Start($"{Directory.GetParent(Assembly.GetExecutingAssembly().Location)}{component.Path.Substring(1)}");
        }

        private void EditFile(object param)
        {
            _editableComponent = (IData)param;
            CanEdit = true;
            ShowAddFolderPanel(null);
            NewFolderName = _editableComponent.Name;
            NotifyOfPropertyChange();
        }

        private void DeleteFile(object param)
        {
            var component = (IData)param;
            var message = string.Empty;
            if (component is IFolder)
            {
                message = "Do you want to delete this folder?";
            }
            else
            {
                message = "Do you want to delete this file?";
            }

            MessageBoxResult result = MessageBox.Show(message, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                component.Delete();
                //RelatedFiles.Remove(component);
            }

        }

        private void ShowInExplorer(object param)
        {
            var component = (IData)param;
            if (component is IFile)
            {
                Process.Start(Directory.GetParent(component.Path).FullName);
                return;
            }
            Process.Start(component.Path);
        }

        #endregion

        #endregion // Private Methods

    }
}
