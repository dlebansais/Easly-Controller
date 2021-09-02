namespace EaslyController.ReadOnly
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class ReadOnlyBlockStateReadOnlyList : ReadOnlyCollection<IReadOnlyBlockState>
    {
        /// <inheritdoc/>
        public ReadOnlyBlockStateReadOnlyList(ReadOnlyBlockStateList list)
            : base(list)
        {
        }
    }
}
