using EaslyController.Frame;
using System;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Base frame for a list of nodes.
    /// </summary>
    public interface IFocusListFrame : IFrameListFrame, IFocusNamedFrame, IFocusNodeFrame
    {
        /// <summary>
        /// True if the associated collection is never empty.
        /// (Set in Xaml)
        /// </summary>
        bool IsNeverEmpty { get; set; }
    }

    /// <summary>
    /// Base frame for a list of nodes.
    /// </summary>
    public abstract class FocusListFrame : FrameListFrame, IFocusListFrame
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

            if (!((IFocusCellViewTreeContext)context).IsVisible)
            {
                foreach (IFocusCellView CellView in EmbeddingCellView.CellViewList)
                {
                    IFocusBlockCellView AsBlockCellView = CellView as IFocusBlockCellView;
                    Debug.Assert(AsBlockCellView != null);
                    Debug.Assert(AsBlockCellView.BlockStateView != null);
                    Debug.Assert(AsBlockCellView.BlockStateView.RootCellView is IFocusEmptyCellView);
                }
            }

            ((IFocusCellViewTreeContext)context).RestoreFrameVisibility(OldNodeFrameVisibility);

            return EmbeddingCellView;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        protected override IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusListFrame));
            return new FocusCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusListFrame));
            return new FocusContainerCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusNodeStateView)childStateView);
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        protected virtual IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusListFrame));
            return new FocusEmptyCellView(stateView);
        }
        #endregion
    }
}
