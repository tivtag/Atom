// <copyright file="PoolTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.Pooling.Tests.PoolTests class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Collections.Pooling.Tests
{
    using System;
    using System.Collections;
    using System.Linq;
    using Microsoft.Pex.Framework;
    using Microsoft.Pex.Framework.Validation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="Pool{T}"/> class.
    /// </summary>
    [TestClass]
    [PexClass( typeof(Pool<>) )]
    public sealed partial class PoolTests
    {
        [PexMethod, PexAllowedException( typeof( ArgumentException ) ) ]
        public void NewPoolHasNoNodes<T>( int initialCapacity )
            where T : new()
        {
            // Assueme
            PexSymbolicValue.Minimize( initialCapacity );

            // Arrange
            var pool = new Pool<T>( initialCapacity, () => { return new T(); } );

            // Assert
            Assert.AreEqual( 0, pool.Capacity );
            Assert.AreEqual( 0, pool.ActiveCount );
            Assert.AreEqual( 0, pool.AvailableCount );
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) )]
        public void Create_ReturnsNewPool_ThatHasInitialSizeAvailableNodes_ButNoActiveNodes<T>( int initialSize )
            where T : new()
        {
            // Assueme
            PexSymbolicValue.Minimize( initialSize );

            // Arrange
            var pool = Pool<T>.Create( initialSize, () => { return new T(); } );

            // Assert
            Assert.AreEqual( 0, pool.ActiveCount );
            Assert.AreEqual( initialSize, pool.AvailableCount );
            Assert.AreEqual( initialSize, pool.Capacity );
        }

        [PexMethod, PexAllowedException( typeof( ArgumentNullException ) )]
        public void CreatingPoolWithNullPooledObjectCreatorThrows<T>()
        {
            new Pool<T>( 2, null );
        }

        [PexMethod]
        public void GettingNodeMakesItActive<T>( [PexAssumeNotNull]Pool<T> pool )
        {
            // Assume
            PexAssume.IsTrue( pool.AvailableCount > 1 );

            // Arrange
            int oldAvailableCount = pool.AvailableCount;

            // Act           
            PoolNode<T> node = pool.Get();

            // Assert
            Assert.IsNotNull( node );
            Assert.IsTrue( node.IsActive );
            Assert.AreEqual( pool, node.Pool );
            Assert.AreEqual( oldAvailableCount - 1, pool.AvailableCount );
        }

        [PexMethod]
        public void GettingAllNodesTurnsAllActiveAndMakesPoolEmpty<T>( [PexAssumeNotNull]Pool<T> pool )
        {
            // Arrange
            pool.IsFixedSize = true;
            int initialSize = pool.AvailableCount;

            // Act
            for( int i = 0; i < pool.Capacity; ++i )
            {
                var node = pool.Get();

                Assert.IsNotNull( node );
                Assert.IsTrue( node.IsActive );
            }

            // Assert
            Assert.AreEqual( initialSize, pool.Capacity );
            Assert.AreEqual( initialSize, pool.ActiveCount );
            Assert.AreEqual( 0, pool.AvailableCount );
        }

        [PexMethod]
        public void ReturningNodeMakesItAvailableAgain<T>( [PexAssumeNotNull]Pool<T> pool )
        {
            // Assume
            PexAssume.IsTrue( pool.AvailableCount > 1 );

            // Arrange
            int initialActiveCount = pool.ActiveCount;
            int initialAvailableCount = pool.AvailableCount;

            var node = pool.Get();
            Assert.IsTrue( node.IsActive );
            Assert.AreEqual( (initialActiveCount + 1), pool.ActiveCount );
            Assert.AreEqual( (initialAvailableCount - 1), pool.AvailableCount );
            
            // Act
            pool.Return( node );

            // Assert
            Assert.IsFalse( node.IsActive );
            Assert.AreEqual( initialActiveCount, pool.ActiveCount );
            Assert.AreEqual( initialAvailableCount, pool.AvailableCount );
        }

        [PexMethod]
        public void DynamicallySizedPoolResizesAutomaticallyWhenRequired<T>( [PexAssumeNotNull]Pool<T> pool )
        {
            // Arrange
            pool.IsFixedSize = false;
            int intialAvailableCount = pool.AvailableCount;
            int intialCapacity = pool.Capacity;
                      
            for( int i = 0; i < intialAvailableCount; ++i )
            {
                var node = pool.Get();
                Assert.IsNotNull( node );
            }
            Assert.AreEqual( 0, pool.AvailableCount );
            
            // Act
            var anotherNodenode = pool.Get();

            // Assert
            Assert.IsNotNull( anotherNodenode );
            Assert.AreEqual( 0, pool.AvailableCount );
            Assert.AreEqual( intialCapacity + 1, pool.Capacity );
        }

        [PexMethod]
        public void FixedSizedPoolDoesntResizesAutomaticallyWhenRequiredButReturnsNull<T>( [PexAssumeNotNull]Pool<T> pool )
        {
            // Arrange
            pool.IsFixedSize = true;
            int intialAvailableCount = pool.AvailableCount;
            int intialCapacity = pool.Capacity;

            for( int i = 0; i < intialAvailableCount; ++i )
            {
                var node = pool.Get();
                Assert.IsNotNull( node );
            }
            Assert.AreEqual( 0, pool.AvailableCount );

            // Act
            var anotherNodenode = pool.Get();

            // Assert
            Assert.IsNull( anotherNodenode );
            Assert.AreEqual( 0, pool.AvailableCount );
            Assert.AreEqual( intialCapacity, pool.Capacity );
        }

        [PexMethod]
        public void ClearingMakesAllNodesAvailableAgain<T>( [PexAssumeNotNull]Pool<T> pool, int nodesToGet )
        {
            // Assume
            PexAssume.IsTrue( nodesToGet >= 0 && nodesToGet < (pool.AvailableCount - 1) );

            // Arrange
            for( int i = 0; i < nodesToGet; ++i )
            {
                pool.Get();
            }

            // Act
            pool.Clear();

            // Assert
            Assert.AreEqual( 0, pool.ActiveCount );
            Assert.AreEqual( pool.Capacity, pool.AvailableCount );
        }

        [PexMethod]
        public void GetEnumeratorIteratesOverActiveNodes<T>( [PexAssumeNotNull]Pool<T> pool, int nodesToGet )
        {
            // Assume
            PexAssume.AreEqual( 0, pool.ActiveCount );
            PexAssume.IsTrue( nodesToGet >= 0 && nodesToGet < pool.AvailableCount );

            // Assert initial state
            Assert.AreEqual( 0, pool.Count() );
            Assert.AreEqual( 0, pool.ActiveNodes.Count() );

            // Arrange
            for( int i = 0; i < nodesToGet; ++i )
            {
                var node = pool.Get();
                Assert.IsNotNull( node );
            }

            // Act & Assert
            int count = 0;
            foreach( var item in pool )
            {
                Assert.IsNotNull( item );
                ++count;
            }

            Assert.AreEqual( nodesToGet, count );

            // Act & Assert
            count = 0;
            foreach( var item in (IEnumerable)pool )
            {
                Assert.IsNotNull( item );
                ++count;
            }

            Assert.AreEqual( nodesToGet, count );

            // Act & Assert
            count = 0;
            foreach( var item in pool.ActiveNodes )
                ++count;
            Assert.AreEqual( nodesToGet, count );
        }

        [TestMethod]
        public void ReturningSameNodeTwiceThrows()
        {
            // Arrange
            var pool = PoolFactory.Create( new int[] { 2,1,0 } );
            var node = pool.Get();

            // Act
            pool.Return( node );
            
            // Assert
            CustomAssert.Throws<InvalidOperationException>( () => {
                pool.Return( node );
            } );
        }

        [TestMethod]
        public void ReturningNodeOfDifferentPoolThrows()
        {
            // Arrange
            var poolA = PoolFactory.Create( new int[]{1,2,0} );
            var poolB = PoolFactory.Create( new int[]{1,2,0} );
            var nodeA = poolA.Get();

            // Assert
            CustomAssert.Throws<ArgumentException>( () => {
                poolB.Return( nodeA );
            } );
        }

        [TestMethod]
        public void ReturningNullNodeThrows()
        {
            // Arrange
            var pool = PoolFactory.Create( new int[] { 0 } );

            // Assert
            CustomAssert.Throws<ArgumentNullException>( () => {
                pool.Return( null );
            } );
        }
    }
}
