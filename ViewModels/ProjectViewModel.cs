using Caliburn.Micro;
using EPJ.Models;
using EPJ.Utilities;
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
            ExpandCommentCommand = new RelayCommand(ExpandCommentView);
            AddSubTaskCommand = new RelayCommand(AddSubTask);
            InitializeProject();
            ShowFolderContent(_projectPath);
        }


        #region Private Properties

        private bool _isSubTask = false;
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
        private bool _isProjectVisible = true;
        private bool _isExpandedCommentVisible = false;
        private DateTime _previewCommentSubmitionDate;
        private string _previewCommentContent;
        private bool _isProjectInfoPanelVisible = true;
        private bool _isAddContributorPanelVisible = false;
        private bool _isAddNewContributorPanelVisible = false;
        private bool _isAllContributorListVisible = false;
        private bool _isContributorPanelVisible = true;
        private bool _isCommentPanelVisible = true;

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

        public bool IsProjectVisible
        {
            get
            {
                return _isProjectVisible;
            }
            set
            {
                _isProjectVisible = value;
                NotifyOfPropertyChange(() => IsProjectVisible);
            }
        }

        public bool IsExpandedCommentVisible
        {
            get
            {
                return _isExpandedCommentVisible;
            }
            set
            {
                _isExpandedCommentVisible = value;
                NotifyOfPropertyChange(() => IsExpandedCommentVisible);
            }
        }

        public bool CanAcceptChildren { get; set; }

        public DateTime PreviewCommentSubmitionDate
        {
            get
            {
                return _previewCommentSubmitionDate;
            }
            set
            {
                _previewCommentSubmitionDate = value;
                NotifyOfPropertyChange(() => PreviewCommentSubmitionDate);
            }
        }
        public string PreviewCommentContent
        {
            get
            {
                return _previewCommentContent;
            }
            set
            {
                _previewCommentContent = value;
                NotifyOfPropertyChange(() => PreviewCommentContent);
            }
        }

        public bool IsProjectInfoPanelVisible
        {
            get
            {
                return _isProjectInfoPanelVisible;
            }
            set
            {
                _isProjectInfoPanelVisible = value;
                NotifyOfPropertyChange(() => IsProjectInfoPanelVisible);
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

        public bool IsAllContributorListVisible
        {
            get
            {
                return _isAllContributorListVisible;
            }
            set
            {
                _isAllContributorListVisible = value;
                NotifyOfPropertyChange(() => IsAllContributorListVisible);
            }
        }

        public bool IsContributorPanelVisible
        {
            get
            {
                return _isContributorPanelVisible;
            }
            set
            {
                _isContributorPanelVisible = value;
                NotifyOfPropertyChange(() => IsContributorPanelVisible);
            }
        }

        public bool IsCommentPanelVisible
        {
            get
            {
                return _isCommentPanelVisible;
            }
            set
            {
                _isCommentPanelVisible = value;
                NotifyOfPropertyChange(() => IsCommentPanelVisible);
            }
        }

        public ObservableCollection<ITask> ProjectTasks { get; set; }

        public ObservableCollection<IComponent> RelatedFiles { get; set; } = new ObservableCollection<IComponent>();
        public ObservableCollection<IContributor> ProjectContributors { get; set; }

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
        public ICommand ExpandCommentCommand { get; set; }
        public ICommand AddSubTaskCommand { get; set; }

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
            GetContributors();
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
                _editableComponent.Rename(newFolderName);
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
            foreach (var item in ProjectTasks)
            {
                Console.WriteLine($"subtask count: {item.SubTasks.Count}");
            }
        }

        private void CloseAddTaskPanel (object param)
        {
            ResetNewTaskProperties();
            ShowAddTaskPanel();
        }

        public void ExpandTaskPanel ()
        {
            IsProjectInfoPanelVisible = !IsProjectInfoPanelVisible;
        }

        public void ShowAddTaskPanel() => IsAddTaskPanelVisible = !IsAddTaskPanelVisible;


        public bool CanSaveTask(string taskContent)
        {
            return ValidateUserInput.IsNullOrWhiteSpace(taskContent);
        }
        public void SaveTask(string taskContent)
        {

            if (_updateableTask != null && !_isSubTask)
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
                if (_isSubTask)
                {
                    DataBase.InsertSubTask(mTask, _updateableTask.ID);

                    _updateableTask.SubTasks.Add(mTask);
                    _isSubTask = false;
                }
                else
                {
                    DataBase.InsertTask(mTask, _project.ID);
                    ProjectTasks.Add(mTask);
                }
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

        public void AddSubTask (object param)
        {
            _updateableTask = (Task)param;
            _isSubTask = true;
            ShowAddTaskPanel();
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

        public void Archive ()
        {
            _project.IsArchived = !_project.IsArchived;
            DataBase.UpdateProject((Project)_project);
        }

        #endregion

        #region Notes

        public void ExpandCommentPanel()
        {
            IsCommentPanelVisible = !IsCommentPanelVisible;
        }
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

        public void ExpandCommentView (object param)
        {
            if (param == null)
            {
                ChangeCommentView();
                _editableComment = null;
                return;
            }
            if (_editableComment == null) ChangeCommentView();

            _editableComment = (Comment)param;
            PreviewCommentSubmitionDate = _editableComment.SubmitionDate;
            PreviewCommentContent = _editableComment.Content;
            
           
        }

        private void ChangeCommentView ()
        {
            IsExpandedCommentVisible = IsProjectVisible;
            IsProjectVisible = !IsProjectVisible;
        }

        public void EditPreviewComment ()
        {
            var index = Notes.IndexOf(_editableComment);
            _editableComment.Content = PreviewCommentContent;
            Notes.Insert(index, _editableComment);
            Notes.RemoveAt(index + 1);
            DataBase.UpdateComment(_editableComment);
            _editableComment = null;
            ChangeCommentView();
        }

        public void EditComment (object param)
        {
            _editableComment = (Comment)param;
            CommentContent = _editableComment.Content;
            ShowAddNotePanel();
        }

        public void DeleteComment (object param)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to delete this Comment?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var comment = (Comment)param;
                DataBase.DeleteComment(comment);
                Notes.Remove(comment);
            }
        }

        #endregion

        #region Contributors 

        public void ExpandContributorsPanel ()
        {
            IsContributorPanelVisible = !IsContributorPanelVisible;
        }

        private void GetContributors ()
        {
            ProjectContributors = new ObservableCollection<IContributor>(DataBase.GetProjectContributors(_project.ID));
        }

        #endregion //Contributors

        #endregion

        #region Drag and Drop

        public void OnDrop (IComponent sourceItem, IComponent destinationItem)
        {
            if (Object.Equals(sourceItem, destinationItem)) return;
            if (destinationItem is IFile) return;
           
            sourceItem.Move(destinationItem.ComponentPath);
            ShowFolderContent(CurrentPath);
        }

        public void OnDropOuterFile (string[] files)
        {
            foreach (var file in files)
            {
                FileAttributes attr = 0;
                try
                {
                   attr = File.GetAttributes(file);
                }
                catch (FileNotFoundException e)
                {
                    MessageBox.Show("Sorry, coulnd't drop file. It might not exist");
                    return;
                }
                IComponent component;
                if (attr.HasFlag(FileAttributes.Directory)) component = new RelatedFolder(file);
                else component = new RelatedFile(file);
                component.Move(CurrentPath);
                ShowFolderContent(CurrentPath);
            }
            
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

        #region Navigation

        public void BackToProjectList ()
        {
            if (_editableComment != null)
            {
                ExpandCommentView(null);
                return;
            }
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
