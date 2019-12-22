using Dapper.Contrib.Extensions;
using EPJ.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
    public class Project : IProject
    {

        #region Properties

        [Key]
        /// <summary>
        /// Current project ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Project name
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Project description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Project creation date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Project deadline
        /// </summary>
        public DateTime DueDate { get; set; }

        public bool IsArchived { get; set; } = false;

        /// <summary>
        /// Project priority
        /// </summary>
        public Priority Priority { get; set; } = Priority.Default;

        [Computed]
        /// <summary>
        /// Project progress
        /// </summary>
        public double Progress { get; set; } = 0;

        [Computed]
        public string ArchiveString
        {
            get
            {
                return IsArchived ? "Unarchive" : "Archive";
            }
        }

        [Computed]
        /// <summary>
        /// Contributors for the project
        /// </summary>
        public ObservableCollection<IContributor> Contributors { get; } = new ObservableCollection<IContributor>();

        [Computed]
        /// <summary>
        /// Related files to the project
        /// </summary>
        public List<IRelatedFile> RelatedFiles { get; } = new List<IRelatedFile>();

        [Computed]
        /// <summary>
        /// Comments for the project
        /// </summary>
        public List<IComment> Comments { get; } = new List<IComment>();

        [Computed]
        public List<IProjectElement> ProjectBodyElements { get; } = new List<IProjectElement>();

        public string ProjectPath { get; set; }


        #endregion

        #region Constructors

        public Project()
        {
           
        }

        #endregion

        #region Builder

        /// <summary>
        /// Creates Contributor and adds it to the list if doesn't exist
        /// </summary>
        /// <param name="firstName">contributor first name</param>
        /// <param name="lastName">contributor last name</param>
        /// <returns></returns>
        public Project AddContributor(Contributor contributor)
        {
            if (!Contributors.Contains(contributor))
                Contributors.Add(contributor);

            return this;
        }

        public void AddContributors(List<Contributor> contributors)
        {
            foreach (var contributor in contributors)
            {
                Contributors.Add(contributor);
                Console.WriteLine(contributor.ToString());
            }
        }

        public Project AddComment(IComment comment)
        {
            Comments.Add(comment);
            return this;
        }

        public Project AddElement(IProjectElement element)
        {
            //todo stuf
            return this;
        }

        public Project AddContributor(IContributor contributor)
        {
            if (!Contributors.Contains(contributor))
                Contributors.Add(contributor);

            return this;
        }

        #endregion

        public override string ToString()
        {
            return $"priority: {Priority.ToString()}; title: {Title}; Description: {Description}; DueDate: {DueDate.ToString()}; Date: {Date.ToString()}";
        }

    }
}
