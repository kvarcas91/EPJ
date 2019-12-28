using EPJ.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EPJ
{
    public interface ITask : IProjectElement
    {
        
        ulong OrderNumber { get; set; }
        DateTime DueDate { get; set; }
        bool IsCompleted { get; set; }
        Priority Priority { get; set; }

        ObservableCollection<ITask> SubTasks { get; set; }

    }
}