namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Cycle manager for Body nodes.
    /// </summary>
    public interface ILayoutCycleManagerBody : IFocusCycleManagerBody, ILayoutCycleManager
    {
    }

    /// <summary>
    /// Cycle manager for Body nodes.
    /// </summary>
    public class LayoutCycleManagerBody : FocusCycleManagerBody, ILayoutCycleManagerBody
    {
    }
}