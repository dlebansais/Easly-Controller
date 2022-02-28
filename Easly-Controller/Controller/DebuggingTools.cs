namespace EaslyController;

using System;

internal static class DebuggingTools
{
    private static Random Empty { get; } = new();

    public static ulong GetInitHash()
    {
        if (Rand == Empty)
            Rand = new Random();

        return (ulong)Rand.Next();
    }

    private static Random Rand = Empty;
}
