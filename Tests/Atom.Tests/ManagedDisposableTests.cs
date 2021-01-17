// <copyright file="ManagedDisposableTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tests.ManagedDisposableTests class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="ManagedDisposable"/> class.
    /// </summary>
    [TestClass]
    public sealed class ManagedDisposableTests
    {
        [TestMethod]
        public void IsDisposed_ReturnsFalse_ByDefault()
        {
            // Arrange
            var obj = new TestClass();

            // Assert
            Assert.IsFalse( obj.IsDisposed );
        }

        [TestMethod]
        public void IsDisposed_ReturnsTrue_AfterDisposing()
        {
            // Arrange
            var obj = new TestClass();

            // Act
            obj.Dispose();

            // Assert
            Assert.IsTrue( obj.IsDisposed );
        }

        [TestMethod]
        public void Dispose_Calls_DisposeManagedResources()
        {
            // Arrange
            var obj = new TestClass();
            Assert.IsFalse( obj.DisposeManagedCalled );

            // Act
            obj.Dispose();

            // Assert
            Assert.IsTrue( obj.DisposeManagedCalled );
        }

        [TestMethod]
        public void Dispose_Calls_DisposeUnmanagedResources()
        {
            // Arrange
            var obj = new TestClass();
            Assert.IsFalse( obj.DisposeUnmanagedCalled );

            // Act
            obj.Dispose();

            // Assert
            Assert.IsTrue( obj.DisposeUnmanagedCalled );
        }

        [TestMethod]
        public void ThrowsIfDisposed_WithDisposedObject_Throws()
        {
            // Arrange
            var obj = new TestClass();
            obj.Dispose();

            // Act & Assert
            CustomAssert.Throws<ObjectDisposedException>( () => { obj.PublicThrowIfDisposed(); } );
        }

        [TestMethod]
        public void ThrowsIfDisposed_WithNonDisposedObject_DoesntThrow()
        {
            // Arrange
            var obj = new TestClass();

            // Act & Assert
            CustomAssert.DoesNotThrow( () => { obj.PublicThrowIfDisposed(); } );
        }

        private sealed class TestClass : ManagedDisposable
        {
            public bool DisposeManagedCalled
            {
                get;
                private set;
            }

            public bool DisposeUnmanagedCalled
            {
                get;
                private set;
            }

            public void PublicThrowIfDisposed()
            {
                base.ThrowIfDisposed();
            }

            protected override void DisposeManagedResources()
            {
                this.DisposeManagedCalled = true;
                base.DisposeManagedResources();
            }

            protected override void DisposeUnmanagedResources()
            {
                this.DisposeUnmanagedCalled = true;
                base.DisposeUnmanagedResources();
            }
        }
    }
}