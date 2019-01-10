using EaslyController.Frame;
using System;
using System.Collections.Generic;

#pragma warning disable 1591

namespace EaslyController.Focus
{
    /// <summary>
    /// Dictionary of ..., IxxxCellView
    /// </summary>
    public interface IFocusCellViewDictionary<TKey> : IFrameCellViewDictionary<TKey>, IDictionary<TKey, IFocusCellView>
    {
        new int Count { get; }
        new Dictionary<TKey, IFocusCellView>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of ..., IxxxCellView
    /// </summary>
    public class FocusCellViewDictionary<TKey> : Dictionary<TKey, IFocusCellView>, IFocusCellViewDictionary<TKey>
    {
        #region Frame
        IFrameCellView IDictionary<TKey, IFrameCellView>.this[TKey key] { get { return this[key]; } set { this[key] = (IFocusCellView)value; } }
        ICollection<TKey> IDictionary<TKey, IFrameCellView>.Keys { get { return Keys; } }
        ICollection<IFrameCellView> IDictionary<TKey, IFrameCellView>.Values { get { return new List<IFrameCellView>(Values); } }
        public void Add(TKey key, IFrameCellView value) { base.Add(key, (IFocusCellView)value); }

        public new IEnumerator<KeyValuePair<TKey, IFrameCellView>> GetEnumerator()
        {
            List<KeyValuePair<TKey, IFrameCellView>> NewList = new List<KeyValuePair<TKey, IFrameCellView>>();
            IEnumerator<KeyValuePair<TKey, IFocusCellView>> Enumerator = base.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, IFocusCellView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<TKey, IFrameCellView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out IFrameCellView value) { bool Result = TryGetValue(key, out IFocusCellView Value); value = Value; return Result; }
        public void Add(KeyValuePair<TKey, IFrameCellView> item) { base.Add(item.Key, (IFocusCellView)item.Value); }
        public bool Contains(KeyValuePair<TKey, IFrameCellView> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<TKey, IFrameCellView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        public bool Remove(KeyValuePair<TKey, IFrameCellView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IFrameCellView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IFocusCellView>>)this).IsReadOnly; } }
        #endregion
    }
}
