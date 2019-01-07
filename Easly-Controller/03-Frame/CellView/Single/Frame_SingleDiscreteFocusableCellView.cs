using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Cell view for discrete elements that can receive the focus but are not the component of a node (insertion points, keywords and other decorations)
    /// </summary>
    public interface IFrameSingleDiscreteFocusableCellView : IFrameFocusableCellView
    {
    }

    /// <summary>
    /// Cell view for discrete elements that can receive the focus but are not the component of a node (insertion points, keywords and other decorations)
    /// </summary>
    public class FrameSingleDiscreteFocusableCellView : FrameFocusableCellView, IFrameSingleDiscreteFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FrameSingleDiscreteFocusableCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        public FrameSingleDiscreteFocusableCellView(IFrameNodeStateView stateView)
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

            if (!(other is IFrameSingleDiscreteFocusableCellView AsSingleDiscreteFocusableCellView))
                return false;

            if (!base.IsEqual(comparer, AsSingleDiscreteFocusableCellView))
                return false;

            return true;
        }
        #endregion
    }
}
