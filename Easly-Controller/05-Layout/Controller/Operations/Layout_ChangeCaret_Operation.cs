namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Operation details for changing the caret in a string property or comment.
    /// </summary>
    public interface ILayoutChangeCaretOperation : IFocusChangeCaretOperation, ILayoutOperation
    {
    }
}
