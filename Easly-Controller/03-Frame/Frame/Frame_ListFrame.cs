﻿namespace EaslyController.Frame
{
    using System.Diagnostics;
    using BaseNodeHelper;
    using NotNullReflection;

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
        /// <param name="commentFrameCount">Number of comment frames found so far.</param>
        public override bool IsValid(Type nodeType, FrameTemplateReadOnlyDictionary nodeTemplateTable, ref int commentFrameCount)
        {
            bool IsValid = true;

            IsValid &= base.IsValid(nodeType, nodeTemplateTable, ref commentFrameCount);
            IsValid &= NodeTreeHelperList.IsNodeListProperty(nodeType, PropertyName, out Type ChildNodeType);

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

            IFrameListInner<IFrameBrowsingListNodeIndex> Inner = State.InnerTable[PropertyName] as IFrameListInner<IFrameBrowsingListNodeIndex>;
            Debug.Assert(Inner != null);

            FrameNodeStateViewDictionary StateViewTable = context.ControllerView.StateViewTable;
            FrameCellViewList CellViewList = CreateCellViewList();
            IFrameCellViewCollection EmbeddingCellView = CreateEmbeddingCellView(context.StateView, parentCellView, CellViewList);
            ValidateEmbeddingCellView(context, EmbeddingCellView);

            foreach (IFrameNodeState ChildState in Inner.StateList)
            {
                Debug.Assert(StateViewTable.ContainsKey(ChildState));

                IFrameNodeStateView StateView = context.StateView;
                IFrameNodeStateView ChildStateView = (IFrameNodeStateView)StateViewTable[ChildState];

                Debug.Assert(ChildStateView.RootCellView == null);
                context.SetChildStateView(ChildStateView);
                ChildStateView.BuildRootCellView(context);
                context.RestoreParentStateView(StateView);
                Debug.Assert(ChildStateView.RootCellView != null);

                IFrameContainerCellView FrameCellView = CreateFrameCellView(context.StateView, EmbeddingCellView, ChildStateView);
                ValidateContainerCellView(context.StateView, EmbeddingCellView, ChildStateView, FrameCellView);

                ChildStateView.SetContainerCellView(FrameCellView);

                CellViewList.Add(FrameCellView);
            }

            AssignEmbeddingCellView(context.StateView, EmbeddingCellView);

            return EmbeddingCellView;
        }

        private protected virtual void ValidateEmbeddingCellView(IFrameCellViewTreeContext context, IFrameCellViewCollection embeddingCellView)
        {
            Debug.Assert(embeddingCellView.StateView == context.StateView);
            IFrameCellViewCollection ParentCellView = embeddingCellView.ParentCellView;
        }

        private protected virtual void ValidateContainerCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFrameContainerCellView containerCellView)
        {
            Debug.Assert(containerCellView.StateView == stateView);
            Debug.Assert(containerCellView.ParentCellView == parentCellView);
            Debug.Assert(containerCellView.ChildStateView == childStateView);
        }

        private protected virtual void AssignEmbeddingCellView(IFrameNodeStateView stateView, IFrameAssignableCellView embeddingCellView)
        {
            embeddingCellView.AssignToCellViewTable();
            ((IFrameReplaceableStateView)stateView).AssignCellViewTable(PropertyName, embeddingCellView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        private protected virtual FrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameListFrame>());
            return new FrameCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected virtual IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameListFrame>());
            return new FrameContainerCellView(stateView, parentCellView, childStateView, this);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected abstract IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameCellViewList list);
        #endregion
    }
}
