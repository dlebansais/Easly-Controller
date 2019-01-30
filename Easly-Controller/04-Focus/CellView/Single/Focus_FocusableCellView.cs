namespace EaslyController.Focus
{
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// Cell view for discrete elements that can receive the focus but are not always the component of a node (insertion points, keywords and other decorations)
    /// </summary>
    public interface IFocusFocusableCellView : IFrameFocusableCellView, IFocusVisibleCellView
    {
    }

    /// <summary>
    /// Cell view for discrete elements that can receive the focus but are not always the component of a node (insertion points, keywords and other decorations)
    /// </summary>
    public class FocusFocusableCellView : FrameFocusableCellView, IFocusFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusFocusableCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        public FocusFocusableCellView(IFocusNodeStateView stateView, IFocusFrame frame)
            : base(stateView, frame)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new IFocusNodeStateView StateView { get { return (IFocusNodeStateView)base.StateView; } }

        /// <summary>
        /// The frame that created this cell view.
        /// </summary>
        public new IFocusFrame Frame { get { return (IFocusFrame)base.Frame; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Updates the focus chain with cells in the tree.
        /// </summary>
        /// <param name="focusChain">The list of focusable cell views found in the tree.</param>
        public virtual void UpdateFocusChain(IFocusFocusableCellViewList focusChain)
        {
            focusChain.Add(this);
        }
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

            if (!(other is IFocusFocusableCellView AsFocusableCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsFocusableCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
