using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Details for insertion operations.
    /// </summary>
    public interface IFocusInsertOperation : IFrameInsertOperation, IFocusOperation
    {
    }

    /// <summary>
    /// Details for insertion operations.
    /// </summary>
    public abstract class FocusInsertOperation : FrameInsertOperation, IFocusInsertOperation
    {
    }
}
