using EPJ.Models.Interfaces;
using System.Drawing;

namespace EPJ.Models.Components
{
    public interface IComponent
    {

        string Name { get; set; }
        uint Version { get; set; }

        Icon Icon { get; set; }

    }
}
