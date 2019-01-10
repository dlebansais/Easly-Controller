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
        /// Clears all views (cells and states) within this cell view.
        /// </summary>
        void ClearCellTree();

        /// <summary>
        /// Updates line numbers in the cell view.
        /// </summary>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        void UpdateLineNumbers(ref int lineNumber, ref int columnNumber);

        /// <summary>
        /// Returns a string representing this part of the cell view tree.
        /// </summary>
        /// <param name="indentation">The indentation level to use.</param>
        /// <param name="isVerbose">True to verbose information.</param>
        string PrintTree(int indentation, bool isVerbose);
    }

    /// <summary>
    /// Atomic cell view of a component in a node.
    /// </summary>
    public abstract class FrameCellView : IFrameCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FrameCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        public FrameCellView(IFrameNodeStateView stateView)
        {
            StateView = stateView;
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

        /// <summary>
        /// Clears all views (cells and states) within this cell view.
        /// </summary>
        public abstract void ClearCellTree();
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
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

        /// <summary>
        /// Returns a string representing this part of the cell view tree.
        /// </summary>
        /// <param name="indentation">The indentation level to use.</param>
        /// <param name="isVerbose">True to verbose information.</param>
        public abstract string PrintTree(int indentation, bool isVerbose);
        #endregion
    }
}
