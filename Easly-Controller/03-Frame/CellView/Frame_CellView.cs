using System.Diagnostics;

namespace EaslyController.Frame
{
    public interface IFrameCellView : IEqualComparable
    {
        IFrameNodeStateView StateView { get; }
        void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber);
        string PrintTree(int indentation);
    }

    public abstract class FrameCellView
    {
        #region Init
        public FrameCellView(IFrameNodeStateView stateView)
        {
            StateView = stateView;

            stateView.ControllerView.GlobalDebugIndex++;
            DebugIndex = stateView.ControllerView.GlobalDebugIndex;
        }

        public int DebugIndex { get; }
        public override string ToString()
        {
            return base.ToString() + $" ({DebugIndex})";
        }
        #endregion

        #region Properties
        public IFrameNodeStateView StateView { get; private set; }
        #endregion

        #region Client Interface
        public abstract void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber);
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameCellView AsCellView))
                return false;

            if (!comparer.VerifyEqual(StateView, AsCellView.StateView))
                return false;

            return true;
        }

        public abstract string PrintTree(int indentation);
        #endregion
    }
}
