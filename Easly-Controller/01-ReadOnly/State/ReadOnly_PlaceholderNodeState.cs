using BaseNode;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// State of an child node.
    /// </summary>
    public interface IReadOnlyPlaceholderNodeState : IReadOnlyNodeState
    {
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IReadOnlyNodeIndex ParentIndex { get; }
    }

    /// <summary>
    /// State of an child node.
    /// </summary>
    public class ReadOnlyPlaceholderNodeState : ReadOnlyNodeState, IReadOnlyPlaceholderNodeState
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPlaceholderNodeState"/> class.
        /// </summary>
        /// <param name="parentIndex">The index used to create the state.</param>
        public ReadOnlyPlaceholderNodeState(IReadOnlyNodeIndex parentIndex)
            : base(parentIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The child node.
        /// </summary>
        public override INode Node { get { return ParentIndex.Node; } }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IReadOnlyNodeIndex ParentIndex { get { return (IReadOnlyNodeIndex)base.ParentIndex; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyNodeState"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlyPlaceholderNodeState AsPlaceholderNodeState))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsPlaceholderNodeState))
                return comparer.Failed();

            if (Node != AsPlaceholderNodeState.Node)
                return comparer.Failed();

            if (!IsChildrenEqual(comparer, AsPlaceholderNodeState))
                return comparer.Failed();

            return true;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            if (ParentInner == null)
                return "Root node";
            else
                return $"Child node of {ParentInner.InterfaceType.Name}";
        }
        #endregion
    }
}
