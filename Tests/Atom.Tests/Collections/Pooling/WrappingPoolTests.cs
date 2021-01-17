// <copyright file="PoolTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.Pooling.Tests.PoolTests class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Collections.Pooling.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="WrappingPool{T}"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class WrappingPoolTests
    {
        [TestMethod]
        public void Get_ReturnsPoolNode_WithWrappedObject_ThatHasSamePoolNode()
        {
            // Arrange
            var wrappingPool = new WrappingPool<int>( 
                0, 
                () => {
                    int pooledObject = 0;
                    return new PooledObjectWrapper<int>( pooledObject ); 
                } 
            );
            
            // Act
            var node = wrappingPool.Get();
            var objectWrapper = node.Item;
            
            // Assert
            Assert.AreEqual( node, objectWrapper.PoolNode );
        }
    }
}
