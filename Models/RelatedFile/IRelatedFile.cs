using System.Drawing;

namespace EPJ
{
    public interface IRelatedFile
    {
        string FileExtention { get; set; }
        string FileName { get; set; }
        string FilePath { get; set; }
        uint FileVersion { get; set; }
        long ID { get; set; }

        Icon FileIcon { get; set; }

        void Copy();

        void Copy(string newName);

        void Replace();
    }
}