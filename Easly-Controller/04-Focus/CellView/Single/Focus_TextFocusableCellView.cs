using EaslyController.Frame;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Cell view for text components that can receive the focus and be modified (identifiers).
    /// </summary>
    public interface IFocusTextFocusableCellView : IFrameTextFocusableCellView, IFocusContentFocusableCellView
    {
    }

    /// <summary>
    /// Cell view for text components that can receive the focus and be modified (identifiers).
    /// </summary>
    public class FocusTextFocusableCellView : FrameTextFocusableCellView, IFocusTextFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FocusFocusableCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        public FocusTextFocusableCellView(IFocusNodeStateView stateView)
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

            if (!(other is IFocusTextFocusableCellView AsTextFocusableCellView))
                return false;

            if (!base.IsEqual(comparer, AsTextFocusableCellView))
                return false;

            return true;
        }
        #endregion
    }
}
