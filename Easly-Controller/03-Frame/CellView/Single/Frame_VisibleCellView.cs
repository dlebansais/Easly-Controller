using System.Diagnostics;

namespace EaslyController.Frame
{
    public interface IFrameVisibleCellView : IFrameCellView
    {
        int LineNumber { get; }
        int ColumnNumber { get; }
    }

    public class FrameVisibleCellView : FrameCellView, IFrameVisibleCellView
    {
        #region Init
        public FrameVisibleCellView(IFrameNodeStateView stateView)
            : base(stateView)
        {
            LineNumber = 0;
            ColumnNumber = 0;
        }
        #endregion

        #region Properties
        public int LineNumber { get; private set; }
        public int ColumnNumber { get; private set; }
        #endregion

        #region Client Interface
        public override void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber)
        {
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;

            IncrementLineNumber(ref lineNumber);
            IncrementColumnNumber(ref columnNumber);
        }
        #endregion

        #region Descendant Interface
        protected virtual void IncrementLineNumber(ref int lineNumber)
        {
            lineNumber++;
        }

        protected virtual void IncrementColumnNumber(ref int columnNumber)
        {
            columnNumber++;
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

            if (!(other is IFrameVisibleCellView AsVisibleCellView))
                return false;

            if (!base.IsEqual(comparer, AsVisibleCellView))
                return false;

            if (LineNumber != AsVisibleCellView.LineNumber)
                return false;

            if (ColumnNumber != AsVisibleCellView.ColumnNumber)
                return false;

            return true;
        }
        #endregion
    }
}
