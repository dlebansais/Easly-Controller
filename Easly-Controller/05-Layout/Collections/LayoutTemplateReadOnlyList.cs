namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class LayoutTemplateReadOnlyList : FocusTemplateReadOnlyList, IReadOnlyCollection<ILayoutTemplate>, IReadOnlyList<ILayoutTemplate>
    {
        /// <inheritdoc/>
        public LayoutTemplateReadOnlyList(LayoutTemplateList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new ILayoutTemplate this[int index] { get { return (ILayoutTemplate)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<ILayoutTemplate> GetEnumerator() { var iterator = ((ReadOnlyCollection<IFrameTemplate>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutTemplate)iterator.Current; } }

        #region ILayoutTemplate
        IEnumerator<ILayoutTemplate> IEnumerable<ILayoutTemplate>.GetEnumerator() { return GetEnumerator(); }
        ILayoutTemplate IReadOnlyList<ILayoutTemplate>.this[int index] { get { return (ILayoutTemplate)this[index]; } }
        #endregion
    }
}
