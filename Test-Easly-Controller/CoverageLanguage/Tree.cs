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
        public virtual ILeaf Placeholder { get; set; }
        public virtual bool ValueBoolean { get; set; }
        public virtual BaseNode.CopySemantic ValueEnum { get; set; }
        public virtual System.Guid ValueGuid { get; set; }
    }
}
