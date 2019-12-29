﻿using Caliburn.Micro;
using EPJ.Models;
using EPJ.Models.Comments;
using EPJ.Models.Components;
using EPJ.Models.Interfaces;
using EPJ.Models.Person;
using EPJ.Models.Project;
using EPJ.Models.Task;
using EPJ.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
            // TODO
            //_projectPath = $".{Path.DirectorySeparatorChar}Projects{Path.DirectorySeparatorChar}{_project.Header}{Path.DirectorySeparatorChar}";
            _projectPath = _project.Path;
            FileListItemClickCommand = new RelayCommand(FileListItemClick);
            IsTaskCompletedCommand = new RelayCommand(SetProgress);
            IsSubTaskCompletedCommand = new RelayCommand(SetTaskProgress);
            EditTaskCommand = new RelayCommand(EditTask);
            EditSubTaskCommand = new RelayCommand(EditSubTask);
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

        public string SubmitionDate { get
            {
                return _project.SubmitionDate.ToShortDateString();
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

        public string CommentHeader
        {
            get
            {
                return _commentHeader;
            }
            set
            {
                _commentHeader = value;
                NotifyOfPropertyChange(() => CommentHeader);
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

        public ObservableCollection<IData> RelatedFiles { get; set; } = new ObservableCollection<IData>();
        public ObservableCollection<IPerson> ProjectContributors { get; set; }

        public ObservableCollection<IComment> Notes { get; set; }

        #endregion

        #region ICommand

        public ICommand FileListItemClickCommand { get; set; }
        public ICommand IsTaskCompletedCommand { get; set; }
        public ICommand IsSubTaskCompletedCommand { get; set; }
        public ICommand EditNoteCommand { get; set; }
        public ICommand DeleteNoteCommand { get; set; }
        public ICommand EditTaskCommand { get; set; }
        public ICommand EditSubTaskCommand { get; set; }
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
            ProjectTitle = _project.Header;
            Description = _project.Content;
            DueDate = _project.DueDate;
            _currentPath = _project.Path;
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

        private void EditFile (object param)
        {
            _editableComponent = (IData)param;
            CanEdit = true;
            ShowAddFolderPanel();
            NewFolderName = _editableComponent.Name;
            NotifyOfPropertyChange();
        }

        private void DeleteFile (object param)
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

        private void ShowInExplorer (object param)
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

            if (_updateableTask == null)
            {
                Console.WriteLine("insert task");
                var task = new Task
                {
                    Content = TaskContent,
                    DueDate = TaskDueDate,
                    Priority = TaskPriority
                };
                DataBase.InsertTask(task, _project.ID);
                ProjectTasks.Add(task);
            }
            else {
               if (_updateableTask is Task task)
                {
                    if (!_isSubTask)
                    {
                        Console.WriteLine("update task");
                        var index = ProjectTasks.IndexOf(task);
                        task.Content = TaskContent;
                        task.DueDate = TaskDueDate;
                        task.Priority = TaskPriority;
                        //ProjectTasks.Insert(index, task);
                        //ProjectTasks.RemoveAt(index + 1);
                        DataBase.UpdateTask(task);
                        _updateableTask = null;
                      
                    }
                    else
                    {
                        Console.WriteLine("insert subtask");
                        var newSubTask = new SubTask
                        {
                            Content = TaskContent,
                            DueDate = TaskDueDate,
                            Priority = TaskPriority
                        };
                        DataBase.InsertSubTask(newSubTask, task.ID);
                        task.SubTasks.Add(newSubTask);
                    }
                   
                }
              
               if (_updateableTask is SubTask subTask)
                {
                    Console.WriteLine("Update subtask");
                    var subtaskIndex = -1;
                    var taskIndex = -1;
                    for (int i = 0; i < ProjectTasks.Count; i++)
                    {
                        for (int j = 0; j < ProjectTasks[i].SubTasks.Count; j++)
                        {
                            if (Equals(_updateableTask, ProjectTasks[i].SubTasks[j]))
                            {
                                taskIndex = i;
                                subtaskIndex = j;
                            }
                        }

                    }

                    subTask.Content = TaskContent;
                    subTask.DueDate = TaskDueDate;
                    subTask.Priority = TaskPriority;
                    //ProjectTasks[taskIndex].SubTasks.Insert(subtaskIndex, subTask);
                    //ProjectTasks[taskIndex].SubTasks.RemoveAt(subtaskIndex + 1);
                    DataBase.UpdateTask(subTask);
                    _updateableTask = null;
                    _isSubTask = false;
                }
                _isSubTask = false;
                _editableComponent = null;

            }
            ShowAddTaskPanel();
            ResetNewTaskProperties();
        }


        public void EditTask(object param)
        {
            _isSubTask = false;
            var task = (Task)param;
            _updateableTask = task;
            ShowAddTaskPanel();
            TaskContent = task.Content;
            TaskDueDate = task.DueDate;
            TaskPriority = task.Priority;
        }

        public void EditSubTask (object param)
        {
            var subTask = (SubTask)param;
            _updateableTask = subTask;
            _isSubTask = true;
            ShowAddTaskPanel();
            TaskContent = subTask.Content;
            TaskDueDate = subTask.DueDate;
            TaskPriority = subTask.Priority;
        }

        public void AddSubTask (object param)
        {
            _updateableTask = (Task)param;
            _isSubTask = true;
            ShowAddTaskPanel();
        }

        public void DeleteTask(object param)
        {
            var task = (IElement)param;
            if (task is ITask ts)
            {
                DataBase.DeleteTask(ts);
                ProjectTasks.RemoveAt(GetSubTaskParentIndex(task));
            }
            if (task is ISubTask subTask)
            {
                DataBase.DeleteSubTask(subTask);
                ProjectTasks[GetSubTaskParentIndex(subTask)].SubTasks.Remove(subTask);
            }
           
           
        }

        private int GetSubTaskParentIndex (IElement sub)
        {
            for (int i = 0; i < ProjectTasks.Count; i++)
            {
                for (int j = 0; j < ProjectTasks[i].SubTasks.Count; j++)
                {
                    if (Equals(sub, ProjectTasks[i].SubTasks[j]))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public void SetTaskProgress (object param)
        {
            var element = (ISubTask)param;

            var taskIndex = GetSubTaskParentIndex(element);
            
            var task = ProjectTasks[taskIndex];
            var totalSubTaskCount = ProjectTasks[taskIndex].SubTasks.Count;
            var completedSubTaskCount = 0;

            foreach (var subTask in task.SubTasks)
            {
                if (subTask.IsCompleted)
                {
                    completedSubTaskCount++;
                }
            }
            task.Progress = (completedSubTaskCount * 100) / totalSubTaskCount;
            if (task.Progress == 100) task.IsCompleted = true;

            if (task.Progress < 100) task.IsCompleted = false;

            //ProjectTasks.Insert(taskIndex, task);
            //ProjectTasks.RemoveAt(taskIndex + 1);
          
            DataBase.UpdateTask(element);
            SetProgress(ProjectTasks[taskIndex]);
          
        }

        #endregion

        #region Project

        private void SetProgress (object param)
        {
            var element = (ITask)param;
            
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
            DataBase.UpdateTask(element);

        }

        private void UpdateProjectTitle (object param)
        {
            if (string.Equals(_project.Header, ProjectTitle.Trim())) return;

            if (!ValidateUserInput.IsNullOrWhiteSpace(ProjectTitle))
            {
                ProjectTitle = _project.Header;
                return;
            }
            _projectPath = $".{Path.DirectorySeparatorChar}Projects{Path.DirectorySeparatorChar}{ProjectTitle}{Path.DirectorySeparatorChar}";
            Directory.Move(_project.Path, _projectPath.Substring(2));
            _project.Header = ProjectTitle;
            _project.Path = _projectPath;
            DataBase.UpdateProject((Project)_project);
        }

        private void UpdateProjectDescription (object param)
        {
            if (!ValidateUserInput.IsNullOrWhiteSpace(Description))
            {
                Description = _project.Content;
                return;
            }

            _project.Content = Description;
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

        #region Comments

        public void ExpandCommentPanel()
        {
            IsCommentPanelVisible = !IsCommentPanelVisible;
        }
        private void GetComments ()
        {
            Notes = new ObservableCollection<IComment>(DataBase.GetProjectComments(_project.ID));
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
                    Content = CommentContent,
                    Header = CommentHeader
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
            ProjectContributors = new ObservableCollection<IPerson>(DataBase.GetProjectContributors(_project.ID));
        }

        #endregion //Contributors

        #endregion

        #region Drag and Drop

        public void OnDrop (IData sourceItem, IData destinationItem)
        {
            if (Equals(sourceItem, destinationItem)) return;
            if (destinationItem is IFile) return;
           
            sourceItem.Move(destinationItem.Path);
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
                catch (FileNotFoundException)
                {
                    MessageBox.Show("Sorry, coulnd't drop file. It might not exist");
                    return;
                }
                IData component;
                if (attr.HasFlag(FileAttributes.Directory)) component = new RelatedFolder(file);
                else component = new RelatedFile(file);
                component.Move(CurrentPath);
                ShowFolderContent(CurrentPath);
            }
            
        }

        public void OnDropTask(IElement source, IElement destination)
        {
           
            
            var startIndex = ProjectTasks.IndexOf((ITask)source);
            var destinationIndex = ProjectTasks.IndexOf((ITask)destination);

            ProjectTasks.Move(startIndex, destinationIndex);
            ((ITask)source).OrderNumber = (uint)destinationIndex;
            ((ITask)destination).OrderNumber = (uint)startIndex;

            DataBase.UpdateTask(source);
            DataBase.UpdateTask(destination);
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
