// <copyright file="PooledObjectWrapperTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.Pooling.Tests.PooledObjectWrapperTests class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Collections.Pooling.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using Microsoft.Pex.Framework;

    /// <summary>
    /// Tests the usage of the <see cref="PooledObjectWrapper{T}"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class PooledObjectWrapperTests
    {
        [TestMethod]
        public void Creation_PassedNullObject_ThrowsArgumentNull()
        {
            CustomAssert.Throws<ArgumentNullException>(
                () => {
                    var wrapped = new PooledObjectWrapper<object>( null );
                }
            );
        }

        [PexMethod]
        public void PooledObject_IsSameAsObjectPassedOnConstruction<T>( [PexAssumeNotNull]T obj )
        {
            // Arrange
            var wrapped = new PooledObjectWrapper<T>( obj );

            // Assert
            Assert.AreEqual( obj, wrapped.PooledObject );
        }
    }
}
