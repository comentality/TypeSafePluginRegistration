namespace RoslynPoweredPluginRegistration.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    using RoslynPoweredPluginRegistrationLib;

    public class Test
    {
        [Test]
        public void ParseRegistration()
        {
            var reger = new Registrator();

            reger.GetRegistrationInstructions(@"..\..\..\TestPlugin\Plugin.Registration.cs");
        }
    }
}
