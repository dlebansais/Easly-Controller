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

        /// <inheritdoc/>
        public new ILayoutFocus this[int index] { get { return (ILayoutFocus)base[index]; } }

        #region ILayoutFocus
        IEnumerator<ILayoutFocus> IEnumerable<ILayoutFocus>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutFocus)iterator.Current; } }
        ILayoutFocus IReadOnlyList<ILayoutFocus>.this[int index] { get { return (ILayoutFocus)this[index]; } }
        #endregion
    }
}
