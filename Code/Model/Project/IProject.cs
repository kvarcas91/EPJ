using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EPJ
{
    public interface IProject
    {

        List<IComment> Comments { get; }
        ObservableCollection<IContributor> Contributors { get; }

        List<IProjectElement> ProjectBodyElements { get; }
        DateTime Date { get; }
        string Description { get; set; }

        string ProjectPath { get; set; }

        DateTime DueDate { get; set; }
        uint ID { get; }
        string Name { get; set; }
        uint Progress { get; set; }
        List<IRelatedFile> RelatedFiles { get; }
        Priority Priority { get; set; }
        Project AddElement(IProjectElement element);
        Project AddComment(string commentText);
        Project AddContributor(Contributor contributor);

        Project AddContributor(IContributor contributor);
    }
}