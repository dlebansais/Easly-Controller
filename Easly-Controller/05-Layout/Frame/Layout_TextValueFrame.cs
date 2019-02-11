namespace EaslyController.Layout
{
    using System;
    using System.Diagnostics;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Layout describing a string value property in a node.
    /// </summary>
    public interface ILayoutTextValueFrame : IFocusTextValueFrame, ILayoutValueFrame
    {
    }

    /// <summary>
    /// Layout describing a string value property in a node.
    /// </summary>
    public class LayoutTextValueFrame : FocusTextValueFrame, ILayoutTextValueFrame
    {
        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public new ILayoutTemplate ParentTemplate { get { return (ILayoutTemplate)base.ParentTemplate; } }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public new ILayoutFrame ParentFrame { get { return (ILayoutFrame)base.ParentFrame; } }

        /// <summary>
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public new ILayoutNodeFrameVisibility Visibility { get { return (ILayoutNodeFrameVisibility)base.Visibility; } set { base.Visibility = value; } }
        #endregion

        #region Implementation
        /// <summary></summary>
        private protected override void ValidateVisibleCellView(IFrameCellViewTreeContext context, IFrameVisibleCellView cellView)
        {
            Debug.Assert(((ILayoutVisibleCellView)cellView).StateView == ((ILayoutCellViewTreeContext)context).StateView);
            Debug.Assert(((ILayoutVisibleCellView)cellView).Frame == this);
        }

        /// <summary></summary>
        private protected override void ValidateEmptyCellView(IFocusCellViewTreeContext context, IFocusEmptyCellView emptyCellView)
        {
            Debug.Assert(((ILayoutEmptyCellView)emptyCellView).StateView == ((ILayoutCellViewTreeContext)context).StateView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxTextFocusableCellView object.
        /// </summary>
        private protected override IFrameVisibleCellView CreateVisibleCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutTextValueFrame));
            return new LayoutTextFocusableCellView((ILayoutNodeStateView)stateView, this, PropertyName);
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        private protected override IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutTextValueFrame));
            return new LayoutEmptyCellView((ILayoutNodeStateView)stateView);
        }
        #endregion
    }
}
