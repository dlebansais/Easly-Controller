using BaseNode;
using EaslyController.Writeable;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Base for list and block list insertion index classes.
    /// </summary>
    public interface IFrameInsertionCollectionNodeIndex : IWriteableInsertionCollectionNodeIndex, IFrameInsertionChildIndex, IFrameNodeIndex
    {
    }

    /// <summary>
    /// Base for list and block list insertion index classes.
    /// </summary>
    public abstract class FrameInsertionCollectionNodeIndex : WriteableInsertionCollectionNodeIndex, IFrameInsertionCollectionNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameInsertionCollectionNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">The node in which the insertion operation is taking place.</param>
        /// <param name="propertyName">The property for the index.</param>
        /// <param name="node">The inserted node.</param>
        public FrameInsertionCollectionNodeIndex(INode parentNode, string propertyName, INode node)
            : base(parentNode, propertyName, node)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameInsertionCollectionNodeIndex AsInsertionCollectionNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsInsertionCollectionNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
