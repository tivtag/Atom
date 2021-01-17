// <copyright file="GlobalServicesTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tests.GlobalServicesTests class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Tests
{
    using System;
    using Atom.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GlobalServicesTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            this.RecreateServiceContainer();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.RecreateServiceContainer();
        }

        /// <summary>
        /// Recreates the ServiceConotainer of the GlobalServices class.
        /// </summary>
        private void RecreateServiceContainer()
        {
            GlobalServices.Container = new System.ComponentModel.Design.ServiceContainer();
        }

        [TestMethod]
        public void TryLog_WithNoLogProvider_DoesntThrow()
        {
            CustomAssert.DoesNotThrow( () => GlobalServices.TryLog( "" ) );
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void SetContainer_ToNull_Throws()
        {
            GlobalServices.Container = null;
        }

        [TestMethod]
        public void GetContainer_ReturnsNonNull_ByDefault()
        {
            Assert.IsNotNull( GlobalServices.Container );
        }

        [TestMethod]
        public void TryGetService_RequestingKnownService_ReturnsService()
        {
            var activator = new TypeActivator();
            GlobalServices.Container.AddService( typeof( ITypeActivator ), activator );

            // Act
            var service = GlobalServices.TryGetService( typeof( ITypeActivator ) );

            // Assert
            Assert.IsNotNull( service );
            Assert.AreEqual( activator, service );
        }

        [TestMethod]
        public void TryGetService_RequestingUnknownService_ReturnsNull()
        {
            // Act
            var service = GlobalServices.TryGetService( typeof( ITypeActivator ) );

            // Assert
            Assert.IsNull( service );
        }

        [TestMethod]
        public void TryGetServiceOfT_RequestingKnownService_ReturnsService()
        {
            var activator = new TypeActivator();
            GlobalServices.Container.AddService( typeof( ITypeActivator ), activator );

            // Act
            var service = GlobalServices.TryGetService<ITypeActivator>();

            // Assert
            Assert.IsNotNull( service );
            Assert.AreEqual( activator, service );
        }

        [TestMethod]
        public void TryGetServiceOfT_RequestingUnknownService_ReturnsNull()
        {
            // Act
            var service = GlobalServices.TryGetService<ITypeActivator>();

            // Assert
            Assert.IsNull( service );
        }

        [TestMethod]
        public void GetServiceOfT_RequestingKnownService_ReturnsService()
        {
            var activator = new TypeActivator();
            GlobalServices.Container.AddService( typeof( ITypeActivator ), activator );

            // Act
            var service = GlobalServices.GetService<ITypeActivator>();

            // Assert
            Assert.IsNotNull( service );
            Assert.AreEqual( activator, service );
        }

        [TestMethod]
        public void GetServiceOfT_RequestingUnknownService_Throws()
        {
            // Act & Assert
            CustomAssert.Throws<ServiceNotFoundException>( () => {
                var service = GlobalServices.GetService<ITypeActivator>();
            } );
        }

        [TestMethod]
        public void GetServiceOfT_WithErrorMessage_RequestingUnknownService_ThrowsExceptionWithSpecifiedErrorMessage()
        {
            const string ErrorMessage = "Error, oh no!";

            // Act & Assert
            var exception = CustomAssert.Throws<ServiceNotFoundException>( () => {
                var service = GlobalServices.GetService<ITypeActivator>( ErrorMessage );
            } );

            Assert.AreEqual( ErrorMessage, exception.Message );
        }
    }
}
