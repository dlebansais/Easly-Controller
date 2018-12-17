using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace EaslyController
{
    public static class ControllerTools
    {
        private static string ExpectedName = null;

        public static void ResetExpectedName()
        {
            ExpectedName = null;
        }

        public static void AssertNoOverride(object thisObject, Type callerType, [CallerMemberName] string callerName = "")
        {
            Type thisType = thisObject.GetType();
            Debug.Assert(callerType.IsAssignableFrom(thisType));

            AssertExpectedName(thisType);
        }

        public static void AssertExpectedName(Type thisType)
        {
            if (thisType.IsGenericType && !thisType.IsGenericTypeDefinition)
            {
                Type GenericDefinition = thisType.GetGenericTypeDefinition();
                AssertExpectedName(GenericDefinition);

                Type[] GenericArguments = thisType.GetGenericArguments();
                foreach (Type GenericArgument in GenericArguments)
                    AssertExpectedName(GenericArgument);
            }
            else
            {
                string ThisName = thisType.FullName;

                if ((ThisName.LastIndexOf('.') is int ThisNameIndex) && ThisNameIndex >= 0)
                    ThisName = ThisName.Substring(0, ThisNameIndex);

                if (ExpectedName == null)
                    ExpectedName = ThisName;

                Debug.Assert(ThisName == ExpectedName);
            }
        }
    }
}
