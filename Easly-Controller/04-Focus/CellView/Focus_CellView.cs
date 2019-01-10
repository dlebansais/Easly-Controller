using EaslyController.Frame;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Atomic cell view of a component in a node.
    /// </summary>
    public interface IFocusCellView : IFrameCellView
    {
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        new IFocusNodeStateView StateView { get; }
    }

    /// <summary>
    /// Atomic cell view of a component in a node.
    /// </summary>
    public abstract class FocusCellView : FrameCellView, IFocusCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FocusCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        public FocusCellView(IFocusNodeStateView stateView)
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

            if (!(other is IFocusCellView AsCellView))
                return false;

            if (!base.IsEqual(comparer, AsCellView))
                return false;

            return true;
        }
        #endregion
    }
}
