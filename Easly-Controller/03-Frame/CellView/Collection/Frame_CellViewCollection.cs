using System.Diagnostics;

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

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameCellViewCollection AsCellViewCollection))
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
