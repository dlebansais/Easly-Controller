namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Base interface for cycle managers.
    /// </summary>
    public interface ILayoutCycleManager : IFocusCycleManager
    {
    }

    /// <summary>
    /// Base class for cycle managers.
    /// </summary>
    public abstract class LayoutCycleManager : FocusCycleManager, ILayoutCycleManager
    {
    }
}
