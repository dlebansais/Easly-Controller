﻿using BaseNode;
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

            Debug.Assert(State.InnerTable.Count == 0);

            _CellViewTable = CreateCellViewTable();

            IFrameNodeTemplate NodeTemplate = Template as IFrameNodeTemplate;
            Debug.Assert(NodeTemplate != null);

            RootCellView = NodeTemplate.BuildNodeCells(controllerView, this);

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
        /// Replaces the cell view for the given property.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="cellView">The new cell view.</param>
        public virtual void ReplaceCellView(string propertyName, IFrameContainerCellView cellView)
        {
            throw new InvalidOperationException();
        }

        public virtual void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber)
        {
            IFrameCellView RootCellView = null;
            RootCellView.RecalculateLineNumbers(controller, ref lineNumber, ref columnNumber);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameSourceStateView"/> objects.
        /// </summary>
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

            if (!comparer.VerifyEqual(RootCellView, AsSourceStateView.RootCellView))
                return false;

            if (!comparer.VerifyEqual(CellViewTable, AsSourceStateView.CellViewTable))
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
            ControllerTools.AssertNoOverride(this, typeof(FrameSourceStateView));
            return new FrameCellViewDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxCellViewReadOnlyDictionary{string} object.
        /// </summary>
        protected virtual IFrameCellViewReadOnlyDictionary<string> CreateCellViewReadOnlyTable(IFrameCellViewDictionary<string> dictionary)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameSourceStateView));
            return new FrameCellViewReadOnlyDictionary<string>(dictionary);
        }
        #endregion
    }
}
