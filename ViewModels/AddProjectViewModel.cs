using Caliburn.Micro;
using EPJ.Utilities;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using GongSolutions.Wpf.DragDrop;
using System.Diagnostics;
using System.Reflection;

namespace EPJ.ViewModels
{
    public class AddProjectViewModel : Screen, IDropTarget
    {

        #region Public Constructors

        public AddProjectViewModel()
        {
            AddContributorCommand = new RelayCommand(AddContributor);
            RemoveContributorCommand = new RelayCommand(RemoveContributor);
            FileListItemClickCommand = new RelayCommand(FileListItemClick);
            CreateProjectFolder(_projectPath);
            ShowFolderContent(_projectPath);
        }

        #endregion

        #region Private Properties

        private readonly Project _project = new Project();

        // Project Properties
        private string _title;
        private string _description;
        private DateTime _dueDate = DateTime.Now;

        // Contributor Properties
        private string _firstName;
        private string _lastName;

        //File Properties
        private readonly string _projectPath = ".\\projects\\temp\\";
        private string _currentPath;
        private string _newFolderName;

        // AddContributor toolbar visibilities
        private bool _isContributorListVisible = false;
        private bool _isAddContributorPanelVisible = false;
        private bool _isAddedContributorListVisible = true;
        private bool _isAddNewContributorPanelVisible = false;

        // AddFile toolbar visibilities
        private bool _isAddFilePanelVisible = false;
        private bool _canNavigateBack = false;
        private bool _isDropping = false;
        private bool _isFileListVisible = true;

        #endregion

        #region ICommand

        /// <summary>
        /// Assign Contributor to the project
        /// </summary>
        public ICommand AddContributorCommand { get; set; }

        /// <summary>
        /// Remove contributor from the project
        /// </summary>
        public ICommand RemoveContributorCommand { get; set; }


        public ICommand FileListItemClickCommand { get; set; }

        #endregion

        #region Public Properties

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

        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                NotifyOfPropertyChange(() => FirstName);
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                NotifyOfPropertyChange(() => LastName);
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

        public bool IsAddContributorPanelVisible
        {
            get
            {
                return _isAddContributorPanelVisible;
            }
            set
            {
                _isAddContributorPanelVisible = value;
                NotifyOfPropertyChange(() => IsAddContributorPanelVisible);
            }
        }

        public bool IsAddedContributorListVisible
        {
            get
            {
                return _isAddedContributorListVisible;
            }
            set
            {
                _isAddedContributorListVisible = value;
                NotifyOfPropertyChange(() => IsAddedContributorListVisible);
            }
        }

        public bool IsAddNewContributorPanelVisible
        {
            get
            {
                return _isAddNewContributorPanelVisible;
            }
            set
            {
                _isAddNewContributorPanelVisible = value;
                NotifyOfPropertyChange(() => IsAddNewContributorPanelVisible);
            }
        }

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

        public ObservableCollection<Contributor> AllContributors { get; } = new ObservableCollection<Contributor>(DataBase.GetContributors());

        public BindableCollection<Contributor> AddedContributors { get; } = new BindableCollection<Contributor>();

        public BindableCollection<RelatedFile> RelatedFiles { get; } = new BindableCollection<RelatedFile>();

        #endregion

        #region Project Methods

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
            _project.AddContributors(AddedContributors.ToList());
            DataBase.InsertProject(_project);

            ProjectListViewModel lg = new ProjectListViewModel();
            var parentConductor = (Conductor<object>)(this.Parent);
            parentConductor.ActivateItem(lg);

        }

        #endregion

        #region Contributor Methods

        public void ShowContributorList()
        {
            IsAddedContributorListVisible = IsAddContributorPanelVisible;
            IsAddContributorPanelVisible = !IsAddContributorPanelVisible;
            IsContributorListVisible = !IsContributorListVisible;
            //Console.WriteLine($"IsAddedContributorListVisible {IsAddedContributorListVisible}; IsAddNewContributorPanelVisible {IsAddNewContributorPanelVisible}");
        }

        private void AddContributor (object param)
        {
            var contributor = (Contributor)param;

            if(!AddedContributors.Contains(contributor)) AddedContributors.Add(contributor);

            IsContributorListVisible = false;
            IsAddedContributorListVisible = true;
            IsAddContributorPanelVisible = false;
        }

