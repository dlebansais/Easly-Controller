#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;

    /// <summary>
    /// List of IxxxInsertionChildNodeIndex
    /// </summary>
    public interface ILayoutInsertionChildNodeIndexList : IFocusInsertionChildNodeIndexList, IList<ILayoutInsertionChildNodeIndex>, IReadOnlyList<ILayoutInsertionChildNodeIndex>
    {
        new ILayoutInsertionChildNodeIndex this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ILayoutInsertionChildNodeIndex> GetEnumerator();
        new void Clear();
    }

    /// <summary>
    /// List of IxxxInsertionChildNodeIndex
    /// </summary>
    internal class LayoutInsertionChildNodeIndexList : Collection<ILayoutInsertionChildNodeIndex>, ILayoutInsertionChildNodeIndexList
    {
        #region Focus
        IFocusInsertionChildNodeIndex IFocusInsertionChildNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutInsertionChildNodeIndex)value; } }
        IFocusInsertionChildNodeIndex IList<IFocusInsertionChildNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutInsertionChildNodeIndex)value; } }
        int IList<IFocusInsertionChildNodeIndex>.IndexOf(IFocusInsertionChildNodeIndex value) { return IndexOf((ILayoutInsertionChildNodeIndex)value); }
        void IList<IFocusInsertionChildNodeIndex>.Insert(int index, IFocusInsertionChildNodeIndex item) { Insert(index, (ILayoutInsertionChildNodeIndex)item); }
        void ICollection<IFocusInsertionChildNodeIndex>.Add(IFocusInsertionChildNodeIndex item) { Add((ILayoutInsertionChildNodeIndex)item); }
        bool ICollection<IFocusInsertionChildNodeIndex>.Contains(IFocusInsertionChildNodeIndex value) { return Contains((ILayoutInsertionChildNodeIndex)value); }
        void ICollection<IFocusInsertionChildNodeIndex>.CopyTo(IFocusInsertionChildNodeIndex[] array, int index) { CopyTo((ILayoutInsertionChildNodeIndex[])array, index); }
        bool ICollection<IFocusInsertionChildNodeIndex>.IsReadOnly { get { return ((ICollection<ILayoutInsertionChildNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IFocusInsertionChildNodeIndex>.Remove(IFocusInsertionChildNodeIndex item) { return Remove((ILayoutInsertionChildNodeIndex)item); }
        IEnumerator<IFocusInsertionChildNodeIndex> IEnumerable<IFocusInsertionChildNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IFocusInsertionChildNodeIndex IReadOnlyList<IFocusInsertionChildNodeIndex>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
