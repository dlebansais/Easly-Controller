using System.Collections.Generic;

namespace Coverage
{
    [System.Serializable]
    public class Main : BaseNode.Node
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public virtual Tree PlaceholderTree { get; set; }
        public virtual Leaf PlaceholderLeaf { get; set; }
        public virtual Easly.OptionalReference<Leaf> UnassignedOptionalLeaf { get; set; }
        public virtual Easly.OptionalReference<Leaf> EmptyOptionalLeaf { get; set; }
        public virtual Easly.OptionalReference<Tree> AssignedOptionalTree { get; set; }
        public virtual Easly.OptionalReference<Leaf> AssignedOptionalLeaf { get; set; }
        public virtual BaseNode.BlockList<Leaf> LeafBlocks { get; set; }
        public virtual IList<Leaf> LeafPath { get; set; }
        public virtual bool ValueBoolean { get; set; }
        public virtual BaseNode.CopySemantic ValueEnum { get; set; }
        public virtual string ValueString { get; set; }
        public virtual System.Guid ValueGuid { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
