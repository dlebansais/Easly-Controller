using EaslyController.Frame;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Cell view for discrete elements that can receive the focus but are not the component of a node (insertion points, keywords and other decorations)
    /// </summary>
    public interface IFocusDiscreteFocusableCellView : IFrameDiscreteFocusableCellView, IFocusFocusableCellView
    {
    }

    /// <summary>
    /// Cell view for discrete elements that can receive the focus but are not the component of a node (insertion points, keywords and other decorations)
    /// </summary>
    public class FocusDiscreteFocusableCellView : FrameDiscreteFocusableCellView, IFocusDiscreteFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FocusDiscreteFocusableCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        public FocusDiscreteFocusableCellView(IFocusNodeStateView stateView)
            : base(stateView)
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

            if (!(other is IFocusDiscreteFocusableCellView AsSingleDiscreteFocusableCellView))
                return false;

            if (!base.IsEqual(comparer, AsSingleDiscreteFocusableCellView))
                return false;

            return true;
        }
        #endregion
    }
}
