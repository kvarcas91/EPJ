using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using EPJ.Models.Interfaces;
using EPJ.Models.Person;
using EPJ.Utilities;

namespace EPJ.Models.Task
{
    public class Task : ITask, INotifyPropertyChanged
    {

        #region Constructors

        public Task()
        {

        }

        #endregion //Constructors

        #region Private 

        private double _progress = 0;
        private bool _isCompleted = false;
        private uint _order;
        private DateTime _dueDate;
        private Priority _priority;
        private string _content;
        private bool _isExpanded = false;


        #endregion //Private Properties

        #region Public Properties

        #region Interface Implementation

        [Key]
        public long ID { get; set; }

        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (_content != value)
                {
                    _content = value;
                    OnPropertyChanged("Content");
                }
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
                if (_priority != value)
                {
                    _priority = value;
                    OnPropertyChanged("Priority");
                }
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
                if (_dueDate != value)
                {
                    _dueDate = value;
                    OnPropertyChanged("DueDate");
                }
            }
        }

        [Computed]
        public DateTime SubmitionDate { get; set; }

        [Computed]
        public double Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = value;
                OnPropertyChanged("Progress");
            }
        }

        public uint OrderNumber
        {
            get
            {
                return _order;
            }
            set
            {
                if (value != _order)
                {
                    _order = value;
                    OnPropertyChanged("OrderNumber");
                }
            }
        }

        public bool IsCompleted
        {
            get
            {
                return _isCompleted;
            }
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged("IsCompleted");
                }
            }
        }

        [Computed]
        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }
            }
        }

        [Computed]
        public IList<IPerson> Contributors { get; } = new ObservableCollection<IPerson>();

        [Computed]
        public IList<ISubTask> SubTasks { get; set; } = new ObservableCollection<ISubTask>();


        #endregion //Interface Implementation

        #endregion //Public Properties

        #region Public Methods

        #region Element

        public bool AddElement(IElement element)
        {
            if (element is ISubTask task) SubTasks.Add(task);
            return true;
        }

        public bool AddElements(IList<IElement> elements)
        {
            foreach (var element in elements)
            {
                AddElement(element);
            }
            return true;
        }

        public bool UpdateElement(IElement element)
        {
            if (element is Task task)
            {
                Content = task.Content;
                Priority = task.Priority;
                DueDate = task.DueDate;
            }
            return true;
        }

        public bool RemoveElement(IElement element)
        {
            if (element is ISubTask task) SubTasks.Remove(task);
            return true;
        }

        #endregion //Element

        #region Contributors

        public bool AddPerson(IPerson person)
        {
            if (person is IContributor contributor) Contributors.Add(contributor);
            return true;
        }

        public bool RemovePerson(IPerson person)
        {
            if (person is IContributor contributor) Contributors.Remove(contributor);
            return true;
        }

        public bool UpdatePerson(IPerson person)
        {
            throw new NotImplementedException();
        }

        public bool AddPersons(IList<IPerson> persons)
        {
            foreach (var person in persons)
            {
                AddPerson(person);
            }
            return true;
        }

        #endregion //Contributors

        #endregion //Public Methods


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
