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
    }
}
