namespace EaslyController.Frame
{
    public interface IFrameCellViewCollection : IFrameCellView
    {
        IFrameCellViewReadOnlyList CellViewList { get; }
    }

    public abstract class FrameCellViewCollection : FrameCellView, IFrameCellViewCollection
    {
        #region Init
        public FrameCellViewCollection(IFrameNodeStateView stateView, IFrameCellViewReadOnlyList cellViewList)
            : base(stateView)
        {
            CellViewList = cellViewList;
        }
        #endregion

        #region Properties
        public IFrameCellViewReadOnlyList CellViewList { get; }
        #endregion
    }
}
