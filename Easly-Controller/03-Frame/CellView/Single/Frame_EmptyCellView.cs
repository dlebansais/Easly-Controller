using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Cell view with no content and that is not displayed.
    /// </summary>
    public interface IFrameEmptyCellView : IFrameCellView
    {
    }

    /// <summary>
    /// Cell view with no content and that is not displayed.
    /// </summary>
    public class FrameEmptyCellView : FrameCellView, IFrameEmptyCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FrameEmptyCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        public FrameEmptyCellView(IFrameNodeStateView stateView)
            : base(stateView)
        {
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Clears all views (cells and states) within this cell view.
        /// </summary>
        public override void ClearCellTree()
        {
        }

        /// <summary>
        /// Update line numbers in the cell view.
        /// </summary>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        public override void UpdateLineNumbers(ref int lineNumber, ref int columnNumber)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameEmptyCellView AsEmptyCellView))
                return false;

            if (!base.IsEqual(comparer, AsEmptyCellView))
                return false;

            return true;
        }

        /// <summary>
        /// Returns a string representing this part of the cell view tree.
        /// </summary>
        /// <param name="indentation">The indentation level to use.</param>
        /// <param name="isVerbose">True to verbose information.</param>
        public override string PrintTree(int indentation, bool isVerbose)
        {
            string Result = "";
            for (int i = 0; i < indentation; i++)
                Result += " ";

            Result += "Empty\n";

            return Result;
        }
        #endregion
    }
}
