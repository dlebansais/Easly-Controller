namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public interface IReadOnlyIndexNodeStateDictionary : IDictionary<IReadOnlyIndex, IReadOnlyNodeState>
    {
        /// <summary>
        /// Gets a read-only view of the dictionary.
        /// </summary>
        IReadOnlyIndexNodeStateReadOnlyDictionary ToReadOnly();
    }

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    internal class ReadOnlyIndexNodeStateDictionary : Dictionary<IReadOnlyIndex, IReadOnlyNodeState>, IReadOnlyIndexNodeStateDictionary
    {
        /// <summary>
        /// Gets a read-only view of the dictionary.
        /// </summary>
        public virtual IReadOnlyIndexNodeStateReadOnlyDictionary ToReadOnly()
        {
            return new ReadOnlyIndexNodeStateReadOnlyDictionary(this);
        }
    }
}
