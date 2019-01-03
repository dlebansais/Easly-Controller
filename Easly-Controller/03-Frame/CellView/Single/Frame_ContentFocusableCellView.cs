using System.Diagnostics;

namespace EaslyController.Frame
{
    public interface IFrameContentFocusableCellView : IFrameFocusableCellView
    {
    }

    public abstract class FrameContentFocusableCellView : FrameFocusableCellView, IFrameContentFocusableCellView
    {
        #region Init
        public FrameContentFocusableCellView(IFrameNodeStateView stateView)
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

            if (!(other is IFrameContentFocusableCellView AsContentFocusableCellView))
                return false;

            if (!base.IsEqual(comparer, AsContentFocusableCellView))
                return false;

            return true;
        }
        #endregion
    }
}
