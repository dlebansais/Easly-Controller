namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;
    using System;

    /// <inheritdoc/>
    public class FocusTemplateReadOnlyDictionary : FrameTemplateReadOnlyDictionary, ICollection<KeyValuePair<Type, IFocusTemplate>>, IEnumerable<KeyValuePair<Type, IFocusTemplate>>, IDictionary<Type, IFocusTemplate>, IReadOnlyCollection<KeyValuePair<Type, IFocusTemplate>>, IReadOnlyDictionary<Type, IFocusTemplate>
    {
        /// <inheritdoc/>
        public FocusTemplateReadOnlyDictionary(FocusTemplateDictionary dictionary)
            : base(dictionary)
        {
        }

        /// <inheritdoc/>
        public bool TryGetValue(Type key, out IFocusTemplate value) { bool Result = TryGetValue(key, out IFrameTemplate Value); value = (IFocusTemplate)Value; return Result; }

        #region Type, IFocusTemplate
        void ICollection<KeyValuePair<Type, IFocusTemplate>>.Add(KeyValuePair<Type, IFocusTemplate> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<Type, IFocusTemplate>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<Type, IFocusTemplate>>.Contains(KeyValuePair<Type, IFocusTemplate> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<Type, IFocusTemplate>>.CopyTo(KeyValuePair<Type, IFocusTemplate>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<Type, IFocusTemplate>>.Remove(KeyValuePair<Type, IFocusTemplate> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<Type, IFocusTemplate>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<Type, IFocusTemplate>> IEnumerable<KeyValuePair<Type, IFocusTemplate>>.GetEnumerator() { return ((IList<KeyValuePair<Type, IFocusTemplate>>)this).GetEnumerator(); }

        IFocusTemplate IDictionary<Type, IFocusTemplate>.this[Type key] { get { return (IFocusTemplate)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<Type> IDictionary<Type, IFocusTemplate>.Keys { get { List<Type> Result = new(); foreach (KeyValuePair<Type, IFocusTemplate> Entry in (ICollection<KeyValuePair<Type, IFocusTemplate>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IFocusTemplate> IDictionary<Type, IFocusTemplate>.Values { get { List<IFocusTemplate> Result = new(); foreach (KeyValuePair<Type, IFocusTemplate> Entry in (ICollection<KeyValuePair<Type, IFocusTemplate>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<Type, IFocusTemplate>.Add(Type key, IFocusTemplate value) { throw new System.InvalidOperationException(); }
        bool IDictionary<Type, IFocusTemplate>.ContainsKey(Type key) { return ContainsKey(key); }
        bool IDictionary<Type, IFocusTemplate>.Remove(Type key) { throw new System.InvalidOperationException(); }
        bool IDictionary<Type, IFocusTemplate>.TryGetValue(Type key, out IFocusTemplate value) { bool Result = TryGetValue(key, out IFrameTemplate Value); value = (IFocusTemplate)Value; return Result; }

        IFocusTemplate IReadOnlyDictionary<Type, IFocusTemplate>.this[Type key] { get { return (IFocusTemplate)this[key]; } }
        IEnumerable<Type> IReadOnlyDictionary<Type, IFocusTemplate>.Keys { get { List<Type> Result = new(); foreach (KeyValuePair<Type, IFocusTemplate> Entry in (ICollection<KeyValuePair<Type, IFocusTemplate>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IFocusTemplate> IReadOnlyDictionary<Type, IFocusTemplate>.Values { get { List<IFocusTemplate> Result = new(); foreach (KeyValuePair<Type, IFocusTemplate> Entry in (ICollection<KeyValuePair<Type, IFocusTemplate>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<Type, IFocusTemplate>.ContainsKey(Type key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<Type, IFocusTemplate>.TryGetValue(Type key, out IFocusTemplate value) { bool Result = TryGetValue(key, out IFrameTemplate Value); value = (IFocusTemplate)Value; return Result; }
        #endregion
    }
}
