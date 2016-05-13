// <copyright file="NonNullListTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tests.NonNullListTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Collections.Tests
{
    using System;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="NonNullList"/> class.
    /// </summary>
    [TestClass]
    [PexClass( typeof( NonNullList<> ) )]
    public sealed partial class NonNullListTests
    {
        [TestMethod]
        public void Construction_WithListThatHasNullElements_Throws()
        {
            // Arrange
            var existingList = new string[] { 
                "Moo",
                null,
                "Boo"
            };

            // Act & Assert
            CustomAssert.Throws<ArgumentException>(
                () => new NonNullList<string>( existingList )    
            );
        }

        [TestMethod]
        public void Adding_NullItem_Throws()
        {
            // Arrange
            var list = new NonNullList<string>();

            // Act & Assert
            CustomAssert.Throws<ArgumentException>( 
                () => list.Add( null )
            );
        }

        [TestMethod]
        public void Inserting_NullItem_Throws()
        {
            // Arrange
            var list = new NonNullList<string>();

            // Act & Assert
            CustomAssert.Throws<ArgumentException>(
                () => list.Insert( 0, null )
            );
        }

        [PexMethod]
        public void SetIndex_ToNullItem_Throws( [PexAssumeNotNull]string[] items, int index )
        {
            // Assume
            PexAssume.IsTrue( items.Length > 1 );
            PexAssume.InRange( index, 0, items.Length );
            PexAssume.TrueForAll( items, item => item != null );

            // Arrange
            var list = new NonNullList<string>();
            list.AddRange( items );

            // Act & Assert
            CustomAssert.Throws<ArgumentException>(
                () => list[index] = null
            );
        }
    }
}
