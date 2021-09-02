namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class ReadOnlyIndexNodeStateDictionary : Dictionary<IReadOnlyIndex, IReadOnlyNodeState>
    {
        /// <inheritdoc/>
        public virtual ReadOnlyIndexNodeStateReadOnlyDictionary ToReadOnly()
        {
            return new ReadOnlyIndexNodeStateReadOnlyDictionary(this);
        }
    }
}
