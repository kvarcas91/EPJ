using Dapper.Contrib.Extensions;
using EPJ.Models.Interfaces;
using EPJ.Models.Person;
using EPJ.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ.Models.Task
{
    public class SubTask : ISubTask, INotifyPropertyChanged
    {

        #region Private 

        private bool _isCompleted = false;
        private uint _order;
        private DateTime _dueDate;
        private Priority _priority;
        private string _content;


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
        public double Progress { get; set; }

        [Computed]
        public IList<IContributor> Contributors { get; }

        [Computed]
        public IList<ISubTask> SubTasks { get; set; } = new ObservableCollection<ISubTask>();


        #endregion //Interface Implementation

        #endregion //Public Properties

        #region Public Methods

        #region Element

        public bool AddElement(IElement element)
        {
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
            throw new NotImplementedException();
        }

        public bool RemoveElement(IElement element)
        {
           
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

        public bool AddPersons(IList<IPerson> person)
        {
            throw new NotImplementedException();
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
