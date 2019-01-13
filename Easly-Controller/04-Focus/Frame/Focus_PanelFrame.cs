using EaslyController.Frame;
using System;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Base frame for displaying more frames.
    /// </summary>
    public interface IFocusPanelFrame : IFramePanelFrame, IFocusFrame, IFocusNodeFrame, IFocusBlockFrame
    {
        /// <summary>
        /// List of frames within this frame.
        /// </summary>
        new IFocusFrameList Items { get; }
    }

    /// <summary>
    /// Base frame for displaying more frames.
    /// </summary>
    public abstract class FocusPanelFrame : FramePanelFrame, IFocusPanelFrame
    {
        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public new IFocusTemplate ParentTemplate { get { return (IFocusTemplate)base.ParentTemplate; } }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public new IFocusFrame ParentFrame { get { return (IFocusFrame)base.ParentFrame; } }

        /// <summary>
        /// List of frames within this frame.
        /// </summary>
        public new IFocusFrameList Items { get { return (IFocusFrameList)base.Items; } }

        /// <summary>
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public IFocusNodeFrameVisibility Visibility { get; set; }

        /// <summary>
        /// Block frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public IFocusBlockFrameVisibility BlockVisibility { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        public override bool IsValid(Type nodeType)
        {
            if (!base.IsValid(nodeType))
                return false;

            if (Visibility != null && !Visibility.IsValid(nodeType))
                return false;

            if (BlockVisibility != null && !BlockVisibility.IsValid(nodeType))
                return false;

            return true;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public override IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            ((IFocusCellViewTreeContext)context).UpdateNodeFrameVisibility(this, out bool OldNodeFrameVisibility);

            IFrameCellViewCollection EmbeddingCellView = base.BuildNodeCells(context, parentCellView) as IFrameCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            IFrameCellView Result;
            if (((IFocusCellViewTreeContext)context).IsVisible)
                Result = EmbeddingCellView;
            else
            {
                Debug.Assert(!EmbeddingCellView.HasVisibleCellView);

                //EmbeddingCellView.ClearCellTree();

                //Result = CreateEmptyCellView(((IFocusCellViewTreeContext)context).StateView);
                Result = EmbeddingCellView;
            }

            ((IFocusCellViewTreeContext)context).RestoreFrameVisibility(OldNodeFrameVisibility);

            return Result;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        public override IFrameCellView BuildBlockCells(IFrameCellViewTreeContext context)
        {
            if (BlockVisibility != null && !BlockVisibility.IsBlockVisible((IFocusCellViewTreeContext)context, this))
            {
                IFocusEmptyCellView EmbeddingCellView = CreateEmptyCellView(((IFocusCellViewTreeContext)context).StateView);
                return EmbeddingCellView;
            }
            else
                return base.BuildBlockCells(context);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFrameList object.
        /// </summary>
        protected override IFrameFrameList CreateItems()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPanelFrame));
            return new FocusFrameList();
        }

        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        protected override IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPanelFrame));
            return new FocusCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPanelFrame));
            return new FocusContainerCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusNodeStateView)childStateView);
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        protected virtual IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPanelFrame));
            return new FocusEmptyCellView(stateView);
        }
        #endregion
    }
}
