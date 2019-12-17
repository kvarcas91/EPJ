using Caliburn.Micro;
using EPJ.Utilities;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace EPJ.ViewModels
{
    class ProjectViewModel : Screen, IDropTarget
    {

        public ProjectViewModel(IProject project)
        {
            _project = project;
            _projectPath = $".{Path.DirectorySeparatorChar}Projects{Path.DirectorySeparatorChar}{_project.Title}{Path.DirectorySeparatorChar}";
            FileListItemClickCommand = new RelayCommand(FileListItemClick);
            IsTaskCompletedCommand = new RelayCommand(SetProgress);
            InitializeProject();
            ShowFolderContent(_projectPath);
        }


        #region Private Properties

        private string _currentPath;
        private string _projectPath;
        private readonly IProject _project;
        private string _projectTitle;
        private string _submitionDate;
        private string _description;
        private DateTime _dueDate;
        private DateTime _taskDueDate  = (DateTime.Now).AddDays(7);
        private bool _isAddTaskPanelVisible = false;
        private bool _isAddFilePanelVisible = false;
        private bool _canNavigateBack = false;
        private bool _isDropping = false;
        private bool _isFileListVisible = true;
        private string _newFolderName;

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

        public DateTime TaskDueDate
        {
            get
            {
                return _taskDueDate;
            }
            set
            {
                _taskDueDate = value;
                NotifyOfPropertyChange(() => TaskDueDate);
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

        public IEnumerable<Priority> Priorities
        {
            get
            {
                return Enum.GetValues(typeof(Priority))
                    .Cast<Priority>();
            }
        }

        public Priority Priority { get; set; }

        public Priority TaskPriority { get; set; }

        public bool IsAddTaskPanelVisible
        {
            get
            {
                return _isAddTaskPanelVisible;
            }
            set
            {
                _isAddTaskPanelVisible = value;
                NotifyOfPropertyChange(() => IsAddTaskPanelVisible);
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

        public ObservableCollection<ITask> ProjectTasks { get; set; }

        public ObservableCollection<IRelatedFile> RelatedFiles { get; } = new ObservableCollection<IRelatedFile>();

        #endregion

        #region ICommand

        public ICommand FileListItemClickCommand { get; set; }
        public ICommand IsTaskCompletedCommand { get; set; }

        #endregion

        #region Private Methods

        private void InitializeProject()
        {
            ProjectTitle = _project.Title;
            Description = _project.Description;
            SubmitionDate = _project.Date.ToString();
            DueDate = _project.DueDate;
            _currentPath = _project.ProjectPath;
            Priority = _project.Priority;
            GetTasks();
        }

        private void ResetNewTaskProperties()
        {
            TaskPriority = Priority.Low;
            TaskDueDate = DateTime.Now.AddDays(7);
        }

        #region File

        private void ShowFolderContent(string path)
        {
            _currentPath = path;
            RelatedFiles.Clear();

            string[] contentDirectories = Directory.GetDirectories(path);
            string[] contentFiles = Directory.GetFiles(path);

            foreach (var item in contentDirectories)
            {
                RelatedFiles.Add(new RelatedFile(item));
            }
            foreach (var item in contentFiles)
            {
                RelatedFiles.Add(new RelatedFile(item));
            }
        }

        public void ShowAddFolderPanel()
        {
            IsAddFilePanelVisible = !IsAddFilePanelVisible;
            //IsFileListVisible = !IsAddFilePanelVisible;
        }

        public bool CanAddNewFolder(string newFolderName)
        {
            return !String.IsNullOrWhiteSpace(newFolderName);
        }

        public void ShowProjectDirectory()
        {
            ShowFolderContent(_projectPath);
        }

        public void AddNewFolder(string newFolderName)
        {
            Directory.CreateDirectory($"{_currentPath}/{NewFolderName}");
            NewFolderName = "";
            IsAddFilePanelVisible = false;
            ShowFolderContent(_currentPath);
        }

        public void FileListItemClick(object param)
        {
            var file = (RelatedFile)param;
            if (String.IsNullOrEmpty(file.FileExtention))
            {
                CanNavigateBack = true;
                _currentPath = $"{_currentPath}{Path.DirectorySeparatorChar}{file.FileName}";
                ShowFolderContent(file.FilePath);
            }
            else
            {
                Process.Start($"{Directory.GetParent(Assembly.GetExecutingAssembly().Location)}{file.FilePath.Substring(1)}");
            }
        }

        #endregion

        #region Task

        private void GetTasks ()
        {
            ProjectTasks = new ObservableCollection<ITask>(DataBase.GetProjectTasks(_project.ID));
        }

        public void ShowAddTaskPanel() => IsAddTaskPanelVisible = !IsAddTaskPanelVisible;


        public bool CanSaveTask(string taskContent)
        {
            return ValidateUserInput.IsNullOrWhiteSpace(taskContent);
        }
        public void SaveTask(string taskContent)
        {
            var task = new Task
            {
                Description = taskContent,
                DueDate = TaskDueDate,
                Priority = TaskPriority
            };
            DataBase.InsertTask(task, _project.ID);
            ProjectTasks.Add(task);
            ShowAddTaskPanel();

            ResetNewTaskProperties();
        }



        #endregion

        #region Project

        private void SetProgress (object param)
        {
            //((CheckBox)param).IsChecked = !((CheckBox)param).IsChecked;
            var totalTaskCount = ProjectTasks.Count;
            var completedTaskCount = 0;
            foreach (var task in ProjectTasks)
            {
                if (task.IsCompleted)
                {
                    completedTaskCount++;
                }
            }
            _project.Progress = (completedTaskCount * 100) / totalTaskCount;
            Console.WriteLine($"progress: {_project.Progress.ToString()}");
        }
             
        #endregion

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

        #region Navigation

        public void BackToProjectList ()
        {
            ProjectListViewModel lg = new ProjectListViewModel();
            var parentConductor = (Conductor<object>)(this.Parent);
            parentConductor.ActivateItem(lg);
        }

        public void NavigateFolderBack()
        {
            if (String.Compare(_currentPath, _projectPath) != 0)
            {
                string[] directories = _currentPath.Split(Path.DirectorySeparatorChar);
                directories = directories.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray();
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < directories.Length - 1; i++)
                {

                    Console.WriteLine($"directory name: {directories[i]}");
                    builder.Append($"{directories[i]}{Path.DirectorySeparatorChar}");


                }
                Console.WriteLine($"output: {builder.ToString()}");
                ShowFolderContent(builder.ToString());
            }
        }

        #endregion
    }
}
