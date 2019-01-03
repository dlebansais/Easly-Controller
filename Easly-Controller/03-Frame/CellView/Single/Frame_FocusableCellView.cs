using System.Diagnostics;

namespace EaslyController.Frame
{
    public interface IFrameFocusableCellView : IFrameVisibleCellView
    {
    }

    public abstract class FrameFocusableCellView : FrameVisibleCellView, IFrameFocusableCellView
    {
        #region Init
        public FrameFocusableCellView(IFrameNodeStateView stateView)
            : base(stateView)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameFocusableCellView AsFocusableCellView))
                return false;

            if (!base.IsEqual(comparer, AsFocusableCellView))
                return false;

            return true;
        }
        #endregion
    }
}
