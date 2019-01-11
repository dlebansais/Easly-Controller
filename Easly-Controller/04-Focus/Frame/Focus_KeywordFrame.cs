using EaslyController.Frame;
using System.Windows.Markup;

namespace EaslyController.Focus
{
    /// <summary>
    /// Focus for decoration purpose only.
    /// </summary>
    public interface IFocusKeywordFrame : IFrameKeywordFrame, IFocusFrame, IFocusNodeFrame
    {
    }

    /// <summary>
    /// Focus for decoration purpose only.
    /// </summary>
    [ContentProperty("Text")]
    public class FocusKeywordFrame : FrameKeywordFrame, IFocusKeywordFrame
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
            ControllerTools.AssertNoOverride(this, typeof(FocusKeywordFrame));
            return new FocusVisibleCellView((IFocusNodeStateView)stateView, this);
        }
        #endregion
    }
}
