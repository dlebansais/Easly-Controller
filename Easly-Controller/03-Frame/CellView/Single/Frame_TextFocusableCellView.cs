namespace EaslyController.Frame
{
    public interface IFrameTextFocusableCellView : IFrameContentFocusableCellView
    {
    }

    public class FrameTextFocusableCellView : FrameContentFocusableCellView, IFrameTextFocusableCellView
    {
        #region Init
        public FrameTextFocusableCellView(IFrameNodeStateView stateView)
            : base(stateView)
        {
        }
        #endregion
    }
}
