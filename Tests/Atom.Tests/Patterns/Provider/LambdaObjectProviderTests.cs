// <copyright file="LambdaObjectProvider.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Patterns.Provider.Tests.LambdaObjectProvider{TObject} class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Patterns.Provider.Tests
{
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Atom.Moles;

    /// <summary>
    /// Tests the usage of the <see cref="LambdaObjectProvider{TObject}"/> class.
    /// </summary>
    [TestClass]
    [PexClass( typeof( LambdaObjectProvider<> ) )]
    public sealed partial class LambdaProviderTests
    {
        [PexMethod]
        public void TryResolve_WithSimpleByPassLambda_ReturnsSpecifiedString( [PexAssumeNotNull]string str )
        {
            // Arrange
            var provider = new LambdaObjectProvider<string>( () => str );

            // Assert
            Assert.AreEqual( str, provider.TryResolve() );
        }

        [TestMethod]
        public void Creation_WithNullLambda_Throws()
        {
            CustomAssert.Throws<System.ArgumentNullException>(
                () => new LambdaObjectProvider<string>( null ) 
            );
        }

        [TestMethod]
        public void Creation_WithNullLambdaAndNonNullContainer_Throws()
        {
            CustomAssert.Throws<System.ArgumentNullException>(
                () => new LambdaObjectProvider<string>( null, new SIObjectProviderContainer() )
            );
        }

        [TestMethod]
        public void Creation_WithNonNullLambdaAndNullContainer_Throws()
        {
            CustomAssert.Throws<System.ArgumentNullException>(
                () => new LambdaObjectProvider<string>( container => string.Empty, null )
            );
        }

        [TestMethod]
        public void TryResolve_PassesContainerToLambda_ToResolveObject()
        {
            // Arrange
            var container = new SIObjectProviderContainer();
            IObjectProviderContainer receivedContainer = null;

            var provider = new LambdaObjectProvider<string>(
                c => {
                    receivedContainer = c;
                    return string.Empty;
                },
                container
            );

            // Act
            provider.TryResolve();

            // Assert
            Assert.AreEqual( container, receivedContainer );
        }
    }
}
