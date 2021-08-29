#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Read-only dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public class ReadOnlyIndexNodeStateReadOnlyDictionary : ReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>
    {
        public ReadOnlyIndexNodeStateReadOnlyDictionary(ReadOnlyIndexNodeStateDictionary dictionary)
            : base(dictionary)
        {
        }
    }
}
