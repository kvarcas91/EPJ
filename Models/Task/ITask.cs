using EPJ.Utilities;
using System;
using System.Collections.Generic;

namespace EPJ
{
    public interface ITask : IProjectElement
    {
        
        ulong OrderNumber { get; set; }
        DateTime DueDate { get; set; }
        bool IsCompleted { get; set; }
        Priority Priority { get; set; }
       
    }
}