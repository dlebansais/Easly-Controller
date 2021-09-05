namespace EaslyController.Focus
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class FocusFocusReadOnlyList : ReadOnlyCollection<IFocusFocus>
    {
        /// <inheritdoc/>
        public FocusFocusReadOnlyList(FocusFocusList list)
            : base(list)
        {
        }
    }
}
