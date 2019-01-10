using EaslyController.Frame;
using System.Windows.Markup;

namespace EaslyController.Focus
{
    /// <summary>
    /// Focus for bringing the focus to an insertion point.
    /// </summary>
    public interface IFocusInsertFrame : IFrameInsertFrame, IFocusStaticFrame
    {
    }

    /// <summary>
    /// Focus for bringing the focus to an insertion point.
    /// </summary>
    [ContentProperty("CollectionName")]
    public class FocusInsertFrame : FrameInsertFrame, IFocusInsertFrame
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
        /// Creates a IxxxFocusableCellView object.
        /// </summary>
        protected override IFrameVisibleCellView CreateFrameCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusInsertFrame));
            return new FocusFocusableCellView((IFocusNodeStateView)stateView);
        }
        #endregion
    }
}
