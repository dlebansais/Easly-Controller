namespace EaslyController.Focus
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class FocusCycleManagerReadOnlyList : ReadOnlyCollection<IFocusCycleManager>
    {
        /// <inheritdoc/>
        public FocusCycleManagerReadOnlyList(FocusCycleManagerList list)
            : base(list)
        {
        }
    }
}