        public bool CanAddNewContributor(string firstName, string lastName)
        {
            return ValidateUserInput.IsNullOrWhiteSpace(firstName, lastName);
        }

        public void AddNewContributor (string firstName, string lastName)
        {
            var contributor = new Contributor(firstName, lastName);
            DataBase.InsertContributor(contributor);
            AllContributors.Clear();
            var contributors = DataBase.GetContributors();
            foreach (var item in contributors)
            {
                if (!AllContributors.Contains(item)) AllContributors.Add(item);
   
            }
          
           // AllContributors.Add(contributor);
            ShowAddNewContributorToolBar();
        }

        private void RemoveContributor (object param)
        {
            var contributor = (Contributor)param;
            AddedContributors.Remove(contributor);
           
        }

        public void ShowAddNewContributorToolBar ()
        {
            IsContributorListVisible = !IsContributorListVisible;
            IsAddNewContributorPanelVisible = !IsAddNewContributorPanelVisible;
        }

        #endregion

        #region File Methods

        public void ShowAddFolderPanel ()
        {
            IsAddFilePanelVisible = !IsAddFilePanelVisible;
            //IsFileListVisible = !IsAddFilePanelVisible;
        }

        public bool CanAddNewFolder(string newFolderName)
        {
            return !String.IsNullOrWhiteSpace(newFolderName);
        }

        public void AddNewFolder(string newFolderName)
        {
            Directory.CreateDirectory($"{_currentPath}/{NewFolderName}");
            NewFolderName = "";
            IsAddFilePanelVisible = false;
            ShowFolderContent(_currentPath);
        }

        public void ShowProjectDirectory ()
        {
            ShowFolderContent(_projectPath);
        }

        private void CreateProjectFolder (string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private void ShowFolderContent (string path)
        {
            _currentPath = path;
            RelatedFiles.Clear();
            Console.WriteLine($"Show folder content path: {path}");
            string[] contentDirectories = Directory.GetDirectories(path);
            string[] contentFiles = Directory.GetFiles(path);

           

            foreach (var item in contentDirectories)
            {
                RelatedFiles.Add(new RelatedFile(item));
            }
            foreach(var item in contentFiles)

            {
                RelatedFiles.Add(new RelatedFile(item));
            }
        }

        public void FileListItemClick (object param)
        {
            var file = (RelatedFile)param;
            if (String.IsNullOrEmpty(file.FileExtention))
            {
                CanNavigateBack = true;
                _currentPath = $"{_currentPath}\\{file.FileName}\\";
                ShowFolderContent(file.FilePath);
            }
            else
            {
                Console.WriteLine($"{Directory.GetParent(Assembly.GetExecutingAssembly().Location)}{file.FilePath.Substring(1)}");
                Process.Start($"{Directory.GetParent(Assembly.GetExecutingAssembly().Location)}{file.FilePath.Substring(1)}");
                
            }
        }

        #endregion


        #region Navigation

        public void BackToProjectList()
        {
            ProjectListViewModel lg = new ProjectListViewModel();
            var parentConductor = (Conductor<object>)(this.Parent);
            parentConductor.ActivateItem(lg);
        }

        public void NavigateFolderBack ()
        {
            if (String.Compare(_currentPath, _projectPath) != 0)
            {
                string[] directories = _currentPath.Split(Path.DirectorySeparatorChar);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < directories.Length-1; i++)
                {
                    Console.WriteLine($"directory name: {directories[i]}");
                    builder.Append($"{directories[i]}{Path.DirectorySeparatorChar}");
                }
                ShowFolderContent(builder.ToString());
            }
        }

        #endregion

        #region DropFile

        public void DragOver(IDropInfo dropInfo)
        {
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
            dropInfo.Effects = dragFileList.Any(item =>
            {
                IsDropping = true;
                IsFileListVisible = false;
              
                var extension = Path.GetExtension(item);
                return extension != null;
            }) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        public void Drop(IDropInfo dropInfo)
        {
            IsDropping = false;
            IsFileListVisible = true;

            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
            dropInfo.Effects = dragFileList.Any(item =>
            {
                var extension = Path.GetExtension(item);
                var newPath = $"{_currentPath}/{Path.GetFileName(item)}";
                Directory.Move(item, newPath);
                RelatedFile file = new RelatedFile(newPath);
                RelatedFiles.Add(file);
                return extension != null;
            }) ? DragDropEffects.Copy : DragDropEffects.None;
        }


        #endregion
    }
}
