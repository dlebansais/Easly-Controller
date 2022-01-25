using System.Collections.Generic;
using BaseNode;
using BaseNodeHelper;
using Easly;

namespace Coverage
{
    [System.Serializable]
    public class Main : Node
    {
        public Main()
            : base(NodeHelper.CreateEmptyDocumentation())
        {
            UnassignedOptionalLeaf = new OptionalReference<Leaf>(new Leaf());
            EmptyOptionalLeaf = new OptionalReference<Leaf>(new Leaf());
            AssignedOptionalTree = new OptionalReference<Tree>(new Tree());
            AssignedOptionalLeaf = new OptionalReference<Leaf>(new Leaf());
            //LeafBlocks = new BlockList<Leaf>();
            LeafPath = new List<Leaf>();
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public virtual Tree PlaceholderTree { get; set; }
        public virtual Leaf PlaceholderLeaf { get; set; }
        public virtual IOptionalReference<Leaf> UnassignedOptionalLeaf { get; set; }
        public virtual IOptionalReference<Leaf> EmptyOptionalLeaf { get; set; }
        public virtual IOptionalReference<Tree> AssignedOptionalTree { get; set; }
        public virtual IOptionalReference<Leaf> AssignedOptionalLeaf { get; set; }
        public virtual IBlockList<Leaf> LeafBlocks { get; set; }
        public virtual IList<Leaf> LeafPath { get; set; }
        public virtual bool ValueBoolean { get; set; }
        public virtual CopySemantic ValueEnum { get; set; }
        public virtual string ValueString { get; set; }
        public virtual System.Guid ValueGuid { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
