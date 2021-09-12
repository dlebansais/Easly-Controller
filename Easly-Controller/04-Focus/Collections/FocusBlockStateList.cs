namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class FocusBlockStateList : FrameBlockStateList, ICollection<IFocusBlockState>, IEnumerable<IFocusBlockState>, IList<IFocusBlockState>, IReadOnlyCollection<IFocusBlockState>, IReadOnlyList<IFocusBlockState>
    {
        /// <inheritdoc/>
        public new IFocusBlockState this[int index] { get { return (IFocusBlockState)base[index]; } set { base[index] = value; } }

        #region IFocusBlockState
        void ICollection<IFocusBlockState>.Add(IFocusBlockState item) { Add(item); }
        bool ICollection<IFocusBlockState>.Contains(IFocusBlockState item) { return Contains(item); }
        void ICollection<IFocusBlockState>.CopyTo(IFocusBlockState[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFocusBlockState>.Remove(IFocusBlockState item) { return Remove(item); }
        bool ICollection<IFocusBlockState>.IsReadOnly { get { return ((ICollection<IFrameBlockState>)this).IsReadOnly; } }
        IEnumerator<IFocusBlockState> IEnumerable<IFocusBlockState>.GetEnumerator() { var iterator = ((List<IReadOnlyBlockState>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusBlockState)iterator.Current; } }
        IFocusBlockState IList<IFocusBlockState>.this[int index] { get { return (IFocusBlockState)this[index]; } set { this[index] = value; } }
        int IList<IFocusBlockState>.IndexOf(IFocusBlockState item) { return IndexOf(item); }
        void IList<IFocusBlockState>.Insert(int index, IFocusBlockState item) { Insert(index, item); }
        IFocusBlockState IReadOnlyList<IFocusBlockState>.this[int index] { get { return (IFocusBlockState)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyBlockStateReadOnlyList ToReadOnly()
        {
            return new FocusBlockStateReadOnlyList(this);
        }
    }
}
