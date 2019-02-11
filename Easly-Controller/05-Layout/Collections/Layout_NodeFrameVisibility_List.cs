#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;

    /// <summary>
    /// List of IxxxNodeFrameVisibility
    /// </summary>
    public interface ILayoutNodeFrameVisibilityList : IFocusNodeFrameVisibilityList, IList<ILayoutNodeFrameVisibility>, IReadOnlyList<ILayoutNodeFrameVisibility>
    {
        new ILayoutNodeFrameVisibility this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ILayoutNodeFrameVisibility> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxNodeFrameVisibility
    /// </summary>
    internal class LayoutNodeFrameVisibilityList : Collection<ILayoutNodeFrameVisibility>, ILayoutNodeFrameVisibilityList
    {
        #region Focus
        IFocusNodeFrameVisibility IFocusNodeFrameVisibilityList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutNodeFrameVisibility)value; } }
        IFocusNodeFrameVisibility IList<IFocusNodeFrameVisibility>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutNodeFrameVisibility)value; } }
        int IList<IFocusNodeFrameVisibility>.IndexOf(IFocusNodeFrameVisibility value) { return IndexOf((ILayoutNodeFrameVisibility)value); }
        void IList<IFocusNodeFrameVisibility>.Insert(int index, IFocusNodeFrameVisibility item) { Insert(index, (ILayoutNodeFrameVisibility)item); }
        void ICollection<IFocusNodeFrameVisibility>.Add(IFocusNodeFrameVisibility item) { Add((ILayoutNodeFrameVisibility)item); }
        bool ICollection<IFocusNodeFrameVisibility>.Contains(IFocusNodeFrameVisibility value) { return Contains((ILayoutNodeFrameVisibility)value); }
        void ICollection<IFocusNodeFrameVisibility>.CopyTo(IFocusNodeFrameVisibility[] array, int index) { CopyTo((ILayoutNodeFrameVisibility[])array, index); }
        bool ICollection<IFocusNodeFrameVisibility>.IsReadOnly { get { return ((ICollection<ILayoutNodeFrameVisibility>)this).IsReadOnly; } }
        bool ICollection<IFocusNodeFrameVisibility>.Remove(IFocusNodeFrameVisibility item) { return Remove((ILayoutNodeFrameVisibility)item); }
        IEnumerator<IFocusNodeFrameVisibility> IEnumerable<IFocusNodeFrameVisibility>.GetEnumerator() { return GetEnumerator(); }
        IFocusNodeFrameVisibility IReadOnlyList<IFocusNodeFrameVisibility>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
