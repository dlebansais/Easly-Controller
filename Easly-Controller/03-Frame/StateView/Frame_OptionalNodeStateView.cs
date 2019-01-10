using BaseNode;
using BaseNodeHelper;
using Easly;
using EaslyController.Writeable;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// View of an optional node state.
    /// </summary>
    public interface IFrameOptionalNodeStateView : IWriteableOptionalNodeStateView, IFrameNodeStateView
    {
        /// <summary>
        /// The optional node state.
        /// </summary>
        new IFrameOptionalNodeState State { get; }
    }

    /// <summary>
    /// View of an optional node state.
    /// </summary>
    public class FrameOptionalNodeStateView : WriteableOptionalNodeStateView, IFrameOptionalNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameOptionalNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The optional node state.</param>
        public FrameOptionalNodeStateView(IFrameControllerView controllerView, IFrameOptionalNodeState state)
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
        /// The optional node state.
        /// </summary>
        public new IFrameOptionalNodeState State { get { return (IFrameOptionalNodeState)base.State; } }
        IFrameNodeState IFrameNodeStateView.State { get { return State; } }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        public IFrameTemplate Template
        {
            get
            {
                IOptionalReference Optional = State.Optional;
                Debug.Assert(Optional != null);

                NodeTreeHelperOptional.GetChildNode(Optional, out bool IsAssigned, out INode ChildNode);
                if (ChildNode != null)
                {
                    Type NodeType = ChildNode.GetType();
                    Debug.Assert(!NodeType.IsInterface && !NodeType.IsAbstract);

                    Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(NodeType);
                    return ControllerView.TemplateSet.NodeTypeToTemplate(InterfaceType);
                }
                else
                    return null;
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
            IOptionalReference Optional = State.Optional;
            Debug.Assert(Optional != null);

            _CellViewTable = CreateCellViewTable();

            Debug.Assert(RootCellView == null);

            if (Optional.IsAssigned)
            {
                foreach (KeyValuePair<string, IFrameInner<IFrameBrowsingChildIndex>> Entry in State.InnerTable)
                    _CellViewTable.Add(Entry.Value.PropertyName, null);

                IFrameNodeTemplate NodeTemplate = Template as IFrameNodeTemplate;
                Debug.Assert(NodeTemplate != null);

                RootCellView = NodeTemplate.BuildNodeCells(ControllerView, this);

                foreach (KeyValuePair<string, IFrameCellView> Entry in _CellViewTable)
                    Debug.Assert(Entry.Value != null);
            }
            else
                RootCellView = CreateEmptyCellView(this);

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
            Debug.Assert(cellView != null);

            _CellViewTable[propertyName] = cellView;
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
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        /// <param name="maxColumnNumber">The maximum column number observed, updated upon return.</param>
        public virtual void UpdateLineNumbers(ref int lineNumber, ref int columnNumber, ref int maxColumnNumber)
        {
            Debug.Assert(RootCellView != null);

            RootCellView.UpdateLineNumbers(ref lineNumber, ref columnNumber, ref maxColumnNumber);
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
        /// Compares two <see cref="IFrameOptionalNodeStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameOptionalNodeStateView AsOptionalNodeStateView))
                return false;

            if (!base.IsEqual(comparer, AsOptionalNodeStateView))
                return false;

            if (Template != AsOptionalNodeStateView.Template)
                return false;

            if ((RootCellView != null && AsOptionalNodeStateView.RootCellView == null) || (RootCellView == null && AsOptionalNodeStateView.RootCellView != null))
                return false;

            if (RootCellView != null)
            {
                Debug.Assert(CellViewTable != null);
                Debug.Assert(AsOptionalNodeStateView.CellViewTable != null);

                if (!comparer.VerifyEqual(RootCellView, AsOptionalNodeStateView.RootCellView))
                    return false;

                if (!comparer.VerifyEqual(CellViewTable, AsOptionalNodeStateView.CellViewTable))
                    return false;
            }
            else
            {
                Debug.Assert(CellViewTable == null);
                Debug.Assert(AsOptionalNodeStateView.CellViewTable == null);
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
            ControllerTools.AssertNoOverride(this, typeof(FrameOptionalNodeStateView));
            return new FrameCellViewDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxCellViewReadOnlyDictionary{string} object.
        /// </summary>
        protected virtual IFrameCellViewReadOnlyDictionary<string> CreateCellViewReadOnlyTable(IFrameCellViewDictionary<string> dictionary)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameOptionalNodeStateView));
            return new FrameCellViewReadOnlyDictionary<string>(dictionary);
        }

        /// <summary>
        /// Creates a IxxxCellViewDictionary{string} object.
        /// </summary>
        protected virtual IFrameEmptyCellView CreateEmptyCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameOptionalNodeStateView));
            return new FrameEmptyCellView(stateView);
        }
        #endregion
    }
}
