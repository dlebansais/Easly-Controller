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
        new IReadOnlyBrowsingBlockNodeIndex this[int index] { get; set; }
        new int Count { get; }
    }

    /// <summary>
    /// List of IxxxBrowsingBlockNodeIndex
    /// </summary>
    internal class ReadOnlyBrowsingBlockNodeIndexList : Collection<IReadOnlyBrowsingBlockNodeIndex>, IReadOnlyBrowsingBlockNodeIndexList, IReadOnlyList<IReadOnlyBrowsingBlockNodeIndex>
    {
    }
}
