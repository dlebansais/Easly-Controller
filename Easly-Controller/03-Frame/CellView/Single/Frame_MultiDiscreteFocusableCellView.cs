using System.Diagnostics;

namespace EaslyController.Frame
{
    public interface IFrameMultiDiscreteFocusableCellView : IFrameContentFocusableCellView
    {
        string PropertyName { get; }
    }

    public class FrameMultiDiscreteFocusableCellView : FrameContentFocusableCellView, IFrameMultiDiscreteFocusableCellView
    {
        #region Init
        public FrameMultiDiscreteFocusableCellView(IFrameNodeStateView stateView, string propertyName)
            : base(stateView)
        {
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        public string PropertyName { get; private set; }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameMultiDiscreteFocusableCellView AsMultiDiscreteFocusableCellView))
                return false;

            if (!base.IsEqual(comparer, AsMultiDiscreteFocusableCellView))
                return false;

            if (PropertyName != AsMultiDiscreteFocusableCellView.PropertyName)
                return false;

            return true;
        }
        #endregion
    }
}
