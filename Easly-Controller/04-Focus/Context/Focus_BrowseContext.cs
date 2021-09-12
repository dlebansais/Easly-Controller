namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Context for browsing child nodes of a parent node.
    /// </summary>
    internal class FocusBrowseContext : FrameBrowseContext
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBrowseContext"/> class.
        /// </summary>
        /// <param name="state">The state that will be browsed.</param>
        public FocusBrowseContext(IFocusNodeState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State this context is browsing.
        /// </summary>
        public new IFocusNodeState State { get { return (IFocusNodeState)base.State; } }

        /// <summary>
        /// List of index collections that have been added during browsing.
        /// </summary>
        public new FocusIndexCollectionReadOnlyList IndexCollectionList { get { return (FocusIndexCollectionReadOnlyList)base.IndexCollectionList; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks the context consistency, for code coverage purpose.
        /// </summary>
        public override void CheckConsistency()
        {
            base.CheckConsistency();

            Debug.Assert(State != null);

            FocusIndexCollectionList InternalList = InternalIndexCollectionList as FocusIndexCollectionList;
            FocusIndexCollectionReadOnlyList PublicList = IndexCollectionList;

            for (int i = 0; i < InternalList.Count; i++)
            {
                IFocusIndexCollection InternalItem = (IFocusIndexCollection)InternalList[i];
                Debug.Assert(PublicList.Contains(InternalItem));
                Debug.Assert(PublicList.IndexOf(InternalItem) >= 0);

                IFocusIndexCollection PublicItem = (IFocusIndexCollection)PublicList[i];
                Debug.Assert(InternalList.Contains(PublicItem));
                Debug.Assert(InternalList.IndexOf(PublicItem) >= 0);

                if (i == 0)
                {
                    Debug.Assert(!((ICollection<IFocusIndexCollection>)InternalList).IsReadOnly);

                    InternalList.Remove(InternalItem);
                    InternalList.Insert(0, InternalItem);

                    if (InternalList.GetType() == typeof(FocusIndexCollectionList))
                    {
                        InternalList.CopyTo((IReadOnlyIndexCollection[])(new IFocusIndexCollection[InternalList.Count]), 0);
                        InternalList.CopyTo((IWriteableIndexCollection[])(new IFocusIndexCollection[InternalList.Count]), 0);
                        InternalList.CopyTo((IFrameIndexCollection[])(new IFocusIndexCollection[InternalList.Count]), 0);
                    }

                    IEnumerable<IFocusIndexCollection> AsFocusEnumerable = InternalList;
                    foreach (IFocusIndexCollection Item in AsFocusEnumerable)
                    {
                        Debug.Assert(Item == InternalItem);
                        break;
                    }

                    IEnumerable<IWriteableIndexCollection> AsWriteableEnumerable = PublicList;
                    foreach (IFocusIndexCollection Item in AsWriteableEnumerable)
                    {
                        Debug.Assert(Item == InternalItem);
                        break;
                    }

                    IEnumerable<IFrameIndexCollection> AsFrameEnumerable = PublicList;
                    foreach (IFocusIndexCollection Item in AsFrameEnumerable)
                    {
                        Debug.Assert(Item == InternalItem);
                        break;
                    }

                    IList<IFrameIndexCollection> AsIList = InternalList;
                    Debug.Assert(AsIList[0] == InternalItem);

                    IReadOnlyList<IFrameIndexCollection> AsIReadOnlyList;

                    AsIReadOnlyList = InternalList;
                    Debug.Assert(AsIReadOnlyList[0] == InternalItem);

                    AsIReadOnlyList = PublicList;
                    Debug.Assert(AsIReadOnlyList[0] == InternalItem);

                    ICollection<IFrameIndexCollection> AsICollection = InternalList;
                    AsICollection.Remove(InternalItem);
                    AsICollection.Add(InternalItem);
                    AsICollection.Remove(InternalItem);
                    InternalList.Insert(0, InternalItem);

                    IEnumerator<IFrameIndexCollection> InternalListEnumerator = ((ICollection<IFrameIndexCollection>)InternalList).GetEnumerator();
                    InternalListEnumerator.MoveNext();
                    Debug.Assert(InternalListEnumerator.Current == InternalItem);

                    IEnumerator<IFrameIndexCollection> PublicListEnumerator = PublicList.GetEnumerator();
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
            ControllerTools.AssertNoOverride(this, typeof(FocusBrowseContext));
            return new FocusIndexCollectionList();
        }
        #endregion
    }
}
