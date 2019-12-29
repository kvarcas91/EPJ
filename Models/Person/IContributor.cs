namespace EPJ.Models.Person
{
    public interface IContributor : IPerson
    {

        string Initials { get; }

        string InitialColor { get; }

    }
}
