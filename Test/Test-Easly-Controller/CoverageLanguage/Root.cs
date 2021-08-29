using System.Collections.Generic;

namespace Coverage
{
    [System.Serializable]
    public class Root : BaseNode.Node
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public virtual BaseNode.BlockList<Main> MainBlocksH { get; set; }
        public virtual BaseNode.BlockList<Main> MainBlocksV { get; set; }
        public virtual Easly.OptionalReference<Main> UnassignedOptionalMain { get; set; }
        public virtual string ValueString { get; set; }
        public virtual IList<Leaf> LeafPathH { get; set; }
        public virtual IList<Leaf> LeafPathV { get; set; }
        public virtual Easly.OptionalReference<Leaf> UnassignedOptionalLeaf { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
