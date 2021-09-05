namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusTemplateReadOnlyList : FrameTemplateReadOnlyList, IReadOnlyCollection<IFocusTemplate>, IReadOnlyList<IFocusTemplate>
    {
        /// <inheritdoc/>
        public FocusTemplateReadOnlyList(FocusTemplateList list)
            : base(list)
        {
        }

        #region IFocusTemplate
        IEnumerator<IFocusTemplate> IEnumerable<IFocusTemplate>.GetEnumerator() { return ((IList<IFocusTemplate>)this).GetEnumerator(); }
        IFocusTemplate IReadOnlyList<IFocusTemplate>.this[int index] { get { return (IFocusTemplate)this[index]; } }
        #endregion
    }
}
