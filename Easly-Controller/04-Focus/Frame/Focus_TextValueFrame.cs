using EaslyController.Frame;

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
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxTextFocusableCellView object.
        /// </summary>
        protected override IFrameVisibleCellView CreateFrameCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTextValueFrame));
            return new FocusTextFocusableCellView((IFocusNodeStateView)stateView);
        }
        #endregion
    }
}
