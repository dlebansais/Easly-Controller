namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Cycle manager for IFeature nodes.
    /// </summary>
    public interface ILayoutCycleManagerFeature : IFocusCycleManagerFeature, ILayoutCycleManager
    {
    }

    /// <summary>
    /// Cycle manager for IFeature nodes.
    /// </summary>
    public class LayoutCycleManagerFeature : FocusCycleManagerFeature, ILayoutCycleManagerFeature
    {
    }
}
