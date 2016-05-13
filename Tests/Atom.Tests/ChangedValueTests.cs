// <copyright file="ChangedValueTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Tests.ChangedValueTests class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Pex.Framework;

    /// <summary>
    /// Tests the usage of the <see cref="ChangedValue{T}"/> structure.
    /// </summary>
    [TestClass]
    [PexClass( typeof( ChangedValue<> ) )]
    public sealed partial class ChangedValueTests
    {
        [PexMethod]
        public void Properties_Return_ExpectedValues_OnConstruction<T>( T oldValue, T newValue )
        {
            var changedValue = new ChangedValue<T>( oldValue, newValue );

            Assert.AreEqual( oldValue, changedValue.OldValue );
            Assert.AreEqual( newValue, changedValue.NewValue );
        }

        [PexMethod]
        public void AreEqual_When_ElementsAreEqual<T>( T valueA, T valueB )
        {
            // Assume
            PexAssume.AreNotEqual( valueA, valueB );

            // Setup
            var a1 = new ChangedValue<T>( valueA, valueB );
            var a2 = new ChangedValue<T>( valueA, valueB );
            var b = new ChangedValue<T>( valueB, valueA );

            // Assert
            Assert.IsTrue( a1.Equals( a1 ) );
            Assert.IsTrue( a1.Equals( a2 ) );
            Assert.IsTrue( a1.Equals( (object)a1 ) );
            Assert.IsTrue( b.Equals( b ) );
            Assert.IsTrue( b.Equals( (object)b ) );

            Assert.IsFalse( a1.Equals( b ) );
            Assert.IsFalse( b.Equals( a1 ) );
            Assert.IsFalse( a1.Equals( null ) );

            Assert.IsTrue( a2 == a1 );
            Assert.IsTrue( a1 == a2 );
            Assert.IsTrue( a1 != b );
            Assert.IsTrue( b != a1 );
            Assert.IsFalse( a1 == b );
            Assert.IsFalse( b == a1 );
        }

        [PexMethod]
        public void AreEqual_When_ElementsAreEqual_WithStrings( string valueA, string valueB )
        {
            AreEqual_When_ElementsAreEqual( valueA, valueB );
        }

        [PexMethod( MaxConstraintSolverTime=2 )]
        public void GetHashCode_Returns_SameValue_WhenProperties_AreSame<T>( T valueA, T valueB, T valueC, T valueD )
        {
            // Arrange
            var a = new ChangedValue<T>( valueA, valueB );
            var b = new ChangedValue<T>( valueC, valueD );

            // Assert
            PexAssert.Case( a.Equals( b ) ).Implies( () => a.GetHashCode() == b.GetHashCode() );
        }

        [PexMethod]
        public void GetHashCode_Returns_SameValue_WhenProperties_AreSame_WithStrings( string valueA, string valueB, string valueC, string valueD )
        {
            GetHashCode_Returns_SameValue_WhenProperties_AreSame( valueA, valueB, valueC, valueD );
        }

        [PexMethod]
        public void ToString_DoesntThrow_AndAlwaysReturns_NonNull<T>( T oldValue, T newValue )
        {
            // Assume
            PexAssume.AreNotEqual( oldValue, newValue );

            // Setup
            var changedValue = new ChangedValue<T>( oldValue, newValue );

            // Handle
            string str = changedValue.ToString();

            // Assert
            Assert.IsNotNull( str );
            Assert.IsTrue( str.Contains( oldValue != null ? oldValue.ToString() : "null" ) );
            Assert.IsTrue( str.Contains( newValue != null ? newValue.ToString() : "null" ) );
        }
        
        [PexMethod]
        public void ToString_DoesntThrow_AndAlwaysReturns_NonNull_WithStrings( string oldValue, string newValue )
        {
            ToString_DoesntThrow_AndAlwaysReturns_NonNull( oldValue, newValue );
        }
    }
}
