#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;

    /// <summary>
    /// List of IxxxTemplate
    /// </summary>
    public interface IFocusTemplateList : IFrameTemplateList, IList<IFocusTemplate>, IReadOnlyList<IFocusTemplate>
    {
        new IFocusTemplate this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IFocusTemplate> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxTemplate
    /// </summary>
    public class FocusTemplateList : Collection<IFocusTemplate>, IFocusTemplateList
    {
        #region Frame
        IFrameTemplate IFrameTemplateList.this[int index] { get { return this[index]; } set { this[index] = (IFocusTemplate)value; } }
        IFrameTemplate IList<IFrameTemplate>.this[int index] { get { return this[index]; } set { this[index] = (IFocusTemplate)value; } }
        int IList<IFrameTemplate>.IndexOf(IFrameTemplate value) { return IndexOf((IFocusTemplate)value); }
        void IList<IFrameTemplate>.Insert(int index, IFrameTemplate item) { Insert(index, (IFocusTemplate)item); }
        void ICollection<IFrameTemplate>.Add(IFrameTemplate item) { Add((IFocusTemplate)item); }
        bool ICollection<IFrameTemplate>.Contains(IFrameTemplate value) { return Contains((IFocusTemplate)value); }
        void ICollection<IFrameTemplate>.CopyTo(IFrameTemplate[] array, int index) { CopyTo((IFocusTemplate[])array, index); }
        bool ICollection<IFrameTemplate>.IsReadOnly { get { return ((ICollection<IFocusTemplate>)this).IsReadOnly; } }
        bool ICollection<IFrameTemplate>.Remove(IFrameTemplate item) { return Remove((IFocusTemplate)item); }
        IEnumerator<IFrameTemplate> IEnumerable<IFrameTemplate>.GetEnumerator() { return GetEnumerator(); }
        IFrameTemplate IReadOnlyList<IFrameTemplate>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
