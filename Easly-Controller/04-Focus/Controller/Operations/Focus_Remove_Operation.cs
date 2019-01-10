using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Details for removal operations.
    /// </summary>
    public interface IFocusRemoveOperation : IFrameRemoveOperation, IFocusOperation
    {
    }

    /// <summary>
    /// Details for removal operations.
    /// </summary>
    public abstract class FocusRemoveOperation : FrameRemoveOperation, IFocusRemoveOperation
    {
    }
}
