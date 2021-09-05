namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutFocusReadOnlyList : FocusFocusReadOnlyList, IReadOnlyCollection<ILayoutFocus>, IReadOnlyList<ILayoutFocus>
    {
        /// <inheritdoc/>
        public LayoutFocusReadOnlyList(LayoutFocusList list)
            : base(list)
        {
        }

        #region ILayoutFocus
        IEnumerator<ILayoutFocus> IEnumerable<ILayoutFocus>.GetEnumerator() { return ((IList<ILayoutFocus>)this).GetEnumerator(); }
        ILayoutFocus IReadOnlyList<ILayoutFocus>.this[int index] { get { return (ILayoutFocus)this[index]; } }
        #endregion
    }
}
