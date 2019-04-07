namespace EaslyController.Frame
{
    using System;
    using System.Diagnostics;
    using BaseNodeHelper;

    /// <summary>
    /// Frame for describing an optional child node.
    /// </summary>
    public interface IFrameOptionalFrame : IFrameNamedFrame, IFrameNodeFrame
    {
    }

    /// <summary>
    /// Frame for describing an optional child node.
    /// </summary>
    public class FrameOptionalFrame : FrameNamedFrame, IFrameOptionalFrame
    {
        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        /// <param name="commentFrameCount">Number of comment frames found so far.</param>
        public override bool IsValid(Type nodeType, IFrameTemplateReadOnlyDictionary nodeTemplateTable, ref int commentFrameCount)
        {
            bool IsValid = true;

            IsValid &= base.IsValid(nodeType, nodeTemplateTable, ref commentFrameCount);
            IsValid &= NodeTreeHelperOptional.IsOptionalChildNodeProperty(nodeType, PropertyName, out Type ChildNodeType);

            Debug.Assert(IsValid);
            return IsValid;
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

            IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex> Inner = State.InnerTable[PropertyName] as IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex>;
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
            ValidateContainerCellView(context.StateView, parentCellView, ChildStateView, EmbeddingCellView);

            ChildStateView.SetContainerCellView(EmbeddingCellView);

            AssignEmbeddingCellView(context.StateView, EmbeddingCellView);

            return EmbeddingCellView;
        }

        /// <summary></summary>
        private protected virtual void ValidateContainerCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFrameContainerCellView containerCellView)
        {
            Debug.Assert(containerCellView.StateView == stateView);
            Debug.Assert(containerCellView.ParentCellView == parentCellView);
            Debug.Assert(containerCellView.ChildStateView == childStateView);
        }

        /// <summary></summary>
        private protected virtual void AssignEmbeddingCellView(IFrameNodeStateView stateView, IFrameAssignableCellView embeddingCellView)
        {
            embeddingCellView.AssignToCellViewTable();
            ((IFrameReplaceableStateView)stateView).AssignCellViewTable(PropertyName, embeddingCellView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected virtual IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameOptionalFrame));
            return new FrameContainerCellView(stateView, parentCellView, childStateView, this);
        }
        #endregion
    }
}
