namespace EaslyController
{
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using NotNullReflection;

    /// <summary>
    /// Tool used to check that objects are created at the same layer than the caller.
    /// </summary>
    public static class ControllerTools
    {
        private static string ExpectedName = string.Empty;

        /// <summary>
        /// Reset the tool (for debug purpose).
        /// </summary>
        public static void ResetExpectedName()
        {
            ExpectedName = string.Empty;
        }

        internal static void AssertNoOverride(object thisObject, Type callerType, [CallerMemberName] string callerName = "")
        {
            Type thisType = Type.FromGetType(thisObject);
            Debug.Assert(callerType.IsAssignableFrom(thisType));

            AssertExpectedName(thisType);
            AssertExpectedName(callerType);
        }

        internal static void AssertExpectedName(Type thisType)
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

                if (ThisName.LastIndexOf('.') is int ThisNameIndex)
                    if (ThisNameIndex >= 0)
                        ThisName = ThisName.Substring(0, ThisNameIndex);

                if (ExpectedName.Length == 0)
                    ExpectedName = ThisName;

                if (ThisName != ExpectedName)
                {
                }
                Debug.Assert(ThisName == ExpectedName);
            }
        }
    }
}
