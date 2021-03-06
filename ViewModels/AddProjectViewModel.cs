﻿using Caliburn.Micro;
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
using System.Diagnostics;
using System.Reflection;
using EPJ.Models;
using EPJ.Models.Project;
using EPJ.Models.Person;
using EPJ.Models.Components;

namespace EPJ.ViewModels
{
    public class AddProjectViewModel : Screen
    {

        #region Public Constructors

        public AddProjectViewModel()
        {
            AddContributorCommand = new RelayCommand(AddContributor);
            RemoveContributorCommand = new RelayCommand(RemoveContributor);
            FileListItemClickCommand = new RelayCommand(FileListItemClick);
            EditFileCommand = new RelayCommand(EditFile);
            DeleteFileCommand = new RelayCommand(DeleteFile);
            ShowInExplorerCommand = new RelayCommand(ShowInExplorer);
            CreateProjectFolder(_projectPath);
            ShowFolderContent(_projectPath);
        }

        #endregion

        #region Private Properties

        private readonly IProject _project = new Project();

        // Project Properties
        private string _title;
        private string _description;
        private DateTime _dueDate = DateTime.Now;

        // Contributor Properties
        private string _firstName;
        private string _lastName;

        //File Properties
        private readonly string _projectPath = ".\\projects\\temp\\";
        private readonly string _allProjectPath = ".\\projects\\";
        private string _currentPath;
        private string _newFolderName;
        private IData _editableComponent = null;

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

        public ICommand ShowInExplorerCommand { get; set; }
        public ICommand EditFileCommand { get; set; }
        public ICommand DeleteFileCommand { get; set; }
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

        public ObservableCollection<IContributor> AllContributors { get; } = new ObservableCollection<IContributor>(DataBase.GetContributors());

        public ObservableCollection<IPerson> AddedContributors { get; } = new ObservableCollection<IPerson>();

        public ObservableCollection<IData> RelatedFiles { get; set; } = new ObservableCollection<IData>();

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
            _project.Header = Title;
            _project.Path = $"Projects{Path.DirectorySeparatorChar}{Title}{Path.DirectorySeparatorChar}";
            _project.SubmitionDate = DateTime.Now;
            _project.DueDate = DueDate;
            _project.Content = Description;
            _project.AddPersons(AddedContributors.ToList());
           
           
            try
            {
                Directory.Move(_projectPath, $"{_allProjectPath}{_project.Header}");
                DataBase.InsertProject(_project);
            }
            catch
            {
                MessageBox.Show("Project directory already exist");
                return;
            }

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

        private void EditFile(object param)
        {
            _editableComponent = (IData)param;
           
            ShowAddFolderPanel();
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
                RelatedFiles.Remove(component);
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
        public void AddNewFolder(string newFolderName)
        {
            if (_editableComponent == null)
            {
                Directory.CreateDirectory($"{_currentPath}/{NewFolderName}");
                ShowFolderContent(_currentPath);
            }
            else
            {
                var index = RelatedFiles.IndexOf(_editableComponent);
                _editableComponent.Rename(newFolderName);
                RelatedFiles.Insert(index, _editableComponent);
                RelatedFiles.RemoveAt(index + 1);
                _editableComponent = null;
            }
            NewFolderName = "";
            IsAddFilePanelVisible = false;
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

            foreach (var item in FileHelper.GetFolderContent(path))
            {
                RelatedFiles.Add(item);
            }
        }

        public void FileListItemClick (object param)
        {
            var component = (IData)param;
            if (component is IFolder folder)
            {
                CanNavigateBack = true;

                _currentPath = $"{_currentPath}{Path.DirectorySeparatorChar}{folder.Name}";
                ShowFolderContent(folder.Path);
                return;
            }
            
            Process.Start($"{Directory.GetParent(Assembly.GetExecutingAssembly().Location)}{component.Path.Substring(1)}"); 
            
        }

        #endregion


        #region Navigation

        public void BackToProjectList()
        {
            Directory.Delete(_projectPath, true);
            ProjectListViewModel lg = new ProjectListViewModel();
            var parentConductor = (Conductor<object>)(this.Parent);
            parentConductor.ActivateItem(lg);
        }

        public void NavigateFolderBack ()
        {
            if (String.Compare(_currentPath, _projectPath) != 0)
            {
                string[] directories = _currentPath.Split(Path.DirectorySeparatorChar);
                directories = directories.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray();
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < directories.Length-1; i++)
                {
                   
                    Console.WriteLine($"directory name: {directories[i]}");
                    builder.Append($"{directories[i]}{Path.DirectorySeparatorChar}");
       
                   
                }
                Console.WriteLine($"output: {builder.ToString()}");
                ShowFolderContent(builder.ToString());
            }
        }

        #endregion

        #region DropFile



        public void OnDrop(IData sourceItem, IData destinationItem)
        {
            if (Object.Equals(sourceItem, destinationItem)) return;
            if (destinationItem is IFile) return;

            sourceItem.Move(destinationItem.Path);
            ShowFolderContent(_currentPath);
        }

        public void OnDropOuterFile(string[] files)
        {
            foreach (var file in files)
            {
                FileAttributes attr = File.GetAttributes(file);
                IData component;
                if (attr.HasFlag(FileAttributes.Directory)) component = new RelatedFolder(file);
                else component = new RelatedFile(file);
                (component).Move(_currentPath);
                ShowFolderContent(_currentPath);
            }

        }


        #endregion
    }
}
