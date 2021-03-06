﻿namespace EaslyController.ReadOnly
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    public interface IReadOnlyPlaceholderInner : IReadOnlySingleInner
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        IReadOnlyPlaceholderNodeState ChildState { get; }
    }

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface IReadOnlyPlaceholderInner<out IIndex> : IReadOnlySingleInner<IIndex>
        where IIndex : IReadOnlyBrowsingPlaceholderNodeIndex
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        IReadOnlyPlaceholderNodeState ChildState { get; }
    }

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index as interface.</typeparam>
    /// <typeparam name="TIndex">Type of the index as class.</typeparam>
    internal class ReadOnlyPlaceholderInner<IIndex, TIndex> : ReadOnlySingleInner<IIndex>, IReadOnlyPlaceholderInner<IIndex>, IReadOnlyPlaceholderInner
        where IIndex : IReadOnlyBrowsingPlaceholderNodeIndex
        where TIndex : ReadOnlyBrowsingPlaceholderNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPlaceholderInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public ReadOnlyPlaceholderInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
            ChildState = null;
        }

        /// <summary>
        /// Initializes a newly created state for the node in the inner.
        /// </summary>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>The created node state.</returns>
        public override IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex)
        {
            Debug.Assert(nodeIndex is IReadOnlyBrowsingPlaceholderNodeIndex);
            return InitChildState((IReadOnlyBrowsingPlaceholderNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Initializes a newly created state for the node in the inner.
        /// </summary>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>The created node state.</returns>
        private protected virtual IReadOnlyPlaceholderNodeState InitChildState(IReadOnlyBrowsingPlaceholderNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.PropertyName == PropertyName);
            Debug.Assert(ChildState == null);

            IReadOnlyPlaceholderNodeState State = CreateNodeState(nodeIndex);
            SetChildState(State);

            return State;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Interface type of the node.
        /// </summary>
        public override Type InterfaceType { get { return NodeTreeHelperChild.ChildInterfaceType(Owner.Node, PropertyName); } }

        /// <summary>
        /// The state of the child node.
        /// </summary>
        public IReadOnlyPlaceholderNodeState ChildState { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Creates a clone of the child node of the inner, using <paramref name="parentNode"/> as the parent.
        /// </summary>
        /// <param name="parentNode">The node that will contains a reference to the cloned child upon return.</param>
        public override void CloneChildren(INode parentNode)
        {
            Debug.Assert(parentNode != null);

            // Clone the child recursively.
            INode ChildNodeClone = ChildState.CloneNode();
            Debug.Assert(ChildNodeClone != null);

            // Set the clone in the parent.
            NodeTreeHelperChild.SetChildNode(parentNode, PropertyName, ChildNodeClone);
        }

        /// <summary>
        /// Attach a view to the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        public override void Attach(IReadOnlyControllerView view, IReadOnlyAttachCallbackSet callbackSet)
        {
            ((IReadOnlyNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)ChildState).Attach(view, callbackSet);
        }

        /// <summary>
        /// Detach a view from the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to no longer call when enumerating existing states.</param>
        public override void Detach(IReadOnlyControllerView view, IReadOnlyAttachCallbackSet callbackSet)
        {
            ((IReadOnlyNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)ChildState).Detach(view, callbackSet);
        }
        #endregion

        #region Implementation
        private protected virtual void SetChildState(IReadOnlyPlaceholderNodeState childState)
        {
            Debug.Assert(childState != null);

            ChildState = childState;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected virtual IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyBrowsingPlaceholderNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyPlaceholderInner<IIndex, TIndex>));
            return new ReadOnlyPlaceholderNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>(nodeIndex);
        }
        #endregion
    }
}
