using EaslyController;
using System;
using System.Collections.Generic;

namespace Coverage;

public class TestDictionary<T, TO, TKey, TValue>
    where T: IDictionary<TKey, TValue>, new()
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

    public void TestToReadOnly()
    {
        T NewInstance = new();
        TO NewInstanceReadOnly = ToReadOnlyHandler(NewInstance);
    }

    public void TestIsEqual()
    {
        T NewInstance = new();
        NewInstance.Add(NeutralKey, NeutralValue);

        TestIsEqual(NewInstance);
        TestIsEqual(ToReadOnlyHandler(NewInstance));
    }

    public void TestIsEqual(object instance)
    {
        if (instance is IEqualComparable AsComparable)
        {
            CompareEqual Comparer = CompareEqual.New(true);
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
}
