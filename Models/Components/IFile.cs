using EPJ.Models.Interfaces;

namespace EPJ.Models.Components
{
    interface IFile : IData
    {

        string Extention { get; set; }

    }
}
