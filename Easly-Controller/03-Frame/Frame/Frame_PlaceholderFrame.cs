namespace EaslyController.Frame
{
    using System;
    using System.Diagnostics;
    using BaseNodeHelper;

    /// <summary>
    /// Frame for describing an child node.
    /// </summary>
    public interface IFramePlaceholderFrame : IFrameNamedFrame, IFrameNodeFrame
    {
    }

    /// <summary>
    /// Frame for describing an child node.
    /// </summary>
    public class FramePlaceholderFrame : FrameNamedFrame, IFramePlaceholderFrame
    {
        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        public override bool IsValid(Type nodeType, IFrameTemplateReadOnlyDictionary nodeTemplateTable)
        {
            if (!base.IsValid(nodeType, nodeTemplateTable))
                return false;

            if (!NodeTreeHelperChild.IsChildNodeProperty(nodeType, PropertyName, out Type ChildNodeType))
                return false;

            return true;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public virtual IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            IFrameNodeState State = context.StateView.State;
            Debug.Assert(State != null);
            Debug.Assert(State.InnerTable != null);
            Debug.Assert(State.InnerTable.ContainsKey(PropertyName));

            IFramePlaceholderInner<IFrameBrowsingPlaceholderNodeIndex> Inner = State.InnerTable[PropertyName] as IFramePlaceholderInner<IFrameBrowsingPlaceholderNodeIndex>;
            Debug.Assert(Inner != null);
            Debug.Assert(Inner.ChildState != null);
            IFrameNodeState ChildState = Inner.ChildState;

            IFrameStateViewDictionary StateViewTable = context.ControllerView.StateViewTable;
            Debug.Assert(StateViewTable.ContainsKey(ChildState));

            IFrameNodeStateView StateView = context.StateView;
            IFrameNodeStateView ChildStateView = StateViewTable[ChildState];

            Debug.Assert(ChildStateView.RootCellView == null);
            context.SetChildStateView(ChildStateView);
            ChildStateView.BuildRootCellView(context);
            context.RestoreParentStateView(StateView);
            Debug.Assert(ChildStateView.RootCellView != null);

            IFrameContainerCellView EmbeddingCellView = CreateFrameCellView(context.StateView, parentCellView, ChildStateView);
            AssignEmbeddingCellView(context.StateView, EmbeddingCellView);

            return EmbeddingCellView;
        }

        /// <summary></summary>
        private protected virtual void AssignEmbeddingCellView(IFrameNodeStateView stateView, IFrameAssignableCellView embeddingCellView)
        {
            embeddingCellView.AssignToCellViewTable();
            stateView.AssignCellViewTable(PropertyName, embeddingCellView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected virtual IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePlaceholderFrame));
            return new FrameContainerCellView(stateView, parentCellView, childStateView);
        }
        #endregion
    }
}
