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

        #region ILayoutTemplate
        IEnumerator<ILayoutTemplate> IEnumerable<ILayoutTemplate>.GetEnumerator() { return ((IList<ILayoutTemplate>)this).GetEnumerator(); }
        ILayoutTemplate IReadOnlyList<ILayoutTemplate>.this[int index] { get { return (ILayoutTemplate)this[index]; } }
        #endregion
    }
}
