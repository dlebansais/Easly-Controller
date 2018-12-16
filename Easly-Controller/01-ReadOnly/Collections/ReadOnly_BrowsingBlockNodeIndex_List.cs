using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// List of IxxxBrowsingBlockNodeIndex
    /// </summary>
    public interface IReadOnlyBrowsingBlockNodeIndexList : IList<IReadOnlyBrowsingBlockNodeIndex>, IReadOnlyList<IReadOnlyBrowsingBlockNodeIndex>
    {
        new int Count { get; }
        new IReadOnlyBrowsingBlockNodeIndex this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxBrowsingBlockNodeIndex
    /// </summary>
    public class ReadOnlyBrowsingBlockNodeIndexList : Collection<IReadOnlyBrowsingBlockNodeIndex>, IReadOnlyBrowsingBlockNodeIndexList
    {
    }
}
