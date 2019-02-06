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
    internal class FocusAssignableCellViewReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, IFocusAssignableCellView>, IFocusAssignableCellViewReadOnlyDictionary<TKey>
    {
        public FocusAssignableCellViewReadOnlyDictionary(IFocusAssignableCellViewDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        #region Frame
        IFrameAssignableCellView IReadOnlyDictionary<TKey, IFrameAssignableCellView>.this[TKey key] { get { return this[key]; } }
        IEnumerable<TKey> IReadOnlyDictionary<TKey, IFrameAssignableCellView>.Keys { get { return Keys; } }
        IEnumerable<IFrameAssignableCellView> IReadOnlyDictionary<TKey, IFrameAssignableCellView>.Values { get { return Values; } }

        IEnumerator<KeyValuePair<TKey, IFrameAssignableCellView>> IEnumerable<KeyValuePair<TKey, IFrameAssignableCellView>>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IFrameAssignableCellView>> NewList = new List<KeyValuePair<TKey, IFrameAssignableCellView>>();
            foreach (KeyValuePair<TKey, IFocusAssignableCellView> Entry in Dictionary)
                NewList.Add(new KeyValuePair<TKey, IFrameAssignableCellView>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }

        bool IReadOnlyDictionary<TKey, IFrameAssignableCellView>.TryGetValue(TKey key, out IFrameAssignableCellView value)
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

            if (!comparer.IsSameType(other, out FocusAssignableCellViewReadOnlyDictionary<TKey> AsAssignableCellViewReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsAssignableCellViewReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<TKey, IFocusAssignableCellView> Entry in this)
            {
                Debug.Assert(Entry.Key != null);

                if (!comparer.IsTrue(AsAssignableCellViewReadOnlyDictionary.ContainsKey(Entry.Key)))
                    return comparer.Failed();

                IFocusAssignableCellView OtherValue = AsAssignableCellViewReadOnlyDictionary[Entry.Key] as IFocusAssignableCellView;

                if (Entry.Value != null && OtherValue != null)
                {
                    if (!comparer.VerifyEqual(Entry.Value, OtherValue))
                        return comparer.Failed();
                }
                else if (!comparer.IsTrue(Entry.Value == null && OtherValue == null))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
