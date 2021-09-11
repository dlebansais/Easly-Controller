namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameBlockStateList : WriteableBlockStateList, ICollection<IFrameBlockState>, IEnumerable<IFrameBlockState>, IList<IFrameBlockState>, IReadOnlyCollection<IFrameBlockState>, IReadOnlyList<IFrameBlockState>
    {
        /// <inheritdoc/>
        public new IFrameBlockState this[int index] { get { return (IFrameBlockState)base[index]; } set { base[index] = value; } }

        #region IFrameBlockState
        void ICollection<IFrameBlockState>.Add(IFrameBlockState item) { Add(item); }
        bool ICollection<IFrameBlockState>.Contains(IFrameBlockState item) { return Contains(item); }
        void ICollection<IFrameBlockState>.CopyTo(IFrameBlockState[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFrameBlockState>.Remove(IFrameBlockState item) { return Remove(item); }
        bool ICollection<IFrameBlockState>.IsReadOnly { get { return ((ICollection<IWriteableBlockState>)this).IsReadOnly; } }
        IEnumerator<IFrameBlockState> IEnumerable<IFrameBlockState>.GetEnumerator() { Enumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IFrameBlockState)iterator.Current; } }
        IFrameBlockState IList<IFrameBlockState>.this[int index] { get { return (IFrameBlockState)this[index]; } set { this[index] = value; } }
        int IList<IFrameBlockState>.IndexOf(IFrameBlockState item) { return IndexOf(item); }
        void IList<IFrameBlockState>.Insert(int index, IFrameBlockState item) { Insert(index, item); }
        IFrameBlockState IReadOnlyList<IFrameBlockState>.this[int index] { get { return (IFrameBlockState)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyBlockStateReadOnlyList ToReadOnly()
        {
            return new FrameBlockStateReadOnlyList(this);
        }
    }
}
