namespace EaslyController.Focus
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class FocusCycleManagerList : List<IFocusCycleManager>
    {
        /// <inheritdoc/>
        public virtual FocusCycleManagerReadOnlyList ToReadOnly()
        {
            return new FocusCycleManagerReadOnlyList(this);
        }
    }
}
