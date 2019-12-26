using Caliburn.Micro;
using EPJ.Models;
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
    class ProjectViewModel : Screen
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
            EditFileCommand = new RelayCommand(EditFile);
            DeleteFileCommand = new RelayCommand(DeleteFile);
            ShowInExplorerCommand = new RelayCommand(ShowInExplorer);
            InitializeProject();
            ShowFolderContent(_projectPath);
        }


        #region Private Properties

        private string _currentPath;
        private string _projectPath;
        private readonly IProject _project;
        private IComponent _editableComponent = null;
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
        private bool _canEdit = false;

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

        public string ArchiveString
        {
            get
            {
                return _project.IsArchived ? "Unarchive" : "Archive";
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

        public string CurrentPath { get
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

        public ObservableCollection<ITask> ProjectTasks { get; set; }

        public ObservableCollection<IComponent> RelatedFiles { get; set; } = new ObservableCollection<IComponent>();

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
        public ICommand ShowInExplorerCommand { get; set; }
        public ICommand EditFileCommand { get; set; }
        public ICommand DeleteFileCommand { get; set; }

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
            CurrentPath = path;

            RelatedFiles.Clear();

            foreach (var item in FileHelper.GetFolderContent(path))
            {
                RelatedFiles.Add(item);
            }
            
        }

        public void ShowAddFolderPanel()
        {
            IsAddFilePanelVisible = !IsAddFilePanelVisible;
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
            if (_editableComponent == null)
            {
                Directory.CreateDirectory($"{_currentPath}/{NewFolderName}");
                ShowFolderContent(_currentPath);
            }
            else
            {
                var index = RelatedFiles.IndexOf(_editableComponent);
                _editableComponent.Name = newFolderName;
                RelatedFiles.Insert(index, _editableComponent);
                RelatedFiles.RemoveAt(index + 1);
                _editableComponent = null;
            }
            NewFolderName = "";
            IsAddFilePanelVisible = false;
        }

        public void FileListItemClick(object param)
        {
            var component = (IComponent)param;
            if (component is IFolder folder)
            {
                CanNavigateBack = true;
                _currentPath = $"{_currentPath}{Path.DirectorySeparatorChar}{folder.Name}";
                ShowFolderContent(folder.ComponentPath);
                return;
            }

            CanNavigateBack = false;
            Process.Start($"{Directory.GetParent(Assembly.GetExecutingAssembly().Location)}{component.ComponentPath.Substring(1)}");
        }

        private void EditFile (object param)
        {
            _editableComponent = (IComponent)param;
            CanEdit = true;
            ShowAddFolderPanel();
            NewFolderName = _editableComponent.Name;
            NotifyOfPropertyChange();
        }

        private void DeleteFile (object param)
        {
            var component = (IComponent)param;
            var message = "";
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
                if (component is IFolder)
                {
                    Directory.Delete(component.ComponentPath);
                }
                else
                {
                    File.Delete(component.ComponentPath);
                }
               
                RelatedFiles.Remove(component);
            }
           
        }

        private void ShowInExplorer (object param)
        {
            var component = (IComponent)param;
            if (component is IFile)
            {
                Process.Start(Directory.GetParent(component.ComponentPath).FullName);
                return;
            }
            Process.Start(component.ComponentPath);
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

        public void OnDrop (IComponent sourceItem, IComponent destinationItem)
        {
            if (Object.Equals(sourceItem, destinationItem)) return;
            if (destinationItem is IFile) return;
           
            sourceItem.Move(destinationItem.ComponentPath);
            ShowFolderContent(CurrentPath);
        }

        public void OnDropTask(ITask source, ITask destination)
        {
            var startIndex = ProjectTasks.IndexOf(source);
            var destinationIndex = ProjectTasks.IndexOf(destination);

            if (startIndex >= 0 && destinationIndex >= 0)
            {
                //Console.WriteLine($"start: {startIndex}; end: {destinationIndex}");
                ProjectTasks.Move(startIndex, destinationIndex);
                source.OrderNumber = (ulong)destinationIndex;
                destination.OrderNumber = (ulong)startIndex;
                DataBase.UpdateTask((Task)source);
                DataBase.UpdateTask((Task)destination);
            }

            //if (Object.Equals(sourceItem, destinationItem)) return;

            //if (destinationItem is IFile) return;

            //sourceItem.Move(destinationItem.ComponentPath);
            //ShowFolderContent(CurrentPath);
        }

        #endregion


        /*
        #region Drag and Drop

          public void DragOver(IDropInfo dropInfo)
        {
            
            try
            {
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
            dropInfo.Effects = dragFileList.Any(item =>
            {

                var extension = Path.GetExtension(item);
                return extension != null;
            }) ? DragDropEffects.Copy : DragDropEffects.None;

            }
            catch
            {
                var dragFileList = (IComponent)dropInfo.Data;
                dropInfo.Effects = dragFileList != null ? DragDropEffects.Copy : DragDropEffects.None;
            };
            
        }

        public void Drop(IDropInfo dropInfo)
        {
            try
            {
                var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
                dropInfo.Effects = dragFileList.Any(item =>
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                    IComponent component;
                    var extension = Path.GetExtension(item);
                    var newPath = $"{_currentPath}/{Path.GetFileName(item)}";
                    Directory.Move(item, newPath);
                    if (String.IsNullOrEmpty(extension))
                    {
                        component = new RelatedFolder(newPath);
                    }
                    else
                    {
                        component = new RelatedFile(newPath);
                    }
                    RelatedFiles.Add(component);
                    return extension != null;
                }) ? DragDropEffects.Copy : DragDropEffects.None;
            }
            catch
            {
                var component = (IComponent)dropInfo.Data;
                //var target = (IComponent)dropInfo.TargetItem;
                Console.WriteLine($"sourse: {component.ComponentPath}");
                
                //Console.WriteLine($"destination: {target.ComponentPath}");
            }
        }

        #endregion

    */
        #region Navigation

        public void BackToProjectList ()
        {
            ProjectListViewModel lg = new ProjectListViewModel();
            var parentConductor = (Conductor<object>)(this.Parent);
            parentConductor.ActivateItem(lg);
        }

        public void NavigateFolderBack()
        {
            if (String.Compare(CurrentPath, _projectPath) != 0)
            {
                string[] directories = CurrentPath.Split(Path.DirectorySeparatorChar);
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
