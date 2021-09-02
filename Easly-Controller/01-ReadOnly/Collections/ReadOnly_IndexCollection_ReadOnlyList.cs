namespace EaslyController.ReadOnly
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class ReadOnlyIndexCollectionReadOnlyList : ReadOnlyCollection<IReadOnlyIndexCollection>
    {
        /// <inheritdoc/>
        public ReadOnlyIndexCollectionReadOnlyList(ReadOnlyIndexCollectionList list)
            : base(list)
        {
        }
    }
}
