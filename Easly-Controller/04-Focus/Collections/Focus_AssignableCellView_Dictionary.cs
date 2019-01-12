using EaslyController.Frame;
using System;
using System.Collections.Generic;

#pragma warning disable 1591

namespace EaslyController.Focus
{
    /// <summary>
    /// Dictionary of ..., IxxxAssignableCellView
    /// </summary>
    public interface IFocusAssignableCellViewDictionary<TKey> : IFrameAssignableCellViewDictionary<TKey>, IDictionary<TKey, IFocusAssignableCellView>
    {
        new int Count { get; }
        new Dictionary<TKey, IFocusAssignableCellView>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of ..., IxxxAssignableCellView
    /// </summary>
    public class FocusAssignableCellViewDictionary<TKey> : Dictionary<TKey, IFocusAssignableCellView>, IFocusAssignableCellViewDictionary<TKey>
    {
        #region Frame
        IFrameAssignableCellView IDictionary<TKey, IFrameAssignableCellView>.this[TKey key] { get { return this[key]; } set { this[key] = (IFocusAssignableCellView)value; } }
        ICollection<TKey> IDictionary<TKey, IFrameAssignableCellView>.Keys { get { return Keys; } }
        ICollection<IFrameAssignableCellView> IDictionary<TKey, IFrameAssignableCellView>.Values { get { return new List<IFrameAssignableCellView>(Values); } }
        public void Add(TKey key, IFrameAssignableCellView value) { base.Add(key, (IFocusAssignableCellView)value); }

        public new IEnumerator<KeyValuePair<TKey, IFrameAssignableCellView>> GetEnumerator()
        {
            List<KeyValuePair<TKey, IFrameAssignableCellView>> NewList = new List<KeyValuePair<TKey, IFrameAssignableCellView>>();
            IEnumerator<KeyValuePair<TKey, IFocusAssignableCellView>> Enumerator = base.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, IFocusAssignableCellView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<TKey, IFrameAssignableCellView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out IFrameAssignableCellView value) { bool Result = TryGetValue(key, out IFocusAssignableCellView Value); value = Value; return Result; }
        public void Add(KeyValuePair<TKey, IFrameAssignableCellView> item) { base.Add(item.Key, (IFocusAssignableCellView)item.Value); }
        public bool Contains(KeyValuePair<TKey, IFrameAssignableCellView> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<TKey, IFrameAssignableCellView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        public bool Remove(KeyValuePair<TKey, IFrameAssignableCellView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IFrameAssignableCellView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IFocusAssignableCellView>>)this).IsReadOnly; } }
        #endregion
    }
}
