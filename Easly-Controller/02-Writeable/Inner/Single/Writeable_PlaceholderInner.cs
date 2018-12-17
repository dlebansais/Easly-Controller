using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Inner for a child node.
    /// </summary>
    public interface IWriteablePlaceholderInner : IReadOnlyPlaceholderInner, IWriteableSingleInner
    {
    }

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    public interface IWriteablePlaceholderInner<out IIndex> : IReadOnlyPlaceholderInner<IIndex>, IWriteableSingleInner<IIndex>
        where IIndex : IWriteableBrowsingPlaceholderNodeIndex
    {
    }

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    public class WriteablePlaceholderInner<IIndex, TIndex> : ReadOnlyPlaceholderInner<IIndex, TIndex>, IWriteablePlaceholderInner<IIndex>, IWriteablePlaceholderInner
        where IIndex : IWriteableBrowsingPlaceholderNodeIndex
        where TIndex : WriteableBrowsingPlaceholderNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteablePlaceholderInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public WriteablePlaceholderInner(IWriteableNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyBrowsingPlaceholderNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteablePlaceholderInner<IIndex, TIndex>));
            return new WriteablePlaceholderNodeState((IWriteableBrowsingPlaceholderNodeIndex)nodeIndex);
        }
        #endregion
    }
}
