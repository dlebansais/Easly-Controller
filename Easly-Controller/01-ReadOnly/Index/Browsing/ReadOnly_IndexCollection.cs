using System.Collections;
using System.Collections.Generic;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyIndexCollection
    {
        string PropertyName { get; }
        IEnumerable NodeIndexList { get; }
    }

    public interface IReadOnlyIndexCollection<out IIndex>
        where IIndex : IReadOnlyBrowsingChildNodeIndex
    {
        string PropertyName { get; }
        IReadOnlyList<IIndex> NodeIndexList { get; }
    }

    public class ReadOnlyIndexCollection<IIndex> : IReadOnlyIndexCollection<IIndex>, IReadOnlyIndexCollection
        where IIndex : IReadOnlyBrowsingChildNodeIndex
    {
        #region Init
        public ReadOnlyIndexCollection(string propertyName, IReadOnlyList<IIndex> nodeIndexList)
        {
            PropertyName = propertyName;
            NodeIndexList = nodeIndexList;
        }
        #endregion

        #region Properties
        public string PropertyName { get; private set; }
        string IReadOnlyIndexCollection.PropertyName { get { return PropertyName; } }
        public IReadOnlyList<IIndex> NodeIndexList { get; private set; }
        IEnumerable IReadOnlyIndexCollection.NodeIndexList { get { return NodeIndexList; } }
        #endregion
    }
}
