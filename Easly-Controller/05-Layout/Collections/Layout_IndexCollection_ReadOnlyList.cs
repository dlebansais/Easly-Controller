#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxIndexCollection
    /// </summary>
    internal interface ILayoutIndexCollectionReadOnlyList : IFocusIndexCollectionReadOnlyList, IReadOnlyList<ILayoutIndexCollection>
    {
        new ILayoutIndexCollection this[int index] { get; }
        new int Count { get; }
        bool Contains(ILayoutIndexCollection value);
        int IndexOf(ILayoutIndexCollection value);
        new IEnumerator<ILayoutIndexCollection> GetEnumerator();
    }

    /// <summary>
    /// Read-only list of IxxxIndexCollection
    /// </summary>
    internal class LayoutIndexCollectionReadOnlyList : ReadOnlyCollection<ILayoutIndexCollection>, ILayoutIndexCollectionReadOnlyList
    {
        public LayoutIndexCollectionReadOnlyList(ILayoutIndexCollectionList list)
            : base(list)
        {
        }

        #region ReadOnly
        bool IReadOnlyIndexCollectionReadOnlyList.Contains(IReadOnlyIndexCollection value) { return Contains((ILayoutIndexCollection)value); }
        int IReadOnlyIndexCollectionReadOnlyList.IndexOf(IReadOnlyIndexCollection value) { return IndexOf((ILayoutIndexCollection)value); }
        IEnumerator<IReadOnlyIndexCollection> IEnumerable<IReadOnlyIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyIndexCollection IReadOnlyList<IReadOnlyIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableIndexCollection IWriteableIndexCollectionReadOnlyList.this[int index] { get { return this[index]; } }
        bool IWriteableIndexCollectionReadOnlyList.Contains(IWriteableIndexCollection value) { return Contains((ILayoutIndexCollection)value); }
        IEnumerator<IWriteableIndexCollection> IWriteableIndexCollectionReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IWriteableIndexCollectionReadOnlyList.IndexOf(IWriteableIndexCollection value) { return IndexOf((ILayoutIndexCollection)value); }
        IEnumerator<IWriteableIndexCollection> IEnumerable<IWriteableIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IWriteableIndexCollection IReadOnlyList<IWriteableIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameIndexCollection IFrameIndexCollectionReadOnlyList.this[int index] { get { return this[index]; } }
        bool IFrameIndexCollectionReadOnlyList.Contains(IFrameIndexCollection value) { return Contains((ILayoutIndexCollection)value); }
        IEnumerator<IFrameIndexCollection> IFrameIndexCollectionReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IFrameIndexCollectionReadOnlyList.IndexOf(IFrameIndexCollection value) { return IndexOf((ILayoutIndexCollection)value); }
        IEnumerator<IFrameIndexCollection> IEnumerable<IFrameIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IFrameIndexCollection IReadOnlyList<IFrameIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion

        #region Focus
        IFocusIndexCollection IFocusIndexCollectionReadOnlyList.this[int index] { get { return this[index]; } }
        bool IFocusIndexCollectionReadOnlyList.Contains(IFocusIndexCollection value) { return Contains((ILayoutIndexCollection)value); }
        IEnumerator<IFocusIndexCollection> IFocusIndexCollectionReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IFocusIndexCollectionReadOnlyList.IndexOf(IFocusIndexCollection value) { return IndexOf((ILayoutIndexCollection)value); }
        IEnumerator<IFocusIndexCollection> IEnumerable<IFocusIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IFocusIndexCollection IReadOnlyList<IFocusIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
