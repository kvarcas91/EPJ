using EPJ.Models.Interfaces;
using EPJ.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ.Models.Comments
{
    public interface IComment : IElement, IDateable
    {

        string Header { get; set; }

    }
}
