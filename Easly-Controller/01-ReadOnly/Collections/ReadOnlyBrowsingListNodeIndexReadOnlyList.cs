namespace EaslyController.ReadOnly
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class ReadOnlyBrowsingListNodeIndexReadOnlyList : ReadOnlyCollection<IReadOnlyBrowsingListNodeIndex>
    {
        /// <inheritdoc/>
        public ReadOnlyBrowsingListNodeIndexReadOnlyList(ReadOnlyBrowsingListNodeIndexList list)
            : base(list)
        {
        }
    }
}
