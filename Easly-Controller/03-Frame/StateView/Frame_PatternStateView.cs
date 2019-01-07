using BaseNode;
using EaslyController.Writeable;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// View of a pattern state.
    /// </summary>
    public interface IFramePatternStateView : IWriteablePatternStateView, IFramePlaceholderNodeStateView
    {
        /// <summary>
        /// The pattern state.
        /// </summary>
        new IFramePatternState State { get; }
    }

    /// <summary>
    /// View of a pattern state.
    /// </summary>
    public class FramePatternStateView : WriteablePatternStateView, IFramePatternStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FramePatternStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The pattern state.</param>
        public FramePatternStateView(IFrameControllerView controllerView, IFramePatternState state)
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
        public new IFramePatternState State { get { return (IFramePatternState)base.State; } }
        IFrameNodeState IFrameNodeStateView.State { get { return State; } }
        IFramePlaceholderNodeState IFramePlaceholderNodeStateView.State { get { return State; } }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        public IFrameTemplate Template
        {
            get
            {
                Type InterfaceType = typeof(IPattern);
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
        public IFrameCellViewReadOnlyDictionary<string> CellViewTable { get; private set; }
        private IFrameCellViewDictionary<string> _CellViewTable;
        #endregion

        #region Client Interface
        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        public virtual void BuildRootCellView()
        {
            Debug.Assert(State.InnerTable.Count == 0);

            Debug.Assert(RootCellView == null);

            _CellViewTable = CreateCellViewTable();

            IFrameNodeTemplate NodeTemplate = Template as IFrameNodeTemplate;
            Debug.Assert(NodeTemplate != null);

            RootCellView = NodeTemplate.BuildNodeCells(ControllerView, this);

            CellViewTable = CreateCellViewReadOnlyTable(_CellViewTable);
        }

        /// <summary>
        /// Assign the cell view corresponding to an inner.
        /// </summary>
        /// <param name="propertyName">The property name of the inner.</param>
        /// <param name="cellView">The assigned cell view.</param>
        public virtual void AssignCellViewTable(string propertyName, IFrameCellView cellView)
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
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        public virtual void UpdateLineNumbers(ref int lineNumber, ref int columnNumber)
        {
            Debug.Assert(RootCellView != null);

            RootCellView.UpdateLineNumbers(ref lineNumber, ref columnNumber);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFramePatternStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFramePatternStateView AsPatternStateView))
                return false;

            if (!base.IsEqual(comparer, AsPatternStateView))
                return false;

            if (Template != AsPatternStateView.Template)
                return false;

            if ((RootCellView != null && AsPatternStateView.RootCellView == null) || (RootCellView == null && AsPatternStateView.RootCellView != null))
                return false;

            if (RootCellView != null)
            {
                Debug.Assert(CellViewTable != null);
                Debug.Assert(AsPatternStateView.CellViewTable != null);

                if (!comparer.VerifyEqual(RootCellView, AsPatternStateView.RootCellView))
                    return false;

                if (!comparer.VerifyEqual(CellViewTable, AsPatternStateView.CellViewTable))
                    return false;
            }
            else
            {
                Debug.Assert(CellViewTable == null);
                Debug.Assert(AsPatternStateView.CellViewTable == null);
            }

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewDictionary{string} object.
        /// </summary>
        protected virtual IFrameCellViewDictionary<string> CreateCellViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePatternStateView));
            return new FrameCellViewDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxCellViewReadOnlyDictionary{string} object.
        /// </summary>
        protected virtual IFrameCellViewReadOnlyDictionary<string> CreateCellViewReadOnlyTable(IFrameCellViewDictionary<string> dictionary)
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePatternStateView));
            return new FrameCellViewReadOnlyDictionary<string>(dictionary);
        }
        #endregion
    }
}
