using System.Diagnostics;

namespace EaslyController.Frame
{
    public interface IFrameEmptyCellView : IFrameCellView
    {
    }

    public class FrameEmptyCellView : FrameCellView, IFrameEmptyCellView
    {
        #region Init
        public FrameEmptyCellView(IFrameNodeStateView stateView)
            : base(stateView)
        {
        }
        #endregion

        #region Client Interface
        public override void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber)
        {
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

            if (!(other is IFrameEmptyCellView AsEmptyCellView))
                return false;

            if (!base.IsEqual(comparer, AsEmptyCellView))
                return false;

            return true;
        }

        public override string PrintTree(int indentation)
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
