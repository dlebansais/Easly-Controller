using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace EaslyController
{
    public static class ControllerTools
    {
        public static void AssertNoOverride(object thisObject, Type callerType, [CallerMemberName] string callerName = "")
        {
            Type thisType = thisObject.GetType();
            Debug.Assert(callerType.IsAssignableFrom(thisType));

            string ThisName = thisType.FullName;
            string ExpectedName = callerType.FullName;

            if ((ThisName.LastIndexOf('.') is int ThisNameIndex) && ThisNameIndex >= 0)
                ThisName = ThisName.Substring(0, ThisNameIndex);
            if ((ExpectedName.LastIndexOf('.') is int ExpectedNameIndex) && ExpectedNameIndex >= 0)
                ExpectedName = ExpectedName.Substring(0, ExpectedNameIndex);

            Debug.Assert(ThisName == ExpectedName);
        }
    }
}
