#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Read-only dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public interface IReadOnlyIndexNodeStateReadOnlyDictionary : IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>
    {
    }

    /// <summary>
    /// Read-only dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    internal class ReadOnlyIndexNodeStateReadOnlyDictionary : ReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>, IReadOnlyIndexNodeStateReadOnlyDictionary
    {
        public ReadOnlyIndexNodeStateReadOnlyDictionary(IReadOnlyIndexNodeStateDictionary dictionary)
            : base(dictionary)
        {
        }
    }
}
