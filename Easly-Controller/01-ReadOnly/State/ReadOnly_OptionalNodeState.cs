using BaseNode;
using BaseNodeHelper;
using Easly;
using System.Collections.Generic;
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
                NodeTreeHelperOptional.GetChildNode(Optional, out bool IsAssigned, out INode ChildNode);

                if (ChildNode != null)
                    return ChildNode;

                UpdateVirtualNode(ParentInner);
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
        /// <param name="parentInner">The inner containing this state as a child.</param>
        public override void BrowseChildren(IReadOnlyBrowseContext browseNodeContext, IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner)
        {
            Debug.Assert(browseNodeContext != null);
            Debug.Assert(parentInner != null);

            NodeTreeHelperOptional.GetChildNode(parentInner.Owner.Node, parentInner.PropertyName, out bool IsAssigned, out INode ChildNode);
            if (ChildNode == null)
                UpdateVirtualNode(parentInner);

            /*
            NodeTreeHelperOptional.GetChildNode(parentInner.Owner.Node, parentInner.PropertyName, out bool IsAssigned, out INode ChildNode);

            if (ChildNode != null)
                BrowseChildrenOfNode(browseNodeContext, ChildNode);
                */
            BrowseChildrenOfNode(browseNodeContext, Node);
        }

        protected virtual void UpdateVirtualNode(IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner)
        {
            if (VirtualNode == null)
            {
                VirtualNode = NodeHelper.CreateDefault(parentInner.InterfaceType);
                Debug.Assert(VirtualNode != null);

                VirtualNodeHash = NodeHelper.NodeHash(VirtualNode);
            }
            else // Check that the node wasn't modified.
                Debug.Assert(VirtualNodeHash == NodeHelper.NodeHash(VirtualNode));
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

        public override string ToString()
        {
            return $"Optional node of {ParentInner.InterfaceType.Name}";
        }
        #endregion

        #region Invariant
        protected override void CheckInvariant()
        {
            InvariantAssert(IsInitialized);
            InvariantAssert(InnerTable != null);

            foreach (KeyValuePair<string, IReadOnlyInner<IReadOnlyBrowsingChildIndex>> Entry in InnerTable)
            {
                IReadOnlyInner<IReadOnlyBrowsingChildIndex> Inner = Entry.Value;

                InvariantAssert((Inner is IReadOnlyBlockListInner) || (Inner is IReadOnlyListInner) || (Inner is IReadOnlyOptionalInner) || (Inner is IReadOnlyPlaceholderInner));
                InvariantAssert(Inner.Owner == this);
            }
        }
        #endregion
    }
}
