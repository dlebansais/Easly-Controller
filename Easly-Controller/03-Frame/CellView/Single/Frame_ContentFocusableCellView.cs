using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Cell view for components that can receive the focus and be modified.
    /// </summary>
    public interface IFrameContentFocusableCellView : IFrameFocusableCellView
    {
        /// <summary>
        /// Property corresponding to the component of the node.
        /// </summary>
        string PropertyName { get; }
    }

    /// <summary>
    /// Cell view for components that can receive the focus and be modified.
    /// </summary>
    public abstract class FrameContentFocusableCellView : FrameFocusableCellView, IFrameContentFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FrameContentFocusableCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        /// <param name="propertyName">Property corresponding to the component of the node.</param>
        public FrameContentFocusableCellView(IFrameNodeStateView stateView, IFrameFrame frame, string propertyName)
            : base(stateView, frame)
        {
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Property corresponding to the component of the node.
        /// </summary>
        public string PropertyName { get; private set; }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameContentFocusableCellView AsContentFocusableCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsContentFocusableCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
