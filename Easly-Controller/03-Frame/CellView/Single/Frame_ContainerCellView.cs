using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// A leaf of the cell view tree for a child state.
    /// </summary>
    public interface IFrameContainerCellView : IFrameCellView
    {
        /// <summary>
        /// The collection of cell views containing this view.
        /// </summary>
        IFrameCellViewCollection ParentCellView { get; }

        /// <summary>
        /// The state view of the state associated to this cell.
        /// </summary>
        IFrameNodeStateView ChildStateView { get; }
    }

    /// <summary>
    /// A leaf of the cell view tree for a child state.
    /// </summary>
    public class FrameContainerCellView : FrameCellView, IFrameContainerCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FrameContainerCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view.</param>
        /// <param name="childStateView">The state view of the state associated to this cell.</param>
        public FrameContainerCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
            : base(stateView)
        {
            ParentCellView = parentCellView;
            ChildStateView = childStateView;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The collection of cell views containing this view.
        /// </summary>
        public IFrameCellViewCollection ParentCellView { get; }

        /// <summary>
        /// The state view of the state associated to this cell.
        /// </summary>
        public IFrameNodeStateView ChildStateView { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Clears all views (cells and states) within this cell view.
        /// </summary>
        public override void ClearCellTree()
        {
            ChildStateView.ClearRootCellView();
        }

        /// <summary>
        /// Update line numbers in the cell view.
        /// </summary>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        /// <param name="maxColumnNumber">The maximum column number observed, updated upon return.</param>
        public override void UpdateLineNumbers(ref int lineNumber, ref int columnNumber, ref int maxColumnNumber)
        {
            RecalculateChildLineNumbers(ChildStateView, ref lineNumber, ref columnNumber);
        }
        #endregion

        #region Descendant Interface
        /// <summary>
        /// Update line numbers in the cell view from the update in the child state view.
        /// </summary>
        /// <param name="nodeStateView">The child state view.</param>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        protected virtual void RecalculateChildLineNumbers(IFrameNodeStateView nodeStateView, ref int lineNumber, ref int columnNumber)
        {
            nodeStateView.UpdateLineNumbers(ref lineNumber, ref columnNumber, ref columnNumber);
        }

        /// <summary>
        /// Enumerate all visible cell views.
        /// </summary>
        /// <param name="list">The list of visible cell views upon return.</param>
        public override void EnumerateVisibleCellViews(IFrameVisibleCellViewList list)
        {
            Debug.Assert(list != null);

            ChildStateView.EnumerateVisibleCellViews(list);
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

            if (!(other is IFrameContainerCellView AsContainerCellView))
                return false;

            if (!base.IsEqual(comparer, AsContainerCellView))
                return false;

            if (!comparer.VerifyEqual(ParentCellView, AsContainerCellView.ParentCellView))
                return false;

            if (!comparer.VerifyEqual(ChildStateView, AsContainerCellView.ChildStateView))
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

            if (ChildStateView.RootCellView != null)
            {
                Result += $"Container, state: {ChildStateView}\n";

                if (isVerbose)
                    Result += ChildStateView.RootCellView.PrintTree(indentation + 1, isVerbose);
            }
            else
            {
                Result += $"Container, state: {ChildStateView} (no root)\n";
            }

            return Result;
        }
        #endregion
    }
}
