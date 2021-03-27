namespace Coverage
{
    public interface ILeaf : BaseNode.INode
    {
        string Text { get; }
    }

    [System.Serializable]
    public class Leaf : BaseNode.Node, ILeaf
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public virtual string Text { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
