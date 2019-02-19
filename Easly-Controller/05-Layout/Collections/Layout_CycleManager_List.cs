#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;

    /// <summary>
    /// List of IxxxCycleManager
    /// </summary>
    public interface ILayoutCycleManagerList : IFocusCycleManagerList, IList<ILayoutCycleManager>, IReadOnlyList<ILayoutCycleManager>
    {
        new ILayoutCycleManager this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ILayoutCycleManager> GetEnumerator();
        new void Clear();
    }

    /// <summary>
    /// List of IxxxCycleManager
    /// </summary>
    internal class LayoutCycleManagerList : Collection<ILayoutCycleManager>, ILayoutCycleManagerList
    {
        #region Focus
        IFocusCycleManager IFocusCycleManagerList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutCycleManager)value; } }
        IFocusCycleManager IList<IFocusCycleManager>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutCycleManager)value; } }
        int IList<IFocusCycleManager>.IndexOf(IFocusCycleManager value) { return IndexOf((ILayoutCycleManager)value); }
        void IList<IFocusCycleManager>.Insert(int index, IFocusCycleManager item) { Insert(index, (ILayoutCycleManager)item); }
        void ICollection<IFocusCycleManager>.Add(IFocusCycleManager item) { Add((ILayoutCycleManager)item); }
        bool ICollection<IFocusCycleManager>.Contains(IFocusCycleManager value) { return Contains((ILayoutCycleManager)value); }
        void ICollection<IFocusCycleManager>.CopyTo(IFocusCycleManager[] array, int index) { CopyTo((ILayoutCycleManager[])array, index); }
        bool ICollection<IFocusCycleManager>.IsReadOnly { get { return ((ICollection<ILayoutCycleManager>)this).IsReadOnly; } }
        bool ICollection<IFocusCycleManager>.Remove(IFocusCycleManager item) { return Remove((ILayoutCycleManager)item); }
        IEnumerator<IFocusCycleManager> IEnumerable<IFocusCycleManager>.GetEnumerator() { return GetEnumerator(); }
        IFocusCycleManager IReadOnlyList<IFocusCycleManager>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
