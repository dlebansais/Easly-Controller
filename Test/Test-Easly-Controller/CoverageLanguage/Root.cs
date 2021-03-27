using System.Collections.Generic;

namespace Coverage
{
    public interface IRoot : BaseNode.INode
    {
        BaseNode.IBlockList<IMain, Main> MainBlocksH { get; }
        BaseNode.IBlockList<IMain, Main> MainBlocksV { get; }
        Easly.IOptionalReference<IMain> UnassignedOptionalMain { get; }
        string ValueString { get; }
        IList<ILeaf> LeafPathH { get; }
        IList<ILeaf> LeafPathV { get; }
        Easly.IOptionalReference<ILeaf> UnassignedOptionalLeaf { get; }
    }

    [System.Serializable]
    public class Root : BaseNode.Node, IRoot
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public virtual BaseNode.IBlockList<IMain, Main> MainBlocksH { get; set; }
        public virtual BaseNode.IBlockList<IMain, Main> MainBlocksV { get; set; }
        public virtual Easly.IOptionalReference<IMain> UnassignedOptionalMain { get; set; }
        public virtual string ValueString { get; set; }
        public virtual IList<ILeaf> LeafPathH { get; set; }
        public virtual IList<ILeaf> LeafPathV { get; set; }
        public virtual Easly.IOptionalReference<ILeaf> UnassignedOptionalLeaf { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
