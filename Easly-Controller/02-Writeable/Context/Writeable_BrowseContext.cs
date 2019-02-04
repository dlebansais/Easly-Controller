namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Context for browsing child nodes of a parent node.
    /// </summary>
    internal interface IWriteableBrowseContext : IReadOnlyBrowseContext
    {
        /// <summary>
        /// State this context is browsing.
        /// </summary>
        new IWriteableNodeState State { get; }

        /// <summary>
        /// List of index collections that have been added during browsing.
        /// </summary>
        new IWriteableIndexCollectionReadOnlyList IndexCollectionList { get; }
    }

    /// <summary>
    /// Context for browsing child nodes of a parent node.
    /// </summary>
    internal class WriteableBrowseContext : ReadOnlyBrowseContext, IWriteableBrowseContext
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBrowseContext"/> class.
        /// </summary>
        /// <param name="state">The state that will be browsed.</param>
        public WriteableBrowseContext(IWriteableNodeState state)
            : base(state)
        {
            Debug.Assert(State == state);
            Debug.Assert(IndexCollectionList != null);
            Debug.Assert(IndexCollectionList.Count == 0);
        }
        #endregion

        #region Properties
        /// <summary>
        /// State this context is browsing.
        /// </summary>
        public new IWriteableNodeState State { get { return (IWriteableNodeState)base.State; } }

        /// <summary>
        /// List of index collections that have been added during browsing.
        /// </summary>
        public new IWriteableIndexCollectionReadOnlyList IndexCollectionList { get { return (IWriteableIndexCollectionReadOnlyList)base.IndexCollectionList; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks the context consistency, for code coverage purpose.
        /// </summary>
        public override void CheckConsistency()
        {
            base.CheckConsistency();

            IWriteableIndexCollectionList InternalList = InternalIndexCollectionList as IWriteableIndexCollectionList;
            IWriteableIndexCollectionReadOnlyList PublicList = IndexCollectionList;

            for (int i = 0; i < InternalList.Count; i++)
            {
                IWriteableIndexCollection InternalItem = InternalList[i];
                Debug.Assert(PublicList.Contains(InternalItem));
                Debug.Assert(PublicList.IndexOf(InternalItem) >= 0);

                IWriteableIndexCollection PublicItem = PublicList[i];
                Debug.Assert(InternalList.Contains(PublicItem));
                Debug.Assert(InternalList.IndexOf(PublicItem) >= 0);

                if (i == 0)
                {
                    Debug.Assert(!((ICollection<IWriteableIndexCollection>)InternalList).IsReadOnly);

                    InternalList.Remove(InternalItem);
                    InternalList.Insert(0, InternalItem);

                    if (InternalList.GetType() == typeof(IWriteableIndexCollectionList))
                        InternalList.CopyTo(new IWriteableIndexCollection[InternalList.Count], 0);

                    IEnumerable<IWriteableIndexCollection> AsEnumerable = InternalList;
                    foreach (IWriteableIndexCollection Item in AsEnumerable)
                    {
                        Debug.Assert(Item == InternalItem);
                        break;
                    }
                }
            }
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCollectionList object.
        /// </summary>
        private protected override IReadOnlyIndexCollectionList CreateIndexCollectionList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBrowseContext));
            return new WriteableIndexCollectionList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollectionReadOnlyList object.
        /// </summary>
        private protected override IReadOnlyIndexCollectionReadOnlyList CreateIndexCollectionListReadOnly(IReadOnlyIndexCollectionList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBrowseContext));
            return new WriteableIndexCollectionReadOnlyList((IWriteableIndexCollectionList)list);
        }
        #endregion
    }
}
