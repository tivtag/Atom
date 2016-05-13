// <copyright file="ObjectProviderExtensionsTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Patterns.Provider.Tests.ObjectProviderExtensionsTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Patterns.Provider.Tests
{
    using System;
    using Atom.Patterns.Provider.Moles;
    using Atom.Moles;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="ObjectProviderExtensions"/> class.
    /// </summary>
    [TestClass]
    [PexClass( typeof( ObjectProviderExtensions ) )]
    public sealed partial class ObjectProviderExtensionsTests
    {
        [TestMethod]
        public void GetObjectProvider_WhenContainerTryGetObjectProviderReturnsNull_Throws()
        {
            // Arrange
            var container = new SIObjectProviderContainer() {
                TryGetObjectProviderType = type => null
            };

            // Act & Assert
            CustomAssert.Throws<ServiceNotFoundException>(
                () => {
                    ObjectProviderExtensions.GetObjectProvider<string>( container );
                }
            );
        }

        [TestMethod]
        public void TryGetObjectProvider_WhenContainerTryGetObjectProviderReturnsNull_ReturnsNull()
        {
            // Arrange
            var container = new SIObjectProviderContainer() {
                TryGetObjectProviderType = type => null
            };

            // Act
            var provider = ObjectProviderExtensions.TryGetObjectProvider<string>( container );

            // Assert
            Assert.IsNull( provider );
        }


        [TestMethod]
        public void Resolve_WhenContainerTryGetObjectProviderReturnsNull_Throws()
        {
            // Arrange
            var container = new SIObjectProviderContainer() {
                TryGetObjectProviderType = type => null
            };

            // Act & Assert
            CustomAssert.Throws<ServiceNotFoundException>(
                () => {
                    ObjectProviderExtensions.Resolve<string>( container );
                }
            );
        }

        [TestMethod]
        public void Resolve_WhenObjectProviderReturnsNull_Throws()
        {
            // Arrange
            var container = new SIObjectProviderContainer() {
                TryGetObjectProviderType = type => new LambdaObjectProvider<string>( () => null )
            };

            // Act & Assert
            CustomAssert.Throws<ServiceNotFoundException>(
                () => {
                    ObjectProviderExtensions.Resolve<string>( container );
                }
            );
        }

        [TestMethod]
        public void TryResolve_WhenContainerTryGetObjectProviderReturnsNull_ReturnsNull()
        {
            // Arrange
            var container = new SIObjectProviderContainer() {
                TryGetObjectProviderType = type => null
            };

            // Act
            var provider = ObjectProviderExtensions.TryResolve<string>( container );

            // Assert
            Assert.IsNull( provider );
        }

        [TestMethod]
        public void TryResolve_WithKnownType_ReturnsExpectedObject()
        {
            const string Object = "abc";

            // Arrange
            var container = new SIObjectProviderContainer() {
                TryGetObjectProviderType = type => new LambdaObjectProvider<string>( () => Object )
            };

            // Act
            var obj = ObjectProviderExtensions.TryResolve<string>( container );

            // Assert
            Assert.AreEqual( Object, obj );
        }
        
        [TestMethod]
        public void Register_WithNullContainerRegistrarAndNonNullLambda_Throws()
        {
            CustomAssert.Throws<System.ArgumentNullException>(
                () => {
                    ObjectProviderExtensions.Register( null, () => string.Empty );
                }
            );
        }

        [TestMethod]
        public void Register_WithNonNullContainerRegistrarAndNullLambda_Throws()
        {
            var container = new SIObjectProviderContainerRegistrar();

            CustomAssert.Throws<System.ArgumentNullException>(
                () => {
                    Func<string> f = null;
                    ObjectProviderExtensions.Register<string>( container, f );
                }
            );
        }

        [TestMethod]
        public void Register_WithNullContainerRegistrarAndNonNullContainerLambda_Throws()
        {
            CustomAssert.Throws<System.ArgumentNullException>(
                () => {
                    ObjectProviderExtensions.Register( null, c => string.Empty );
                }
            );
        }

        [TestMethod]
        public void Register_WithNonNullContainerRegistrarAndNullContainerLambda_Throws()
        {
            var container = new SIObjectProviderContainerRegistrar();

            CustomAssert.Throws<System.ArgumentNullException>(
                () => {
                    Func<IObjectProviderContainer, string> f = null;
                    ObjectProviderExtensions.Register<string>( container, f );
                }
            );
        }

        [TestMethod]
        public void Register_WithLambda_RegistersObjectProvider()
        {
            const string Object = "abc";

            // Arrange
            var container = new ObjectProviderContainer();

            // Act
            container.Register<string>( () => Object );

            // Assert
            Assert.AreEqual( Object, container.Resolve<string>() );
        }

        [TestMethod]
        public void Register_WithContainerLambda_RegistersObjectProvider()
        {
            Type type = typeof( int );
            string ExpectedObject = type.Name;

            // Arrange
            var container = new ObjectProviderContainer();
            container.Register<Type>( () => type );

            // Act
            container.Register<string>( c => c.Resolve<Type>().Name );

            // Assert
            Assert.AreEqual( ExpectedObject, container.Resolve<string>() );
        }
        
        [TestMethod]
        public void Resolve_OnNullObjectProvider_Throws()
        {
            IObjectProvider<string> provider = null;

            CustomAssert.Throws<System.ArgumentNullException>(
                () => {
                    ObjectProviderExtensions.Resolve( provider );
                }
            );
        }

        [TestMethod]
        public void Resolve_OnObjectProvider_ThatReturnsNull_Throws()
        {
            var provider = new LambdaObjectProvider<string>( () => null );

            CustomAssert.Throws<ServiceNotFoundException>(
                () => {
                    ObjectProviderExtensions.Resolve( provider );
                }
            );
        }

        [TestMethod]
        public void Resolve_OnObjectProvider_ReturnsExpectedObject()
        {
            const string Object = "abc";

            // Arrange
            var provider = new LambdaObjectProvider<string>( () => Object );

            // Act
            var actualObject = ObjectProviderExtensions.Resolve( provider );

            // Assert
            Assert.AreEqual( Object, actualObject );
        }
    }
}
