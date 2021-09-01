﻿namespace EaslyController.ReadOnly
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    public interface IReadOnlyOptionalInner : IReadOnlySingleInner
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        IReadOnlyOptionalNodeState ChildState { get; }

        /// <summary>
        /// True if the optional node is provided.
        /// </summary>
        bool IsAssigned { get; }
    }

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    /// <typeparam name="TIndex">Type of the index.</typeparam>
    internal interface IReadOnlyOptionalInner<out TIndex> : IReadOnlySingleInner<TIndex>
        where TIndex : ReadOnlyBrowsingOptionalNodeIndex
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        IReadOnlyOptionalNodeState ChildState { get; }

        /// <summary>
        /// True if the optional node is provided.
        /// </summary>
        bool IsAssigned { get; }
    }
    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    /// <typeparam name="TIndex">Type of the index as class.</typeparam>
    internal class ReadOnlyOptionalInner<TIndex> : ReadOnlySingleInner<TIndex>, IReadOnlyOptionalInner<TIndex>, IReadOnlyOptionalInner, IReadOnlySingleInner
        where TIndex : ReadOnlyBrowsingOptionalNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyOptionalInner{TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public ReadOnlyOptionalInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
            ChildState = null;
        }

        /// <summary>
        /// Initializes a newly created state for the node in the inner, provided or not.
        /// </summary>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>The created node state.</returns>
        public override IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex)
        {
            Debug.Assert(nodeIndex is ReadOnlyBrowsingOptionalNodeIndex);
            return InitChildState((ReadOnlyBrowsingOptionalNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Initializes a newly created state for the node in the inner, provided or not.
        /// </summary>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>The created node state.</returns>
        private protected virtual IReadOnlyOptionalNodeState InitChildState(ReadOnlyBrowsingOptionalNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.PropertyName == PropertyName);
            Debug.Assert(ChildState == null);

            IReadOnlyOptionalNodeState State = CreateNodeState(nodeIndex);
            SetChildState(State);

            return State;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Interface type of the node.
        /// </summary>
        public override Type InterfaceType { get { return NodeTreeHelperOptional.OptionalChildInterfaceType(Owner.Node, PropertyName); } }

        /// <summary>
        /// True if the optional node is provided.
        /// </summary>
        public bool IsAssigned { get { return NodeTreeHelperOptional.IsChildNodeAssigned(Owner.Node, PropertyName); } }

        /// <summary>
        /// The state of the optional node.
        /// </summary>
        public IReadOnlyOptionalNodeState ChildState { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Creates a clone of the optional node of the inner, using <paramref name="parentNode"/> as the parent.
        /// </summary>
        /// <param name="parentNode">The node that will contains a reference to the cloned child upon return.</param>
        public override void CloneChildren(Node parentNode)
        {
            Debug.Assert(parentNode != null);

            // Clone the child recursively.
            Node ChildNodeClone = ChildState.CloneNode();

            // If the original is set, set the clone too.
            if (ChildNodeClone != null)
            {
                NodeTreeHelperOptional.SetOptionalChildNode(parentNode, PropertyName, ChildNodeClone);
                if (!IsAssigned)
                    NodeTreeHelperOptional.UnassignChildNode(parentNode, PropertyName);
            }
        }

        /// <summary>
        /// Attach a view to the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        public override void Attach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet)
        {
            ((ReadOnlyOptionalNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)ChildState).Attach(view, callbackSet);
        }

        /// <summary>
        /// Detach a view from the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to no longer call when enumerating existing states.</param>
        public override void Detach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet)
        {
            ((ReadOnlyOptionalNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)ChildState).Detach(view, callbackSet);
        }
        #endregion

        #region Implementation
        private protected virtual void SetChildState(IReadOnlyOptionalNodeState childState)
        {
            Debug.Assert(childState != null);

            ChildState = childState;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxOptionalNodeState object.
        /// </summary>
        private protected virtual IReadOnlyOptionalNodeState CreateNodeState(ReadOnlyBrowsingOptionalNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyOptionalInner<TIndex>));
            return new ReadOnlyOptionalNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>(nodeIndex);
        }
        #endregion
    }
}
