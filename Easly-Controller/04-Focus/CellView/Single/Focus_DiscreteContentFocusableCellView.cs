using EaslyController.Frame;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Cell view for discrete components that can receive the focus and be modified (enum, bool...)
    /// </summary>
    public interface IFocusDiscreteContentFocusableCellView : IFrameDiscreteContentFocusableCellView, IFocusContentFocusableCellView
    {
    }

    /// <summary>
    /// Cell view for discrete components that can receive the focus and be modified (enum, bool...)
    /// </summary>
    public class FocusDiscreteContentFocusableCellView : FrameDiscreteContentFocusableCellView, IFocusDiscreteContentFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FocusDiscreteContentFocusableCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="propertyName">Property corresponding to the component of the node.</param>
        public FocusDiscreteContentFocusableCellView(IFocusNodeStateView stateView, string propertyName)
            : base(stateView, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new IFocusNodeStateView StateView { get { return (IFocusNodeStateView)base.StateView; } }
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

            if (!(other is IFocusDiscreteContentFocusableCellView AsMultiDiscreteFocusableCellView))
                return false;

            if (!base.IsEqual(comparer, AsMultiDiscreteFocusableCellView))
                return false;

            return true;
        }
        #endregion
    }
}
