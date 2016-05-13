// <copyright file="HashCodeBuilderTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tests.HashCodeBuilderTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="HashCodeBuilder"/> structure.
    /// </summary>
    [TestClass]
    [PexClass( typeof( HashCodeBuilder ))]
    public sealed partial class HashCodeBuilderTests
    {
        [PexMethod]
        public void GetHashCode_AfterAppendingObjects_ReturnsDifferentValue( 
            [PexAssumeNotNull]IEnumerable<object> values )
        {
            // Assume
            PexAssume.IsTrue( values.Count() > 0 );

            // Arrange
            var hashBuilder = new HashCodeBuilder();
            int oldHashCode = hashBuilder.GetHashCode();

            // Act
            foreach( var value in values )
            {
                hashBuilder.Append( value );
            }

            int newHashCode = hashBuilder.GetHashCode();

            // Assert
            Assert.AreNotEqual( oldHashCode, newHashCode );        
        }

        [PexMethod]
        public void GetHashCode_AfterAppendingIntegers_ReturnsDifferentValue( int count )
        {
            // Assume
            PexAssume.IsTrue( count > 0 );

            // Arrange
            var hashBuilder = new HashCodeBuilder();
            int oldHashCode = hashBuilder.GetHashCode();

            // Act
            for( int value = 0; value < count; ++value )
            {
                hashBuilder.AppendStruct( value );
            }

            int newHashCode = hashBuilder.GetHashCode();

            // Assert
            Assert.AreNotEqual( oldHashCode, newHashCode );
        }
    }
}
