using System.Diagnostics;

namespace EaslyController.Frame
{
    public interface IFrameMutableCellViewCollection : IFrameCellView
    {
        IFrameCellViewList CellViewList { get; }
        void Insert(int index, IFrameCellView cellView);
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

        #region Client Interface
        public virtual void Insert(int index, IFrameCellView cellView)
        {
            CellViewList.Insert(index, cellView);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameMutableCellViewCollection AsCellViewCollection))
                return false;

            if (!base.IsEqual(comparer, AsCellViewCollection))
                return false;

            if (!comparer.VerifyEqual(CellViewList, AsCellViewCollection.CellViewList))
                return false;

            return true;
        }
        #endregion
    }
}
