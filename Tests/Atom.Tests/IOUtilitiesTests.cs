// <copyright file="IOUtilitiesTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Tests.IOUtilitiesTests class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Tests
{
    using System.IO;
    using Atom.Collections;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    /// <summary>
    /// Tests the usage of the <see cref="IOUtilities"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class IOUtilitiesTests
    {
        [PexMethod]
        public void CopyStream_Copies_TheExact_InputData( [PexAssumeNotNull]byte[] data )
        {
            // Assume
            PexAssume.IsTrue( data.Length < 64 );
            PexAssume.IsTrue( data.HasDistinctElements() );

            // Arrange
            var input = new MemoryStream( data );
            var output = new MemoryStream();
           
            // Act
            IOUtilities.CopyStream( input, output );

            // Assert
            Assert.IsTrue( data.ElementsEqual( output.ToArray() ) );
        }

        [TestMethod]
        public void CopyStream_Throws_WhenPassed_NullInput()
        {
            CustomAssert.Throws<ArgumentNullException>(
                () => IOUtilities.CopyStream( null, new MemoryStream() )
            );
        }
        
        [TestMethod]
        public void CopyStream_Throws_WhenPassed_NullOutput()
        {
            CustomAssert.Throws<ArgumentNullException>(
                () => IOUtilities.CopyStream( new MemoryStream(), null )
            );
        }
    }
}