namespace RoslynPoweredPluginRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using RoslynPoweredPluginRegistrationLib;

    class Program
    {
        public static void Main(string[] args)
        {
            var registrator = new Registrator();

            var statement = registrator.GetRegistrationInstructions(@"..\..\..\TestPlugin\Registration.cs");

            Console.Write(statement);
        }

        /// <summary>
        /// Register assembly.
        /// </summary>
        private static void Register()
        {
            
        }

        /// <summary>
        /// Check if Registration class is correct.
        /// </summary>
        private static void Check()
        {
            
        }
    }
}
