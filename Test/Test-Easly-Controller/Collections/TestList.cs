using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Coverage;

public class TestList<T, TO, TItem>
    where T: IList<TItem>, IReadOnlyList<TItem>, new()
    where TO: IReadOnlyList<TItem>
{
    public TestList(Func<T, TO> toReadOnlyHandler, TItem neutralItem)
    {
        ToReadOnlyHandler = toReadOnlyHandler;
        NeutralItem = neutralItem;
    }

    private Func<T, TO> ToReadOnlyHandler;
    private TItem NeutralItem;

    public void Test()
    {
        TestToReadOnly();
        TestInterfaces();
        TestReadOnlyInterfaces();
    }

    private void TestToReadOnly()
    {
        T NewInstance = new();
        _ = ToReadOnlyHandler(NewInstance);
    }

    private void TestInterfaces()
    {
        T NewInstance = new();
        NewInstance.Add(NeutralItem);

        IList<TItem> AsList = NewInstance;
        TItem Item = AsList[0];
        AsList[0] = Item;

        Assert.That(AsList.Count == 1);

        int ItemIndex = AsList.IndexOf(Item);
        Assert.AreEqual(0, ItemIndex);

        NewInstance.Clear();
        Assert.That(AsList.Count == 0);

        AsList.Insert(0, Item);
        Assert.That(AsList.Count == 1);

        NewInstance.Clear();
        Assert.That(AsList.Count == 0);

        ICollection<TItem> AsCollection = NewInstance;
        AsCollection.Add(Item);
        Assert.That(AsList.Count == 1);

        bool ContainsItem = AsCollection.Contains(Item);
        Assert.IsTrue(ContainsItem);

        TItem[] ArrayInstance = new TItem[1];
        AsCollection.CopyTo(ArrayInstance, 0);
        Assert.AreEqual(ArrayInstance[0], Item);

        Assert.That(AsList.Count == 1);
        AsCollection.Remove(Item);
        Assert.That(AsList.Count == 0);

        bool IsReadOnly = AsCollection.IsReadOnly;
        Assert.IsFalse(IsReadOnly);

        NewInstance.Add(Item);
        Assert.That(AsList.Count == 1);

        IEnumerable<TItem> AsEnumerable = NewInstance;

        foreach (TItem CollectionItem in AsEnumerable)
        {
            Assert.AreEqual(CollectionItem, Item);
        }

        IReadOnlyList<TItem> AsReadOnlyList = NewInstance;

        TItem Item2 = AsReadOnlyList[0];
        Assert.AreEqual(Item2, Item);
    }

    private void TestReadOnlyInterfaces()
    {
        T NewInstance = new();
        NewInstance.Add(NeutralItem);
        TO ReadOnlyInstance = ToReadOnlyHandler(NewInstance);

        IReadOnlyList<TItem> AsReadOnlyList = ReadOnlyInstance;

        TItem Item = AsReadOnlyList[0];

        foreach (TItem CollectionItem in ReadOnlyInstance)
        {
            Assert.AreEqual(CollectionItem, Item);
        }

        IEnumerable<TItem> AsEnumerable = ReadOnlyInstance;

        foreach (TItem CollectionItem in AsEnumerable)
        {
            Assert.AreEqual(CollectionItem, Item);
        }
    }
}
