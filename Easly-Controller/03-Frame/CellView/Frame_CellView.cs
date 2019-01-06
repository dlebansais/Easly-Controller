using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Atomic cell view of a component in a node.
    /// </summary>
    public interface IFrameCellView : IEqualComparable
    {
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        IFrameNodeStateView StateView { get; }

        /// <summary>
        /// Updates line numbers in the cell view.
        /// </summary>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        void UpdateLineNumbers(ref int lineNumber, ref int columnNumber);

        string PrintTree(int indentation, bool printFull);
    }

    /// <summary>
    /// Atomic cell view of a component in a node.
    /// </summary>
    public abstract class FrameCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FrameCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
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
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public IFrameNodeStateView StateView { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Updates line numbers in the cell view.
        /// </summary>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        public abstract void UpdateLineNumbers(ref int lineNumber, ref int columnNumber);
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

        public abstract string PrintTree(int indentation, bool printFull);
        #endregion
    }
}
