using EaslyController.Frame;
using System;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Base frame for a list of nodes displayed horizontally.
    /// </summary>
    public interface IFocusHorizontalListFrame : IFrameHorizontalListFrame, IFocusListFrame
    {
    }

    /// <summary>
    /// Base frame for a list of nodes displayed horizontally.
    /// </summary>
    public class FocusHorizontalListFrame : FrameHorizontalListFrame, IFocusHorizontalListFrame
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
        /// True if the associated collection is never empty.
        /// (Set in Xaml)
        /// </summary>
        public bool IsNeverEmpty { get; set; }

        /// <summary>
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public IFocusNodeFrameVisibility Visibility { get; set; }
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
                foreach (IFocusCellView CellView in EmbeddingCellView.CellViewList)
                {
                    IFocusContainerCellView AsContainerCellView = CellView as IFocusContainerCellView;
                    Debug.Assert(AsContainerCellView != null);
                    Debug.Assert(AsContainerCellView.ChildStateView != null);
                    Debug.Assert(AsContainerCellView.ChildStateView.RootCellView is IFocusEmptyCellView);
                }

                //EmbeddingCellView.ClearCellTree();

                //Result = CreateEmptyCellView(((IFocusCellViewTreeContext)context).StateView);
                Result = EmbeddingCellView;
            }

            ((IFocusCellViewTreeContext)context).RestoreFrameVisibility(OldNodeFrameVisibility);

            return Result;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        protected override IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusHorizontalListFrame));
            return new FocusCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusHorizontalListFrame));
            return new FocusContainerCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusNodeStateView)childStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusHorizontalListFrame));
            return new FocusLine((IFocusNodeStateView)stateView, (IFocusCellViewList)list);
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        protected virtual IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusHorizontalListFrame));
            return new FocusEmptyCellView(stateView);
        }
        #endregion
    }
}
