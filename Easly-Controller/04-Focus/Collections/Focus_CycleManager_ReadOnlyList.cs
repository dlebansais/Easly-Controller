namespace EaslyController.Focus
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class FocusCycleManagerReadOnlyList : ReadOnlyCollection<FocusCycleManager>
    {
        /// <inheritdoc/>
        public FocusCycleManagerReadOnlyList(FocusCycleManagerList list)
            : base(list)
        {
        }
    }
}
