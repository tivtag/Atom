// <copyright file="ObjectProviderContainerTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Patterns.Provider.Tests.ObjectProviderContainerTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Patterns.Provider.Tests
{
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="ObjectProviderContainer"/> class.
    /// </summary>
    [TestClass]
    [PexClass( typeof( ObjectProviderContainer ) )]
    public sealed partial class ObjectProviderContainerTests
    {
        [TestMethod]
        public void TryGetObjectProvider_WithKnownType_ReturnsRegisteredProvider()
        {            
            // Arrange
            var container = new ObjectProviderContainer();
            var provider = new DirectObjectProvider<string>( string.Empty );

            container.Register<string>( provider );

            // Act
            var resolvedProvider = container.TryGetObjectProvider( typeof( string ) );

            // Assert
            Assert.AreEqual( provider, resolvedProvider );
        }
        
        [TestMethod]
        public void TryGetObjectProvider_WithUnknownType_ReturnsNull()
        {
            // Arrange
            var container = new ObjectProviderContainer();

            // Act
            var provider = container.TryGetObjectProvider( typeof( string ) );

            // Assert
            Assert.AreEqual( null, provider );
        }

        [TestMethod]
        public void Register_WithNullProvider_Throws()
        {
            var container = new ObjectProviderContainer();

            CustomAssert.Throws<System.ArgumentNullException>(
                () => {
                    container.Register<string>( null );
                }
            );
        }
    }
}
