namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using NotSupportedException = System.NotSupportedException;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using NotNullReflection;

    /// <inheritdoc/>
    public class LayoutTemplateReadOnlyDictionary : FocusTemplateReadOnlyDictionary, ICollection<KeyValuePair<Type, ILayoutTemplate>>, IEnumerable<KeyValuePair<Type, ILayoutTemplate>>, IDictionary<Type, ILayoutTemplate>, IReadOnlyCollection<KeyValuePair<Type, ILayoutTemplate>>, IReadOnlyDictionary<Type, ILayoutTemplate>
    {
        /// <inheritdoc/>
        public LayoutTemplateReadOnlyDictionary(LayoutTemplateDictionary dictionary)
            : base(dictionary)
        {
        }

        #region Type, ILayoutTemplate
        void ICollection<KeyValuePair<Type, ILayoutTemplate>>.Add(KeyValuePair<Type, ILayoutTemplate> item) { throw new NotSupportedException("Collection is read-only."); }
        void ICollection<KeyValuePair<Type, ILayoutTemplate>>.Clear() { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<Type, ILayoutTemplate>>.Contains(KeyValuePair<Type, ILayoutTemplate> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<Type, ILayoutTemplate>>.CopyTo(KeyValuePair<Type, ILayoutTemplate>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<Type, IFrameTemplate> Entry in this) array[i++] = new KeyValuePair<Type, ILayoutTemplate>((Type)Entry.Key, (ILayoutTemplate)Entry.Value); }
        bool ICollection<KeyValuePair<Type, ILayoutTemplate>>.Remove(KeyValuePair<Type, ILayoutTemplate> item) { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<Type, ILayoutTemplate>>.IsReadOnly { get { return true; } }

        IEnumerator<KeyValuePair<Type, ILayoutTemplate>> IEnumerable<KeyValuePair<Type, ILayoutTemplate>>.GetEnumerator() { IEnumerator<KeyValuePair<Type, IFrameTemplate>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<Type, ILayoutTemplate>((Type)iterator.Current.Key, (ILayoutTemplate)iterator.Current.Value); } }

        ILayoutTemplate IDictionary<Type, ILayoutTemplate>.this[Type key] { get { return (ILayoutTemplate)this[key]; } set { throw new NotSupportedException("Collection is read-only."); } }
        ICollection<Type> IDictionary<Type, ILayoutTemplate>.Keys { get { List<Type> Result = new(); foreach (KeyValuePair<Type, ILayoutTemplate> Entry in (ICollection<KeyValuePair<Type, ILayoutTemplate>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<ILayoutTemplate> IDictionary<Type, ILayoutTemplate>.Values { get { List<ILayoutTemplate> Result = new(); foreach (KeyValuePair<Type, ILayoutTemplate> Entry in (ICollection<KeyValuePair<Type, ILayoutTemplate>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<Type, ILayoutTemplate>.Add(Type key, ILayoutTemplate value) { throw new NotSupportedException("Collection is read-only."); }
        bool IDictionary<Type, ILayoutTemplate>.ContainsKey(Type key) { return ContainsKey(key); }
        bool IDictionary<Type, ILayoutTemplate>.Remove(Type key) { throw new NotSupportedException("Collection is read-only."); }
        bool IDictionary<Type, ILayoutTemplate>.TryGetValue(Type key, out ILayoutTemplate value) { bool Result = TryGetValue(key, out IFrameTemplate Value); value = (ILayoutTemplate)Value; return Result; }

        ILayoutTemplate IReadOnlyDictionary<Type, ILayoutTemplate>.this[Type key] { get { return (ILayoutTemplate)this[key]; } }
        IEnumerable<Type> IReadOnlyDictionary<Type, ILayoutTemplate>.Keys { get { List<Type> Result = new(); foreach (KeyValuePair<Type, ILayoutTemplate> Entry in (ICollection<KeyValuePair<Type, ILayoutTemplate>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<ILayoutTemplate> IReadOnlyDictionary<Type, ILayoutTemplate>.Values { get { List<ILayoutTemplate> Result = new(); foreach (KeyValuePair<Type, ILayoutTemplate> Entry in (ICollection<KeyValuePair<Type, ILayoutTemplate>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<Type, ILayoutTemplate>.ContainsKey(Type key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<Type, ILayoutTemplate>.TryGetValue(Type key, out ILayoutTemplate value) { bool Result = TryGetValue(key, out IFrameTemplate Value); value = (ILayoutTemplate)Value; return Result; }
        #endregion
    }
}
