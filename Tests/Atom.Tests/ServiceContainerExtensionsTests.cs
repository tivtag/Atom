// <copyright file="ServiceContainerExtensionsTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tests.ServiceContainerExtensionsTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tests
{
    using Atom.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="ServiceContainerExtensions"/> class.
    /// </summary>
    [TestClass]
    public sealed class ServiceContainerExtensionsTests
    {
        public void AddService_RegistersServiceUsingTheSpecifiedTypeArgument()
        {
            // Arrange
            var log = new Atom.Diagnostics.Moles.SILog();
            var container = new System.ComponentModel.Design.ServiceContainer();

            // Act
            container.AddService<ILog>( log );

            // Assert
            Assert.IsNull( container.GetService<Atom.Diagnostics.Moles.SILog>() );
            Assert.AreEqual( log, container.GetService<ILog>() );
        }
    }
}
