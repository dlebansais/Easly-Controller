#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxIndexCollection
    /// </summary>
    internal interface IFocusIndexCollectionReadOnlyList : IFrameIndexCollectionReadOnlyList, IReadOnlyList<IFocusIndexCollection>
    {
        new int Count { get; }
        new IFocusIndexCollection this[int index] { get; }
        bool Contains(IFocusIndexCollection value);
        int IndexOf(IFocusIndexCollection value);
        new IEnumerator<IFocusIndexCollection> GetEnumerator();
    }

    /// <summary>
    /// Read-only list of IxxxIndexCollection
    /// </summary>
    internal class FocusIndexCollectionReadOnlyList : ReadOnlyCollection<IFocusIndexCollection>, IFocusIndexCollectionReadOnlyList
    {
        public FocusIndexCollectionReadOnlyList(IFocusIndexCollectionList list)
            : base(list)
        {
        }

        #region ReadOnly
        bool IReadOnlyIndexCollectionReadOnlyList.Contains(IReadOnlyIndexCollection value) { return Contains((IFocusIndexCollection)value); }
        int IReadOnlyIndexCollectionReadOnlyList.IndexOf(IReadOnlyIndexCollection value) { return IndexOf((IFocusIndexCollection)value); }
        IEnumerator<IReadOnlyIndexCollection> IEnumerable<IReadOnlyIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyIndexCollection IReadOnlyList<IReadOnlyIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableIndexCollection IWriteableIndexCollectionReadOnlyList.this[int index] { get { return this[index]; } }
        bool IWriteableIndexCollectionReadOnlyList.Contains(IWriteableIndexCollection value) { return Contains((IFocusIndexCollection)value); }
        IEnumerator<IWriteableIndexCollection> IWriteableIndexCollectionReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IWriteableIndexCollectionReadOnlyList.IndexOf(IWriteableIndexCollection value) { return IndexOf((IFocusIndexCollection)value); }
        IEnumerator<IWriteableIndexCollection> IEnumerable<IWriteableIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IWriteableIndexCollection IReadOnlyList<IWriteableIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameIndexCollection IFrameIndexCollectionReadOnlyList.this[int index] { get { return this[index]; } }
        bool IFrameIndexCollectionReadOnlyList.Contains(IFrameIndexCollection value) { return Contains((IFocusIndexCollection)value); }
        IEnumerator<IFrameIndexCollection> IFrameIndexCollectionReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IFrameIndexCollectionReadOnlyList.IndexOf(IFrameIndexCollection value) { return IndexOf((IFocusIndexCollection)value); }
        IEnumerator<IFrameIndexCollection> IEnumerable<IFrameIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IFrameIndexCollection IReadOnlyList<IFrameIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
