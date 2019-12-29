using Dapper.Contrib.Extensions;
using EPJ.Models.Interfaces;
using EPJ.Models.Person;
using System.Collections.Generic;

namespace EPJ.Models.Task
{
    public interface ITask : IInteractiveElement, IOrderable
    {

        bool IsCompleted { get; set; }

        [Computed]
        IList<IContributor> Contributors { get; }

        [Computed]
        IList<ISubTask> SubTasks { get;}

    }
}
