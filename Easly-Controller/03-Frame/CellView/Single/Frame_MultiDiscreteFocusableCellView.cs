namespace EaslyController.Frame
{
    public interface IFrameMultiDiscreteFocusableCellView : IFrameContentFocusableCellView
    {
        string PropertyName { get; }
    }

    public class FrameMultiDiscreteFocusableCellView : FrameContentFocusableCellView, IFrameMultiDiscreteFocusableCellView
    {
        #region Init
        public FrameMultiDiscreteFocusableCellView(IFrameNodeStateView stateView, string propertyName)
            : base(stateView)
        {
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        public string PropertyName { get; private set; }
        #endregion
    }
}
