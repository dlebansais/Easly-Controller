namespace EaslyController.ReadOnly
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    internal interface IReadOnlyIndexCollection
    {
        /// <summary>
        /// Property indexed for all nodes in the collection.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Collection of node indexes.
        /// </summary>
        IEnumerable NodeIndexList { get; }

        /// <summary>
        /// True is the collection is empty.
        /// </summary>
        bool IsEmpty { get; }
    }

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    /// <typeparam name="TIndex">Type of the index.</typeparam>
    internal class ReadOnlyIndexCollection<TIndex> : IReadOnlyIndexCollection
        where TIndex : IReadOnlyBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyIndexCollection{TIndex}"/> class.
        /// </summary>
        /// <param name="propertyName">Property indexed for all nodes in the collection.</param>
        /// <param name="nodeIndexList">Collection of node indexes.</param>
        public ReadOnlyIndexCollection(string propertyName, IReadOnlyList<TIndex> nodeIndexList)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(IsSamePropertyName(propertyName, nodeIndexList));
            Debug.Assert(nodeIndexList.Count == 0 || !IsSamePropertyName(string.Empty, nodeIndexList));

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
        public IReadOnlyList<TIndex> NodeIndexList { get; }
        IEnumerable IReadOnlyIndexCollection.NodeIndexList { get { return NodeIndexList; } }

        /// <summary>
        /// True is the collection is empty.
        /// </summary>
        public bool IsEmpty { get { return NodeIndexList.Count == 0; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Checks if indexes are already in the collection of indexes.
        /// </summary>
        /// <param name="propertyName">Property indexed for all nodes in <paramref name="nodeIndexList"/>.</param>
        /// <param name="nodeIndexList">Collection of node indexes.</param>
        public static bool IsSamePropertyName(string propertyName, IReadOnlyList<TIndex> nodeIndexList)
        {
            Debug.Assert(propertyName != null); // The empty string is acceptable.
            Debug.Assert(nodeIndexList != null);

            foreach (TIndex item in nodeIndexList)
                if (item.PropertyName != propertyName)
                    return false;

            return true;
        }
        #endregion
    }
}
