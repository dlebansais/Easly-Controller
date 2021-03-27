using System.Collections.Generic;

namespace Coverage
{
    public interface IMain : BaseNode.INode
    {
        ITree PlaceholderTree { get; }
        ILeaf PlaceholderLeaf { get; }
        Easly.IOptionalReference<ILeaf> UnassignedOptionalLeaf { get; }
        Easly.IOptionalReference<ILeaf> EmptyOptionalLeaf { get; }
        Easly.IOptionalReference<ITree> AssignedOptionalTree { get; }
        Easly.IOptionalReference<ILeaf> AssignedOptionalLeaf { get; }
        BaseNode.IBlockList<ILeaf, Leaf> LeafBlocks { get; }
        IList<ILeaf> LeafPath { get; }
        bool ValueBoolean { get; }
        BaseNode.CopySemantic ValueEnum { get; }
        string ValueString { get; }
        System.Guid ValueGuid { get; }
    }

    [System.Serializable]
    public class Main : BaseNode.Node, IMain
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public virtual ITree PlaceholderTree { get; set; }
        public virtual ILeaf PlaceholderLeaf { get; set; }
        public virtual Easly.IOptionalReference<ILeaf> UnassignedOptionalLeaf { get; set; }
        public virtual Easly.IOptionalReference<ILeaf> EmptyOptionalLeaf { get; set; }
        public virtual Easly.IOptionalReference<ITree> AssignedOptionalTree { get; set; }
        public virtual Easly.IOptionalReference<ILeaf> AssignedOptionalLeaf { get; set; }
        public virtual BaseNode.IBlockList<ILeaf, Leaf> LeafBlocks { get; set; }
        public virtual IList<ILeaf> LeafPath { get; set; }
        public virtual bool ValueBoolean { get; set; }
        public virtual BaseNode.CopySemantic ValueEnum { get; set; }
        public virtual string ValueString { get; set; }
        public virtual System.Guid ValueGuid { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
