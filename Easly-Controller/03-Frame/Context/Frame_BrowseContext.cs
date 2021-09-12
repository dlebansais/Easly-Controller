namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Context for browsing child nodes of a parent node.
    /// </summary>
    internal class FrameBrowseContext : WriteableBrowseContext
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBrowseContext"/> class.
        /// </summary>
        /// <param name="state">The state that will be browsed.</param>
        public FrameBrowseContext(IFrameNodeState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State this context is browsing.
        /// </summary>
        public new IFrameNodeState State { get { return (IFrameNodeState)base.State; } }

        /// <summary>
        /// List of index collections that have been added during browsing.
        /// </summary>
        public new FrameIndexCollectionReadOnlyList IndexCollectionList { get { return (FrameIndexCollectionReadOnlyList)base.IndexCollectionList; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks the context consistency, for code coverage purpose.
        /// </summary>
        public override void CheckConsistency()
        {
            base.CheckConsistency();

            Debug.Assert(State != null);

            FrameIndexCollectionList InternalList = InternalIndexCollectionList as FrameIndexCollectionList;
            FrameIndexCollectionReadOnlyList PublicList = IndexCollectionList;

            for (int i = 0; i < InternalList.Count; i++)
            {
                IFrameIndexCollection InternalItem = (IFrameIndexCollection)InternalList[i];
                Debug.Assert(PublicList.Contains(InternalItem));
                Debug.Assert(PublicList.IndexOf(InternalItem) >= 0);

                IFrameIndexCollection PublicItem = (IFrameIndexCollection)PublicList[i];
                Debug.Assert(InternalList.Contains(PublicItem));
                Debug.Assert(InternalList.IndexOf(PublicItem) >= 0);

                if (i == 0)
                {
                    Debug.Assert(!((ICollection<IFrameIndexCollection>)InternalList).IsReadOnly);

                    InternalList.Remove(InternalItem);
                    InternalList.Insert(0, InternalItem);

                    if (InternalList.GetType() == typeof(FrameIndexCollectionList))
                    {
                        InternalList.CopyTo((IReadOnlyIndexCollection[])(new IFrameIndexCollection[InternalList.Count]), 0);
                        InternalList.CopyTo((IWriteableIndexCollection[])(new IFrameIndexCollection[InternalList.Count]), 0);
                    }

                    IEnumerable<IFrameIndexCollection> AsFrameEnumerable = InternalList;
                    foreach (IFrameIndexCollection Item in AsFrameEnumerable)
                    {
                        Debug.Assert(Item == InternalItem);
                        break;
                    }

                    IEnumerable<IWriteableIndexCollection> AsWriteableEnumerable = PublicList;
                    foreach (IFrameIndexCollection Item in AsWriteableEnumerable)
                    {
                        Debug.Assert(Item == InternalItem);
                        break;
                    }

                    IList<IWriteableIndexCollection> AsIList = InternalList;
                    Debug.Assert(AsIList[0] == InternalItem);

                    IReadOnlyList<IWriteableIndexCollection> AsIReadOnlyList;

                    AsIReadOnlyList = InternalList;
                    Debug.Assert(AsIReadOnlyList[0] == InternalItem);

                    AsIReadOnlyList = PublicList;
                    Debug.Assert(AsIReadOnlyList[0] == InternalItem);

                    ICollection<IWriteableIndexCollection> AsICollection = InternalList;
                    AsICollection.Remove(InternalItem);
                    AsICollection.Add(InternalItem);
                    AsICollection.Remove(InternalItem);
                    InternalList.Insert(0, InternalItem);

                    IEnumerator<IWriteableIndexCollection> InternalListEnumerator = ((IList<IWriteableIndexCollection>)InternalList).GetEnumerator();
                    InternalListEnumerator.MoveNext();
                    Debug.Assert(InternalListEnumerator.Current == InternalItem);

                    IEnumerator<IWriteableIndexCollection> PublicListEnumerator = PublicList.GetEnumerator();
                    PublicListEnumerator.MoveNext();
                    Debug.Assert(PublicListEnumerator.Current == InternalItem);
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
            ControllerTools.AssertNoOverride(this, typeof(FrameBrowseContext));
            return new FrameIndexCollectionList();
        }
        #endregion
    }
}
