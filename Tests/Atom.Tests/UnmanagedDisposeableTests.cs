// <copyright file="UnmanagedDisposableTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tests.UnmanagedDisposableTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="UnmanagedDisposable"/> class.
    /// </summary>
    [TestClass]
    public sealed class UnmanagedDisposableTests
    {
        [TestMethod]
        public void DisposeUnmanagedResources_IsCalled_WhenObjectIsFiniialized()
        {
            bool disposedManaged = false;
            bool disposedUnmanaged = false;

            // Arrange
            var obj = new TestClass( 
                () => { disposedManaged = true; },
                () => { disposedUnmanaged = true; } 
            );

            // Act
            obj = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Assert
            Assert.IsFalse( disposedManaged );
            Assert.IsTrue( disposedUnmanaged );
        }

        private sealed class TestClass : UnmanagedDisposable
        {
            public TestClass( Action disposeManaged, Action disposeUnmanaged )
            {
                this.disposeManaged = disposeManaged;
                this.disposeUnmanaged = disposeUnmanaged;
            }

            protected override void DisposeManagedResources()
            {
                this.disposeManaged();
            }

            protected override void DisposeUnmanagedResources()
            {
                this.disposeUnmanaged();
            }

            private readonly Action disposeUnmanaged;
            private readonly Action disposeManaged;
        }
    }
}