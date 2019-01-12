﻿using EaslyController.Frame;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// View of a child node.
    /// </summary>
    public interface IFocusPlaceholderNodeStateView : IFramePlaceholderNodeStateView, IFocusNodeStateView
    {
        /// <summary>
        /// The child node.
        /// </summary>
        new IFocusPlaceholderNodeState State { get; }
    }

    /// <summary>
    /// View of a child node.
    /// </summary>
    public class FocusPlaceholderNodeStateView : FramePlaceholderNodeStateView, IFocusPlaceholderNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusPlaceholderNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The child node state.</param>
        public FocusPlaceholderNodeStateView(IFocusControllerView controllerView, IFocusPlaceholderNodeState state)
            : base(controllerView, state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new IFocusControllerView ControllerView { get { return (IFocusControllerView)base.ControllerView; } }

        /// <summary>
        /// The child node.
        /// </summary>
        public new IFocusPlaceholderNodeState State { get { return (IFocusPlaceholderNodeState)base.State; } }
        IFocusNodeState IFocusNodeStateView.State { get { return State; } }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        public new IFocusTemplate Template { get { return (IFocusTemplate)base.Template; } }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public new IFocusCellView RootCellView { get { return (IFocusCellView)base.RootCellView; } }

        /// <summary>
        /// Table of cell views that are mutable lists of cells.
        /// </summary>
        public new IFocusCellViewReadOnlyDictionary<string> CellViewTable { get { return (IFocusCellViewReadOnlyDictionary<string>)base.CellViewTable; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusPlaceholderNodeStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFocusPlaceholderNodeStateView AsPlaceholderNodeStateView))
                return false;

            if (!base.IsEqual(comparer, AsPlaceholderNodeStateView))
                return false;

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewDictionary{string} object.
        /// </summary>
        protected override IFrameCellViewDictionary<string> CreateCellViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPlaceholderNodeStateView));
            return new FocusCellViewDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxCellViewReadOnlyDictionary{string} object.
        /// </summary>
        protected override IFrameCellViewReadOnlyDictionary<string> CreateCellViewReadOnlyTable(IFrameCellViewDictionary<string> dictionary)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPlaceholderNodeStateView));
            return new FocusCellViewReadOnlyDictionary<string>((IFocusCellViewDictionary<string>)dictionary);
        }
        #endregion
    }
}