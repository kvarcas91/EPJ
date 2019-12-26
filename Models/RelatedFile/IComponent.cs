using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ.Models
{
    public interface IComponent
    {
        ulong ID { get; set; }
        ulong Version { get; set; } 
        string ComponentPath { get; set; }
        string Name { get; set; }
        Icon Icon { get; set; }

        void Copy(string destination);
        void Replace(string destination);
        void Move(string destination);
        void Rename(string newName);

    }
}
