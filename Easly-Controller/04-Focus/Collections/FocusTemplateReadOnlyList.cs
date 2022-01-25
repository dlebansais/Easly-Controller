namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusTemplateReadOnlyList : FrameTemplateReadOnlyList, IReadOnlyCollection<IFocusTemplate>, IReadOnlyList<IFocusTemplate>
    {
        /// <inheritdoc/>
        public FocusTemplateReadOnlyList(FocusTemplateList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IFocusTemplate this[int index] { get { return (IFocusTemplate)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<IFocusTemplate> GetEnumerator() { var iterator = ((ReadOnlyCollection<IFrameTemplate>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusTemplate)iterator.Current; } }

        #region IFocusTemplate
        IEnumerator<IFocusTemplate> IEnumerable<IFocusTemplate>.GetEnumerator() { return GetEnumerator(); }
        IFocusTemplate IReadOnlyList<IFocusTemplate>.this[int index] { get { return (IFocusTemplate)this[index]; } }
        #endregion
    }
}
