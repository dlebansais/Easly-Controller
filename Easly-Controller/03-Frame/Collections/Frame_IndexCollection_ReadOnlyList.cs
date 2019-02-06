#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxIndexCollection
    /// </summary>
    internal interface IFrameIndexCollectionReadOnlyList : IWriteableIndexCollectionReadOnlyList, IReadOnlyList<IFrameIndexCollection>
    {
        new IFrameIndexCollection this[int index] { get; }
        new int Count { get; }
        bool Contains(IFrameIndexCollection value);
        new IEnumerator<IFrameIndexCollection> GetEnumerator();
        int IndexOf(IFrameIndexCollection value);
    }

    /// <summary>
    /// Read-only list of IxxxIndexCollection
    /// </summary>
    internal class FrameIndexCollectionReadOnlyList : ReadOnlyCollection<IFrameIndexCollection>, IFrameIndexCollectionReadOnlyList
    {
        public FrameIndexCollectionReadOnlyList(IFrameIndexCollectionList list)
            : base(list)
        {
        }

        #region ReadOnly
        bool IReadOnlyIndexCollectionReadOnlyList.Contains(IReadOnlyIndexCollection value) { return Contains((IFrameIndexCollection)value); }
        int IReadOnlyIndexCollectionReadOnlyList.IndexOf(IReadOnlyIndexCollection value) { return IndexOf((IFrameIndexCollection)value); }
        IEnumerator<IReadOnlyIndexCollection> IEnumerable<IReadOnlyIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyIndexCollection IReadOnlyList<IReadOnlyIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableIndexCollection IWriteableIndexCollectionReadOnlyList.this[int index] { get { return this[index]; } }
        bool IWriteableIndexCollectionReadOnlyList.Contains(IWriteableIndexCollection value) { return Contains((IFrameIndexCollection)value); }
        IEnumerator<IWriteableIndexCollection> IWriteableIndexCollectionReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IWriteableIndexCollectionReadOnlyList.IndexOf(IWriteableIndexCollection value) { return IndexOf((IFrameIndexCollection)value); }
        IEnumerator<IWriteableIndexCollection> IEnumerable<IWriteableIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IWriteableIndexCollection IReadOnlyList<IWriteableIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
