using EaslyController.Frame;
using System.Windows.Markup;

namespace EaslyController.Focus
{
    /// <summary>
    /// Focus for displaying more frames horizontally.
    /// </summary>
    public interface IFocusHorizontalPanelFrame : IFrameHorizontalPanelFrame, IFocusPanelFrame
    {
    }

    /// <summary>
    /// Focus for displaying more frames horizontally.
    /// </summary>
    [ContentProperty("Items")]
    public class FocusHorizontalPanelFrame : FrameHorizontalPanelFrame, IFocusHorizontalPanelFrame
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
        /// List of frames within this frame.
        /// </summary>
        public new IFocusFrameList Items { get { return (IFocusFrameList)base.Items; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusHorizontalPanelFrame));
            return new FocusLine((IFocusNodeStateView)stateView, (IFocusCellViewList)list);
        }
        #endregion
    }
}
