using EPJ.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EPJ
{
    public interface IProject
    {

        long ID { get; set; }

        string Title { get; set; }

        string Description { get; set; }

        DateTime Date { get; set; }

        DateTime DueDate { get; set; }

        string ProjectPath { get; set; }

        bool IsArchived { get; set; }

        Priority Priority { get; set; }

        double Progress { get; set; }

        List<IComment> Comments { get; }
        ObservableCollection<IContributor> Contributors { get; }

        List<IProjectElement> ProjectBodyElements { get; }
      
       
        List<IRelatedFile> RelatedFiles { get; }
     
        Project AddElement(IProjectElement element);
        Project AddComment(IComment comment);
        //Project AddContributor(Contributor contributor);

        Project AddContributor(IContributor contributor);
    }
}