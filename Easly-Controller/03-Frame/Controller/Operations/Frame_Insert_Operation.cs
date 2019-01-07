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
        /// Initializes a new instance of <see cref="FrameInsertOperation"/>.
        /// </summary>
        public FrameInsertOperation()
            : base()
        {
        }
        #endregion
    }
}
