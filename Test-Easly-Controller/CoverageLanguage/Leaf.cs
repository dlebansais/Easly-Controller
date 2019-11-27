namespace Coverage
{
    public interface ILeaf : BaseNode.INode
    {
        string Text { get; }
    }

    [System.Serializable]
    public class Leaf : BaseNode.Node, ILeaf
    {
        public virtual string Text { get; set; }
    }
}
