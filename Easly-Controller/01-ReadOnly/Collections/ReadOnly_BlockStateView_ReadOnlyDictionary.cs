namespace EaslyController.ReadOnly
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class ReadOnlyBlockStateViewReadOnlyDictionary : ReadOnlyDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>
    {
        /// <inheritdoc/>
        public ReadOnlyBlockStateViewReadOnlyDictionary(ReadOnlyBlockStateViewDictionary dictionary)
            : base(dictionary)
        {
        }
    }
}
