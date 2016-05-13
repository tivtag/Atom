// <copyright file="HeapTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.Tests.HeapTests class.
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
    /// Tests the usage of the Heap{} class.
    /// </summary>
    [TestClass]
    [PexClass( typeof( Heap<> ) )]
    public partial class HeapTests 
    {
        [PexMethod]
        public void RootGet_WithEmptyHeap_Throws<T>( HeapType heapType )
        {
            // Arrange
            var heap = new Heap<T>( heapType );

            // Act & Assert
            CustomAssert.Throws<InvalidOperationException>(
                () => {
                    var root = heap.Root;
                }
            );
        }

        [PexMethod]
        public void Construction_WithCapacityAndNoSpecificComparer_UsesDefaultComparer<T>( HeapType heapType, int capacity )
        {
            // Assume
            PexAssume.IsTrue( capacity >= 0 );

            // Act
            var heap = new Heap<T>( heapType, capacity );

            // Assert
            if( heapType == HeapType.Minimum )
            {
                Assert.AreEqual( System.Collections.Generic.Comparer<T>.Default, heap.Comparer );
            }
            else
            {
                Assert.AreEqual( 
                    System.Collections.Generic.Comparer<T>.Default,
                    ((Atom.Collections.Comparers.ReverseComparer<T>)heap.Comparer).Comparer
                );
            }
        }

        [PexMethod]
        public void Clear_RemovesAllElements<T>( [PexAssumeNotNull]Heap<T> heap )
        {
            // Act
            heap.Clear();

            // Assert
            Assert.AreEqual( 0, heap.Count );
        }

        [PexMethod]
        public void Contains_NewlyAddedItem<T>( HeapType heapType, T item )
        {
            // Arrange
            var heap = new Heap<T>( heapType );
            heap.Add( item );

            // Act
            bool contains = heap.Contains( item );

            // Assert
            Assert.IsTrue( contains );
        }

        [PexMethod]
        public void Contains_OnEmptyHeap_ReturnsFalse<T>( HeapType heapType, T item )
        {
            // Arrange
            var heap = new Heap<T>( heapType );

            // Act
            bool contains = heap.Contains( item );

            // Assert
            Assert.IsFalse( contains );
        }

        [PexMethod]
        public void Contains_Null_WithHeapWithoutNullEntries_ReturnsFalse( HeapType heapType )
        {
            // Arrange
            var heap = new Heap<string>( heapType );

            // Act
            bool contains = heap.Contains( (string)null );

            // Assert
            Assert.IsFalse( contains );
        }

        [PexMethod]
        public void Contains_Null_WithHeapWithNullEntries_ReturnsTrue( HeapType heapType )
        {
            // Arrange
            var heap = new Heap<string>( heapType );
            heap.Add( null );

            // Act
            bool contains = heap.Contains( (string)null );

            // Assert
            Assert.IsTrue( contains );
        }

        [PexMethod]
        public void Pop_WithNonEmptyHeap_ReturnsOldRoot<T>( [PexAssumeNotNull]Heap<T> heap )
        {
            // Assume
            PexAssume.IsTrue( heap.Count > 0 );

            // Act
            var originalRoot = heap.Root;
                            
            // Act
            var root = heap.Pop();

            // Assert
            Assert.AreEqual( originalRoot, root );
        }

        [PexMethod]
        public void Pop_WithEmptyHeap_Throws<T>( HeapType heapType )
        {  
            // Arrange
            var heap = new Heap<T>( heapType );
            
            // Act & Assert
            CustomAssert.Throws<InvalidOperationException>(
                () => {
                    heap.Pop();
                }
            );
        }

        [PexMethod]
        public void NewlyCreatedHeap_IsEmpty<T>( HeapType heapType )
        {
            // Arrange
            var heap = new Heap<T>( heapType );
         
            // Act
            int count = heap.Count;

            // Assert
            Assert.AreEqual( 0, count ); 
        }

        [PexMethod]
        public void CopyTo_WithValidArguments_ClonesAllElements<T>( [PexAssumeNotNull]Heap<T> heap )
        {
            var targetArray = new T[heap.Count];

            // Act
            heap.CopyTo( targetArray, 0 );
            
            // Assert
            Assert.IsTrue( targetArray.ElementsEqual( heap ) );
        }

        [PexMethod]
        public void IsReadOnly_ReturnsFalse<T>( [PexAssumeNotNull]Heap<T> heap )
        {
            // Arrange
            var collection = (System.Collections.Generic.ICollection<T>)heap;

            // Act
            bool isReadOnly = collection.IsReadOnly;

            // Assert
            Assert.IsFalse( isReadOnly );
        }

        [PexMethod]
        public void Remove_ThrowsNotSupported<T>( [PexAssumeNotNull]Heap<T> heap, T item )
        {
            // Arrange
            var collection = (System.Collections.Generic.ICollection<T>)heap;

            // Act & Assert
            CustomAssert.Throws<NotSupportedException>(
                () => {
                    collection.Remove( item );
                }
            );
        }

        [PexMethod]
        public void GetEnumerator_ContainsAddedItems<T>( HeapType heapType, [PexAssumeNotNull]T[] items )
        {
            // Setup
            var heap = new Heap<T>( heapType );
            
            foreach( var item in items )
            {
                heap.Add( item );
            }

            // Act
            bool containsAllItems = heap.Contains( items );

            // Assert
            Assert.IsTrue( containsAllItems );
        }

        [PexMethod]
        public void GetHeapType_ReturnsExpectedType<T>( HeapType initialHeapType )
        {
            // Setup
            var heap = new Heap<T>( initialHeapType );
            
            // Act
            HeapType heapType = heap.HeapType;

            // Assert
            Assert.AreEqual( initialHeapType, heapType );
        }
    }
}
