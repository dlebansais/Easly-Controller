#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;

    /// <summary>
    /// List of IxxxFocus
    /// </summary>
    public interface ILayoutFocusList : IFocusFocusList, IList<ILayoutFocus>, IReadOnlyList<ILayoutFocus>
    {
        new ILayoutFocus this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ILayoutFocus> GetEnumerator();
        new void Clear();
    }

    /// <summary>
    /// List of IxxxFocus
    /// </summary>
    internal class LayoutFocusList : Collection<ILayoutFocus>, ILayoutFocusList
    {
        #region Focus
        IFocusFocus IFocusFocusList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutFocus)value; } }
        IFocusFocus IList<IFocusFocus>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutFocus)value; } }
        int IList<IFocusFocus>.IndexOf(IFocusFocus value) { return IndexOf((ILayoutFocus)value); }
        void IList<IFocusFocus>.Insert(int index, IFocusFocus item) { Insert(index, (ILayoutFocus)item); }
        void ICollection<IFocusFocus>.Add(IFocusFocus item) { Add((ILayoutFocus)item); }
        bool ICollection<IFocusFocus>.Contains(IFocusFocus value) { return Contains((ILayoutFocus)value); }
        void ICollection<IFocusFocus>.CopyTo(IFocusFocus[] array, int index) { CopyTo((ILayoutFocus[])array, index); }
        bool ICollection<IFocusFocus>.IsReadOnly { get { return ((ICollection<ILayoutFocus>)this).IsReadOnly; } }
        bool ICollection<IFocusFocus>.Remove(IFocusFocus item) { return Remove((ILayoutFocus)item); }
        IEnumerator<IFocusFocus> IEnumerable<IFocusFocus>.GetEnumerator() { return GetEnumerator(); }
        IFocusFocus IReadOnlyList<IFocusFocus>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
