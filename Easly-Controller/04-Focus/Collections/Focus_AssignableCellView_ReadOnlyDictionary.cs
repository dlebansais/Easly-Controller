#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// Read-only dictionary of ..., IxxxAssignableCellView
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public interface IFocusAssignableCellViewReadOnlyDictionary<TKey> : IFrameAssignableCellViewReadOnlyDictionary<TKey>, IReadOnlyDictionary<TKey, IFocusAssignableCellView>, IEqualComparable
    {
        new int Count { get; }
        new IFocusAssignableCellView this[TKey key] { get; }
        new IEnumerator<KeyValuePair<TKey, IFocusAssignableCellView>> GetEnumerator();
        new bool ContainsKey(TKey key);
    }

    /// <summary>
    /// Read-only dictionary of ..., IxxxAssignableCellView
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public class FocusAssignableCellViewReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, IFocusAssignableCellView>, IFocusAssignableCellViewReadOnlyDictionary<TKey>
    {
        public FocusAssignableCellViewReadOnlyDictionary(IFocusAssignableCellViewDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        #region Frame
        public new IFrameAssignableCellView this[TKey key] { get { return base[key]; } }
        public new IEnumerable<TKey> Keys { get { return base.Keys; } }
        public new IEnumerable<IFrameAssignableCellView> Values { get { return base.Values; } }

        public new IEnumerator<KeyValuePair<TKey, IFrameAssignableCellView>> GetEnumerator()
        {
            List<KeyValuePair<TKey, IFrameAssignableCellView>> NewList = new List<KeyValuePair<TKey, IFrameAssignableCellView>>();
            foreach (KeyValuePair<TKey, IFocusAssignableCellView> Entry in Dictionary)
                NewList.Add(new KeyValuePair<TKey, IFrameAssignableCellView>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }
        public bool TryGetValue(TKey key, out IFrameAssignableCellView value)
        {
            bool Result = TryGetValue(key, out IFocusAssignableCellView Value);
            value = Value;
            return Result;
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusAssignableCellViewReadOnlyDictionary{TKey}"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFocusAssignableCellViewReadOnlyDictionary<TKey> AsAssignableCellViewReadOnlyDictionary))
                return comparer.Failed();

            if (Count != AsAssignableCellViewReadOnlyDictionary.Count)
                return comparer.Failed();

            foreach (KeyValuePair<TKey, IFrameAssignableCellView> Entry in this)
            {
                Debug.Assert(Entry.Key != null);

                if (!AsAssignableCellViewReadOnlyDictionary.ContainsKey(Entry.Key))
                    return comparer.Failed();

                IFocusAssignableCellView OtherValue = AsAssignableCellViewReadOnlyDictionary[Entry.Key] as IFocusAssignableCellView;

                if (Entry.Value != null && OtherValue != null)
                {
                    if (!comparer.VerifyEqual(Entry.Value, OtherValue))
                        return comparer.Failed();
                }
                else if (Entry.Value != null || OtherValue != null)
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
