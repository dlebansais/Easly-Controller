using System.Collections.Generic;
using BaseNode;

namespace Coverage
{
    [System.Serializable]
    public class Root : Node
    {
        public Root(Document documentation)
            : base(documentation)
        {
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public virtual BaseNode.IBlockList<Main> MainBlocksH { get; set; }
        public virtual BaseNode.IBlockList<Main> MainBlocksV { get; set; }
        public virtual Easly.IOptionalReference<Main> UnassignedOptionalMain { get; set; }
        public virtual string ValueString { get; set; }
        public virtual IList<Leaf> LeafPathH { get; set; }
        public virtual IList<Leaf> LeafPathV { get; set; }
        public virtual Easly.IOptionalReference<Leaf> UnassignedOptionalLeaf { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
