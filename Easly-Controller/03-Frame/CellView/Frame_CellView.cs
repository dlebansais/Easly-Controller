namespace EaslyController.Frame
{
    using System;
    using System.Diagnostics;

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
        /// The collection of cell views containing this view. Null for the root of the cell tree.
        /// </summary>
        IFrameCellViewCollection ParentCellView { get; }

        /// <summary>
        /// True if the cell view is visible or contains at least one visible cell view.
        /// </summary>
        bool HasVisibleCellView { get; }

        /// <summary>
        /// Clears all views (cells and states) within this cell view.
        /// </summary>
        void ClearCellTree();

        /// <summary>
        /// Updates line numbers in the cell view.
        /// </summary>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="maxLineNumber">The maximum line number observed, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        /// <param name="maxColumnNumber">The maximum column number observed, updated upon return.</param>
        void UpdateLineNumbers(ref int lineNumber, ref int maxLineNumber, ref int columnNumber, ref int maxColumnNumber);

        /// <summary>
        /// Enumerate all visible cell views. Enumeration is interrupted if <paramref name="handler"/> returns true.
        /// </summary>
        /// <param name="handler">A handler to execute for each cell view.</param>
        /// <param name="cellView">The cell view for which <paramref name="handler"/> returned true. Null if none.</param>
        /// <param name="reversed">If true, search in reverse order.</param>
        /// <returns>The last value returned by <paramref name="handler"/>.</returns>
        bool EnumerateVisibleCellViews(Func<IFrameVisibleCellView, bool> handler, out IFrameVisibleCellView cellView, bool reversed);

        /// <summary>
        /// Returns a string representing this part of the cell view tree.
        /// </summary>
        /// <param name="indentation">The indentation level to use.</param>
        /// <param name="isVerbose">True to verbose information.</param>
        string PrintTree(int indentation, bool isVerbose);

        /// <summary>
        /// Checks if the tree of cell views under this state is valid.
        /// </summary>
        /// <param name="expectedCellViewTable">Cell views that are associated to a property of the node.</param>
        /// <param name="actualCellViewTable">Cell views that are found in the tree.</param>
        bool IsCellViewTreeValid(FrameAssignableCellViewReadOnlyDictionary<string> expectedCellViewTable, FrameAssignableCellViewDictionary<string> actualCellViewTable);
    }

    /// <summary>
    /// Atomic cell view of a component in a node.
    /// </summary>
    internal abstract class FrameCellView : IFrameCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        public FrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            StateView = stateView;
            ParentCellView = parentCellView;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public IFrameNodeStateView StateView { get; }

        /// <summary>
        /// The collection of cell views containing this view. Null for the root of the cell tree.
        /// </summary>
        public IFrameCellViewCollection ParentCellView { get; }

        /// <summary>
        /// True if the cell view is visible or contains at least one visible cell view.
        /// </summary>
        public abstract bool HasVisibleCellView { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Updates line numbers in the cell view.
        /// </summary>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="maxLineNumber">The maximum line number observed, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        /// <param name="maxColumnNumber">The maximum column number observed, updated upon return.</param>
        public abstract void UpdateLineNumbers(ref int lineNumber, ref int maxLineNumber, ref int columnNumber, ref int maxColumnNumber);

        /// <summary>
        /// Enumerate all visible cell views. Enumeration is interrupted if <paramref name="handler"/> returns true.
        /// </summary>
        /// <param name="handler">A handler to execute for each cell view.</param>
        /// <param name="cellView">The cell view for which <paramref name="handler"/> returned true. Null if none.</param>
        /// <param name="reversed">If true, search in reverse order.</param>
        /// <returns>The last value returned by <paramref name="handler"/>.</returns>
        public abstract bool EnumerateVisibleCellViews(Func<IFrameVisibleCellView, bool> handler, out IFrameVisibleCellView cellView, bool reversed);

        /// <summary>
        /// Clears all views (cells and states) within this cell view.
        /// </summary>
        public abstract void ClearCellTree();
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameCellView AsCellView))
                return comparer.Failed();

            if (!comparer.VerifyEqual(StateView, AsCellView.StateView))
                return comparer.Failed();

            return true;
        }

        /// <summary>
        /// Returns a string representing this part of the cell view tree.
        /// </summary>
        /// <param name="indentation">The indentation level to use.</param>
        /// <param name="isVerbose">True to verbose information.</param>
        public abstract string PrintTree(int indentation, bool isVerbose);

        /// <summary>
        /// Checks if the tree of cell views under this state is valid.
        /// </summary>
        /// <param name="expectedCellViewTable">Cell views that are associated to a property of the node.</param>
        /// <param name="actualCellViewTable">Cell views that are found in the tree.</param>
        public abstract bool IsCellViewTreeValid(FrameAssignableCellViewReadOnlyDictionary<string> expectedCellViewTable, FrameAssignableCellViewDictionary<string> actualCellViewTable);
        #endregion
    }
}
