namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using System;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class LayoutTemplateReadOnlyDictionary : FocusTemplateReadOnlyDictionary, ICollection<KeyValuePair<Type, ILayoutTemplate>>, IEnumerable<KeyValuePair<Type, ILayoutTemplate>>, IDictionary<Type, ILayoutTemplate>, IReadOnlyCollection<KeyValuePair<Type, ILayoutTemplate>>, IReadOnlyDictionary<Type, ILayoutTemplate>
    {
        /// <inheritdoc/>
        public LayoutTemplateReadOnlyDictionary(LayoutTemplateDictionary dictionary)
            : base(dictionary)
        {
        }

        #region Type, ILayoutTemplate
        void ICollection<KeyValuePair<Type, ILayoutTemplate>>.Add(KeyValuePair<Type, ILayoutTemplate> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<Type, ILayoutTemplate>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<Type, ILayoutTemplate>>.Contains(KeyValuePair<Type, ILayoutTemplate> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<Type, ILayoutTemplate>>.CopyTo(KeyValuePair<Type, ILayoutTemplate>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<Type, ILayoutTemplate>>.Remove(KeyValuePair<Type, ILayoutTemplate> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<Type, ILayoutTemplate>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<Type, ILayoutTemplate>> IEnumerable<KeyValuePair<Type, ILayoutTemplate>>.GetEnumerator() { return ((IList<KeyValuePair<Type, ILayoutTemplate>>)this).GetEnumerator(); }

        ILayoutTemplate IDictionary<Type, ILayoutTemplate>.this[Type key] { get { return (ILayoutTemplate)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<Type> IDictionary<Type, ILayoutTemplate>.Keys { get { List<Type> Result = new(); foreach (KeyValuePair<Type, ILayoutTemplate> Entry in (ICollection<KeyValuePair<Type, ILayoutTemplate>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<ILayoutTemplate> IDictionary<Type, ILayoutTemplate>.Values { get { List<ILayoutTemplate> Result = new(); foreach (KeyValuePair<Type, ILayoutTemplate> Entry in (ICollection<KeyValuePair<Type, ILayoutTemplate>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<Type, ILayoutTemplate>.Add(Type key, ILayoutTemplate value) { throw new System.InvalidOperationException(); }
        bool IDictionary<Type, ILayoutTemplate>.ContainsKey(Type key) { return ContainsKey(key); }
        bool IDictionary<Type, ILayoutTemplate>.Remove(Type key) { throw new System.InvalidOperationException(); }
        bool IDictionary<Type, ILayoutTemplate>.TryGetValue(Type key, out ILayoutTemplate value) { bool Result = TryGetValue(key, out IFrameTemplate Value); value = (ILayoutTemplate)Value; return Result; }

        ILayoutTemplate IReadOnlyDictionary<Type, ILayoutTemplate>.this[Type key] { get { return (ILayoutTemplate)this[key]; } }
        IEnumerable<Type> IReadOnlyDictionary<Type, ILayoutTemplate>.Keys { get { List<Type> Result = new(); foreach (KeyValuePair<Type, ILayoutTemplate> Entry in (ICollection<KeyValuePair<Type, ILayoutTemplate>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<ILayoutTemplate> IReadOnlyDictionary<Type, ILayoutTemplate>.Values { get { List<ILayoutTemplate> Result = new(); foreach (KeyValuePair<Type, ILayoutTemplate> Entry in (ICollection<KeyValuePair<Type, ILayoutTemplate>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<Type, ILayoutTemplate>.ContainsKey(Type key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<Type, ILayoutTemplate>.TryGetValue(Type key, out ILayoutTemplate value) { bool Result = TryGetValue(key, out IFrameTemplate Value); value = (ILayoutTemplate)Value; return Result; }
        #endregion
    }
}
