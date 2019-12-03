namespace EPJ
{
    public interface IContributor
    {
        long ID { get; set; }
        string FirstName { get; }
        string FullName { get; }
        string LastName { get; }
        string Initials { get; }
        string InitialColor { get; }

    }
}