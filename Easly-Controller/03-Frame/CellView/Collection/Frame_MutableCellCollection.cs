namespace EaslyController.Frame
{
    public interface IFrameMutableCellViewCollection : IFrameCellView
    {
        IFrameCellViewList CellViewList { get; }
    }

    public abstract class FrameMutableCellViewCollection : FrameCellView, IFrameMutableCellViewCollection
    {
        #region Init
        public FrameMutableCellViewCollection(IFrameNodeStateView stateView, IFrameCellViewList cellViewList)
            : base(stateView)
        {
            CellViewList = cellViewList;
        }
        #endregion

        #region Properties
        public IFrameCellViewList CellViewList { get; }
        #endregion
    }
}
