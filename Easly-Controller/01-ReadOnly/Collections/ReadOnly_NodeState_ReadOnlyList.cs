namespace EaslyController.ReadOnly
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class ReadOnlyNodeStateReadOnlyList : ReadOnlyCollection<IReadOnlyNodeState>
    {
        /// <inheritdoc/>
        public ReadOnlyNodeStateReadOnlyList(ReadOnlyNodeStateList list)
            : base(list)
        {
        }
    }
}
