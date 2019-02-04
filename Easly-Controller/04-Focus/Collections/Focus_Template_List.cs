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
        new int Count { get; }
        new IFocusTemplate this[int index] { get; set; }
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
        IFrameTemplate IReadOnlyList<IFrameTemplate>.this[int index] { get { return this[index]; } }
        public void Add(IFrameTemplate item) { base.Add((IFocusTemplate)item); }
        public void Insert(int index, IFrameTemplate item) { base.Insert(index, (IFocusTemplate)item); }
        public bool Remove(IFrameTemplate item) { return base.Remove((IFocusTemplate)item); }
        public void CopyTo(IFrameTemplate[] array, int index) { base.CopyTo((IFocusTemplate[])array, index); }
        bool ICollection<IFrameTemplate>.IsReadOnly { get { return ((ICollection<IFocusTemplate>)this).IsReadOnly; } }
        public bool Contains(IFrameTemplate value) { return base.Contains((IFocusTemplate)value); }
        public int IndexOf(IFrameTemplate value) { return base.IndexOf((IFocusTemplate)value); }
        IEnumerator<IFrameTemplate> IFrameTemplateList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFrameTemplate> IEnumerable<IFrameTemplate>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
