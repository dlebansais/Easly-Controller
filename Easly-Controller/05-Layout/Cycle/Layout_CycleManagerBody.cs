namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Cycle manager for IBody nodes.
    /// </summary>
    public interface ILayoutCycleManagerBody : IFocusCycleManagerBody, ILayoutCycleManager
    {
    }

    /// <summary>
    /// Cycle manager for IBody nodes.
    /// </summary>
    public class LayoutCycleManagerBody : FocusCycleManagerBody, ILayoutCycleManagerBody
    {
    }
}