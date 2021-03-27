namespace Coverage
{
    public interface ITree : BaseNode.INode
    {
        ILeaf Placeholder { get; }
        bool ValueBoolean { get; }
        BaseNode.CopySemantic ValueEnum { get; }
        System.Guid ValueGuid { get; }
    }

    [System.Serializable]
    public class Tree : BaseNode.Node, ITree
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public virtual ILeaf Placeholder { get; set; }
        public virtual bool ValueBoolean { get; set; }
        public virtual BaseNode.CopySemantic ValueEnum { get; set; }
        public virtual System.Guid ValueGuid { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
