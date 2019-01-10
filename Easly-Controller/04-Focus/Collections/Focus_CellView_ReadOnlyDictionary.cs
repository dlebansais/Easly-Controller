using EaslyController.Frame;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

#pragma warning disable 1591

namespace EaslyController.Focus
{
    /// <summary>
    /// Read-only dictionary of ..., IxxxCellView
    /// </summary>
    public interface IFocusCellViewReadOnlyDictionary<TKey> : IFrameCellViewReadOnlyDictionary<TKey>, IReadOnlyDictionary<TKey, IFocusCellView>, IEqualComparable
    {
        new int Count { get; }
        new IFocusCellView this[TKey key] { get; }
        new IEnumerator<KeyValuePair<TKey, IFocusCellView>> GetEnumerator();
        new bool ContainsKey(TKey key);
    }

    /// <summary>
    /// Read-only dictionary of ..., IxxxCellView
    /// </summary>
    public class FocusCellViewReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, IFocusCellView>, IFocusCellViewReadOnlyDictionary<TKey>
    {
        public FocusCellViewReadOnlyDictionary(IFocusCellViewDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        #region Frame
        public new IFrameCellView this[TKey key] { get { return base[key]; } }
        public new IEnumerable<TKey> Keys { get { return base.Keys; } }
        public new IEnumerable<IFrameCellView> Values { get { return base.Values; } }

        public new IEnumerator<KeyValuePair<TKey, IFrameCellView>> GetEnumerator()
        {
            List<KeyValuePair<TKey, IFrameCellView>> NewList = new List<KeyValuePair<TKey, IFrameCellView>>();
            foreach (KeyValuePair<TKey, IFocusCellView> Entry in Dictionary)
                NewList.Add(new KeyValuePair<TKey, IFrameCellView>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }
        public bool TryGetValue(TKey key, out IFrameCellView value) { bool Result = TryGetValue(key, out IFocusCellView Value); value = Value; return Result; }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusCellViewReadOnlyDictionary{TKey}"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFocusCellViewReadOnlyDictionary<TKey> AsCellViewReadOnlyDictionary))
                return false;

            if (Count != AsCellViewReadOnlyDictionary.Count)
                return false;

            foreach (KeyValuePair<TKey, IFrameCellView> Entry in this)
            {
                Debug.Assert(Entry.Key != null);

                if (!AsCellViewReadOnlyDictionary.ContainsKey(Entry.Key))
                    return false;

                Debug.Assert(Entry.Value != null);
                Debug.Assert(AsCellViewReadOnlyDictionary[Entry.Key] != null);

                if (!comparer.VerifyEqual(Entry.Value, AsCellViewReadOnlyDictionary[Entry.Key] as IFocusCellView))
                    return false;
            }

            return true;
        }
        #endregion
    }
}
