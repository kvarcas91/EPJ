using Dapper.Contrib.Extensions;
using EPJ.Models.Comments;
using EPJ.Models.Interfaces;
using EPJ.Models.Person;
using EPJ.Models.Task;
using EPJ.Utilities;
using System;
using System.Collections.Generic;

namespace EPJ.Models.Project
{
    public class Project : IProject
    {

        #region Constructors 

        public Project()
        {

        }

        #endregion //Constructors

        #region Public Properties

        #region Interface Implementation

        [Key]
        public long ID { get; set; }

        public string Header { get; set; }

        public string Content { get; set; }

        public Priority Priority { get; set; } = Priority.Default;

        public DateTime SubmitionDate { get; set; }

        public DateTime DueDate { get; set; }

        public string Path { get; set; }

        [Computed]
        public double Progress { get; set; } = 0;

        public bool IsArchived { get; set; } = false;

        [Computed]
        public IList<IElement> Comments { get; set; }

        [Computed]
        public IList<IContributor> Contributors { get; set; } = new List<IContributor>();

        [Computed]
        public IList<IElement> Tasks { get; set; }

        #endregion //Interface Implementation

        [Computed]
        public string ArchiveString => IsArchived ? "Unarchive" : "Archive";

        #endregion //Public Properties

        #region Public Methods

        #region Elements

        public bool AddElement(IElement element)
        {
            if (element is ITask) Tasks.Add(element);
            if (element is IComment) Comments.Add(element);
            return true;
        }

        public bool AddElements(IList<IElement> elements)
        {
            foreach (var element in elements)
            {
                AddElement(element);
            }
            return true;
        }

        public bool RemoveElement(IElement element)
        {
            throw new NotImplementedException();
        }

        public bool UpdateElement(IElement element)
        {
            throw new NotImplementedException();
        }

        #endregion //Elements

        #region Contributors

        public bool AddPerson(IPerson person)
        {
            if (person is IContributor contributor) Contributors.Add(contributor);
            return true;
        }

        public bool AddPersons (IList<IPerson> persons)
        {
            foreach (var person in persons)
            {
                AddPerson(person);
            }
            return true;
        }

        public bool RemovePerson(IPerson person)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePerson(IPerson person)
        {
            throw new NotImplementedException();
        }


        #endregion //Contributors

        #region Override Methods

        public override string ToString()
        {
            return $"priority: {Priority.ToString()}; title: {Header}; Description: {Content}; DueDate: {DueDate.ToString()}; Date: {SubmitionDate.ToString()}";
        }

        #endregion //Override Methods

        #endregion //Public Methods

    }
}
