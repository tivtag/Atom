// <copyright file="ListExtensionsTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.Tests.ListExtensionsTests class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Collections.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="ListExtensions"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class ListExtensionsTests
    {
        [PexMethod]
        public void AsReadOnly_ReturnedList_ThrowsNotSupportedException_WhenAdding<T>( [PexAssumeNotNull]IList<T> list )
        {
            // Act
            var readOnlyList = list.AsReadOnly();

            // Assert            
            CustomAssert.Throws<NotSupportedException>(
               () => {
                   readOnlyList.Add( default( T ) );
               }
            );
        }
        
        [PexMethod]
        public void AsReadOnly_ReturnedList_IsReadOnly<T>( [PexAssumeNotNull]IList<T> list )
        {
            // Act
            var readOnlyList = list.AsReadOnly();

            // Assert
            Assert.IsTrue( readOnlyList.IsReadOnly );
        }

        [PexMethod]
        public void AsReadOnly_ReturnedList_HasSameSize_AsOriginalList<T>( [PexAssumeNotNull]IList<T> list )
        {
            // Assume
            PexAssume.IsTrue( list.Count > 0 );

            // Act
            var readOnlyList = list.AsReadOnly();

            // Assert
            Assert.AreEqual( list.Count, readOnlyList.Count );
        }

        [PexMethod]
        public void AsReadOnly_ReturnedList_HasSameElements_AsOriginalList<T>( [PexAssumeNotNull]IList<T> list )
        {
            // Assume
            PexAssume.IsTrue( list.Count > 0 );

            // Act
            var readOnlyList = list.AsReadOnly();

            // Assert
            Assert.IsTrue( list.ElementsEqual( readOnlyList ) );
        }

        [PexMethod]
        public void AsReadOnly_ReturnedListGrows_WhenAddingToOriginalList<T>( [PexAssumeNotNull]List<T> list, T itemToAdd )
        {
            // Assume
            PexAssume.IsFalse( ((IList<T>)list).IsReadOnly );
            PexAssume.IsFalse( list.Contains( itemToAdd ) );

            // Act
            var readOnlyList = list.AsReadOnly();
            list.Add( itemToAdd );

            // Assert
            Assert.IsTrue( readOnlyList.Contains( itemToAdd ) );
        }

        [PexMethod]
        public void SwapItems_InvalidIndex_Throws<T>( [PexAssumeNotNull]IList<T> list, int indexA, int indexB )
        {
            // Assume
            PexAssume.IsTrue( indexA < 0 || indexB < 0 || indexA >= list.Count || indexB >= list.Count );

            // Act & Assert
            CustomAssert.Throws<ArgumentOutOfRangeException>( () => {
                list.SwapItems( indexA, indexB );
            });
        }

        [PexMethod]
        public void SwapItems_ValidIndex_WorksAsExpected<T>( [PexAssumeNotNull]IList<T> list, int indexA, int indexB )
        {
            // Assume
            PexAssume.AreNotEqual( indexA, indexB );
            PexAssume.IsTrue( list.HasDistinctElements() );
            PexAssume.IsTrue( indexA >= 0 && indexB >= 0 && indexA < list.Count && indexB < list.Count );

            // Arrange
            T a = list[indexA];
            T b = list[indexB];

            // Act
            list.SwapItems( indexA, indexB );

            // Assert
            Assert.AreEqual( a, list[indexB] );
            Assert.AreEqual( b, list[indexA] );
        }
    }
}
