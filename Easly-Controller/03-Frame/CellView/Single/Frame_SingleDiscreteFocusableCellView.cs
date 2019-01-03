using System.Diagnostics;

namespace EaslyController.Frame
{
    public interface IFrameSingleDiscreteFocusableCellView : IFrameFocusableCellView
    {
    }

    public class FrameSingleDiscreteFocusableCellView : FrameFocusableCellView, IFrameSingleDiscreteFocusableCellView
    {
        #region Init
        public FrameSingleDiscreteFocusableCellView(IFrameNodeStateView stateView)
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

            if (!(other is IFrameSingleDiscreteFocusableCellView AsSingleDiscreteFocusableCellView))
                return false;

            if (!base.IsEqual(comparer, AsSingleDiscreteFocusableCellView))
                return false;

            return true;
        }
        #endregion
    }
}
