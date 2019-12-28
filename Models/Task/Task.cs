using Dapper.Contrib.Extensions;
using EPJ.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
    public class Task : ITask
    {

        #region Properties

        [Key]
        public long ID { get; set; }
        public string Content { get; set; }

        public ulong OrderNumber { get; set; }

        [Computed]
        public List<IContributor> Contributors { get; } = new List<IContributor>();

        [Computed]
        public ObservableCollection<ITask> SubTasks { get; set; } = new ObservableCollection<ITask>();

        public bool IsCompleted { get; set; } = false;

        public DateTime DueDate { get; set; }

        public Priority Priority { get; set; }

        #endregion

        #region Constructors

        public Task()
        {

        }

        #endregion


        #region Public Methods

        public void AddSubTasks (List<Task> subTasks)
        {
            foreach (var item in subTasks)
            {
                SubTasks.Add(item);
            }
        }

        #endregion

    }
}
