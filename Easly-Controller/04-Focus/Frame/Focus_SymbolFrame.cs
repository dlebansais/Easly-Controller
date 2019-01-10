using EaslyController.Frame;

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
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxVisibleCellView object.
        /// </summary>
        protected override IFrameVisibleCellView CreateFrameCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSymbolFrame));
            return new FocusVisibleCellView((IFocusNodeStateView)stateView);
        }
        #endregion
    }
}
