using BaseNode;
using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public interface IWriteableBlockListInner : IReadOnlyBlockListInner, IWriteableCollectionInner
    {
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public interface IWriteableBlockListInner<out IIndex> : IReadOnlyBlockListInner<IIndex>, IWriteableCollectionInner<IIndex>
        where IIndex : IWriteableBrowsingBlockNodeIndex
    {
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public class WriteableBlockListInner<IIndex, TIndex> : ReadOnlyBlockListInner<IIndex, TIndex>, IWriteableBlockListInner<IIndex>, IWriteableBlockListInner
        where IIndex : IWriteableBrowsingBlockNodeIndex
        where TIndex : WriteableBrowsingBlockNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBlockListInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public WriteableBlockListInner(IWriteableNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBlockStateList object.
        /// </summary>
        protected override IReadOnlyBlockStateList CreateBlockStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockListInner<IIndex, TIndex>));
            return new WriteableBlockStateList();
        }

        /// <summary>
        /// Creates a IxxxBlockStateReadOnlyList object.
        /// </summary>
        protected override IReadOnlyBlockStateReadOnlyList CreateBlockStateListReadOnly(IReadOnlyBlockStateList blockStateList)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockListInner<IIndex, TIndex>));
            return new WriteableBlockStateReadOnlyList((IWriteableBlockStateList)blockStateList);
        }

        /// <summary>
        /// Creates a IxxxBlockState object.
        /// </summary>
        protected override IReadOnlyBlockState CreateBlockState(IReadOnlyBrowsingNewBlockNodeIndex nodeIndex, IBlock childBlock)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockListInner<IIndex, TIndex>));
            return new WriteableBlockState(this, (IWriteableBrowsingNewBlockNodeIndex)nodeIndex, childBlock);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockListInner<IIndex, TIndex>));
            return new WriteablePlaceholderNodeState((IWriteableNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates an index object.
        /// </summary>
        protected override IIndex CreateNodeIndex(IReadOnlyPlaceholderNodeState state, string propertyName, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockListInner<IIndex, TIndex>));
            return (TIndex)(IWriteableBrowsingBlockNodeIndex)new WriteableBrowsingExistingBlockNodeIndex(Owner.Node, state.Node, propertyName, blockIndex, index);
        }
        #endregion
    }
}
