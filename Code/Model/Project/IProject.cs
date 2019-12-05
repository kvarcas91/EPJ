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
        long ID { get; set; }
        string Title { get; set; }
        double Progress { get; set; }
        List<IRelatedFile> RelatedFiles { get; }
        Priority Priority { get; set; }
        Project AddElement(IProjectElement element);
        Project AddComment(IComment comment);
        //Project AddContributor(Contributor contributor);

        Project AddContributor(IContributor contributor);
    }
}