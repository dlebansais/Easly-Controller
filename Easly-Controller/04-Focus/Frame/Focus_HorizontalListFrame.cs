using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Base frame for a list of nodes displayed horizontally.
    /// </summary>
    public interface IFocusHorizontalListFrame : IFrameHorizontalListFrame, IFocusListFrame
    {
    }

    /// <summary>
    /// Base frame for a list of nodes displayed horizontally.
    /// </summary>
    public class FocusHorizontalListFrame : FrameHorizontalListFrame, IFocusHorizontalListFrame
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
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusHorizontalListFrame));
            return new FocusLine((IFocusNodeStateView)stateView, (IFocusCellViewList)list);
        }
        #endregion
    }
}
