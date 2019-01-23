using System;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Operation details for replacing a node.
    /// </summary>
    public interface IWriteableGenericRefreshOperation : IWriteableOperation
    {
        /// <summary>
        /// State in the source where to start refresh.
        /// </summary>
        IWriteableNodeState RefreshState { get; }
    }

    /// <summary>
    /// Operation details for replacing a node in a list or block list.
    /// </summary>
    public class WriteableGenericRefreshOperation : WriteableOperation, IWriteableGenericRefreshOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableReplaceOperation"/>.
        /// </summary>
        /// <param name="refreshState">State in the source where to start refresh.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableGenericRefreshOperation(IWriteableNodeState refreshState, Action<IWriteableOperation> handlerRedo, bool isNested)
            : base(handlerRedo, isNested)
        {
            RefreshState = refreshState;
        }
        #endregion

        #region Properties
        /// <summary>
        /// State in the source where to start refresh.
        /// </summary>
        public IWriteableNodeState RefreshState { get; }
        #endregion
    }
}
