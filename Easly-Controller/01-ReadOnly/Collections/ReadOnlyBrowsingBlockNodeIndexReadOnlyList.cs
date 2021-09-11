namespace EaslyController.ReadOnly
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class ReadOnlyBrowsingBlockNodeIndexReadOnlyList : ReadOnlyCollection<IReadOnlyBrowsingBlockNodeIndex>
    {
        /// <inheritdoc/>
        public ReadOnlyBrowsingBlockNodeIndexReadOnlyList(ReadOnlyBrowsingBlockNodeIndexList list)
            : base(list)
        {
        }
    }
}
