namespace EaslyController.ReadOnly
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class ReadOnlyIndexNodeStateReadOnlyDictionary : ReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>
    {
        /// <inheritdoc/>
        public ReadOnlyIndexNodeStateReadOnlyDictionary(ReadOnlyIndexNodeStateDictionary dictionary)
            : base(dictionary)
        {
        }
    }
}
