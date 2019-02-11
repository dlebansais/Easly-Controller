namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Context for browsing child nodes of a parent node.
    /// </summary>
    internal interface ILayoutBrowseContext : IFocusBrowseContext
    {
        /// <summary>
        /// State this context is browsing.
        /// </summary>
        new ILayoutNodeState State { get; }

        /// <summary>
        /// List of index collections that have been added during browsing.
        /// </summary>
        new ILayoutIndexCollectionReadOnlyList IndexCollectionList { get; }
    }

    /// <summary>
    /// Context for browsing child nodes of a parent node.
    /// </summary>
    internal class LayoutBrowseContext : FocusBrowseContext, ILayoutBrowseContext
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBrowseContext"/> class.
        /// </summary>
        /// <param name="state">The state that will be browsed.</param>
        public LayoutBrowseContext(ILayoutNodeState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State this context is browsing.
        /// </summary>
        public new ILayoutNodeState State { get { return (ILayoutNodeState)base.State; } }

        /// <summary>
        /// List of index collections that have been added during browsing.
        /// </summary>
        public new ILayoutIndexCollectionReadOnlyList IndexCollectionList { get { return (ILayoutIndexCollectionReadOnlyList)base.IndexCollectionList; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks the context consistency, for code coverage purpose.
        /// </summary>
        public override void CheckConsistency()
        {
            base.CheckConsistency();

            Debug.Assert(State != null);

            ILayoutIndexCollectionList InternalList = InternalIndexCollectionList as ILayoutIndexCollectionList;
            ILayoutIndexCollectionReadOnlyList PublicList = IndexCollectionList;

            for (int i = 0; i < InternalList.Count; i++)
            {
                ILayoutIndexCollection InternalItem = InternalList[i];
                Debug.Assert(PublicList.Contains(InternalItem));
                Debug.Assert(PublicList.IndexOf(InternalItem) >= 0);

                ILayoutIndexCollection PublicItem = PublicList[i];
                Debug.Assert(InternalList.Contains(PublicItem));
                Debug.Assert(InternalList.IndexOf(PublicItem) >= 0);

                if (i == 0)
                {
                    Debug.Assert(!((ICollection<ILayoutIndexCollection>)InternalList).IsReadOnly);

                    InternalList.Remove(InternalItem);
                    InternalList.Insert(0, InternalItem);

                    if (InternalList.GetType() == typeof(LayoutIndexCollectionList))
                    {
                        InternalList.CopyTo((IReadOnlyIndexCollection[])(new ILayoutIndexCollection[InternalList.Count]), 0);
                        InternalList.CopyTo((IWriteableIndexCollection[])(new ILayoutIndexCollection[InternalList.Count]), 0);
                        InternalList.CopyTo((IFocusIndexCollection[])(new ILayoutIndexCollection[InternalList.Count]), 0);
                    }

                    IEnumerable<ILayoutIndexCollection> AsLayoutEnumerable = InternalList;
                    foreach (ILayoutIndexCollection Item in AsLayoutEnumerable)
                    {
                        Debug.Assert(Item == InternalItem);
                        break;
                    }

                    IEnumerable<IWriteableIndexCollection> AsWriteableEnumerable = PublicList;
                    foreach (ILayoutIndexCollection Item in AsWriteableEnumerable)
                    {
                        Debug.Assert(Item == InternalItem);
                        break;
                    }

                    IEnumerable<IFocusIndexCollection> AsFocusEnumerable = PublicList;
                    foreach (ILayoutIndexCollection Item in AsFocusEnumerable)
                    {
                        Debug.Assert(Item == InternalItem);
                        break;
                    }

                    IList<IFocusIndexCollection> AsIList = InternalList;
                    Debug.Assert(AsIList[0] == InternalItem);

                    IReadOnlyList<IFocusIndexCollection> AsIReadOnlyList;

                    AsIReadOnlyList = InternalList;
                    Debug.Assert(AsIReadOnlyList[0] == InternalItem);

                    AsIReadOnlyList = PublicList;
                    Debug.Assert(AsIReadOnlyList[0] == InternalItem);

                    ICollection<IFocusIndexCollection> AsICollection = InternalList;
                    AsICollection.Remove(InternalItem);
                    AsICollection.Add(InternalItem);
                    AsICollection.Remove(InternalItem);
                    InternalList.Insert(0, InternalItem);

                    IEnumerator<IFocusIndexCollection> InternalListEnumerator = ((IFocusIndexCollectionList)InternalList).GetEnumerator();
                    InternalListEnumerator.MoveNext();
                    Debug.Assert(InternalListEnumerator.Current == InternalItem);

                    IEnumerator<IFocusIndexCollection> PublicListEnumerator = ((IFocusIndexCollectionReadOnlyList)PublicList).GetEnumerator();
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
        private protected override IReadOnlyIndexCollectionList CreateIndexCollectionList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutBrowseContext));
            return new LayoutIndexCollectionList();
        }
        #endregion
    }
}
