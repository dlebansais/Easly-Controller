#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Dictionary of ..., IxxxAssignableCellView
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public interface ILayoutAssignableCellViewDictionary<TKey> : IFocusAssignableCellViewDictionary<TKey>, IDictionary<TKey, ILayoutAssignableCellView>
    {
        new ILayoutAssignableCellView this[TKey key] { get; set; }
        new int Count { get; }
        new Dictionary<TKey, ILayoutAssignableCellView>.Enumerator GetEnumerator();
        new bool ContainsKey(TKey key);
    }

    /// <summary>
    /// Dictionary of ..., IxxxAssignableCellView
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    internal class LayoutAssignableCellViewDictionary<TKey> : Dictionary<TKey, ILayoutAssignableCellView>, ILayoutAssignableCellViewDictionary<TKey>
    {
        /// <summary>
        /// Gets a read-only view of the dictionary.
        /// </summary>
        public virtual IFrameAssignableCellViewReadOnlyDictionary<TKey> ToReadOnly()
        {
            return new LayoutAssignableCellViewReadOnlyDictionary<TKey>(this);
        }

        #region Frame
        IFrameAssignableCellView IDictionary<TKey, IFrameAssignableCellView>.this[TKey key] { get { return this[key]; } set { this[key] = (ILayoutAssignableCellView)value; } }
        void IDictionary<TKey, IFrameAssignableCellView>.Add(TKey key, IFrameAssignableCellView value) { Add(key, (ILayoutAssignableCellView)value); }
        ICollection<TKey> IDictionary<TKey, IFrameAssignableCellView>.Keys { get { return Keys; } }

        bool IDictionary<TKey, IFrameAssignableCellView>.TryGetValue(TKey key, out IFrameAssignableCellView value)
        {
            bool Result = TryGetValue(key, out ILayoutAssignableCellView Value);
            value = Value;
            return Result;
        }

        ICollection<IFrameAssignableCellView> IDictionary<TKey, IFrameAssignableCellView>.Values { get { return new List<IFrameAssignableCellView>(Values); } }

        void ICollection<KeyValuePair<TKey, IFrameAssignableCellView>>.CopyTo(KeyValuePair<TKey, IFrameAssignableCellView>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<TKey, ILayoutAssignableCellView> Entry in this)
                array[arrayIndex++] = new KeyValuePair<TKey, IFrameAssignableCellView>(Entry.Key, Entry.Value);
        }

        void ICollection<KeyValuePair<TKey, IFrameAssignableCellView>>.Add(KeyValuePair<TKey, IFrameAssignableCellView> item) { Add(item.Key, (ILayoutAssignableCellView)item.Value); }
        bool ICollection<KeyValuePair<TKey, IFrameAssignableCellView>>.Contains(KeyValuePair<TKey, IFrameAssignableCellView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        bool ICollection<KeyValuePair<TKey, IFrameAssignableCellView>>.Remove(KeyValuePair<TKey, IFrameAssignableCellView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IFrameAssignableCellView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>)this).IsReadOnly; } }

        IEnumerator<KeyValuePair<TKey, IFrameAssignableCellView>> IEnumerable<KeyValuePair<TKey, IFrameAssignableCellView>>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IFrameAssignableCellView>> NewList = new List<KeyValuePair<TKey, IFrameAssignableCellView>>();
            IEnumerator<KeyValuePair<TKey, ILayoutAssignableCellView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, ILayoutAssignableCellView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<TKey, IFrameAssignableCellView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion

        #region Focus
        IFocusAssignableCellView IFocusAssignableCellViewDictionary<TKey>.this[TKey key] { get { return this[key]; } set { this[key] = (ILayoutAssignableCellView)value; } }

        Dictionary<TKey, IFocusAssignableCellView>.Enumerator IFocusAssignableCellViewDictionary<TKey>.GetEnumerator()
        {
            Dictionary<TKey, IFocusAssignableCellView> NewDictionary = new Dictionary<TKey, IFocusAssignableCellView>();
            foreach (KeyValuePair<TKey, ILayoutAssignableCellView> Entry in this)
                NewDictionary.Add(Entry.Key, Entry.Value);

            return NewDictionary.GetEnumerator();
        }

        IFocusAssignableCellView IDictionary<TKey, IFocusAssignableCellView>.this[TKey key] { get { return this[key]; } set { this[key] = (ILayoutAssignableCellView)value; } }
        void IDictionary<TKey, IFocusAssignableCellView>.Add(TKey key, IFocusAssignableCellView value) { Add(key, (ILayoutAssignableCellView)value); }
        ICollection<TKey> IDictionary<TKey, IFocusAssignableCellView>.Keys { get { return Keys; } }

        bool IDictionary<TKey, IFocusAssignableCellView>.TryGetValue(TKey key, out IFocusAssignableCellView value)
        {
            bool Result = TryGetValue(key, out ILayoutAssignableCellView Value);
            value = Value;
            return Result;
        }

        ICollection<IFocusAssignableCellView> IDictionary<TKey, IFocusAssignableCellView>.Values { get { return new List<IFocusAssignableCellView>(Values); } }

        void ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>.CopyTo(KeyValuePair<TKey, IFocusAssignableCellView>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<TKey, ILayoutAssignableCellView> Entry in this)
                array[arrayIndex++] = new KeyValuePair<TKey, IFocusAssignableCellView>(Entry.Key, Entry.Value);
        }

        void ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>.Add(KeyValuePair<TKey, IFocusAssignableCellView> item) { Add(item.Key, (ILayoutAssignableCellView)item.Value); }
        bool ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>.Contains(KeyValuePair<TKey, IFocusAssignableCellView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        bool ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>.Remove(KeyValuePair<TKey, IFocusAssignableCellView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, ILayoutAssignableCellView>>)this).IsReadOnly; } }

        IEnumerator<KeyValuePair<TKey, IFocusAssignableCellView>> IEnumerable<KeyValuePair<TKey, IFocusAssignableCellView>>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IFocusAssignableCellView>> NewList = new List<KeyValuePair<TKey, IFocusAssignableCellView>>();
            IEnumerator<KeyValuePair<TKey, ILayoutAssignableCellView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, ILayoutAssignableCellView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<TKey, IFocusAssignableCellView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion
    }
}
