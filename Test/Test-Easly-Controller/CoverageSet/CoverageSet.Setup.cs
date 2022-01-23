using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using EaslyController;
using EaslyController.Constants;
using EaslyController.Controller;
using EaslyController.Focus;
using EaslyController.Frame;
using EaslyController.Layout;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using NUnit.Framework;

namespace Coverage
{
    [TestFixture]
    public partial class CoverageSet
    {
        [OneTimeSetUp]
        public static void InitCoverageSession()
        {
            CultureInfo enUS = CultureInfo.CreateSpecificCulture("en-US");
            CultureInfo.DefaultThreadCurrentCulture = enUS;
            CultureInfo.DefaultThreadCurrentUICulture = enUS;
            Thread.CurrentThread.CurrentCulture = enUS;
            Thread.CurrentThread.CurrentUICulture = enUS;

            Assembly EaslyControllerAssembly;

            try
            {
                EaslyControllerAssembly = Assembly.Load("Easly-Controller");
            }
            catch
            {
                EaslyControllerAssembly = null;
            }
            Assume.That(EaslyControllerAssembly != null);

        }
    }
}
