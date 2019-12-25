using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ.Models
{
    public interface IFile : IComponent
    {

        string Extention { get; set; }

    }
}
