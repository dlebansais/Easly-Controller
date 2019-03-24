namespace EaslyController.Frame
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Writeable;

    /// <summary>
    /// View of a node state.
    /// </summary>
    public interface IFrameNodeStateView : IWriteableNodeStateView
    {
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        new IFrameControllerView ControllerView { get; }

        /// <summary>
        /// The node state.
        /// </summary>
        new IFrameNodeState State { get; }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        IFrameTemplate Template { get; }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        IFrameCellView RootCellView { get; }

        /// <summary>
        /// Table of cell views that are mutable lists of cells.
        /// </summary>
        IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable { get; }

        /// <summary>
        /// True if the node view contain at least one visible cell view.
        /// </summary>
        bool HasVisibleCellView { get; }

        /// <summary>
        /// The cell view that is embedding this state view. Can be null.
        /// </summary>
        IFrameContainerCellView ParentContainer { get; }

        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        void BuildRootCellView(IFrameCellViewTreeContext context);

        /// <summary>
        /// Set the container for this state view.
        /// </summary>
        void SetContainerCellView(IFrameContainerCellView parentContainer);

        /// <summary>
        /// Clears the cell view tree for this view.
        /// </summary>
        void ClearRootCellView();

        /// <summary>
        /// Update line numbers in the root cell view.
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
        /// Checks if the tree of cell views under this state is valid.
        /// </summary>
        bool IsCellViewTreeValid();
    }

    /// <summary>
    /// View of a node state.
    /// </summary>
    internal abstract class FrameNodeStateView : WriteableNodeStateView, IFrameNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The node state.</param>
        public FrameNodeStateView(IFrameControllerView controllerView, IFrameNodeState state)
            : base(controllerView, state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new IFrameControllerView ControllerView { get { return (IFrameControllerView)base.ControllerView; } }

        /// <summary>
        /// The node state.
        /// </summary>
        public new IFrameNodeState State { get { return (IFrameNodeState)base.State; } }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        public virtual IFrameTemplate Template { get { throw new NotImplementedException(); } } // Can't make this abstract, thank you C#...

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public virtual IFrameCellView RootCellView { get { throw new NotImplementedException(); } } // Can't make this abstract, thank you C#...

        /// <summary>
        /// Table of cell views that are mutable lists of cells.
        /// </summary>
        public virtual IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable { get { throw new NotImplementedException(); } } // Can't make this abstract, thank you C#...

        /// <summary>
        /// True if the node view contain at least one visible cell view.
        /// </summary>
        public virtual bool HasVisibleCellView
        {
            get
            {
                Debug.Assert(RootCellView != null);
                return RootCellView.HasVisibleCellView;
            }
        }

        /// <summary>
        /// The cell view that is embedding this state view. Can be null.
        /// </summary>
        public virtual IFrameContainerCellView ParentContainer { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        public abstract void BuildRootCellView(IFrameCellViewTreeContext context);

        /// <summary>
        /// Set the container for this state view.
        /// </summary>
        /// <param name="parentContainer">The cell view where the tree is restarted.</param>
        public abstract void SetContainerCellView(IFrameContainerCellView parentContainer);

        /// <summary>
        /// Clears the cell view tree for this view.
        /// </summary>
        public abstract void ClearRootCellView();

        /// <summary>
        /// Update line numbers in the root cell view.
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
        #endregion

        #region Debugging
        /// <summary>
        /// Checks if the tree of cell views under this state is valid.
        /// </summary>
        public abstract bool IsCellViewTreeValid();
        #endregion
    }
}
