namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class ReadOnlyNodeStateReadOnlyDictionary : System.Collections.ObjectModel.ReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>
    {
        /// <inheritdoc/>
        public ReadOnlyNodeStateReadOnlyDictionary(ReadOnlyNodeStateDictionary dictionary)
            : base(dictionary)
        {
        }
    }
}
