namespace EaslyController.ReadOnly
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using Easly;
    using EaslyController.Constants;

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
        new IReadOnlyOptionalInner ParentInner { get; }

        /// <summary>
        /// Interface to the optional object for the node.
        /// </summary>
        IOptionalReference Optional { get; }
    }

    /// <summary>
    /// State of an optional node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface IReadOnlyOptionalNodeState<out IInner> : IReadOnlyNodeState<IInner>
        where IInner : IReadOnlyInner<IReadOnlyBrowsingChildIndex>
    {
    }

    /// <inheritdoc/>
    internal class ReadOnlyOptionalNodeState<IInner> : ReadOnlyNodeState<IInner>, IReadOnlyOptionalNodeState<IInner>, IReadOnlyOptionalNodeState, IReadOnlyNodeState
        where IInner : IReadOnlyInner<IReadOnlyBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyOptionalNodeState{IInner}"/> class.
        /// </summary>
        /// <param name="parentIndex">The index used to create the state.</param>
        public ReadOnlyOptionalNodeState(IReadOnlyBrowsingOptionalNodeIndex parentIndex)
            : base(parentIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IReadOnlyBrowsingOptionalNodeIndex ParentIndex { get { return (IReadOnlyBrowsingOptionalNodeIndex)base.ParentIndex; } }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public new IReadOnlyOptionalInner ParentInner { get { return (IReadOnlyOptionalInner)base.ParentInner; } }

        /// <summary>
        /// The node, or null if not assigned.
        /// </summary>
        public override Node Node
        {
            get
            {
                NodeTreeHelperOptional.GetChildNode(Optional, out bool IsAssigned, out Node ChildNode);
                return ChildNode;
            }
        }

        /// <summary>
        /// The comment associated to this state. Null if none.
        /// </summary>
        public override string Comment
        {
            get
            {
                NodeTreeHelperOptional.GetChildNode(Optional, out bool IsAssigned, out Node ChildNode);
                return IsAssigned ? ChildNode.Documentation.Comment : string.Empty;
            }
        }

        /// <summary>
        /// Interface to the optional object for the node.
        /// </summary>
        public IOptionalReference Optional { get { return ParentIndex.Optional; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Browse the optional node in the node tree.
        /// </summary>
        /// <param name="browseContext">The context used to browse the node tree.</param>
        /// <param name="parentInner">The inner containing this state as a child.</param>
        public override void BrowseChildren(ReadOnlyBrowseContext browseContext, IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner)
        {
            NodeTreeHelperOptional.GetChildNode(Optional, out bool IsAssigned, out Node ChildNode);

            if (ChildNode != null)
                BrowseChildrenOfNode(browseContext, ChildNode);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ReadOnlyOptionalNodeState{IInner}"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out ReadOnlyOptionalNodeState<IInner> AsOptionalNodeState))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsOptionalNodeState))
                return comparer.Failed();

            if (!comparer.IsTrue(Optional.IsAssigned == AsOptionalNodeState.Optional.IsAssigned))
                return comparer.Failed();

            if (!comparer.IsSameReference(Node, AsOptionalNodeState.Node))
                return comparer.Failed();

            if (!IsChildrenEqual(comparer, AsOptionalNodeState))
                return comparer.Failed();

            return true;
        }

        /// <summary>
        /// Returns a clone of the node of this state.
        /// </summary>
        /// <returns>The cloned node.</returns>
        public override Node CloneNode()
        {
            Node NewNode = null;

            NodeTreeHelperOptional.GetChildNode(Optional, out bool IsAssigned, out Node ChildNode);
            if (ChildNode != null)
            {
                // Create a clone, initially empty and full of null references.
                NewNode = NodeHelper.CreateEmptyNode(ChildNode.GetType());

                // Clone and assign reference to all nodes, optional or not, list and block lists.
                foreach (KeyValuePair<string, IReadOnlyInner> Entry in InnerTable)
                {
                    string PropertyName = Entry.Key;
                    IReadOnlyInner Inner = Entry.Value;
                    ((IReadOnlyInner<IReadOnlyBrowsingChildIndex>)Inner).CloneChildren(NewNode);
                }

                // Copy other properties.
                foreach (KeyValuePair<string, ValuePropertyType> Entry in ValuePropertyTypeTable)
                {
                    string PropertyName = Entry.Key;
                    ValuePropertyType Type = Entry.Value;
                    bool IsHandled = false;

                    switch (Type)
                    {
                        case ValuePropertyType.Boolean:
                            NodeTreeHelper.CopyBooleanProperty(Node, NewNode, Entry.Key);
                            IsHandled = true;
                            break;

                        case ValuePropertyType.Enum:
                            NodeTreeHelper.CopyEnumProperty(Node, NewNode, Entry.Key);
                            IsHandled = true;
                            break;

                        case ValuePropertyType.String:
                            NodeTreeHelper.CopyStringProperty(Node, NewNode, Entry.Key);
                            IsHandled = true;
                            break;

                        case ValuePropertyType.Guid:
                            NodeTreeHelper.CopyGuidProperty(Node, NewNode, Entry.Key);
                            IsHandled = true;
                            break;
                    }

                    Debug.Assert(IsHandled);
                }

                // Also copy comments.
                NodeTreeHelper.CopyDocumentation(Node, NewNode, cloneCommentGuid: true);
            }

            return NewNode;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"Optional node of {ParentInner.InterfaceType.Name}";
        }
        #endregion

        #region Invariant
        private protected override void CheckInvariant()
        {
            InvariantAssert(IsInitialized);
            InvariantAssert(InnerTable != null);

            foreach (KeyValuePair<string, IReadOnlyInner> Entry in InnerTable)
            {
                IReadOnlyInner Inner = Entry.Value;

                InvariantAssert((Inner is IReadOnlyBlockListInner) || (Inner is IReadOnlyListInner) || (Inner is IReadOnlyOptionalInner) || (Inner is IReadOnlyPlaceholderInner));
                InvariantAssert(Inner.Owner == this);
            }
        }
        #endregion
    }
}
