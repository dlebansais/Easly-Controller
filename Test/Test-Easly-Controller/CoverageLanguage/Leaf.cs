using BaseNode;
using BaseNodeHelper;

namespace Coverage
{
    [System.Serializable]
    public class Leaf : Node
    {
        public Leaf()
            : base(NodeHelper.CreateEmptyDocument())
        {
            Text = string.Empty;
        }

        public Leaf(Document documentation, string text)
            : base(documentation)
        {
            Text = text;
        }

        public virtual string Text { get; set; }
    }
}
