using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyIndexCollection
    {
        string PropertyName { get; }
        IEnumerable NodeIndexList { get; }
    }

    public interface IReadOnlyIndexCollection<out IIndex>
        where IIndex : IReadOnlyBrowsingChildIndex
    {
        string PropertyName { get; }
        IReadOnlyList<IIndex> NodeIndexList { get; }
    }

    public class ReadOnlyIndexCollection<IIndex> : IReadOnlyIndexCollection<IIndex>, IReadOnlyIndexCollection
        where IIndex : IReadOnlyBrowsingChildIndex
    {
        #region Init
        public ReadOnlyIndexCollection(string propertyName, IReadOnlyList<IIndex> nodeIndexList)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(IsSamePropertyName(propertyName, nodeIndexList));

            PropertyName = propertyName;
            NodeIndexList = nodeIndexList;
        }
        #endregion

        #region Properties
        public string PropertyName { get; }
        public IReadOnlyList<IIndex> NodeIndexList { get; }
        IEnumerable IReadOnlyIndexCollection.NodeIndexList { get { return NodeIndexList; } }
        #endregion

        #region Debugging
        public static bool IsSamePropertyName(string propertyName, IReadOnlyList<IIndex> nodeIndexList)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(nodeIndexList != null);

            foreach (IIndex item in nodeIndexList)
                if (item.PropertyName != propertyName)
                    return false;

            return true;
        }
        #endregion
    }
}
