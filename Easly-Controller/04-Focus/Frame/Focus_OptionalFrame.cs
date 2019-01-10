using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Focus for describing an optional child node.
    /// </summary>
    public interface IFocusOptionalFrame : IFrameOptionalFrame, IFocusNamedFrame, IFocusNodeFrame
    {
    }

    /// <summary>
    /// Focus for describing an optional child node.
    /// </summary>
    public class FocusOptionalFrame : FrameOptionalFrame, IFocusOptionalFrame
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
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusOptionalFrame));
            return new FocusContainerCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusNodeStateView)childStateView);
        }
        #endregion
    }
}
