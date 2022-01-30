namespace EaslyController;

using System;

internal static class DebuggingTools
{
    public static ulong GetInitHash()
    {
        if (Rand == null)
            Rand = new Random();

        return (ulong)Rand.Next();
    }

    private static Random Rand;
}
