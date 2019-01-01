using EaslyController.Writeable;

namespace EaslyController.Frame
{
    /// <summary>
    /// Base interface for any index representing the child node of a parent node.
    /// </summary>
    public interface IFrameChildIndex : IWriteableChildIndex, IFrameIndex
    {
    }
}
