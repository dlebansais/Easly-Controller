using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    public interface IWriteableListInner : IReadOnlyListInner, IWriteableCollectionInner
    {
    }

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    public interface IWriteableListInner<out IIndex> : IReadOnlyListInner<IIndex>, IWriteableCollectionInner<IIndex>
        where IIndex : IWriteableBrowsingListNodeIndex
    {
    }

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    public class WriteableListInner<IIndex, TIndex> : ReadOnlyListInner<IIndex, TIndex>, IWriteableListInner<IIndex>, IWriteableListInner
        where IIndex : IWriteableBrowsingListNodeIndex
        where TIndex : WriteableBrowsingListNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableListInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public WriteableListInner(IWriteableNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateList object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableListInner<IIndex, TIndex>));
            return new WriteablePlaceholderNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateReadOnlyList object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeStateReadOnlyList CreateStateListReadOnly(IReadOnlyPlaceholderNodeStateList stateList)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableListInner<IIndex, TIndex>));
            return new WriteablePlaceholderNodeStateReadOnlyList((IWriteablePlaceholderNodeStateList)stateList);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableListInner<IIndex, TIndex>));
            return new WriteablePlaceholderNodeState((IWriteableNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected override IIndex CreateNodeIndex(IReadOnlyPlaceholderNodeState state, string propertyName, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableListInner<IIndex, TIndex>));
            return (TIndex)new WriteableBrowsingListNodeIndex(Owner.Node, state.Node, propertyName, index);
        }
        #endregion
    }
}
