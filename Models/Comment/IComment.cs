using System;

namespace EPJ
{
    public interface IComment
    {
        string ProjectComment { get; set; }
        uint ProjectID { get; }
        DateTime SubmitDate { get; }
    }
}