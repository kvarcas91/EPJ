using System;
using System.Collections.Generic;

namespace EPJ
{
    public interface ITask : IProjectElement
    {
        
        DateTime DueDate { get; set; }
        bool IsCompleted { get; set; }
        Priority Priority { get; set; }
       
    }
}