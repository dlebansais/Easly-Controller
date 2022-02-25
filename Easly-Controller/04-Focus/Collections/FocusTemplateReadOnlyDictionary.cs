namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using NotSupportedException = System.NotSupportedException;
    using EaslyController.Frame;
    using NotNullReflection;

    /// <inheritdoc/>
    public class FocusTemplateReadOnlyDictionary : FrameTemplateReadOnlyDictionary, ICollection<KeyValuePair<Type, IFocusTemplate>>, IEnumerable<KeyValuePair<Type, IFocusTemplate>>, IDictionary<Type, IFocusTemplate>, IReadOnlyCollection<KeyValuePair<Type, IFocusTemplate>>, IReadOnlyDictionary<Type, IFocusTemplate>
    {
        /// <inheritdoc/>
        public FocusTemplateReadOnlyDictionary(FocusTemplateDictionary dictionary)
            : base(dictionary)
        {
        }

        #region Type, IFocusTemplate
        void ICollection<KeyValuePair<Type, IFocusTemplate>>.Add(KeyValuePair<Type, IFocusTemplate> item) { throw new NotSupportedException("Collection is read-only."); }
        void ICollection<KeyValuePair<Type, IFocusTemplate>>.Clear() { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<Type, IFocusTemplate>>.Contains(KeyValuePair<Type, IFocusTemplate> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<Type, IFocusTemplate>>.CopyTo(KeyValuePair<Type, IFocusTemplate>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<Type, IFrameTemplate> Entry in this) array[i++] = new KeyValuePair<Type, IFocusTemplate>((Type)Entry.Key, (IFocusTemplate)Entry.Value); }
        bool ICollection<KeyValuePair<Type, IFocusTemplate>>.Remove(KeyValuePair<Type, IFocusTemplate> item) { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<Type, IFocusTemplate>>.IsReadOnly { get { return true; } }

        IEnumerator<KeyValuePair<Type, IFocusTemplate>> IEnumerable<KeyValuePair<Type, IFocusTemplate>>.GetEnumerator() { IEnumerator<KeyValuePair<Type, IFrameTemplate>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<Type, IFocusTemplate>((Type)iterator.Current.Key, (IFocusTemplate)iterator.Current.Value); } }

        IFocusTemplate IDictionary<Type, IFocusTemplate>.this[Type key] { get { return (IFocusTemplate)this[key]; } set { throw new NotSupportedException("Collection is read-only."); } }
        ICollection<Type> IDictionary<Type, IFocusTemplate>.Keys { get { List<Type> Result = new(); foreach (KeyValuePair<Type, IFocusTemplate> Entry in (ICollection<KeyValuePair<Type, IFocusTemplate>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IFocusTemplate> IDictionary<Type, IFocusTemplate>.Values { get { List<IFocusTemplate> Result = new(); foreach (KeyValuePair<Type, IFocusTemplate> Entry in (ICollection<KeyValuePair<Type, IFocusTemplate>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<Type, IFocusTemplate>.Add(Type key, IFocusTemplate value) { throw new NotSupportedException("Collection is read-only."); }
        bool IDictionary<Type, IFocusTemplate>.ContainsKey(Type key) { return ContainsKey(key); }
        bool IDictionary<Type, IFocusTemplate>.Remove(Type key) { throw new NotSupportedException("Collection is read-only."); }
        bool IDictionary<Type, IFocusTemplate>.TryGetValue(Type key, out IFocusTemplate value) { bool Result = TryGetValue(key, out IFrameTemplate Value); value = (IFocusTemplate)Value; return Result; }

        IFocusTemplate IReadOnlyDictionary<Type, IFocusTemplate>.this[Type key] { get { return (IFocusTemplate)this[key]; } }
        IEnumerable<Type> IReadOnlyDictionary<Type, IFocusTemplate>.Keys { get { List<Type> Result = new(); foreach (KeyValuePair<Type, IFocusTemplate> Entry in (ICollection<KeyValuePair<Type, IFocusTemplate>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IFocusTemplate> IReadOnlyDictionary<Type, IFocusTemplate>.Values { get { List<IFocusTemplate> Result = new(); foreach (KeyValuePair<Type, IFocusTemplate> Entry in (ICollection<KeyValuePair<Type, IFocusTemplate>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<Type, IFocusTemplate>.ContainsKey(Type key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<Type, IFocusTemplate>.TryGetValue(Type key, out IFocusTemplate value) { bool Result = TryGetValue(key, out IFrameTemplate Value); value = (IFocusTemplate)Value; return Result; }
        #endregion
    }
}
