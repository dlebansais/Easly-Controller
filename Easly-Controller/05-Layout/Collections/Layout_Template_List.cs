namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class LayoutTemplateList : FocusTemplateList, ICollection<ILayoutTemplate>, IEnumerable<ILayoutTemplate>, IList<ILayoutTemplate>, IReadOnlyCollection<ILayoutTemplate>, IReadOnlyList<ILayoutTemplate>
    {
        /// <inheritdoc/>
        public new ILayoutTemplate this[int index] { get { return (ILayoutTemplate)base[index]; } set { base[index] = value; } }

        #region ILayoutTemplate
        void ICollection<ILayoutTemplate>.Add(ILayoutTemplate item) { Add(item); }
        bool ICollection<ILayoutTemplate>.Contains(ILayoutTemplate item) { return Contains(item); }
        void ICollection<ILayoutTemplate>.CopyTo(ILayoutTemplate[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutTemplate>.Remove(ILayoutTemplate item) { return Remove(item); }
        bool ICollection<ILayoutTemplate>.IsReadOnly { get { return ((ICollection<IFocusTemplate>)this).IsReadOnly; } }
        IEnumerator<ILayoutTemplate> IEnumerable<ILayoutTemplate>.GetEnumerator() { return ((IList<ILayoutTemplate>)this).GetEnumerator(); }
        ILayoutTemplate IList<ILayoutTemplate>.this[int index] { get { return (ILayoutTemplate)this[index]; } set { this[index] = value; } }
        int IList<ILayoutTemplate>.IndexOf(ILayoutTemplate item) { return IndexOf(item); }
        void IList<ILayoutTemplate>.Insert(int index, ILayoutTemplate item) { Insert(index, item); }
        ILayoutTemplate IReadOnlyList<ILayoutTemplate>.this[int index] { get { return (ILayoutTemplate)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override FrameTemplateReadOnlyList ToReadOnly()
        {
            return new LayoutTemplateReadOnlyList(this);
        }
    }
}
