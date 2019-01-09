using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Cell view for discrete elements that can receive the focus but are not the component of a node (insertion points, keywords and other decorations)
    /// </summary>
    public interface IFrameDiscreteFocusableCellView : IFrameFocusableCellView
    {
    }

    /// <summary>
    /// Cell view for discrete elements that can receive the focus but are not the component of a node (insertion points, keywords and other decorations)
    /// </summary>
    public class FrameDiscreteFocusableCellView : FrameFocusableCellView, IFrameDiscreteFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FrameDiscreteFocusableCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        public FrameDiscreteFocusableCellView(IFrameNodeStateView stateView)
            : base(stateView)
        {
        }
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

            if (!(other is IFrameDiscreteFocusableCellView AsSingleDiscreteFocusableCellView))
                return false;

            if (!base.IsEqual(comparer, AsSingleDiscreteFocusableCellView))
                return false;

            return true;
        }
        #endregion
    }
}
