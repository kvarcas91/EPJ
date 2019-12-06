using Caliburn.Micro;
using EPJ.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EPJ.ViewModels
{
    public class AddProjectViewModel : Screen
    {

        #region Public Constructors

        public AddProjectViewModel()
        {
            AddContributorCommand = new RelayCommand(AddContributor);
            RemoveContributorCommand = new RelayCommand(RemoveContributor);
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
        private readonly string _projectPath = "./projects/temp/";

        // AddContributor toolbar visibilities
        private bool _isContributorListVisible = false;
        private bool _isAddContributorPanelVisible = false;
        private bool _isAddedContributorListVisible = true;
        private bool _isAddNewContributorPanelVisible = false;

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


        //public ICommand ShowAddNewContributorToolBarCommand { get; set; }

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
            Console.WriteLine(_project.ToString());
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
           // MessageBox.Show("test");
            var contributor = (Contributor)param;

            if(!AddedContributors.Contains(contributor)) AddedContributors.Add(contributor);
            Console.WriteLine(contributor.ToString());

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
            Console.WriteLine($"IsContributorListVisible {IsContributorListVisible}; IsAddNewContributorPanelVisible {IsAddNewContributorPanelVisible}");
        
        }

        #endregion

        #region File Methods

        private void CreateProjectFolder (string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private void ShowFolderContent (string path)
        {
            string[] contentDirectories = Directory.GetDirectories(path);
            string[] contentFiles = Directory.GetFiles(path);

            foreach (var item in contentDirectories)
            {
                Console.WriteLine($"Directory: {item}");
            }
            foreach(var item in contentFiles)
            {
                Console.WriteLine($"File: {item}");
            }

        }

        public void AddFiles()
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == true)
            {
                foreach (string filename in dialog.FileNames)
                {
                    RelatedFile file = new RelatedFile(filename);
                    RelatedFiles.Add(file);
                }
                    
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

        #endregion
    }
}
