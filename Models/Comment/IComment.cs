using System;

namespace EPJ
{
    public interface IComment
    {
        string Content { get; set; }
        long ID { get; set; }
        DateTime SubmitionDate { get; set; }
        string Header { get; set; }
    }
}