#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// List of IxxxTemplate
    /// </summary>
    public interface ILayoutTemplateList : IFocusTemplateList, IList<ILayoutTemplate>, IReadOnlyList<ILayoutTemplate>
    {
        new ILayoutTemplate this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ILayoutTemplate> GetEnumerator();
        new void Clear();
    }

    /// <summary>
    /// List of IxxxTemplate
    /// </summary>
    public class LayoutTemplateList : Collection<ILayoutTemplate>, ILayoutTemplateList
    {
        #region Frame
        IFrameTemplate IFrameTemplateList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutTemplate)value; } }
        IFrameTemplate IList<IFrameTemplate>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutTemplate)value; } }
        int IList<IFrameTemplate>.IndexOf(IFrameTemplate value) { return IndexOf((ILayoutTemplate)value); }
        void IList<IFrameTemplate>.Insert(int index, IFrameTemplate item) { Insert(index, (ILayoutTemplate)item); }
        void ICollection<IFrameTemplate>.Add(IFrameTemplate item) { Add((ILayoutTemplate)item); }
        bool ICollection<IFrameTemplate>.Contains(IFrameTemplate value) { return Contains((ILayoutTemplate)value); }
        void ICollection<IFrameTemplate>.CopyTo(IFrameTemplate[] array, int index) { CopyTo((ILayoutTemplate[])array, index); }
        bool ICollection<IFrameTemplate>.IsReadOnly { get { return ((ICollection<ILayoutTemplate>)this).IsReadOnly; } }
        bool ICollection<IFrameTemplate>.Remove(IFrameTemplate item) { return Remove((ILayoutTemplate)item); }
        IEnumerator<IFrameTemplate> IEnumerable<IFrameTemplate>.GetEnumerator() { return GetEnumerator(); }
        IFrameTemplate IReadOnlyList<IFrameTemplate>.this[int index] { get { return this[index]; } }
        #endregion

        #region Focus
        IFocusTemplate IFocusTemplateList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutTemplate)value; } }
        IEnumerator<IFocusTemplate> IFocusTemplateList.GetEnumerator() { return GetEnumerator(); }
        IFocusTemplate IList<IFocusTemplate>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutTemplate)value; } }
        int IList<IFocusTemplate>.IndexOf(IFocusTemplate value) { return IndexOf((ILayoutTemplate)value); }
        void IList<IFocusTemplate>.Insert(int index, IFocusTemplate item) { Insert(index, (ILayoutTemplate)item); }
        void ICollection<IFocusTemplate>.Add(IFocusTemplate item) { Add((ILayoutTemplate)item); }
        bool ICollection<IFocusTemplate>.Contains(IFocusTemplate value) { return Contains((ILayoutTemplate)value); }
        void ICollection<IFocusTemplate>.CopyTo(IFocusTemplate[] array, int index) { CopyTo((ILayoutTemplate[])array, index); }
        bool ICollection<IFocusTemplate>.IsReadOnly { get { return ((ICollection<ILayoutTemplate>)this).IsReadOnly; } }
        bool ICollection<IFocusTemplate>.Remove(IFocusTemplate item) { return Remove((ILayoutTemplate)item); }
        IEnumerator<IFocusTemplate> IEnumerable<IFocusTemplate>.GetEnumerator() { return GetEnumerator(); }
        IFocusTemplate IReadOnlyList<IFocusTemplate>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
