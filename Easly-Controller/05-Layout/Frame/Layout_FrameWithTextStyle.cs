namespace EaslyController.Layout
{
    using EaslyController.Constants;

    /// <summary>
    /// Frame that can have a custom text style.
    /// </summary>
    public interface ILayoutFrameWithTextStyle : ILayoutFrame
    {
        /// <summary>
        /// Text style.
        /// (Set in Xaml)
        /// </summary>
        TextStyles TextStyle { get; }
    }
}
