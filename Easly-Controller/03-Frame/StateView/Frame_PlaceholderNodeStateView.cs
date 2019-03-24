namespace EaslyController.Frame
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNodeHelper;
    using EaslyController.Writeable;

    /// <summary>
    /// View of a child node.
    /// </summary>
    public interface IFramePlaceholderNodeStateView : IWriteablePlaceholderNodeStateView, IFrameNodeStateView, IFrameReplaceableStateView
    {
        /// <summary>
        /// The child node.
        /// </summary>
        new IFramePlaceholderNodeState State { get; }
    }

    /// <summary>
    /// View of a child node.
    /// </summary>
    internal class FramePlaceholderNodeStateView : WriteablePlaceholderNodeStateView, IFramePlaceholderNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FramePlaceholderNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The child node state.</param>
        public FramePlaceholderNodeStateView(IFrameControllerView controllerView, IFramePlaceholderNodeState state)
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
        /// The child node.
        /// </summary>
        public new IFramePlaceholderNodeState State { get { return (IFramePlaceholderNodeState)base.State; } }
        IFrameNodeState IFrameNodeStateView.State { get { return State; } }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        public IFrameTemplate Template
        {
            get
            {
                Type NodeType = State.Node.GetType();
                Debug.Assert(!NodeType.IsInterface && !NodeType.IsAbstract);

                Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(NodeType);
                return ControllerView.TemplateSet.NodeTypeToTemplate(InterfaceType);
            }
        }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public IFrameCellView RootCellView { get; private set; }

        /// <summary>
        /// Table of cell views that are mutable lists of cells.
        /// </summary>
        public IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable { get; private set; }
        private IFrameAssignableCellViewDictionary<string> _CellViewTable;

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
        public IFrameContainerCellView ParentContainer { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        public virtual void BuildRootCellView(IFrameCellViewTreeContext context)
        {
            Debug.Assert(context.StateView == this);
            Debug.Assert(RootCellView == null);

            InitCellViewTable();

            IFrameNodeTemplate NodeTemplate = Template as IFrameNodeTemplate;
            Debug.Assert(NodeTemplate != null);

            SetRootCellView(NodeTemplate.BuildNodeCells(context));

            SealCellViewTable();
        }

        /// <summary></summary>
        private protected virtual void InitCellViewTable()
        {
            _CellViewTable = CreateCellViewTable();
            foreach (KeyValuePair<string, IFrameInner> Entry in State.InnerTable)
                _CellViewTable.Add(Entry.Value.PropertyName, null);
        }

        /// <summary></summary>
        private protected virtual void SetRootCellView(IFrameCellView cellView)
        {
            Debug.Assert(cellView != null);
            Debug.Assert(RootCellView == null);

            RootCellView = cellView;
        }

        /// <summary></summary>
        private protected virtual void SealCellViewTable()
        {
            CellViewTable = _CellViewTable.ToReadOnly();
        }

        /// <summary>
        /// Assign the cell view corresponding to an inner.
        /// </summary>
        /// <param name="propertyName">The property name of the inner.</param>
        /// <param name="cellView">The assigned cell view.</param>
        public virtual void AssignCellViewTable(string propertyName, IFrameAssignableCellView cellView)
        {
            Debug.Assert(_CellViewTable.ContainsKey(propertyName));
            Debug.Assert(_CellViewTable[propertyName] == null);
            Debug.Assert(cellView != null);

            _CellViewTable[propertyName] = cellView;
        }

        /// <summary>
        /// Set the container for this state view.
        /// </summary>
        /// <param name="parentContainer">The cell view where the tree is restarted.</param>
        public virtual void SetContainerCellView(IFrameContainerCellView parentContainer)
        {
            ParentContainer = parentContainer;
        }

        /// <summary>
        /// Clears the cell view tree for this view.
        /// </summary>
        public virtual void ClearRootCellView()
        {
            if (RootCellView != null)
                RootCellView.ClearCellTree();

            RootCellView = null;
            _CellViewTable = null;
            CellViewTable = null;
        }

        /// <summary>
        /// Replaces the cell view for the given property.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="cellView">The new cell view.</param>
        public virtual void ReplaceCellView(string propertyName, IFrameContainerCellView cellView)
        {
            Debug.Assert(_CellViewTable.ContainsKey(propertyName));
            Debug.Assert(_CellViewTable[propertyName] != null);
            Debug.Assert(cellView != null);

            _CellViewTable[propertyName] = cellView;
        }

        /// <summary>
        /// Update line numbers in the root cell view.
        /// </summary>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="maxLineNumber">The maximum line number observed, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        /// <param name="maxColumnNumber">The maximum column number observed, updated upon return.</param>
        public virtual void UpdateLineNumbers(ref int lineNumber, ref int maxLineNumber, ref int columnNumber, ref int maxColumnNumber)
        {
            Debug.Assert(RootCellView != null);

            RootCellView.UpdateLineNumbers(ref lineNumber, ref maxLineNumber, ref columnNumber, ref maxColumnNumber);
        }

        /// <summary>
        /// Enumerate all visible cell views. Enumeration is interrupted if <paramref name="handler"/> returns true.
        /// </summary>
        /// <param name="handler">A handler to execute for each cell view.</param>
        /// <param name="cellView">The cell view for which <paramref name="handler"/> returned true. Null if none.</param>
        /// <param name="reversed">If true, search in reverse order.</param>
        /// <returns>The last value returned by <paramref name="handler"/>.</returns>
        public virtual bool EnumerateVisibleCellViews(Func<IFrameVisibleCellView, bool> handler, out IFrameVisibleCellView cellView, bool reversed)
        {
            Debug.Assert(handler != null);

            Debug.Assert(RootCellView != null);
            return RootCellView.EnumerateVisibleCellViews(handler, out cellView, reversed);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFramePlaceholderNodeStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FramePlaceholderNodeStateView AsPlaceholderNodeStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsPlaceholderNodeStateView))
                return comparer.Failed();

            if (!comparer.IsSameReference(Template, AsPlaceholderNodeStateView.Template))
                return comparer.Failed();

            if (!comparer.IsTrue((RootCellView == null || AsPlaceholderNodeStateView.RootCellView != null) && (RootCellView != null || AsPlaceholderNodeStateView.RootCellView == null)))
                return comparer.Failed();

            if (RootCellView != null)
            {
                Debug.Assert(CellViewTable != null);
                Debug.Assert(AsPlaceholderNodeStateView.CellViewTable != null);

                if (!comparer.VerifyEqual(RootCellView, AsPlaceholderNodeStateView.RootCellView))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(CellViewTable, AsPlaceholderNodeStateView.CellViewTable))
                    return comparer.Failed();
            }

            return true;
        }

        /// <summary>
        /// Checks if the tree of cell views under this state is valid.
        /// </summary>
        public virtual bool IsCellViewTreeValid()
        {
            bool IsValid = true;

            IsValid &= RootCellView != null;

            if (IsValid && !(RootCellView is IFrameEmptyCellView))
            {
                IFrameAssignableCellViewDictionary<string> ActualCellViewTable = CreateCellViewTable();
                IsValid &= RootCellView.IsCellViewTreeValid(CellViewTable, ActualCellViewTable);
                IsValid &= AllCellViewsProperlyAssigned(CellViewTable, ActualCellViewTable);

                DebugObjects.AddReference(ActualCellViewTable);
            }

            return IsValid;
        }

        /// <summary></summary>
        private protected virtual bool AllCellViewsProperlyAssigned(IFrameAssignableCellViewReadOnlyDictionary<string> expectedCellViewTable, IFrameAssignableCellViewDictionary<string> actualCellViewTable)
        {
            bool IsAssigned = true;

            int ActualCount = 0;
            foreach (KeyValuePair<string, IFrameAssignableCellView> Entry in CellViewTable)
                if (Entry.Value != null)
                {
                    ActualCount++;
                    IsAssigned &= actualCellViewTable.ContainsKey(Entry.Key);
                }

            IsAssigned &= actualCellViewTable.Count == ActualCount;

            return IsAssigned;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxAssignableCellViewDictionary{string} object.
        /// </summary>
        private protected virtual IFrameAssignableCellViewDictionary<string> CreateCellViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePlaceholderNodeStateView));
            return new FrameAssignableCellViewDictionary<string>();
        }
        #endregion
    }
}
