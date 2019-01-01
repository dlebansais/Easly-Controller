using EaslyController.Writeable;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// View of a node state.
    /// </summary>
    public interface IFrameNodeStateView : IWriteableNodeStateView
    {
        /// <summary>
        /// The node state.
        /// </summary>
        new IFrameNodeState State { get; }
    }

    /// <summary>
    /// View of a node state.
    /// </summary>
    public class FrameNodeStateView : WriteableNodeStateView, IFrameNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameNodeStateView"/> class.
        /// </summary>
        /// <param name="state">The node state.</param>
        public FrameNodeStateView(IFrameNodeState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The node state.
        /// </summary>
        public new IFrameNodeState State { get { return (IFrameNodeState)base.State; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameNodeStateView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameNodeStateView AsFrame))
                return false;

            if (!base.IsEqual(comparer, AsFrame))
                return false;

            return true;
        }
        #endregion
    }
}
