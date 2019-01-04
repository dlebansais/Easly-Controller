using System.Diagnostics;

namespace EaslyController.Frame
{
    public interface IFrameMutableCellViewCollection : IFrameCellView
    {
        IFrameCellViewList CellViewList { get; }
        void Insert(int index, IFrameCellView cellView);
        void Remove(int index);
        void Replace(int index, IFrameCellView cellView);
        void Replace(IFrameCellView oldCellView, IFrameCellView newCellView);
        void Move(IFrameCellView cellView, int direction);
        void Move(int index, int direction);
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
            Debug.Assert(index >= 0 && index <= CellViewList.Count);
            Debug.Assert(cellView != null);

            CellViewList.Insert(index, cellView);

            foreach (IFrameCellView Item in CellViewList)
                if (Item is IFrameContainerCellView AsContainerCellView)
                    Debug.Assert(AsContainerCellView.ParentCellView == this);
        }

        public virtual void Remove(int index)
        {
            Debug.Assert(index >= 0 && index <= CellViewList.Count);

            CellViewList.RemoveAt(index);
        }

        public virtual void Replace(IFrameCellView oldCellView, IFrameCellView newCellView)
        {
            Debug.Assert(CellViewList.Contains(oldCellView));
            Debug.Assert(!CellViewList.Contains(newCellView));

            int Index = CellViewList.IndexOf(oldCellView);
            Replace(Index, newCellView);
        }

        public virtual void Replace(int index, IFrameCellView cellView)
        {
            Debug.Assert(index >= 0 && index < CellViewList.Count);
            Debug.Assert(cellView != null);

            CellViewList[index] = cellView;

            foreach (IFrameCellView Item in CellViewList)
                if (Item is IFrameContainerCellView AsContainerCellView)
                    Debug.Assert(AsContainerCellView.ParentCellView == this);
        }

        public virtual void Move(IFrameCellView cellView, int direction)
        {
            Debug.Assert(CellViewList.Contains(cellView));

            int Index = CellViewList.IndexOf(cellView);
            Move(Index, direction);
        }

        public virtual void Move(int index, int direction)
        {
            Debug.Assert(index >= 0 && index < CellViewList.Count);
            Debug.Assert(index + direction >= 0 && index + direction < CellViewList.Count);

            IFrameCellView CellView = CellViewList[index];
            CellViewList.RemoveAt(index);
            CellViewList.Insert(index + direction, CellView);
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
