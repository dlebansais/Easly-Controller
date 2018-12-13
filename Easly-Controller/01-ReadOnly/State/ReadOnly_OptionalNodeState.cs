using BaseNode;
using BaseNodeHelper;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyOptionalNodeState : IReadOnlyNodeState
    {
        new IReadOnlyBrowsingOptionalNodeIndex ParentIndex { get; }
        new IReadOnlyOptionalInner ParentInner { get; }
    }

    public class ReadOnlyOptionalNodeState : ReadOnlyNodeState, IReadOnlyOptionalNodeState
    {
        #region Init
        public ReadOnlyOptionalNodeState(IReadOnlyBrowsingOptionalNodeIndex nodeIndex)
            : base(nodeIndex)
        {
            VirtualNode = null;
        }
        #endregion

        #region Properties
        public override INode Node
        {
            get
            {
                INode ParentNode = ParentIndex.ParentNode;
                string PropertyName = ParentIndex.PropertyName;
                NodeTreeHelper.GetChildNode(ParentNode, PropertyName, out bool IsAssigned, out INode ChildNode);

                if (IsAssigned)
                    return ChildNode;

                if (VirtualNode == null)
                {
                    VirtualNode = NodeHelper.CreateDefault(ParentInner.InterfaceType);
                    Debug.Assert(VirtualNode != null);
                }

                return VirtualNode;
            }
        }
        private INode VirtualNode;

        public new IReadOnlyOptionalInner ParentInner { get { return (IReadOnlyOptionalInner)base.ParentInner; } }
        public new IReadOnlyBrowsingOptionalNodeIndex ParentIndex { get { return (IReadOnlyBrowsingOptionalNodeIndex)base.ParentIndex; } }
        #endregion

        #region Client Interface
        public override void BrowseChildren(IReadOnlyBrowseContext browseNodeContext)
        {
            Debug.Assert(browseNodeContext != null);

            INode ParentNode = ParentIndex.ParentNode;
            string PropertyName = ParentIndex.PropertyName;
            NodeTreeHelper.GetChildNode(ParentNode, PropertyName, out bool IsAssigned, out INode ChildNode);

            if (IsAssigned)
                base.BrowseChildren(browseNodeContext);
        }
        #endregion
    }
}
