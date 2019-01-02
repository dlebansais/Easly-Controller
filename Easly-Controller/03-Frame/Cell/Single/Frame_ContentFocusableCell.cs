namespace EaslyController.Frame
{
    public interface IFrameContentFocusableCellView : IFrameFocusableCellView
    {
    }

    public abstract class FrameContentFocusableCellView : FrameFocusableCellView, IFrameContentFocusableCellView
    {
        #region Init
        public FrameContentFocusableCellView(IFrameNodeStateView stateView)
            : base(stateView)
        {
        }
        #endregion
    }
}
