namespace EaslyController.ReadOnly
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class ReadOnlyNodeStateReadOnlyDictionary : ReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>
    {
        /// <inheritdoc/>
        public ReadOnlyNodeStateReadOnlyDictionary(ReadOnlyNodeStateDictionary dictionary)
            : base(dictionary)
        {
        }
    }
}
