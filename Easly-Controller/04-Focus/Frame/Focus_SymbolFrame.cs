using EaslyController.Frame;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Focus for decoration purpose only.
    /// </summary>
    public interface IFocusSymbolFrame : IFrameSymbolFrame, IFocusStaticFrame
    {
    }

    /// <summary>
    /// Focus for decoration purpose only.
    /// </summary>
    public class FocusSymbolFrame : FrameSymbolFrame, IFocusSymbolFrame
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
            if (Visibility != null && !Visibility.IsVisible((IFocusCellViewTreeContext)context, this))
            {
                IFocusEmptyCellView EmbeddingCellView = CreateEmptyCellView(((IFocusCellViewTreeContext)context).StateView);
                return EmbeddingCellView;
            }
            else
                return base.BuildNodeCells(context, parentCellView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxVisibleCellView object.
        /// </summary>
        protected override IFrameVisibleCellView CreateFrameCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSymbolFrame));
            return new FocusVisibleCellView((IFocusNodeStateView)stateView, this);
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        protected virtual IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSymbolFrame));
            return new FocusEmptyCellView(stateView);
        }
        #endregion
    }
}
