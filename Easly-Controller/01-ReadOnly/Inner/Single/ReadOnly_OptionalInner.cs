using BaseNode;
using BaseNodeHelper;
using System;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    public interface IReadOnlyOptionalInner : IReadOnlySingleInner
    {
        /// <summary>
        /// True if the optional node is provided.
        /// </summary>
        bool IsAssigned { get; }
    }

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    public interface IReadOnlyOptionalInner<out IIndex> : IReadOnlySingleInner<IIndex>
        where IIndex : IReadOnlyBrowsingOptionalNodeIndex
    {
        /// <summary>
        /// True if the optional node is provided.
        /// </summary>
        bool IsAssigned { get; }
    }

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    public class ReadOnlyOptionalInner<IIndex, TIndex> : ReadOnlySingleInner<IIndex>, IReadOnlyOptionalInner<IIndex>, IReadOnlyOptionalInner
        where IIndex : IReadOnlyBrowsingOptionalNodeIndex
        where TIndex : ReadOnlyBrowsingOptionalNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyOptionalInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public ReadOnlyOptionalInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
            _ChildState = null;
        }

        /// <summary>
        /// Initializes a newly created state for the node in the inner, provided or not.
        /// </summary>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>The created node state.</returns>
        public override IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex)
        {
            Debug.Assert(nodeIndex is IReadOnlyBrowsingOptionalNodeIndex);
            return InitChildState((IReadOnlyBrowsingOptionalNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Initializes a newly created state for the node in the inner, provided or not.
        /// </summary>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>The created node state.</returns>
        protected virtual IReadOnlyOptionalNodeState InitChildState(IReadOnlyBrowsingOptionalNodeIndex nodeIndex)
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
        public override Type InterfaceType { get { return NodeTreeHelper.OptionalChildInterfaceType(Owner.Node, PropertyName); } }

        /// <summary>
        /// True if the optional node is provided.
        /// </summary>
        public bool IsAssigned { get { return NodeTreeHelper.IsChildNodeAssigned(Owner.Node, PropertyName); } }

        /// <summary>
        /// The state of the optional node.
        /// </summary>
        public override IReadOnlyNodeState ChildState { get { return _ChildState; } }
        private IReadOnlyOptionalNodeState _ChildState;
        #endregion

        #region Client Interface
        /// <summary>
        /// Creates a clone of the optional node of the inner, using <paramref name="parentNode"/> as the parent.
        /// </summary>
        /// <param name="parentNode">The node that will contains a reference to the cloned child upon return.</param>
        public override void CloneChildren(INode parentNode)
        {
            Debug.Assert(parentNode != null);

            // Clone the child recursively, assigned or not.
            INode ChildNodeClone = ChildState.CloneNode();
            Debug.Assert(ChildNodeClone != null);

            // Set the clone in the parent, assigned or not.
            NodeHelper.InitializeOptionalChildNode(parentNode, PropertyName, ChildNodeClone);

            // If the original is assigned, set the clone as assigned too.
            if (IsAssigned)
                NodeTreeHelper.AssignChildNode(parentNode, PropertyName);
        }

        /// <summary>
        /// Attach a view to the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        public override void Attach(IReadOnlyControllerView view, IReadOnlyAttachCallbackSet callbackSet)
        {
            ChildState.Attach(view, callbackSet);
        }
        #endregion

        #region Implementation
        protected virtual void SetChildState(IReadOnlyOptionalNodeState childState)
        {
            Debug.Assert(childState != null);
            Debug.Assert(ChildState == null);

            _ChildState = childState;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxOptionalNodeState object.
        /// </summary>
        protected virtual IReadOnlyOptionalNodeState CreateNodeState(IReadOnlyBrowsingOptionalNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyOptionalInner<IIndex, TIndex>));
            return new ReadOnlyOptionalNodeState(nodeIndex);
        }
        #endregion
    }
}
