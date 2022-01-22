namespace Coverage
{
    using System.Collections.Generic;
    using BaseNode;
    using Easly;

    [System.Serializable]
    public class Root : Node
    {
        public Root(Document documentation)
            : base(documentation)
        {
            UnassignedOptionalMain = new OptionalReference<Main>();
            LeafPathH = new List<Leaf>();
            LeafPathV = new List<Leaf>();
            UnassignedOptionalLeaf = new OptionalReference<Leaf>();
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public virtual IBlockList<Main> MainBlocksH { get; set; }
        public virtual IBlockList<Main> MainBlocksV { get; set; }
        public virtual IOptionalReference<Main> UnassignedOptionalMain { get; set; }
        public virtual string ValueString { get; set; }
        public virtual IList<Leaf> LeafPathH { get; set; }
        public virtual IList<Leaf> LeafPathV { get; set; }
        public virtual IOptionalReference<Leaf> UnassignedOptionalLeaf { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
