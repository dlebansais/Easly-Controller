namespace EaslyController.Frame
{
    public interface IFrameFocusableCellView : IFrameVisibleCellView
    {
    }

    public abstract class FrameFocusableCellView : FrameVisibleCellView, IFrameFocusableCellView
    {
        #region Init
        public FrameFocusableCellView(IFrameNodeStateView stateView)
            : base(stateView)
        {
        }
        #endregion
    }
}
