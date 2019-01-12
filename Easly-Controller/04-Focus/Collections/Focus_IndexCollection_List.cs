﻿using EaslyController.Frame;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace EaslyController.Focus
{
    /// <summary>
    /// List of IxxxIndexCollection
    /// </summary>
    public interface IFocusIndexCollectionList : IFrameIndexCollectionList, IList<IFocusIndexCollection>, IReadOnlyList<IFocusIndexCollection>
    {
        new int Count { get; }
        new IFocusIndexCollection this[int index] { get; set; }
        new IEnumerator<IFocusIndexCollection> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxIndexCollection
    /// </summary>
    public class FocusIndexCollectionList : Collection<IFocusIndexCollection>, IFocusIndexCollectionList
    {
        #region ReadOnly
        public new IReadOnlyIndexCollection this[int index] { get { return base[index]; } set { base[index] = (IFocusIndexCollection)value; } }
        public void Add(IReadOnlyIndexCollection item) { base.Add((IFocusIndexCollection)item); }
        public void Insert(int index, IReadOnlyIndexCollection item) { base.Insert(index, (IFocusIndexCollection)item); }
        public bool Remove(IReadOnlyIndexCollection item) { return base.Remove((IFocusIndexCollection)item); }
        public void CopyTo(IReadOnlyIndexCollection[] array, int index) { base.CopyTo((IFocusIndexCollection[])array, index); }
        bool ICollection<IReadOnlyIndexCollection>.IsReadOnly { get { return ((ICollection<IFocusIndexCollection>)this).IsReadOnly; } }
        public bool Contains(IReadOnlyIndexCollection value) { return base.Contains((IFocusIndexCollection)value); }
        public int IndexOf(IReadOnlyIndexCollection value) { return base.IndexOf((IFocusIndexCollection)value); }
        public new IEnumerator<IReadOnlyIndexCollection> GetEnumerator() { return base.GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableIndexCollection IWriteableIndexCollectionList.this[int index] { get { return base[index]; } set { base[index] = (IFocusIndexCollection)value; } }
        IWriteableIndexCollection IList<IWriteableIndexCollection>.this[int index] { get { return base[index]; } set { base[index] = (IFocusIndexCollection)value; } }
        IWriteableIndexCollection IReadOnlyList<IWriteableIndexCollection>.this[int index] { get { return base[index]; } }
        public void Add(IWriteableIndexCollection item) { base.Add((IFocusIndexCollection)item); }
        public void Insert(int index, IWriteableIndexCollection item) { base.Insert(index, (IFocusIndexCollection)item); }
        public bool Remove(IWriteableIndexCollection item) { return base.Remove((IFocusIndexCollection)item); }
        public void CopyTo(IWriteableIndexCollection[] array, int index) { base.CopyTo((IFocusIndexCollection[])array, index); }
        bool ICollection<IWriteableIndexCollection>.IsReadOnly { get { return ((ICollection<IFocusIndexCollection>)this).IsReadOnly; } }
        public bool Contains(IWriteableIndexCollection value) { return base.Contains((IFocusIndexCollection)value); }
        public int IndexOf(IWriteableIndexCollection value) { return base.IndexOf((IFocusIndexCollection)value); }
        IEnumerator<IWriteableIndexCollection> IWriteableIndexCollectionList.GetEnumerator() { return base.GetEnumerator(); }
        IEnumerator<IWriteableIndexCollection> IEnumerable<IWriteableIndexCollection>.GetEnumerator() { return base.GetEnumerator(); }
        #endregion

        #region Frame
        IFrameIndexCollection IFrameIndexCollectionList.this[int index] { get { return base[index]; } set { base[index] = (IFocusIndexCollection)value; } }
        IFrameIndexCollection IList<IFrameIndexCollection>.this[int index] { get { return base[index]; } set { base[index] = (IFocusIndexCollection)value; } }
        IFrameIndexCollection IReadOnlyList<IFrameIndexCollection>.this[int index] { get { return base[index]; } }
        public void Add(IFrameIndexCollection item) { base.Add((IFocusIndexCollection)item); }
        public void Insert(int index, IFrameIndexCollection item) { base.Insert(index, (IFocusIndexCollection)item); }
        public bool Remove(IFrameIndexCollection item) { return base.Remove((IFocusIndexCollection)item); }
        public void CopyTo(IFrameIndexCollection[] array, int index) { base.CopyTo((IFocusIndexCollection[])array, index); }
        bool ICollection<IFrameIndexCollection>.IsReadOnly { get { return ((ICollection<IFocusIndexCollection>)this).IsReadOnly; } }
        public bool Contains(IFrameIndexCollection value) { return base.Contains((IFocusIndexCollection)value); }
        public int IndexOf(IFrameIndexCollection value) { return base.IndexOf((IFocusIndexCollection)value); }
        IEnumerator<IFrameIndexCollection> IFrameIndexCollectionList.GetEnumerator() { return base.GetEnumerator(); }
        IEnumerator<IFrameIndexCollection> IEnumerable<IFrameIndexCollection>.GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}