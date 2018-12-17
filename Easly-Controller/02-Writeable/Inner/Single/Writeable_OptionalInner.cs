using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    public interface IWriteableOptionalInner : IReadOnlyOptionalInner, IWriteableSingleInner
    {
    }

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    public interface IWriteableOptionalInner<out IIndex> : IReadOnlyOptionalInner<IIndex>, IWriteableSingleInner<IIndex>
        where IIndex : IWriteableBrowsingOptionalNodeIndex
    {
    }

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    public class WriteableOptionalInner<IIndex, TIndex> : ReadOnlyOptionalInner<IIndex, TIndex>, IWriteableOptionalInner<IIndex>, IWriteableOptionalInner
        where IIndex : IWriteableBrowsingOptionalNodeIndex
        where TIndex : WriteableBrowsingOptionalNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableOptionalInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public WriteableOptionalInner(IWriteableNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxOptionalNodeState object.
        /// </summary>
        protected override IReadOnlyOptionalNodeState CreateNodeState(IReadOnlyBrowsingOptionalNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalInner<IIndex, TIndex>));
            return new WriteableOptionalNodeState((IWriteableBrowsingOptionalNodeIndex)nodeIndex);
        }
        #endregion
    }
}
