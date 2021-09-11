namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutFocusList : FocusFocusList, ICollection<ILayoutFocus>, IEnumerable<ILayoutFocus>, IList<ILayoutFocus>, IReadOnlyCollection<ILayoutFocus>, IReadOnlyList<ILayoutFocus>
    {
        /// <inheritdoc/>
        public new ILayoutFocus this[int index] { get { return (ILayoutFocus)base[index]; } set { base[index] = value; } }

        #region ILayoutFocus
        void ICollection<ILayoutFocus>.Add(ILayoutFocus item) { Add(item); }
        bool ICollection<ILayoutFocus>.Contains(ILayoutFocus item) { return Contains(item); }
        void ICollection<ILayoutFocus>.CopyTo(ILayoutFocus[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutFocus>.Remove(ILayoutFocus item) { return Remove(item); }
        bool ICollection<ILayoutFocus>.IsReadOnly { get { return ((ICollection<IFocusFocus>)this).IsReadOnly; } }
        IEnumerator<ILayoutFocus> IEnumerable<ILayoutFocus>.GetEnumerator() { Enumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutFocus)iterator.Current; } }
        ILayoutFocus IList<ILayoutFocus>.this[int index] { get { return (ILayoutFocus)this[index]; } set { this[index] = value; } }
        int IList<ILayoutFocus>.IndexOf(ILayoutFocus item) { return IndexOf(item); }
        void IList<ILayoutFocus>.Insert(int index, ILayoutFocus item) { Insert(index, item); }
        ILayoutFocus IReadOnlyList<ILayoutFocus>.this[int index] { get { return (ILayoutFocus)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override FocusFocusReadOnlyList ToReadOnly()
        {
            return new LayoutFocusReadOnlyList(this);
        }
    }
}
