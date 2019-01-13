using EaslyController.Frame;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Focus describing a string value property in a node.
    /// </summary>
    public interface IFocusTextValueFrame : IFrameTextValueFrame, IFocusValueFrame
    {
    }

    /// <summary>
    /// Focus describing a string value property in a node.
    /// </summary>
    public class FocusTextValueFrame : FrameTextValueFrame, IFocusTextValueFrame
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
            ((IFocusCellViewTreeContext)context).UpdateNodeFrameVisibility(this, out bool OldNodeFrameVisibility);

            IFrameCellView Result;
            if (((IFocusCellViewTreeContext)context).IsVisible)
                Result = base.BuildNodeCells(context, parentCellView);
            else
                Result = CreateEmptyCellView(((IFocusCellViewTreeContext)context).StateView);

            ((IFocusCellViewTreeContext)context).RestoreFrameVisibility(OldNodeFrameVisibility);

            return Result;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxTextFocusableCellView object.
        /// </summary>
        protected override IFrameVisibleCellView CreateFrameCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTextValueFrame));
            return new FocusTextFocusableCellView((IFocusNodeStateView)stateView, this, PropertyName);
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        protected virtual IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTextValueFrame));
            return new FocusEmptyCellView(stateView);
        }
        #endregion
    }
}
