namespace EaslyController.Focus
{
    public interface IFocusNodeFrameWithVisibility : IFocusNodeFrame
    {
        /// <summary>
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        IFocusNodeFrameVisibility Visibility { get; set; }
    }
}
