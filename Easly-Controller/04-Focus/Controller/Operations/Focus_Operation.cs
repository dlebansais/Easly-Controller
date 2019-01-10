using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public interface IFocusOperation : IFrameOperation
    {
    }

    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public class FocusOperation : FrameOperation, IFocusOperation
    {
    }
}
