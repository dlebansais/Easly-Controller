﻿namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using NotNullReflection;

    /// <inheritdoc/>
    public class LayoutTemplateDictionary : FocusTemplateDictionary, ICollection<KeyValuePair<Type, ILayoutTemplate>>, IEnumerable<KeyValuePair<Type, ILayoutTemplate>>, IDictionary<Type, ILayoutTemplate>, IReadOnlyCollection<KeyValuePair<Type, ILayoutTemplate>>, IReadOnlyDictionary<Type, ILayoutTemplate>
    {
        /// <inheritdoc/>
        public LayoutTemplateDictionary() : base() { }
        /// <inheritdoc/>
        public LayoutTemplateDictionary(IDictionary<Type, ILayoutTemplate> dictionary) : base() { foreach (KeyValuePair<Type, ILayoutTemplate> Entry in dictionary) Add(Entry.Key, Entry.Value); }
        /// <inheritdoc/>
        public LayoutTemplateDictionary(int capacity) : base(capacity) { }

        #region Type, ILayoutTemplate
        void ICollection<KeyValuePair<Type, ILayoutTemplate>>.Add(KeyValuePair<Type, ILayoutTemplate> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<Type, ILayoutTemplate>>.Contains(KeyValuePair<Type, ILayoutTemplate> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<Type, ILayoutTemplate>>.CopyTo(KeyValuePair<Type, ILayoutTemplate>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<Type, IFrameTemplate> Entry in this) array[i++] = new KeyValuePair<Type, ILayoutTemplate>((Type)Entry.Key, (ILayoutTemplate)Entry.Value); }
        bool ICollection<KeyValuePair<Type, ILayoutTemplate>>.Remove(KeyValuePair<Type, ILayoutTemplate> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<Type, ILayoutTemplate>>.IsReadOnly { get { return ((ICollection<KeyValuePair<Type, IFocusTemplate>>)this).IsReadOnly; } }

        IEnumerator<KeyValuePair<Type, ILayoutTemplate>> IEnumerable<KeyValuePair<Type, ILayoutTemplate>>.GetEnumerator() { IEnumerator<KeyValuePair<Type, IFrameTemplate>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<Type, ILayoutTemplate>((Type)iterator.Current.Key, (ILayoutTemplate)iterator.Current.Value); } }

        ILayoutTemplate IDictionary<Type, ILayoutTemplate>.this[Type key] { get { return (ILayoutTemplate)this[key]; } set { this[key] = value; } }
        ICollection<Type> IDictionary<Type, ILayoutTemplate>.Keys { get { List<Type> Result = new(); foreach (KeyValuePair<Type, ILayoutTemplate> Entry in (ICollection<KeyValuePair<Type, ILayoutTemplate>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<ILayoutTemplate> IDictionary<Type, ILayoutTemplate>.Values { get { List<ILayoutTemplate> Result = new(); foreach (KeyValuePair<Type, ILayoutTemplate> Entry in (ICollection<KeyValuePair<Type, ILayoutTemplate>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<Type, ILayoutTemplate>.Add(Type key, ILayoutTemplate value) { Add(key, value); }
        bool IDictionary<Type, ILayoutTemplate>.ContainsKey(Type key) { return ContainsKey(key); }
        bool IDictionary<Type, ILayoutTemplate>.Remove(Type key) { return Remove(key); }
        bool IDictionary<Type, ILayoutTemplate>.TryGetValue(Type key, out ILayoutTemplate value) { bool Result = TryGetValue(key, out IFrameTemplate Value); value = (ILayoutTemplate)Value; return Result; }

        ILayoutTemplate IReadOnlyDictionary<Type, ILayoutTemplate>.this[Type key] { get { return (ILayoutTemplate)this[key]; } }
        IEnumerable<Type> IReadOnlyDictionary<Type, ILayoutTemplate>.Keys { get { List<Type> Result = new(); foreach (KeyValuePair<Type, ILayoutTemplate> Entry in (ICollection<KeyValuePair<Type, ILayoutTemplate>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<ILayoutTemplate> IReadOnlyDictionary<Type, ILayoutTemplate>.Values { get { List<ILayoutTemplate> Result = new(); foreach (KeyValuePair<Type, ILayoutTemplate> Entry in (ICollection<KeyValuePair<Type, ILayoutTemplate>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<Type, ILayoutTemplate>.ContainsKey(Type key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<Type, ILayoutTemplate>.TryGetValue(Type key, out ILayoutTemplate value) { bool Result = TryGetValue(key, out IFrameTemplate Value); value = (ILayoutTemplate)Value; return Result; }
        #endregion

        /// <inheritdoc/>
        public override FrameTemplateReadOnlyDictionary ToReadOnly()
        {
            return new LayoutTemplateReadOnlyDictionary(this);
        }
    }
}
