﻿using System.Diagnostics;

namespace EaslyController.Frame
{
    public interface IFrameTextFocusableCellView : IFrameContentFocusableCellView
    {
    }

    public class FrameTextFocusableCellView : FrameContentFocusableCellView, IFrameTextFocusableCellView
    {
        #region Init
        public FrameTextFocusableCellView(IFrameNodeStateView stateView)
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

            if (!(other is IFrameTextFocusableCellView AsTextFocusableCellView))
                return false;

            if (!base.IsEqual(comparer, AsTextFocusableCellView))
                return false;

            return true;
        }
        #endregion
    }
}
