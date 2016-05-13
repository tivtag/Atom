// <copyright file="ServiceProviderExtensionsTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tests.ServiceProviderExtensionsTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Defines the usage of the ServiceProviderExtensions class.
    /// </summary>
    [TestClass]
    public class ServiceProviderExtensionsTests
    {
        [TestMethod]
        public void GetServiceOfT_RequestingKnownService_ReturnsService()
        {
            // Arrange
            var serviceProvider = new System.ComponentModel.Design.ServiceContainer();

            var activator = new TypeActivator();
            serviceProvider.AddService( typeof( ITypeActivator ), activator );

            // Act
            var service = serviceProvider.GetService<ITypeActivator>();

            // Assert
            Assert.IsNotNull( service );
            Assert.AreEqual( activator, service );
        }

        [TestMethod]
        public void GetServiceOfT_RequestingUnknownService_ReturnsNull()
        {
            // Arrange
            var serviceProvider = new System.ComponentModel.Design.ServiceContainer();

            // Act
            var service = serviceProvider.GetService<ITypeActivator>();

            // Assert
            Assert.IsNull( service );
        }
    }
}
