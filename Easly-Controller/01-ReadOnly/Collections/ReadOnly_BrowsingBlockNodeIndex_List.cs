#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

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
