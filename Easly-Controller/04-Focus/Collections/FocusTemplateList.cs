namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusTemplateList : FrameTemplateList, ICollection<IFocusTemplate>, IEnumerable<IFocusTemplate>, IList<IFocusTemplate>, IReadOnlyCollection<IFocusTemplate>, IReadOnlyList<IFocusTemplate>
    {
        /// <inheritdoc/>
        public new IFocusTemplate this[int index] { get { return (IFocusTemplate)base[index]; } set { base[index] = value; } }

        #region IFocusTemplate
        void ICollection<IFocusTemplate>.Add(IFocusTemplate item) { Add(item); }
        bool ICollection<IFocusTemplate>.Contains(IFocusTemplate item) { return Contains(item); }
        void ICollection<IFocusTemplate>.CopyTo(IFocusTemplate[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFocusTemplate>.Remove(IFocusTemplate item) { return Remove(item); }
        bool ICollection<IFocusTemplate>.IsReadOnly { get { return ((ICollection<IFrameTemplate>)this).IsReadOnly; } }
        IEnumerator<IFocusTemplate> IEnumerable<IFocusTemplate>.GetEnumerator() { var iterator = ((List<IFrameTemplate>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusTemplate)iterator.Current; } }
        IFocusTemplate IList<IFocusTemplate>.this[int index] { get { return (IFocusTemplate)this[index]; } set { this[index] = value; } }
        int IList<IFocusTemplate>.IndexOf(IFocusTemplate item) { return IndexOf(item); }
        void IList<IFocusTemplate>.Insert(int index, IFocusTemplate item) { Insert(index, item); }
        IFocusTemplate IReadOnlyList<IFocusTemplate>.this[int index] { get { return (IFocusTemplate)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override FrameTemplateReadOnlyList ToReadOnly()
        {
            return new FocusTemplateReadOnlyList(this);
        }
    }
}
