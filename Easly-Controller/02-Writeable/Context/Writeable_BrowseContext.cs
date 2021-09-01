namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Context for browsing child nodes of a parent node.
    /// </summary>
    internal class WriteableBrowseContext : ReadOnlyBrowseContext
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
        public new WriteableIndexCollectionReadOnlyList IndexCollectionList { get { return (WriteableIndexCollectionReadOnlyList)base.IndexCollectionList; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks the context consistency, for code coverage purpose.
        /// </summary>
        public override void CheckConsistency()
        {
            base.CheckConsistency();

            Debug.Assert(State != null);

            WriteableIndexCollectionList InternalList = InternalIndexCollectionList as WriteableIndexCollectionList;
            WriteableIndexCollectionReadOnlyList PublicList = IndexCollectionList;

            for (int i = 0; i < InternalList.Count; i++)
            {
                IWriteableIndexCollection InternalItem = (IWriteableIndexCollection)InternalList[i];
                Debug.Assert(PublicList.Contains(InternalItem));
                Debug.Assert(PublicList.IndexOf(InternalItem) >= 0);

                IWriteableIndexCollection PublicItem = (IWriteableIndexCollection)PublicList[i];
                Debug.Assert(InternalList.Contains(PublicItem));
                Debug.Assert(InternalList.IndexOf(PublicItem) >= 0);

                if (i == 0)
                {
                    Debug.Assert(!((ICollection<IWriteableIndexCollection>)InternalList).IsReadOnly);

                    InternalList.Remove(InternalItem);
                    InternalList.Insert(0, InternalItem);

                    if (InternalList.GetType() == typeof(WriteableIndexCollectionList))
                        InternalList.CopyTo((IReadOnlyIndexCollection[])(new IWriteableIndexCollection[InternalList.Count]), 0);

                    IEnumerable<IWriteableIndexCollection> AsEnumerable = InternalList;
                    foreach (IWriteableIndexCollection Item in AsEnumerable)
                    {
                        Debug.Assert(Item == InternalItem);
                        break;
                    }

                    IList<IReadOnlyIndexCollection> AsIList = InternalList;
                    Debug.Assert(AsIList[0] == InternalItem);

                    IReadOnlyList<IReadOnlyIndexCollection> AsIReadOnlyList = InternalList;
                    Debug.Assert(AsIReadOnlyList[0] == InternalItem);
                }
            }
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCollectionList object.
        /// </summary>
        private protected override ReadOnlyIndexCollectionList CreateIndexCollectionList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBrowseContext));
            return new WriteableIndexCollectionList();
        }
        #endregion
    }
}
