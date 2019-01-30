namespace EaslyController.Frame
{
    using System;
    using System.Diagnostics;
    using BaseNodeHelper;

    /// <summary>
    /// Base frame for a list of nodes.
    /// </summary>
    public interface IFrameListFrame : IFrameNamedFrame, IFrameNodeFrame
    {
    }

    /// <summary>
    /// Base frame for a list of nodes.
    /// </summary>
    public abstract class FrameListFrame : FrameNamedFrame, IFrameListFrame
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

            if (!NodeTreeHelperList.IsNodeListProperty(nodeType, PropertyName, out Type ChildNodeType))
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

            IFrameListInner<IFrameBrowsingListNodeIndex> Inner = State.InnerTable[PropertyName] as IFrameListInner<IFrameBrowsingListNodeIndex>;
            Debug.Assert(Inner != null);

            IFrameStateViewDictionary StateViewTable = context.ControllerView.StateViewTable;
            IFrameCellViewList CellViewList = CreateCellViewList();
            IFrameCellViewCollection EmbeddingCellView = CreateEmbeddingCellView(context.StateView, CellViewList);

            foreach (IFrameNodeState ChildState in Inner.StateList)
            {
                Debug.Assert(StateViewTable.ContainsKey(ChildState));

                IFrameNodeStateView StateView = context.StateView;
                IFrameNodeStateView ChildStateView = StateViewTable[ChildState];

                Debug.Assert(ChildStateView.RootCellView == null);
                context.SetChildStateView(ChildStateView);
                ChildStateView.BuildRootCellView(context);
                context.RestoreParentStateView(StateView);
                Debug.Assert(ChildStateView.RootCellView != null);

                IFrameCellView FrameCellView = CreateFrameCellView(context.StateView, EmbeddingCellView, ChildStateView);
                CellViewList.Add(FrameCellView);
            }

            AssignEmbeddingCellView(context.StateView, EmbeddingCellView);

            return EmbeddingCellView;
        }

        /// <summary></summary>
        protected virtual void AssignEmbeddingCellView(IFrameNodeStateView stateView, IFrameAssignableCellView embeddingCellView)
        {
            embeddingCellView.AssignToCellViewTable();
            stateView.AssignCellViewTable(PropertyName, embeddingCellView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        protected virtual IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameListFrame));
            return new FrameCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        protected virtual IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameListFrame));
            return new FrameContainerCellView(stateView, parentCellView, childStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        protected abstract IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list);
        #endregion
    }
}
