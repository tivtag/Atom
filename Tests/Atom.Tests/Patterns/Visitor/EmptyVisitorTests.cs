// <copyright file="EmptyVisitorTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Patterns.Visitor.Tests.EmptyVisitorTests class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Patterns.Visitor.Tests
{
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="EmptyVisitor{T}"/> class.
    /// </summary>
    [TestClass]
    [PexClass( typeof( EmptyVisitor<> ) )]
    public sealed partial class EmptyVisitorTests
    {
        [PexMethod]
        public void HasCompleted_AlwaysReturnsTrue<T>( [PexAssumeNotNull]EmptyVisitor<T> emptyVisitor )
        {
            Assert.IsTrue( emptyVisitor.HasCompleted );
        }

        [PexMethod]
        public void StaticInstance_IsNotNull<T>()
        {
            Assert.IsNotNull( EmptyVisitor<T>.Instance );
        }

        [TestMethod]
        public void Visit_DoesNotThrow()
        {
            CustomAssert.DoesNotThrow( () => EmptyVisitor<object>.Instance.Visit( null ) );
        }
    }
}
