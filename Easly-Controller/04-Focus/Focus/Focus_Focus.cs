namespace EaslyController.Focus
{
    using System;
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Base focus.
    /// </summary>
    public interface IFocusFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        IFocusFocusableCellView CellView { get; }

        /// <summary>
        /// Get the location in the source code corresponding to this focus.
        /// </summary>
        /// <param name="node">The node in the source code upon return.</param>
        /// <param name="frame">The frame in the template associated to <paramref name="node"/> that indicates where in the node is the focus upon return.</param>
        void GetLocationInSourceCode(out Node node, out IFocusFrame frame);
    }

    /// <summary>
    /// Base focus.
    /// </summary>
    public class FocusFocus : IFocusFocus
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusFocus"/> class.
        /// </summary>
        public FocusFocus(IFocusFocusableCellView cellView)
        {
            CellView = cellView;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        public IFocusFocusableCellView CellView { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Get the location in the source code corresponding to this focus.
        /// </summary>
        /// <param name="node">The node in the source code upon return.</param>
        /// <param name="frame">The frame in the template associated to <paramref name="node"/> that indicates where in the node is the focus upon return.</param>
        public virtual void GetLocationInSourceCode(out Node node, out IFocusFrame frame)
        {
            node = null;
            frame = null;

            // We might be reading the focus of a node that has just been unassigned, so don't assert that IsAssigned == true.
            IFocusOptionalNodeState AsOptionalNodeState = CellView.StateView.State as IFocusOptionalNodeState;
            if (AsOptionalNodeState == null || AsOptionalNodeState.ParentInner.IsAssigned)
            {
                node = CellView.StateView.State.Node;
                frame = CellView.Frame;
            }
        }
        #endregion

        #region Debugging
        /// <summary>
        /// A hash code for debug purpose.
        /// </summary>
        public ulong Hash { get; } = GetInitHash();

        private static ulong GetInitHash()
        {
            return (ulong)Rand.Next();
        }

        private static Random Rand = new Random();
        #endregion
    }
}
