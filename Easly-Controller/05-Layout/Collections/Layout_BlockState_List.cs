namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutBlockStateList : FocusBlockStateList, ICollection<ILayoutBlockState>, IEnumerable<ILayoutBlockState>, IList<ILayoutBlockState>, IReadOnlyCollection<ILayoutBlockState>, IReadOnlyList<ILayoutBlockState>
    {
        /// <inheritdoc/>
        public new ILayoutBlockState this[int index] { get { return (ILayoutBlockState)base[index]; } set { base[index] = value; } }

        #region ILayoutBlockState
        void ICollection<ILayoutBlockState>.Add(ILayoutBlockState item) { Add(item); }
        bool ICollection<ILayoutBlockState>.Contains(ILayoutBlockState item) { return Contains(item); }
        void ICollection<ILayoutBlockState>.CopyTo(ILayoutBlockState[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutBlockState>.Remove(ILayoutBlockState item) { return Remove(item); }
        bool ICollection<ILayoutBlockState>.IsReadOnly { get { return ((ICollection<IFocusBlockState>)this).IsReadOnly; } }
        IEnumerator<ILayoutBlockState> IEnumerable<ILayoutBlockState>.GetEnumerator() { Enumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutBlockState)iterator.Current; } }
        ILayoutBlockState IList<ILayoutBlockState>.this[int index] { get { return (ILayoutBlockState)this[index]; } set { this[index] = value; } }
        int IList<ILayoutBlockState>.IndexOf(ILayoutBlockState item) { return IndexOf(item); }
        void IList<ILayoutBlockState>.Insert(int index, ILayoutBlockState item) { Insert(index, item); }
        ILayoutBlockState IReadOnlyList<ILayoutBlockState>.this[int index] { get { return (ILayoutBlockState)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyBlockStateReadOnlyList ToReadOnly()
        {
            return new LayoutBlockStateReadOnlyList(this);
        }
    }
}
