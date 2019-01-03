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
        /// <param name="state">The pattern state.</param>
        /// <param name="templateSet">The template set used to display the state.</param>
        public FramePatternStateView(IFramePatternState state, IFrameTemplateSet templateSet)
            : base(state)
        {
            Debug.Assert(templateSet != null);
            Debug.Assert(state.ParentInner != null);

            Type NodeType = typeof(IPattern);
            Template = templateSet.NodeTypeToTemplate(NodeType);
        }
        #endregion

        #region Properties
        /// <summary>
        /// The pattern state.
        /// </summary>
        public new IFramePatternState State { get { return (IFramePatternState)base.State; } }
        IFrameNodeState IFrameNodeStateView.State { get { return State; } }
        IFramePlaceholderNodeState IFramePlaceholderNodeStateView.State { get { return State; } }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        public IFrameTemplate Template { get; }

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
        /// <param name="controllerView">The view in which the state is initialized.</param>
        public virtual void BuildRootCellView(IFrameControllerView controllerView)
        {
            Debug.Assert(controllerView != null);

            _CellViewTable = CreateCellViewTable();
            foreach (KeyValuePair<string, IFrameInner<IFrameBrowsingChildIndex>> Entry in State.InnerTable)
                _CellViewTable.Add(Entry.Value.PropertyName, null);

            IFrameNodeTemplate NodeTemplate = Template as IFrameNodeTemplate;
            Debug.Assert(NodeTemplate != null);

            RootCellView = NodeTemplate.BuildNodeCells(controllerView, this);

            foreach (KeyValuePair<string, IFrameCellView> Entry in _CellViewTable)
                Debug.Assert(Entry.Value != null);

            CellViewTable = CreateCellViewReadOnlyTable(_CellViewTable);
        }

        /// <summary>
        /// Assign the cell view corresponding to an inner.
        /// </summary>
        /// <param name="propertyName">The property name of the inner.</param>
        /// <param name="cellView">The assigned cell view.</param>
        public virtual void AssignCellViewTable(string propertyName, IFrameCellView cellView)
        {
            Debug.Assert(_CellViewTable.ContainsKey(propertyName));
            Debug.Assert(_CellViewTable[propertyName] == null);

            _CellViewTable[propertyName] = cellView;
        }

        public virtual void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber)
        {
            IFrameCellView RootCellView = null;
            RootCellView.RecalculateLineNumbers(controller, ref lineNumber, ref columnNumber);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFramePatternStateView"/> objects.
        /// </summary>
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

            if (!comparer.VerifyEqual(RootCellView, AsPatternStateView.RootCellView))
                return false;

            if (!comparer.VerifyEqual(CellViewTable, AsPatternStateView.CellViewTable))
                return false;

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
