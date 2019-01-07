using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Cell view for discrete components that can receive the focus and be modified (enum, bool...)
    /// </summary>
    public interface IFrameMultiDiscreteFocusableCellView : IFrameContentFocusableCellView
    {
        /// <summary>
        /// Property corresponding to the component of the node.
        /// </summary>
        string PropertyName { get; }
    }

    /// <summary>
    /// Cell view for discrete components that can receive the focus and be modified (enum, bool...)
    /// </summary>
    public class FrameMultiDiscreteFocusableCellView : FrameContentFocusableCellView, IFrameMultiDiscreteFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FrameMultiDiscreteFocusableCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="propertyName">Property corresponding to the component of the node.</param>
        public FrameMultiDiscreteFocusableCellView(IFrameNodeStateView stateView, string propertyName)
            : base(stateView)
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
