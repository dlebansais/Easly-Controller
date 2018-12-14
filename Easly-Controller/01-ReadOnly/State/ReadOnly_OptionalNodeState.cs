using BaseNode;
using BaseNodeHelper;
using Easly;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyOptionalNodeState : IReadOnlyNodeState
    {
        new IReadOnlyBrowsingOptionalNodeIndex ParentIndex { get; }
        new IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex> ParentInner { get; }
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
                IOptionalReference Optional = ParentIndex.Optional;

                if (Optional.IsAssigned)
                {
                    INode Node = Optional.AnyItem as INode;
                    Debug.Assert(Node != null);

                    return Node;
                }

                if (VirtualNode == null)
                {
                    VirtualNode = NodeHelper.CreateDefault(ParentInner.InterfaceType);
                    Debug.Assert(VirtualNode != null);
                }

                return VirtualNode;
            }
        }
        private INode VirtualNode;

        public new IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex> ParentInner { get { return (IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex>)base.ParentInner; } }
        public new IReadOnlyBrowsingOptionalNodeIndex ParentIndex { get { return (IReadOnlyBrowsingOptionalNodeIndex)base.ParentIndex; } }
        #endregion

        #region Client Interface
        public override void BrowseChildren(IReadOnlyBrowseContext browseNodeContext)
        {
            Debug.Assert(browseNodeContext != null);

            if (ParentIndex.Optional.IsAssigned)
                base.BrowseChildren(browseNodeContext);
        }
        #endregion
    }
}
