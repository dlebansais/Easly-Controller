using EaslyController.Frame;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Cell view for components that can receive the focus.
    /// </summary>
    public interface IFocusFocusableCellView : IFrameFocusableCellView, IFocusVisibleCellView
    {
    }

    /// <summary>
    /// Cell view for components that can receive the focus.
    /// </summary>
    public class FocusFocusableCellView : FrameFocusableCellView, IFocusFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FocusFocusableCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        public FocusFocusableCellView(IFocusNodeStateView stateView)
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

            if (!(other is IFocusFocusableCellView AsFocusableCellView))
                return false;

            if (!base.IsEqual(comparer, AsFocusableCellView))
                return false;

            return true;
        }
        #endregion
    }
}
