using EaslyController.Writeable;

namespace EaslyController.Frame
{
    /// <summary>
    /// Details for insertion operations.
    /// </summary>
    public interface IFrameInsertOperation : IWriteableInsertOperation, IFrameOperation
    {
    }

    /// <summary>
    /// Details for insertion operations.
    /// </summary>
    public abstract class FrameInsertOperation : WriteableInsertOperation, IFrameInsertOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="FrameInsertOperation"/> object.
        /// </summary>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameInsertOperation(bool isNested)
            : base(isNested)
        {
        }
        #endregion
    }
}
