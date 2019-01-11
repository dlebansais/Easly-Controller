using EaslyController.Frame;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Cell view for components that can receive the focus and be modified.
    /// </summary>
    public interface IFocusContentFocusableCellView : IFrameContentFocusableCellView, IFocusFocusableCellView
    {
    }

    /// <summary>
    /// Cell view for components that can receive the focus and be modified.
    /// </summary>
    public abstract class FocusContentFocusableCellView : FrameContentFocusableCellView, IFocusContentFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FocusContentFocusableCellView"/>.
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

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFocusContentFocusableCellView AsContentFocusableCellView))
                return false;

            if (!base.IsEqual(comparer, AsContentFocusableCellView))
                return false;

            return true;
        }
        #endregion
    }
}
