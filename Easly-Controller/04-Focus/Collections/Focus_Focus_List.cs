namespace EaslyController.Focus
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class FocusFocusList : List<IFocusFocus>
    {
        /// <inheritdoc/>
        public virtual FocusFocusReadOnlyList ToReadOnly()
        {
            return new FocusFocusReadOnlyList(this);
        }
    }
}
