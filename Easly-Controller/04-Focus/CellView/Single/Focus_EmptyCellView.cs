﻿using EaslyController.Frame;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Cell view with no content and that is not displayed.
    /// </summary>
    public interface IFocusEmptyCellView : IFrameEmptyCellView, IFocusCellView
    {
    }

    /// <summary>
    /// Cell view with no content and that is not displayed.
    /// </summary>
    public class FocusEmptyCellView : FrameEmptyCellView, IFocusEmptyCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FocusEmptyCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        public FocusEmptyCellView(IFocusNodeStateView stateView)
            : base(stateView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new IFocusNodeStateView StateView { get { return (IFocusNodeStateView)base.StateView; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFocusEmptyCellView AsEmptyCellView))
                return false;

            if (!base.IsEqual(comparer, AsEmptyCellView))
                return false;

            return true;
        }
        #endregion
    }
}