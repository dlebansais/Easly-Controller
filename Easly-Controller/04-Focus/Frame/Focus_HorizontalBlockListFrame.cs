using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Frame for a block list displayed horizontally.
    /// </summary>
    public interface IFocusHorizontalBlockListFrame : IFrameHorizontalBlockListFrame, IFocusBlockListFrame
    {
    }

    /// <summary>
    /// Frame for a block list displayed horizontally.
    /// </summary>
    public class FocusHorizontalBlockListFrame : FrameBlockListFrame, IFocusHorizontalBlockListFrame
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
            ControllerTools.AssertNoOverride(this, typeof(FocusHorizontalBlockListFrame));
            return new FocusLine((IFocusNodeStateView)stateView, (IFocusCellViewList)list);
        }
        #endregion
    }
}
