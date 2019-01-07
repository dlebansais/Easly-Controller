﻿using BaseNode;
using BaseNodeHelper;
using Easly;
using System;
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

        /// <summary>
        /// Interface to the optional object for the node.
        /// </summary>
        IOptionalReference Optional { get; }
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
        public new IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex> ParentInner { get { return (IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex>)base.ParentInner; } }

        /// <summary>
        /// The node, or null if not assigned.
        /// </summary>
        public override INode Node
        {
            get
            {
                NodeTreeHelperOptional.GetChildNode(Optional, out bool IsAssigned, out INode ChildNode);
                return ChildNode;
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
        public override void BrowseChildren(IReadOnlyBrowseContext browseContext, IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner)
        {
            Debug.Assert(browseContext != null);
            Debug.Assert(parentInner != null);

            NodeTreeHelperOptional.GetChildNode(Optional, out bool IsAssigned, out INode ChildNode);

            if (ChildNode != null)
                BrowseChildrenOfNode(browseContext, ChildNode);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyNodeState"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlyOptionalNodeState AsOptionalNodeState))
                return false;

            if (!base.IsEqual(comparer, AsOptionalNodeState))
                return false;

            if (Optional.IsAssigned != AsOptionalNodeState.Optional.IsAssigned)
                return false;

            if (Optional.HasItem)
            {
                if (Node != AsOptionalNodeState.Node)
                    return false;

                if (!IsChildrenEqual(comparer, AsOptionalNodeState))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns a clone of the node of this state.
        /// </summary>
        /// <returns>The cloned node.</returns>
        public override INode CloneNode()
        {
            NodeTreeHelperOptional.GetChildNode(Optional, out bool IsAssigned, out INode ChildNode);
            if (ChildNode != null)
            {
                // Create a clone, initially empty and full of null references.
                INode NewNode = NodeHelper.CreateEmptyNode(ChildNode.GetType());

                // Clone and assign reference to all nodes, optional or not, list and block lists.
                foreach (KeyValuePair<string, IReadOnlyInner<IReadOnlyBrowsingChildIndex>> Entry in InnerTable)
                {
                    string PropertyName = Entry.Key;
                    IReadOnlyInner<IReadOnlyBrowsingChildIndex> Inner = Entry.Value;
                    Inner.CloneChildren(NewNode);
                }

                // Copy other properties.
                foreach (KeyValuePair<string, ValuePropertyType> Entry in ValuePropertyTypeTable)
                {
                    string PropertyName = Entry.Key;
                    ValuePropertyType Type = Entry.Value;

                    switch (Type)
                    {
                        case ValuePropertyType.Boolean:
                            NodeTreeHelper.CopyBooleanProperty(Node, NewNode, Entry.Key);
                            break;
                        case ValuePropertyType.Enum:
                            NodeTreeHelper.CopyEnumProperty(Node, NewNode, Entry.Key);
                            break;
                        case ValuePropertyType.String:
                            NodeTreeHelper.CopyStringProperty(Node, NewNode, Entry.Key);
                            break;
                        case ValuePropertyType.Guid:
                            NodeTreeHelper.CopyGuidProperty(Node, NewNode, Entry.Key);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(Type));
                    }
                }

                // Also copy comments.
                NodeTreeHelper.CopyDocumentation(Node, NewNode);

                return NewNode;
            }
            else
                return null;
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
