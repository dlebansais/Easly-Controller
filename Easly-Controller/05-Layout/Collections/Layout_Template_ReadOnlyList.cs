namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

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

        #region ILayoutTemplate
        IEnumerator<ILayoutTemplate> IEnumerable<ILayoutTemplate>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutTemplate)iterator.Current; } }
        ILayoutTemplate IReadOnlyList<ILayoutTemplate>.this[int index] { get { return (ILayoutTemplate)this[index]; } }
        #endregion
    }
}
