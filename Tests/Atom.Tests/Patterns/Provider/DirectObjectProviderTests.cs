// <copyright file="DirectObjectProvider.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Patterns.Provider.Tests.DirectObjectProvider{TObject} class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Patterns.Provider.Tests
{
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="DirectObjectProvider{TObject}"/> class.
    /// </summary>
    [TestClass]
    [PexClass( typeof( DirectObjectProvider<> ) )]
    public sealed partial class DirectProviderTests
    {
        [PexMethod]
        public void TryResolve_ReturnsSpecifiedString( [PexAssumeNotNull]string str )
        {
            // Arrange
            var provider = new DirectObjectProvider<string>( str );

            // Assert
            Assert.AreEqual( str, provider.TryResolve() );
        }

        [TestMethod]
        public void Creation_WithNullObject_Throws()
        {
            CustomAssert.Throws<System.ArgumentNullException>( () => new DirectObjectProvider<string>( null ) );
        }
    }
}
