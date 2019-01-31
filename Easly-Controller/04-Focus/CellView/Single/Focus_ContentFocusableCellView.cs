﻿namespace EaslyController.Focus
{
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// Cell view for components that can receive the focus and be modified.
    /// </summary>
    public interface IFocusContentFocusableCellView : IFrameContentFocusableCellView, IFocusFocusableCellView
    {
    }

    /// <summary>
    /// Cell view for components that can receive the focus and be modified.
    /// </summary>
    internal abstract class FocusContentFocusableCellView : FrameContentFocusableCellView, IFocusContentFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusContentFocusableCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        /// <param name="propertyName">Property corresponding to the component of the node.</param>
        public FocusContentFocusableCellView(IFocusNodeStateView stateView, IFocusFrame frame, string propertyName)
            : base(stateView, frame, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new IFocusNodeStateView StateView { get { return (IFocusNodeStateView)base.StateView; } }

        /// <summary>
        /// The frame that created this cell view.
        /// </summary>
        public new IFocusFrame Frame { get { return (IFocusFrame)base.Frame; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Updates the focus chain with cells in the tree.
        /// </summary>
        /// <param name="focusChain">The list of focusable cell views found in the tree.</param>
        public abstract void UpdateFocusChain(IFocusFocusableCellViewList focusChain);
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

            if (!comparer.IsSameType(other, out FocusContentFocusableCellView AsContentFocusableCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsContentFocusableCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
