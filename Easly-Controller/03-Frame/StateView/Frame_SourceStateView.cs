using BaseNode;
using EaslyController.Writeable;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// View of a source state.
    /// </summary>
    public interface IFrameSourceStateView : IWriteableSourceStateView, IFramePlaceholderNodeStateView
    {
        /// <summary>
        /// The pattern state.
        /// </summary>
        new IFrameSourceState State { get; }
    }

    /// <summary>
    /// View of a source state.
    /// </summary>
    public class FrameSourceStateView : WriteableSourceStateView, IFrameSourceStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameSourceStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The source state.</param>
        public FrameSourceStateView(IFrameControllerView controllerView, IFrameSourceState state)
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
        /// The pattern state.
        /// </summary>
        public new IFrameSourceState State { get { return (IFrameSourceState)base.State; } }
        IFrameNodeState IFrameNodeStateView.State { get { return State; } }
        IFramePlaceholderNodeState IFramePlaceholderNodeStateView.State { get { return State; } }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        public IFrameTemplate Template
        {
            get
            {
                Type InterfaceType = typeof(IIdentifier);
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
        #endregion

        #region Client Interface
        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        public virtual void BuildRootCellView(IFrameCellViewTreeContext context)
        {
            Debug.Assert(context.StateView == this);

            Debug.Assert(State.InnerTable.Count == 0);

            Debug.Assert(RootCellView == null);

            InitCellViewTable();

            IFrameNodeTemplate NodeTemplate = Template as IFrameNodeTemplate;
            Debug.Assert(NodeTemplate != null);

            SetRootCellView(NodeTemplate.BuildNodeCells(context));

            SealCellViewTable();
        }

        /// <summary></summary>
        protected virtual void InitCellViewTable()
        {
            _CellViewTable = CreateCellViewTable();
        }

        /// <summary></summary>
        protected virtual void SetRootCellView(IFrameCellView cellView)
        {
            Debug.Assert(cellView != null);
            Debug.Assert(RootCellView == null);

            RootCellView = cellView;
        }

        /// <summary></summary>
        protected virtual void SealCellViewTable()
        {
            CellViewTable = CreateCellViewReadOnlyTable(_CellViewTable);
        }

        /// <summary>
        /// Assign the cell view corresponding to an inner.
        /// </summary>
        /// <param name="propertyName">The property name of the inner.</param>
        /// <param name="cellView">The assigned cell view.</param>
        public virtual void AssignCellViewTable(string propertyName, IFrameAssignableCellView cellView)
        {
            throw new InvalidOperationException();
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
            throw new InvalidOperationException();
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
        /// Enumerate all visible cell views.
        /// </summary>
        /// <param name="list">The list of visible cell views upon return.</param>
        public void EnumerateVisibleCellViews(IFrameVisibleCellViewList list)
        {
            Debug.Assert(list != null);

            Debug.Assert(RootCellView != null);
            RootCellView.EnumerateVisibleCellViews(list);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameSourceStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameSourceStateView AsSourceStateView))
                return false;

            if (!base.IsEqual(comparer, AsSourceStateView))
                return false;

            if (Template != AsSourceStateView.Template)
                return false;

            if ((RootCellView != null && AsSourceStateView.RootCellView == null) || (RootCellView == null && AsSourceStateView.RootCellView != null))
                return false;

            if (RootCellView != null)
            {
                Debug.Assert(CellViewTable != null);
                Debug.Assert(AsSourceStateView.CellViewTable != null);

                if (!comparer.VerifyEqual(RootCellView, AsSourceStateView.RootCellView))
                    return false;

                if (!comparer.VerifyEqual(CellViewTable, AsSourceStateView.CellViewTable))
                    return false;
            }
            else
            {
                Debug.Assert(CellViewTable == null);
                Debug.Assert(AsSourceStateView.CellViewTable == null);
            }

            return true;
        }

        /// <summary>
        /// Checks if the tree of cell views under this state is valid.
        /// </summary>
        public virtual bool IsCellViewTreeValid()
        {
            if (RootCellView == null)
                return false;

            if (!(RootCellView is IFrameEmptyCellView))
            {
                IFrameAssignableCellViewDictionary<string> ActualCellViewTable = CreateCellViewTable();
                if (!RootCellView.IsCellViewTreeValid(CellViewTable, ActualCellViewTable))
                    return false;

                if (!AllCellViewsProperlyAssigned(CellViewTable, ActualCellViewTable))
                    return false;
            }

            return true;
        }

        /// <summary></summary>
        protected virtual bool AllCellViewsProperlyAssigned(IFrameAssignableCellViewReadOnlyDictionary<string> expectedCellViewTable, IFrameAssignableCellViewDictionary<string> actualCellViewTable)
        {
            int ActualCount = 0;
            foreach (KeyValuePair<string, IFrameAssignableCellView> Entry in CellViewTable)
                if (Entry.Value != null)
                {
                    ActualCount++;
                    if (!actualCellViewTable.ContainsKey(Entry.Key))
                        return false;
                }

            if (actualCellViewTable.Count != ActualCount)
                return false;

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxAssignableCellViewDictionary{string} object.
        /// </summary>
        protected virtual IFrameAssignableCellViewDictionary<string> CreateCellViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameSourceStateView));
            return new FrameAssignableCellViewDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxAssignableCellViewReadOnlyDictionary{string} object.
        /// </summary>
        protected virtual IFrameAssignableCellViewReadOnlyDictionary<string> CreateCellViewReadOnlyTable(IFrameAssignableCellViewDictionary<string> dictionary)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameSourceStateView));
            return new FrameAssignableCellViewReadOnlyDictionary<string>(dictionary);
        }
        #endregion
    }
}
