using BaseNode;
using EaslyController.Writeable;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Index for inserting a node in a list of nodes.
    /// </summary>
    public interface IFrameInsertionListNodeIndex : IWriteableInsertionListNodeIndex, IFrameInsertionCollectionNodeIndex
    {
    }

    /// <summary>
    /// Index for inserting a node in a list of nodes.
    /// </summary>
    public class FrameInsertionListNodeIndex : WriteableInsertionListNodeIndex, IFrameInsertionListNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameInsertionListNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the list.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the list.</param>
        /// <param name="node">Inserted node.</param>
        /// <param name="index">Position where to insert <paramref name="node"/> in the list.</param>
        public FrameInsertionListNodeIndex(INode parentNode, string propertyName, INode node, int index)
            : base(parentNode, propertyName, node, index)
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

            if (!(other is IFrameInsertionListNodeIndex AsInsertionListNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsInsertionListNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        protected override IWriteableBrowsingListNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameInsertionListNodeIndex));
            return new FrameBrowsingListNodeIndex(ParentNode, Node, PropertyName, Index);
        }
        #endregion
    }
}
