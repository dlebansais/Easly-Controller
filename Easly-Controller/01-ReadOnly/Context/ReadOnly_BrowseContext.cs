using EaslyController.Constants;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Context for browsing child nodes of a parent node.
    /// </summary>
    public interface IReadOnlyBrowseContext
    {
        /// <summary>
        /// State this context is browsing.
        /// </summary>
        IReadOnlyNodeState State { get; }

        /// <summary>
        /// List of index collections that have been added during browsing.
        /// </summary>
        IReadOnlyIndexCollectionReadOnlyList IndexCollectionList { get; }

        /// <summary>
        /// Adds a collection of indexes to <see cref="IndexCollectionList"/>:
        /// . For placeholder node and optional nodes, the collection is just one index.
        /// . For list of nodes, the collection contains as many indexes as nodes.
        /// . For block lists, the collection contains as many indexes as nodes. The first index of each block is a new block index, and others existing block indexes.
        /// </summary>
        /// <param name="collection">The collection to add.</param>
        void AddIndexCollection(IReadOnlyIndexCollection collection);

        /// <summary>
        /// List of properties that are not nodes, list of nodes or block lists, that have been added during browsing.
        /// </summary>
        IReadOnlyDictionary<string, ValuePropertyType> ValuePropertyTypeTable { get; }

        /// <summary>
        /// Adds a property to <see cref="ValuePropertyTypeTable"/>.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="type">Property type.</param>
        void AddValueProperty(string propertyName, ValuePropertyType type);
    }

    /// <summary>
    /// Context for browsing child nodes of a parent node.
    /// </summary>
    public class ReadOnlyBrowseContext : IReadOnlyBrowseContext
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="ReadOnlyBrowseContext"/>.
        /// </summary>
        /// <param name="state">The state that will be browsed.</param>
        public ReadOnlyBrowseContext(IReadOnlyNodeState state)
        {
            Debug.Assert(state != null);

            State = state;
            _IndexCollectionList = CreateIndexCollectionList();
            IndexCollectionList = CreateIndexCollectionListReadOnly(_IndexCollectionList);
            _ValuePropertyTypeTable = new Dictionary<string, ValuePropertyType>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// State this context is browsing.
        /// </summary>
        public IReadOnlyNodeState State { get; }

        /// <summary>
        /// List of index collections that have been added during browsing.
        /// </summary>
        public IReadOnlyIndexCollectionReadOnlyList IndexCollectionList { get; }
        private IReadOnlyIndexCollectionList _IndexCollectionList;

        /// <summary>
        /// List of properties that are not nodes, list of nodes or block lists, that have been added during browsing.
        /// </summary>
        public IReadOnlyDictionary<string, ValuePropertyType> ValuePropertyTypeTable { get { return _ValuePropertyTypeTable; } }
        private Dictionary<string, ValuePropertyType> _ValuePropertyTypeTable;
        #endregion

        #region Client Interface
        /// <summary>
        /// Adds a collection of indexes to <see cref="IndexCollectionList"/>:
        /// . For placeholder node and optional nodes, the collection is just one index.
        /// . For list of nodes, the collection contains as many indexes as nodes.
        /// . For block lists, the collection contains as many indexes as nodes. The first index of each block is a new block index, and others existing block indexes.
        /// </summary>
        /// <param name="collection">The collection to add.</param>
        public virtual void AddIndexCollection(IReadOnlyIndexCollection collection)
        {
            Debug.Assert(collection != null);
            Debug.Assert(IsCollectionSeparate(collection, IndexCollectionList));

            _IndexCollectionList.Add(collection);
        }

        /// <summary>
        /// Adds a property to <see cref="ValuePropertyTypeTable"/>.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="type">Property type.</param>
        public void AddValueProperty(string propertyName, ValuePropertyType type)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(!ValuePropertyTypeTable.ContainsKey(propertyName));

            _ValuePropertyTypeTable.Add(propertyName, type);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Checks if a index collection is contained is a list of index collection.
        /// . Properties must be different.
        /// . They must not share an index.
        /// </summary>
        /// <param name="collection">The collection to check.</param>
        /// <param name="collectionList">The list of collections already accumulated.</param>
        /// <returns></returns>
        public static bool IsCollectionSeparate(IReadOnlyIndexCollection collection, IReadOnlyIndexCollectionReadOnlyList collectionList)
        {
            foreach (IReadOnlyBrowsingChildIndex Index0 in collection.NodeIndexList)
            {
                foreach (IReadOnlyIndexCollection Item in collectionList)
                {
                    if (Item.PropertyName == collection.PropertyName)
                        return true;

                    foreach (IReadOnlyBrowsingChildIndex Index1 in Item.NodeIndexList)
                        if (Index0.Equals(Index1))
                            return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"{State}, {IndexCollectionList.Count} collections, {ValuePropertyTypeTable.Count} values";
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCollectionList object.
        /// </summary>
        protected virtual IReadOnlyIndexCollectionList CreateIndexCollectionList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBrowseContext));
            return new ReadOnlyIndexCollectionList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollectionReadOnlyList object.
        /// </summary>
        /// <param name="list"></param>
        protected virtual IReadOnlyIndexCollectionReadOnlyList CreateIndexCollectionListReadOnly(IReadOnlyIndexCollectionList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBrowseContext));
            return new ReadOnlyIndexCollectionReadOnlyList(list);
        }
        #endregion
    }
}
