using EaslyController.Frame;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

#pragma warning disable 1591

namespace EaslyController.Focus
{
    /// <summary>
    /// List of IxxxTemplate
    /// </summary>
    public interface IFocusTemplateList : IFrameTemplateList, IList<IFocusTemplate>, IReadOnlyList<IFocusTemplate>
    {
        new int Count { get; }
        new IFocusTemplate this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxTemplate
    /// </summary>
    public class FocusTemplateList : Collection<IFocusTemplate>, IFocusTemplateList
    {
        #region Frame
        public new IFrameTemplate this[int index] { get { return base[index]; } set { base[index] = (IFocusTemplate)value; } }
        public void Add(IFrameTemplate item) { base.Add((IFocusTemplate)item); }
        public void Insert(int index, IFrameTemplate item) { base.Insert(index, (IFocusTemplate)item); }
        public bool Remove(IFrameTemplate item) { return base.Remove((IFocusTemplate)item); }
        public void CopyTo(IFrameTemplate[] array, int index) { base.CopyTo((IFocusTemplate[])array, index); }
        bool ICollection<IFrameTemplate>.IsReadOnly { get { return ((ICollection<IFocusTemplate>)this).IsReadOnly; } }
        public bool Contains(IFrameTemplate value) { return base.Contains((IFocusTemplate)value); }
        public int IndexOf(IFrameTemplate value) { return base.IndexOf((IFocusTemplate)value); }
        public new IEnumerator<IFrameTemplate> GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}
