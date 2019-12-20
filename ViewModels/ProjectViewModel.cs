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
using System.Windows;
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
            EditTaskCommand = new RelayCommand(EditTask);
            CloseAddTaskPanelCommand = new RelayCommand(CloseAddTaskPanel);
            TitleLostFocusCommand = new RelayCommand(UpdateProjectTitle);
            DescriptionLostFocusCommand = new RelayCommand(UpdateProjectDescription);
            DeleteTaskCommand = new RelayCommand(DeleteTask);
            EditCommentCommand = new RelayCommand(EditComment);
            DeleteCommentCommand = new RelayCommand(DeleteComment);
            InitializeProject();
            ShowFolderContent(_projectPath);
        }


        #region Private Properties

        private string _currentPath;
        private string _projectPath;
        private readonly IProject _project;
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
        private Task _updateableTask = null;
        private Comment _editableComment = null;
        private Priority _taskPriority = Priority.Default;
        private Priority _priority = Priority.Default;
        private string _commentContent;

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

        public String SubmitionDate { get
            {
                return _project.Date.ToShortDateString();
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
                UpdateProjectDeadline();
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

        public Priority Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
                UpdateProjectPriority();
                NotifyOfPropertyChange(() => Priority);
            }
        }

        public Priority TaskPriority
        {
            get
            {
                return _taskPriority;
            }
            set
            {
                _taskPriority = value;
                NotifyOfPropertyChange(() => TaskPriority);
            }
        }

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

        public bool IsAddCommentPanelVisible
        {
            get
            {
                return _isAddCommentPanelVisible;
            }
            set
            {
                _isAddCommentPanelVisible = value;
                NotifyOfPropertyChange(() => IsAddCommentPanelVisible);
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

        public string TaskContent
        {
            get
            {
                return _taskContent;
            }
            set
            {
                _taskContent = value;
                NotifyOfPropertyChange(() => TaskContent);
            }
        }

        public string CommentContent
        {
            get
            {
                return _commentContent;
            }
            set
            {
                _commentContent = value;
                NotifyOfPropertyChange(() => CommentContent);
            }
        }

        public double Progress
        {
            get
            {
                return _project.Progress;
            }
            set
            {
                _project.Progress = value;
                NotifyOfPropertyChange(() => Progress);
            }
        }

        public ObservableCollection<ITask> ProjectTasks { get; set; }

        public ObservableCollection<IRelatedFile> RelatedFiles { get; } = new ObservableCollection<IRelatedFile>();

        public ObservableCollection<Comment> Notes { get; set; }

        #endregion

        #region ICommand

        public ICommand FileListItemClickCommand { get; set; }
        public ICommand IsTaskCompletedCommand { get; set; }
        public ICommand EditNoteCommand { get; set; }
        public ICommand DeleteNoteCommand { get; set; }
        public ICommand EditTaskCommand { get; set; }
        public ICommand DeleteTaskCommand { get; set; }
        public ICommand CloseAddTaskPanelCommand { get; set; }
        public ICommand TitleLostFocusCommand {get; set; }
        public ICommand DescriptionLostFocusCommand { get; set; }
        public ICommand EditCommentCommand { get; set; }
        public ICommand DeleteCommentCommand { get; set; }

        #endregion

        #region Private Methods

        private void InitializeProject()
        {
            ProjectTitle = _project.Title;
            Description = _project.Description;
            DueDate = _project.DueDate;
            _currentPath = _project.ProjectPath;
            Priority = _project.Priority;
            GetTasks();
            GetComments();
        }

        private void ResetNewTaskProperties()
        {
            TaskPriority = Priority.Low;
            TaskContent = "";
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

        private void CloseAddTaskPanel (object param)
        {
            ResetNewTaskProperties();
            ShowAddTaskPanel();
        }

        public void ShowAddTaskPanel() => IsAddTaskPanelVisible = !IsAddTaskPanelVisible;


        public bool CanSaveTask(string taskContent)
        {
            return ValidateUserInput.IsNullOrWhiteSpace(taskContent);
        }
        public void SaveTask(string taskContent)
        {

            if (_updateableTask != null)
            {
                var index = ProjectTasks.IndexOf(_updateableTask);
                _updateableTask.Content = TaskContent;
                _updateableTask.DueDate = TaskDueDate;
                _updateableTask.Priority = TaskPriority;
                ProjectTasks.Insert(index, _updateableTask);
                ProjectTasks.RemoveAt(index + 1);
                DataBase.UpdateTask(_updateableTask);
                //ProjectTasks[index] = _updateableTask;
                _updateableTask = null;
                ShowAddTaskPanel();
            }
            else
            {
                var mTask = new Task
                {
                    Content = TaskContent,
                    DueDate = TaskDueDate,
                    Priority = TaskPriority
                };
                DataBase.InsertTask(mTask, _project.ID);
                ProjectTasks.Add(mTask);
                ShowAddTaskPanel();
            }
            
            ResetNewTaskProperties();
        }

        public void EditTask(object task)
        {
            _updateableTask = (Task)task;
            ShowAddTaskPanel();
            TaskContent = _updateableTask.Content;
            TaskDueDate = _updateableTask.DueDate;
            TaskPriority = _updateableTask.Priority;
        }

        public void DeleteTask(object param)
        {
            var task = (Task)param;
            DataBase.DeleteTask(task);
            ProjectTasks.Remove(task);
        }

        #endregion

        #region Project

        private void SetProgress (object param)
        {
            var mTask = (Task)param;
            var totalTaskCount = ProjectTasks.Count;
            var completedTaskCount = 0;
            foreach (var task in ProjectTasks)
            {
                if (task.IsCompleted)
                {
                    completedTaskCount++;
                }
            }
            Progress = (completedTaskCount * 100) / totalTaskCount;
            DataBase.UpdateTask(mTask);

        }

        private void UpdateProjectTitle (object param)
        {
            if (String.Equals(_project.Title, ProjectTitle.Trim())) return;

            if (!ValidateUserInput.IsNullOrWhiteSpace(ProjectTitle))
            {
                ProjectTitle = _project.Title;
                return;
            }
            _projectPath = $".{Path.DirectorySeparatorChar}Projects{Path.DirectorySeparatorChar}{ProjectTitle}{Path.DirectorySeparatorChar}";
            Directory.Move(_project.ProjectPath, _projectPath.Substring(2));
            _project.Title = ProjectTitle;
            _project.ProjectPath = _projectPath;
            DataBase.UpdateProject((Project)_project);
        }

        private void UpdateProjectDescription (object param)
        {
            if (!ValidateUserInput.IsNullOrWhiteSpace(Description))
            {
                Description = _project.Description;
                return;
            }

            Console.WriteLine("updating");
            _project.Description = Description;
            DataBase.UpdateProject((Project)_project);
        }

        private void UpdateProjectDeadline()
        {
            if (DateTime.Compare(_dueDate, DateTime.Now) < 0)
            {
                _dueDate = _project.DueDate;
                return;
            }
            _project.DueDate = DueDate;
            DataBase.UpdateProject((Project)_project);
        }

        private void UpdateProjectPriority ()
        {
            if (_priority == _project.Priority) return;
            _project.Priority = _priority;
            DataBase.UpdateProject((Project)_project);
        }

        #endregion

        #region Notes

        private void GetComments ()
        {
            Notes = new ObservableCollection<Comment>(DataBase.GetProjectComments(_project.ID));
            Console.WriteLine($"size: {Notes.Count}");
        }

        public void ShowAddNotePanel ()
        {
            IsAddCommentPanelVisible = !IsAddCommentPanelVisible;
        }

        public void AddComment ()
        {
            if (!ValidateUserInput.IsNullOrWhiteSpace(CommentContent)) return;


            if (_editableComment != null)
            {
                var index = Notes.IndexOf(_editableComment);
                _editableComment.Content = CommentContent;
                Notes.Insert(index, _editableComment);
                Notes.RemoveAt(index + 1);
                DataBase.UpdateComment(_editableComment);
                _editableComment = null;
            }
            else
            {
                var comment = new Comment
                {
                    SubmitionDate = DateTime.Now,
                    Content = CommentContent
                };
                DataBase.InsertComment(comment, _project.ID);
                Notes.Add(comment);
            }
            CommentContent = String.Empty;
            ShowAddNotePanel();
        }

        public void EditComment (object param)
        {
            _editableComment = (Comment)param;
            CommentContent = _editableComment.Content;
            ShowAddNotePanel();
        }

        public void DeleteComment (object param)
        {
            var comment = (Comment)param;
            DataBase.DeleteComment(comment);
            Notes.Remove(comment);
        }

        #endregion

        #endregion

        #region Drag and Drop

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
