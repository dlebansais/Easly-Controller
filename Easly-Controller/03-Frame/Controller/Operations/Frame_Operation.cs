using EaslyController.Writeable;

namespace EaslyController.Frame
{
    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public interface IFrameOperation : IWriteableOperation
    {
    }

    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public class FrameOperation : WriteableOperation, IFrameOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FrameOperation"/>.
        /// </summary>
        public FrameOperation()
            : base()
        {
        }
        #endregion
    }
}
