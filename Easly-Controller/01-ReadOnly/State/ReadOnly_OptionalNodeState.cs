using BaseNode;
using BaseNodeHelper;
using Easly;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// State of an optional node.
    /// </summary>
    public interface IReadOnlyOptionalNodeState : IReadOnlyNodeState
    {
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IReadOnlyBrowsingOptionalNodeIndex ParentIndex { get; }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        new IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex> ParentInner { get; }
    }

    /// <summary>
    /// State of an optional node.
    /// </summary>
    public class ReadOnlyOptionalNodeState : ReadOnlyNodeState, IReadOnlyOptionalNodeState
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyOptionalNodeState"/> class.
        /// </summary>
        /// <param name="parentIndex">The index used to create the state.</param>
        public ReadOnlyOptionalNodeState(IReadOnlyBrowsingOptionalNodeIndex parentIndex)
            : base(parentIndex)
        {
            VirtualNode = null;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The node, or a default object if not assigned.
        /// . If the node is assigned, this property returns the node itself.
        /// . Otherwise, it returns a default object created at the first attempt to read the node. This object should not be modified. Instead it should be replaced by the proper node when that one is created and assigned.
        /// </summary>
        public override INode Node
        {
            get
            {
                IOptionalReference Optional = ParentIndex.Optional;

                if (Optional.IsAssigned)
                {
                    INode Node = Optional.Item as INode;
                    Debug.Assert(Node != null);

                    return Node;
                }

                if (VirtualNode == null)
                {
                    VirtualNode = NodeHelper.CreateDefault(ParentInner.InterfaceType);
                    Debug.Assert(VirtualNode != null);

                    VirtualNodeHash = NodeHelper.NodeHash(VirtualNode);
                }
                else // Check that the node wasn't modified.
                    Debug.Assert(VirtualNodeHash == NodeHelper.NodeHash(VirtualNode));

                return VirtualNode;
            }
        }
        private INode VirtualNode;
        private ulong VirtualNodeHash;

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IReadOnlyBrowsingOptionalNodeIndex ParentIndex { get { return (IReadOnlyBrowsingOptionalNodeIndex)base.ParentIndex; } }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public new IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex> ParentInner { get { return (IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex>)base.ParentInner; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Browse the optional node in the node tree.
        /// </summary>
        /// <param name="browseContext">The context used to browse the node tree.</param>
        public override void BrowseChildren(IReadOnlyBrowseContext browseNodeContext)
        {
            Debug.Assert(browseNodeContext != null);

            if (ParentIndex.Optional.HasItem)
            {
                if (ParentIndex.Optional.IsAssigned)
                    base.BrowseChildren(browseNodeContext);
                else
                {
                    ParentIndex.Optional.Assign();
                    base.BrowseChildren(browseNodeContext);
                    ParentIndex.Optional.Unassign();
                }
            }
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyNodeState"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlyOptionalNodeState AsOptionalNodeState))
                return false;

            if (!base.IsEqual(comparer, AsOptionalNodeState))
                return false;

            // Don't compare virtual nodes, they are allowed to be independant.

            return true;
        }
        #endregion
    }
}
