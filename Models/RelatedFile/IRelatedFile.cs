namespace EPJ
{
    public interface IRelatedFile
    {
        string FileExtention { get; }
        string FileName { get; }
        string FilePath { get; }
        uint FileVersion { get; set; }
        uint ProjectID { get; }

        void Copy();

        void Copy(string newName);

        void Replace();
    }
}