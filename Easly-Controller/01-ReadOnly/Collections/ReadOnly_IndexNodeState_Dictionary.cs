namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public class ReadOnlyIndexNodeStateDictionary : Dictionary<IReadOnlyIndex, IReadOnlyNodeState>
    {
        /// <summary>
        /// Gets a read-only view of the dictionary.
        /// </summary>
        public virtual ReadOnlyIndexNodeStateReadOnlyDictionary ToReadOnly()
        {
            return new ReadOnlyIndexNodeStateReadOnlyDictionary(this);
        }
    }
}
