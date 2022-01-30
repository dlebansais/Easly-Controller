using EaslyController;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Coverage;

public class TestDictionary<T, TO, TKey, TValue>
    where T: IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, new()
    where TO: ICollection<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
{
    public TestDictionary(Func<T, TO> toReadOnlyHandler, TKey neutralKey, TValue neutralValue)
    {
        ToReadOnlyHandler = toReadOnlyHandler;
        NeutralKey = neutralKey;
        NeutralValue = neutralValue;
    }

    private Func<T, TO> ToReadOnlyHandler;
    private TKey NeutralKey;
    private TValue NeutralValue;

    public void Test()
    {
        TestToReadOnly();
        TestIsEqual();
        TestInterfaces();
        TestReadOnlyInterfaces();
        TestExceptions();
    }

    private void TestToReadOnly()
    {
        T NewInstance = new();
        _ = ToReadOnlyHandler(NewInstance);
    }

    private void TestIsEqual()
    {
        T NewInstance = new();
        NewInstance.Add(NeutralKey, NeutralValue);

        TestIsEqual(NewInstance);
        TestIsEqual(ToReadOnlyHandler(NewInstance));
    }

    private void TestIsEqual(object instance)
    {
        if (instance is IEqualComparable AsComparable)
        {
            CompareEqual Comparer = CompareEqual.New(true);
            AsComparable.IsEqual(Comparer, AsComparable);

            Comparer.SetFailIndex(0);
            AsComparable.IsEqual(Comparer, AsComparable);

            Comparer.SetFailIndex(1);
            AsComparable.IsEqual(Comparer, AsComparable);

            Comparer.SetFailIndex(2);
            AsComparable.IsEqual(Comparer, AsComparable);

            Comparer.SetFailIndex(3);
            AsComparable.IsEqual(Comparer, AsComparable);

            Comparer.SetFailIndex(4);
            AsComparable.IsEqual(Comparer, AsComparable);
        }
    }

    private void TestInterfaces()
    {
        T NewInstance = new();
        NewInstance.Add(NeutralKey, NeutralValue);

        IDictionary<TKey, TValue> AsDictionary = NewInstance;
        IEnumerator<KeyValuePair<TKey, TValue>> Enumerator = NewInstance.GetEnumerator();
        Enumerator.MoveNext();
        KeyValuePair<TKey, TValue> Entry = Enumerator.Current;
        AsDictionary[Entry.Key] = Entry.Value;

        Assert.AreEqual(Entry.Value, AsDictionary[Entry.Key]);

        ICollection<TKey> Keys = AsDictionary.Keys;
        Assert.AreEqual(1, Keys.Count);

        ICollection<TValue> Values = AsDictionary.Values;
        Assert.AreEqual(1, Values.Count);

        NewInstance.Clear();

        AsDictionary.Add(NeutralKey, NeutralValue);

        bool ContainsItem = AsDictionary.Contains(Entry);
        Assert.IsTrue(ContainsItem);

        ContainsItem = AsDictionary.ContainsKey(NeutralKey);
        Assert.IsTrue(ContainsItem);

        bool IsValueFound = AsDictionary.TryGetValue(NeutralKey, out _);
        Assert.IsTrue(IsValueFound);

        AsDictionary.Remove(NeutralKey);
        Assert.AreEqual(0, AsDictionary.Count);

        ICollection<KeyValuePair<TKey, TValue>> AsCollection = NewInstance;

        AsCollection.Add(Entry);
        Assert.AreEqual(1, AsDictionary.Count);

        ContainsItem = AsCollection.Contains(Entry);
        Assert.IsTrue(ContainsItem);

        KeyValuePair<TKey, TValue>[] ArrayInstance = new KeyValuePair<TKey, TValue>[1];
        AsCollection.CopyTo(ArrayInstance, 0);
        Assert.AreEqual(ArrayInstance[0], Entry);

        Assert.AreEqual(1, AsDictionary.Count);
        AsCollection.Remove(Entry);
        Assert.AreEqual(0, AsDictionary.Count);

        bool IsReadOnly = AsCollection.IsReadOnly;
        Assert.IsFalse(IsReadOnly);

        NewInstance.Add(NeutralKey, NeutralValue);

        IEnumerable<KeyValuePair<TKey, TValue>> AsEnumerable = NewInstance;

        foreach (KeyValuePair<TKey, TValue> CollectionEntry in AsEnumerable)
        {
            Assert.AreEqual(CollectionEntry, Entry);
        }

        IReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary = NewInstance;
        bool HasContent;

        Assert.AreEqual(Entry.Value, AsReadOnlyDictionary[Entry.Key]);

        IEnumerable<TKey> EnumerableKeys = AsReadOnlyDictionary.Keys;

        HasContent = false;
        foreach (TKey Key in EnumerableKeys)
        {
            Assert.AreEqual(NeutralKey, Key);
            HasContent = true;
        }
        Assert.IsTrue(HasContent);

        IEnumerable<TValue> EnumerableValues = AsReadOnlyDictionary.Values;

        HasContent = false;
        foreach (TValue Value in EnumerableValues)
        {
            Assert.AreEqual(NeutralValue, Value);
            HasContent = true;
        }
        Assert.IsTrue(HasContent);

        IsValueFound = AsReadOnlyDictionary.ContainsKey(NeutralKey);
        Assert.IsTrue(IsValueFound);
        
        IsValueFound = AsReadOnlyDictionary.TryGetValue(NeutralKey, out _);
        Assert.IsTrue(IsValueFound);
    }

    private void TestReadOnlyInterfaces()
    {
        T NewInstance = new();
        NewInstance.Add(NeutralKey, NeutralValue);

        IDictionary<TKey, TValue> AsDictionary = NewInstance;
        IEnumerator<KeyValuePair<TKey, TValue>> Enumerator = NewInstance.GetEnumerator();
        Enumerator.MoveNext();
        KeyValuePair<TKey, TValue> Entry = Enumerator.Current;

        TO ReadOnlyInstance = ToReadOnlyHandler(NewInstance);

        ICollection<KeyValuePair<TKey, TValue>> AsCollection = ReadOnlyInstance;

        bool ContainsItem = AsCollection.Contains(Entry);
        Assert.IsTrue(ContainsItem);

        KeyValuePair<TKey, TValue>[] ArrayInstance = new KeyValuePair<TKey, TValue>[1];
        AsCollection.CopyTo(ArrayInstance, 0);
        Assert.AreEqual(ArrayInstance[0], Entry);

        bool IsReadOnly = AsCollection.IsReadOnly;
        Assert.IsTrue(IsReadOnly);

        IEnumerable<KeyValuePair<TKey, TValue>> AsEnumerable = ReadOnlyInstance;

        foreach (KeyValuePair<TKey, TValue> CollectionEntry in AsEnumerable)
        {
            Assert.AreEqual(CollectionEntry, Entry);
        }

        AsDictionary = ReadOnlyInstance;
        Assert.AreEqual(NeutralValue, AsDictionary[NeutralKey]);

        ICollection<TKey> Keys = AsDictionary.Keys;
        Assert.AreEqual(1, Keys.Count);

        ICollection<TValue> Values = AsDictionary.Values;
        Assert.AreEqual(1, Values.Count);

        ContainsItem = AsDictionary.ContainsKey(NeutralKey);
        Assert.IsTrue(ContainsItem);

        bool IsValueFound = AsDictionary.TryGetValue(NeutralKey, out _);
        Assert.IsTrue(IsValueFound);

        IReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary = ReadOnlyInstance;
        bool HasContent;

        Assert.AreEqual(NeutralValue, AsReadOnlyDictionary[NeutralKey]);

        IEnumerable<TKey> EnumerableKeys = AsReadOnlyDictionary.Keys;

        HasContent = false;
        foreach (TKey Key in EnumerableKeys)
        {
            Assert.AreEqual(NeutralKey, Key);
            HasContent = true;
        }
        Assert.IsTrue(HasContent);

        IEnumerable<TValue> EnumerableValues = AsReadOnlyDictionary.Values;

        HasContent = false;
        foreach (TValue Value in EnumerableValues)
        {
            Assert.AreEqual(NeutralValue, Value);
            HasContent = true;
        }
        Assert.IsTrue(HasContent);

        ContainsItem = AsReadOnlyDictionary.ContainsKey(NeutralKey);
        Assert.IsTrue(ContainsItem);

        IsValueFound = AsReadOnlyDictionary.TryGetValue(NeutralKey, out _);
        Assert.IsTrue(IsValueFound);
    }

    private void TestExceptions()
    {
        T NewInstance = new();
        TO ReadOnlyInstance = ToReadOnlyHandler(NewInstance);
        KeyValuePair<TKey, TValue> Entry = new(NeutralKey, NeutralValue);
        ICollection<KeyValuePair<TKey, TValue>> AsCollection = ReadOnlyInstance;

        Assert.Throws<NotSupportedException>(() => { AsCollection.Add(Entry); }, "Collection is read-only.");
        Assert.Throws<NotSupportedException>(() => { AsCollection.Clear(); }, "Collection is read-only.");
        Assert.Throws<NotSupportedException>(() => { AsCollection.Remove(Entry); }, "Collection is read-only.");

        IDictionary<TKey, TValue> AsDictionary = ReadOnlyInstance;

        Assert.Throws<NotSupportedException>(() => { AsDictionary[NeutralKey] = NeutralValue; }, "Collection is read-only.");
        Assert.Throws<NotSupportedException>(() => { AsDictionary.Add(NeutralKey, NeutralValue); }, "Collection is read-only.");
        Assert.Throws<NotSupportedException>(() => { AsDictionary.Remove(NeutralKey); }, "Collection is read-only.");
    }
}
