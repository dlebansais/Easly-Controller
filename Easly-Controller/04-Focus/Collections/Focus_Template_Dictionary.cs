﻿namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Frame;
    using System;

    /// <inheritdoc/>
    public class FocusTemplateDictionary : FrameTemplateDictionary, ICollection<KeyValuePair<Type, IFocusTemplate>>, IEnumerable<KeyValuePair<Type, IFocusTemplate>>, IDictionary<Type, IFocusTemplate>, IReadOnlyCollection<KeyValuePair<Type, IFocusTemplate>>, IReadOnlyDictionary<Type, IFocusTemplate>
    {
        /// <inheritdoc/>
        public FocusTemplateDictionary() : base() { }
        /// <inheritdoc/>
        public FocusTemplateDictionary(IDictionary<Type, IFocusTemplate> dictionary) { foreach (KeyValuePair<Type, IFocusTemplate> Entry in dictionary) Add(Entry.Key, Entry.Value); }
        /// <inheritdoc/>
        public FocusTemplateDictionary(IEnumerable<KeyValuePair<Type, IFocusTemplate>> collection) { foreach (KeyValuePair<Type, IFocusTemplate> Entry in collection) Add(Entry.Key, Entry.Value); }
        /// <inheritdoc/>
        public FocusTemplateDictionary(IEqualityComparer<Type> comparer) : base(comparer) { }
        /// <inheritdoc/>
        public FocusTemplateDictionary(int capacity) : base(capacity) { }
        /// <inheritdoc/>
        public FocusTemplateDictionary(IDictionary<Type, IFocusTemplate> dictionary, IEqualityComparer<Type> comparer) : base(comparer) { foreach (KeyValuePair<Type, IFocusTemplate> Entry in dictionary) Add(Entry.Key, Entry.Value); }
        /// <inheritdoc/>
        public FocusTemplateDictionary(IEnumerable<KeyValuePair<Type, IFocusTemplate>> collection, IEqualityComparer<Type> comparer) : base(comparer) { foreach (KeyValuePair<Type, IFocusTemplate> Entry in collection) Add(Entry.Key, Entry.Value); }
        /// <inheritdoc/>
        public FocusTemplateDictionary(int capacity, IEqualityComparer<Type> comparer) : base(capacity, comparer) { }

        #region Type, IFocusTemplate
        void ICollection<KeyValuePair<Type, IFocusTemplate>>.Add(KeyValuePair<Type, IFocusTemplate> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<Type, IFocusTemplate>>.Contains(KeyValuePair<Type, IFocusTemplate> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<Type, IFocusTemplate>>.CopyTo(KeyValuePair<Type, IFocusTemplate>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<Type, IFocusTemplate>>.Remove(KeyValuePair<Type, IFocusTemplate> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<Type, IFocusTemplate>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<Type, IFocusTemplate>> IEnumerable<KeyValuePair<Type, IFocusTemplate>>.GetEnumerator() { return ((IList<KeyValuePair<Type, IFocusTemplate>>)this).GetEnumerator(); }

        IFocusTemplate IDictionary<Type, IFocusTemplate>.this[Type key] { get { return (IFocusTemplate)this[key]; } set { this[key] = value; } }
        ICollection<Type> IDictionary<Type, IFocusTemplate>.Keys { get { List<Type> Result = new(); foreach (KeyValuePair<Type, IFocusTemplate> Entry in (ICollection<KeyValuePair<Type, IFocusTemplate>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IFocusTemplate> IDictionary<Type, IFocusTemplate>.Values { get { List<IFocusTemplate> Result = new(); foreach (KeyValuePair<Type, IFocusTemplate> Entry in (ICollection<KeyValuePair<Type, IFocusTemplate>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<Type, IFocusTemplate>.Add(Type key, IFocusTemplate value) { Add(key, value); }
        bool IDictionary<Type, IFocusTemplate>.ContainsKey(Type key) { return ContainsKey(key); }
        bool IDictionary<Type, IFocusTemplate>.Remove(Type key) { return Remove(key); }
        bool IDictionary<Type, IFocusTemplate>.TryGetValue(Type key, out IFocusTemplate value) { bool Result = TryGetValue(key, out IFrameTemplate Value); value = (IFocusTemplate)Value; return Result; }

        IFocusTemplate IReadOnlyDictionary<Type, IFocusTemplate>.this[Type key] { get { return (IFocusTemplate)this[key]; } }
        IEnumerable<Type> IReadOnlyDictionary<Type, IFocusTemplate>.Keys { get { List<Type> Result = new(); foreach (KeyValuePair<Type, IFocusTemplate> Entry in (ICollection<KeyValuePair<Type, IFocusTemplate>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IFocusTemplate> IReadOnlyDictionary<Type, IFocusTemplate>.Values { get { List<IFocusTemplate> Result = new(); foreach (KeyValuePair<Type, IFocusTemplate> Entry in (ICollection<KeyValuePair<Type, IFocusTemplate>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<Type, IFocusTemplate>.ContainsKey(Type key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<Type, IFocusTemplate>.TryGetValue(Type key, out IFocusTemplate value) { bool Result = TryGetValue(key, out IFrameTemplate Value); value = (IFocusTemplate)Value; return Result; }
        #endregion

        /// <inheritdoc/>
        public override FrameTemplateReadOnlyDictionary ToReadOnly()
        {
            return new FocusTemplateReadOnlyDictionary(this);
        }
    }
}
