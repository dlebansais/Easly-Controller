namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
        /// <inheritdoc/>
        public new IEnumerator<ILayoutFocus> GetEnumerator() { var iterator = ((ReadOnlyCollection<IFocusFocus>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutFocus)iterator.Current; } }

        #region ILayoutFocus
        IEnumerator<ILayoutFocus> IEnumerable<ILayoutFocus>.GetEnumerator() { return GetEnumerator(); }
        ILayoutFocus IReadOnlyList<ILayoutFocus>.this[int index] { get { return (ILayoutFocus)this[index]; } }
        #endregion
    }
}
