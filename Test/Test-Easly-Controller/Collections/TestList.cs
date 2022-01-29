using System;
using System.Collections.Generic;

namespace Coverage;

public class TestList<T, TO, TItem>
    where T: IList<TItem>, new()
{
    public TestList(Func<T, TO> toReadOnlyHandler)
    {
        ToReadOnlyHandler = toReadOnlyHandler;
    }

    private Func<T, TO> ToReadOnlyHandler;

    public void TestToReadOnly()
    {
        T NewInstance = new();

        TO To = ToReadOnlyHandler(NewInstance);
    }

    public void Test()
    {
        T NewInstance = new();

        ICollection<TItem> AsCollection = NewInstance;
    }
}
