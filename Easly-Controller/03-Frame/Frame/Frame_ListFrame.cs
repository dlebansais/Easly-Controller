﻿using BaseNodeHelper;
using System;
using System.Diagnostics;

namespace EaslyController.Frame
{
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
        public override bool IsValid(Type nodeType)
        {
            if (!base.IsValid(nodeType))
                return false;

            return NodeTreeHelperList.IsNodeListProperty(nodeType, PropertyName, out Type ChildNodeType);
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public virtual IFrameCellView BuildNodeCells(IFrameControllerView controllerView, IFrameNodeStateView stateView, IFrameMutableCellViewCollection parentCellView)
        {
            IFrameNodeState State = stateView.State;
            Debug.Assert(State != null);
            Debug.Assert(State.InnerTable != null);
            Debug.Assert(State.InnerTable.ContainsKey(PropertyName));

            IFrameListInner<IFrameBrowsingListNodeIndex> Inner = State.InnerTable[PropertyName] as IFrameListInner<IFrameBrowsingListNodeIndex>;
            Debug.Assert(Inner != null);

            IFrameStateViewDictionary StateViewTable = controllerView.StateViewTable;
            IFrameCellViewList CellViewList = CreateCellViewList();
            IFrameMutableCellViewCollection EmbeddingCellView = CreateEmbeddingCellView(stateView, CellViewList);

            foreach (IFrameNodeState ChildState in Inner.StateList)
            {
                Debug.Assert(StateViewTable.ContainsKey(ChildState));

                IFrameNodeStateView ChildStateView = StateViewTable[ChildState];
                IFrameCellView FrameCellView = CreateFrameCellView(stateView, EmbeddingCellView, ChildStateView);
                CellViewList.Add(FrameCellView);
            }

            stateView.AssignCellViewTable(PropertyName, EmbeddingCellView);

            return EmbeddingCellView;
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
        protected virtual IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameMutableCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameListFrame));
            return new FrameContainerCellView(stateView, parentCellView, childStateView);
        }

        /// <summary>
        /// Creates a IxxxMutableCellViewCollection object.
        /// </summary>
        protected abstract IFrameMutableCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list);
        #endregion
    }
}
