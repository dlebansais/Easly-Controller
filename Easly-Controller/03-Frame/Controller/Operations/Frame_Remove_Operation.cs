using EaslyController.Writeable;

namespace EaslyController.Frame
{
    /// <summary>
    /// Details for removal operations.
    /// </summary>
    public interface IFrameRemoveOperation : IWriteableRemoveOperation, IFrameOperation
    {
    }

    /// <summary>
    /// Details for removal operations.
    /// </summary>
    public abstract class FrameRemoveOperation : WriteableRemoveOperation, IFrameRemoveOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="FrameRemoveOperation"/> object.
        /// </summary>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameRemoveOperation(bool isNested)
            : base(isNested)
        {
        }
        #endregion
    }
}
