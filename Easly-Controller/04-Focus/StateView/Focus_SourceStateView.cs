using BaseNode;
using EaslyController.Frame;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// View of a source state.
    /// </summary>
    public interface IFocusSourceStateView : IFrameSourceStateView, IFocusPlaceholderNodeStateView
    {
        /// <summary>
        /// The pattern state.
        /// </summary>
        new IFocusSourceState State { get; }
    }

    /// <summary>
    /// View of a source state.
    /// </summary>
    public class FocusSourceStateView : FrameSourceStateView, IFocusSourceStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusSourceStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The source state.</param>
        public FocusSourceStateView(IFocusControllerView controllerView, IFocusSourceState state)
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
        /// The pattern state.
        /// </summary>
        public new IFocusSourceState State { get { return (IFocusSourceState)base.State; } }
        IFocusNodeState IFocusNodeStateView.State { get { return State; } }
        IFocusPlaceholderNodeState IFocusPlaceholderNodeStateView.State { get { return State; } }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        public new IFocusTemplate Template { get { return (IFocusTemplate)base.Template; } }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public new IFocusCellView RootCellView { get { return (IFocusCellView )base.RootCellView; } }

        /// <summary>
        /// Table of cell views that are mutable lists of cells.
        /// </summary>
        public new IFocusCellViewReadOnlyDictionary<string> CellViewTable { get { return (IFocusCellViewReadOnlyDictionary<string>)base.CellViewTable; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusSourceStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFocusSourceStateView AsSourceStateView))
                return false;

            if (!base.IsEqual(comparer, AsSourceStateView))
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
            ControllerTools.AssertNoOverride(this, typeof(FocusSourceStateView));
            return new FocusCellViewDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxCellViewReadOnlyDictionary{string} object.
        /// </summary>
        protected override IFrameCellViewReadOnlyDictionary<string> CreateCellViewReadOnlyTable(IFrameCellViewDictionary<string> dictionary)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSourceStateView));
            return new FocusCellViewReadOnlyDictionary<string>((IFocusCellViewDictionary<string>)dictionary);
        }
        #endregion
    }
}
