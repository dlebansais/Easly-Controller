﻿namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// Context for browsing child nodes of a parent node.
    /// </summary>
    internal class LayoutBrowseContext : FocusBrowseContext
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
        public new LayoutIndexCollectionReadOnlyList IndexCollectionList { get { return (LayoutIndexCollectionReadOnlyList)base.IndexCollectionList; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks the context consistency, for code coverage purpose.
        /// </summary>
        public override void CheckConsistency()
        {
            base.CheckConsistency();

            Debug.Assert(State != null);

            LayoutIndexCollectionList InternalList = InternalIndexCollectionList as LayoutIndexCollectionList;
            LayoutIndexCollectionReadOnlyList PublicList = IndexCollectionList;

            for (int i = 0; i < InternalList.Count; i++)
            {
                ILayoutIndexCollection InternalItem = (ILayoutIndexCollection)InternalList[i];
                Debug.Assert(PublicList.Contains(InternalItem));
                Debug.Assert(PublicList.IndexOf(InternalItem) >= 0);

                ILayoutIndexCollection PublicItem = (ILayoutIndexCollection)PublicList[i];
                Debug.Assert(InternalList.Contains(PublicItem));
                Debug.Assert(InternalList.IndexOf(PublicItem) >= 0);

                if (i == 0)
                {
                    Debug.Assert(!((ICollection<ILayoutIndexCollection>)InternalList).IsReadOnly);

                    InternalList.Remove(InternalItem);
                    InternalList.Insert(0, InternalItem);

                    if (Type.FromGetType(InternalList).IsTypeof<LayoutIndexCollectionList>())
                    {
                        InternalList.CopyTo((IReadOnlyIndexCollection[])(new ILayoutIndexCollection[InternalList.Count]), 0);
                        InternalList.CopyTo((IWriteableIndexCollection[])(new ILayoutIndexCollection[InternalList.Count]), 0);
                        InternalList.CopyTo((IFrameIndexCollection[])(new ILayoutIndexCollection[InternalList.Count]), 0);
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

                    IEnumerable<IFrameIndexCollection> AsFrameEnumerable = PublicList;
                    foreach (ILayoutIndexCollection Item in AsFrameEnumerable)
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

                    IEnumerator<IFocusIndexCollection> InternalListEnumerator = ((ICollection<ILayoutIndexCollection>)InternalList).GetEnumerator();
                    InternalListEnumerator.MoveNext();
                    Debug.Assert(InternalListEnumerator.Current == InternalItem);

                    IEnumerator<IFocusIndexCollection> PublicListEnumerator = PublicList.GetEnumerator();
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
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutBrowseContext>());
            return new LayoutIndexCollectionList();
        }
        #endregion
    }
}
