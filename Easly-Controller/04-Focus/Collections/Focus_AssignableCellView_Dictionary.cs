#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

    /// <summary>
    /// Dictionary of ..., IxxxAssignableCellView
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public interface IFocusAssignableCellViewDictionary<TKey> : IFrameAssignableCellViewDictionary<TKey>, IDictionary<TKey, IFocusAssignableCellView>
    {
        new int Count { get; }
        new Dictionary<TKey, IFocusAssignableCellView>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of ..., IxxxAssignableCellView
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    internal class FocusAssignableCellViewDictionary<TKey> : Dictionary<TKey, IFocusAssignableCellView>, IFocusAssignableCellViewDictionary<TKey>
    {
        #region Frame
        IFrameAssignableCellView IDictionary<TKey, IFrameAssignableCellView>.this[TKey key] { get { return this[key]; } set { this[key] = (IFocusAssignableCellView)value; } }
        void IDictionary<TKey, IFrameAssignableCellView>.Add(TKey key, IFrameAssignableCellView value) { Add(key, (IFocusAssignableCellView)value); }
        ICollection<TKey> IDictionary<TKey, IFrameAssignableCellView>.Keys { get { return Keys; } }

        bool IDictionary<TKey, IFrameAssignableCellView>.TryGetValue(TKey key, out IFrameAssignableCellView value)
        {
            bool Result = TryGetValue(key, out IFocusAssignableCellView Value);
            value = Value;
            return Result;
        }

        ICollection<IFrameAssignableCellView> IDictionary<TKey, IFrameAssignableCellView>.Values { get { return new List<IFrameAssignableCellView>(Values); } }

        void ICollection<KeyValuePair<TKey, IFrameAssignableCellView>>.CopyTo(KeyValuePair<TKey, IFrameAssignableCellView>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<TKey, IFocusAssignableCellView> Entry in this)
                array[arrayIndex++] = new KeyValuePair<TKey, IFrameAssignableCellView>(Entry.Key, Entry.Value);
        }

        void ICollection<KeyValuePair<TKey, IFrameAssignableCellView>>.Add(KeyValuePair<TKey, IFrameAssignableCellView> item) { Add(item.Key, (IFocusAssignableCellView)item.Value); }
        bool ICollection<KeyValuePair<TKey, IFrameAssignableCellView>>.Contains(KeyValuePair<TKey, IFrameAssignableCellView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        bool ICollection<KeyValuePair<TKey, IFrameAssignableCellView>>.Remove(KeyValuePair<TKey, IFrameAssignableCellView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IFrameAssignableCellView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>)this).IsReadOnly; } }

        IEnumerator<KeyValuePair<TKey, IFrameAssignableCellView>> IEnumerable<KeyValuePair<TKey, IFrameAssignableCellView>>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IFrameAssignableCellView>> NewList = new List<KeyValuePair<TKey, IFrameAssignableCellView>>();
            IEnumerator<KeyValuePair<TKey, IFocusAssignableCellView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, IFocusAssignableCellView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<TKey, IFrameAssignableCellView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion
    }
}
