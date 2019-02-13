namespace EaslyController.Layout
{
    using EaslyController.Constants;

    /// <summary>
    /// Frame that can have a custom text style.
    /// </summary>
    public interface ILayoutNodeFrameWithTextStyle : ILayoutNodeFrame
    {
        /// <summary>
        /// Text style.
        /// (Set in Xaml)
        /// </summary>
        TextStyles TextStyle { get; }
    }
}
