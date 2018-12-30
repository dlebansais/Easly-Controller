using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    public interface IReadOnlyIndexCollection : IEqualComparable
    {
        /// <summary>
        /// Property indexed for all nodes in the collection.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Collection of node indexes.
        /// </summary>
        IEnumerable NodeIndexList { get; }
    }

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    public interface IReadOnlyIndexCollection<out IIndex> : IEqualComparable
        where IIndex : IReadOnlyBrowsingChildIndex
    {
        /// <summary>
        /// Property indexed for all nodes in the collection.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Collection of node indexes.
        /// </summary>
        IReadOnlyList<IIndex> NodeIndexList { get; }
    }

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    public class ReadOnlyIndexCollection<IIndex> : IReadOnlyIndexCollection<IIndex>, IReadOnlyIndexCollection
        where IIndex : IReadOnlyBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyIndexCollection{IIndex}"/> class.
        /// </summary>
        /// <param name="propertyName">Property indexed for all nodes in the collection.</param>
        /// <param name="nodeIndexList">Collection of node indexes.</param>
        public ReadOnlyIndexCollection(string propertyName, IReadOnlyList<IIndex> nodeIndexList)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(IsSamePropertyName(propertyName, nodeIndexList));

            PropertyName = propertyName;
            NodeIndexList = nodeIndexList;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Property indexed for all nodes in the collection.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Collection of node indexes.
        /// </summary>
        public IReadOnlyList<IIndex> NodeIndexList { get; }
        IEnumerable IReadOnlyIndexCollection.NodeIndexList { get { return NodeIndexList; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Checks if indexes are already in the collection of indexes.
        /// </summary>
        /// <param name="propertyName">Property indexed for all nodes in <paramref name="nodeIndexList"/>.</param>
        /// <param name="nodeIndexList">Collection of node indexes.</param>
        public static bool IsSamePropertyName(string propertyName, IReadOnlyList<IIndex> nodeIndexList)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(nodeIndexList != null);

            foreach (IIndex item in nodeIndexList)
                if (item.PropertyName != propertyName)
                    return false;

            return true;
        }

        /// <summary>
        /// Compares two <see cref="IReadOnlyIndexCollection"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlyIndexCollection<IIndex> AsIndexCollection))
                return false;

            if (NodeIndexList.Count != AsIndexCollection.NodeIndexList.Count)
                return false;

            for (int i = 0; i < NodeIndexList.Count; i++)
                if (!comparer.VerifyEqual(NodeIndexList[i], AsIndexCollection.NodeIndexList[i]))
                    return false;

            return true;
        }
        #endregion
    }
}
