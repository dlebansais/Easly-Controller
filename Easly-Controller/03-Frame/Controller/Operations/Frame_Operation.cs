using EaslyController.Writeable;

namespace EaslyController.Frame
{
    public interface IFrameOperation : IWriteableOperation
    {
    }

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
