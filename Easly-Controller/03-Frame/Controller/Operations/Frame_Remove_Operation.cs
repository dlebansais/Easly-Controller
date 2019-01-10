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
    }
}
