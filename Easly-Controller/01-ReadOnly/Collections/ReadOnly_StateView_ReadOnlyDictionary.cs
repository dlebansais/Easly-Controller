namespace EaslyController.ReadOnly
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class ReadOnlyStateViewReadOnlyDictionary : ReadOnlyDictionary<IReadOnlyNodeState, ReadOnlyNodeStateView>
    {
        /// <inheritdoc/>
        public ReadOnlyStateViewReadOnlyDictionary(ReadOnlyStateViewDictionary dictionary)
            : base(dictionary)
        {
        }
    }
}
