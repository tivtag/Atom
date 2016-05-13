// <copyright file="HatTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.Tests.HatTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Collections.Tests
{
    using System;
    using System.Linq;
    using Atom.Math.Moles;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the Heap{} class.
    /// </summary>
    [TestClass]
    [PexClass( typeof( Hat<> ) )]
    public partial class HatTests
    {
        [TestMethod]
        public void Construction_WithNullRandomNumberGenerator_Throws()
        {
            CustomAssert.Throws<ArgumentNullException>( () => {
                new Hat<int>( null );
            } );
        }

        [TestMethod]
        public void Construction_WithNullRandomNumberGeneratorAndCapacity_Throws()
        {
            CustomAssert.Throws<ArgumentNullException>( () => {
                new Hat<int>( null, 0 );
            } );
        }

        [TestMethod]
        public void Count_ByDefault_ReturnsZero()
        {
            // Arrange
            var hat = new Hat<int>( new SIRand() );

            // Assert
            Assert.AreEqual( 0, hat.Count );
        }
        
        [TestMethod]
        public void Count_ByDefaultWithGreaterThanZeroCapacity_ReturnsZero()
        {
            // Arrange
            var hat = new Hat<int>( new SIRand(), 10 );

            // Assert
            Assert.AreEqual( 0, hat.Count );
        }

        [TestMethod]
        public void TotalWeight_ByDefault_ReturnsZero()
        {
            // Arrange
            var hat = new Hat<int>( new SIRand() );

            // Assert
            Assert.AreEqual( 0.0f, hat.TotalWeight );
        }

        [TestMethod]
        public void Get_WithEmptyHat_ReturnsDefault()
        {
            // Arrange
            var rand = new SIRand() {
                RandomSingleGet = () => 0.0f
            };

            var hat = new Hat<string>( rand );
           
            // Act
            var item = hat.Get();

            // Assert
            Assert.IsNull( item );
        }

        [PexMethod( MaxRunsWithoutNewTests = 200 )]
        public void InsertItem_IncreasesTotalWeight( [PexAssumeNotNull]float[] weights )
        {
            // Assume
            PexAssume.IsTrue( weights.Length > 0 );
            PexAssume.AreDistinctValues( weights );
            PexAssume.IsTrue( weights.All( item => item > 0.0f ) );

            // Arrange
            var hat = new Hat<string>( new SIRand() );

            // Act
            foreach( var weight in weights )
        	{
                hat.Insert( weight.ToString(), weight ); 
        	}

            // Assert
            float assumedTotalWeight = weights.Sum();

            PexAssume.AreEqual( weights.Length, hat.Count );
            PexAssume.AreEqual( assumedTotalWeight, hat.TotalWeight, 0.00001f );            
        }
        
        [PexMethod]
        public void Clear_RemovesAllItems( [PexAssumeNotNull]Hat<string> hat )
        {
            // Act
            hat.Clear();

            // Assert
            PexAssume.AreEqual( 0, hat.Count );
            PexAssume.AreEqual( 0.0f, hat.TotalWeight );
        }

        [PexMethod]
        public void Get_WithNonEmptyHat_ReturnsNonNull( [PexAssumeNotNull]Hat<string> hat )
        {
            // Act
            var item = hat.Get();

            // Assert
            PexAssume.IsNotNull( item );
        }

        [PexMethod]
        public void Get_Repeatedly_WithNonEmptyHat_ReturnsNonNull( [PexAssumeNotNull]Hat<string> hat, int repeatCount )
        {
            PexAssume.IsTrue( repeatCount > 1 );

            for( int i = 0; i < repeatCount; ++i )
            {
                Get_WithNonEmptyHat_ReturnsNonNull( hat ); 
            }
        }

        [PexMethod]
        public void Remove_WithEmptyHat_ReturnsFalse( int itemToRemove )
        {
            // Arrange
            var hat = new Hat<int>( new SIRand() );

            // Act
            bool wasRemoved = hat.Remove( itemToRemove );
            
            // Assert
            Assert.IsFalse( wasRemoved );
        }

        [TestMethod]
        public void Remove_SetsOwnerOfHatEntryOfItemToNull()
        {
            // Arrange
            const int ItemToRemove = 1;
            var hat = new Hat<int>( new SIRand() );

            var entry = hat.Insert( ItemToRemove, 0.0f );
            hat.Insert( 2, 0.0f );
            hat.Insert( 3, 0.0f );

            // Act
            hat.Remove( ItemToRemove );

            // Assert
            Assert.IsNull( entry.Owner );
        }

        [TestMethod]
        public void Remove_WithHatThatHasNullItems_IfPassedNull_ReturnsTrue()
        {
            // Arrange
            var hat = new Hat<string>( new SIRand() );
            hat.Insert( "a", 1.0f );
            hat.Insert( null, 1.0f );
            hat.Insert( "b", 1.0f );

            // Act
            bool wasRemoved = hat.Remove( null );

            // Assert
            Assert.IsTrue( wasRemoved );
        }

        [TestMethod]
        public void RemoveEntry_IfPassedNull_ReturnsFalse()
        {
            // Arrange
            var hat = new Hat<int>( new SIRand() );

            // Act
            bool wasRemoved = hat.RemoveEntry( (HatEntry<int>)null );

            // Assert
            Assert.IsFalse( wasRemoved );
        }

        [TestMethod]
        public void RemoveEntry_SetsOwnerOfHatEntryOfItemToNull()
        {
            // Arrange
            var hat = new Hat<int>( new SIRand() );

            hat.Insert( 2, 0.0f );
            hat.Insert( 3, 0.0f );
            var entry = hat.Insert( 1, 0.0f );

            // Act
            hat.RemoveEntry( entry );

            // Assert
            Assert.IsNull( entry.Owner );
        }

        [TestMethod]
        public void RemoveAt_SetsOwnerOfHatEntryOfItemToNull()
        {
            // Arrange
            var hat = new Hat<int>( new SIRand() );

            var entry = hat.Insert( 1, 0.0f );
            hat.Insert( 2, 0.0f );
            hat.Insert( 3, 0.0f );

            // Act
            hat.RemoveAt( 0 );

            // Assert
            Assert.IsNull( entry.Owner );
        }

        [TestMethod]
        public void Get_WithRandomValueReturningZero_ReturnsFirstItem()
        {
            // Arrange
            var hat = new Hat<string>( new SIRand() { RandomSingleGet = () => 0.0f } );

            hat.Insert( "1", 100.0f );
            hat.Insert( "2", 100.0f );
            hat.Insert( "3", 100.0f );
            hat.Insert( "4", 100.0f );
            hat.Insert( "5", 100.0f );

            // Act
            string item = hat.Get();

            // Assert
            Assert.AreEqual( "1", item );
        }

        [TestMethod]
        public void Get_WithRandomValueReturningOne_ReturnsLastItem()
        {
            // Arrange
            var hat = new Hat<string>( new SIRand() { RandomSingleGet = () => 1.0f } );

            hat.Insert( "1", 100.0f );
            hat.Insert( "2", 100.0f );
            hat.Insert( "3", 100.0f );
            hat.Insert( "4", 100.0f );
            hat.Insert( "5", 100.0f );

            // Act
            string item = hat.Get();

            // Assert
            Assert.AreEqual( "5", item );
        }

        [TestMethod]
        public void SetWeightModifier_ModifiesWeightOfEntries_WithSpecifiedId()
        {
            // Arrange
            var hat = new Hat<string>( new SIRand() );

            const int IdA = 100;
            const int IdB = 200;

            var a1 = hat.Insert( "1", 100.0f, IdA );
            var a2 = hat.Insert( "2", 100.0f, IdA );
            var b1 = hat.Insert( "3", 100.0f, IdB );
            var b2 = hat.Insert( "4", 100.0f, IdB );

            // Act
            hat.SetWeightModifier( IdA, 0.0f );

            // Assert
            Assert.AreEqual( 0.0f, a1.Weight );
            Assert.AreEqual( 0.0f, a1.WeightModifier );
            Assert.AreEqual( 100.0f, a1.OriginalWeight );

            Assert.AreEqual( 100.0f, b1.Weight );
            Assert.AreEqual( 1.0f, b1.WeightModifier );
            Assert.AreEqual( 100.0f, b1.OriginalWeight );
            Assert.AreEqual( 200.0f, hat.TotalWeight );
        }

        [TestMethod]
        public void GetEnumeratorOfT_ReturnsAllHatEntries()
        {
            // Arrange
            var hat = new Hat<string>( new SIRand() { RandomSingleGet = () => 0.0f } );
            var entries = new System.Collections.Generic.List<HatEntry<string>>();

            entries.Add( hat.Insert( "1", 100.0f ) );
            entries.Add( hat.Insert( "2", 100.0f ) );
            entries.Add( hat.Insert( "3", 100.0f ) );

            // Act && Assert
            Assert.IsTrue( hat.ElementsEqual( entries ) );
        }

        [TestMethod]
        public void GetEnumerator_ReturnsAllHatEntries()
        {
            // Arrange
            var hat = new Hat<string>( new SIRand() { RandomSingleGet = () => 0.0f } );
            var entries = new System.Collections.Generic.List<HatEntry<string>>();

            entries.Add( hat.Insert( "1", 100.0f ) );
            entries.Add( hat.Insert( "2", 100.0f ) );
            entries.Add( hat.Insert( "3", 100.0f ) );

            System.Collections.IEnumerable enumerable = hat;

            // Act && Assert
            Assert.IsTrue( enumerable.Cast<HatEntry<string>>().ElementsEqual( entries ) );
        }

        [TestMethod]
        public void Remove_ItemWithId_ThatIsKnown_ReturnsTrue()
        {
            // Arrange
            var hat = new Hat<string>( new SIRand() );

            const int IdA = 100;
            const int IdB = 200;
            hat.Insert( "2", 100.0f, IdA );
            hat.Insert( "3", 100.0f, IdB );
            hat.Insert( "1", 100.0f, IdA );
            hat.Insert( "1", 100.0f, IdB );

            // Act
            bool wasRemoved = hat.Remove( "1", IdB );

            // Assert
            Assert.IsTrue( wasRemoved );
        }

        [TestMethod]
        public void Remove_ItemWithId_ThatIsUnknown_ReturnsFalse()
        {
            // Arrange
            var hat = new Hat<string>( new SIRand() );

            const int IdA = 100;
            const int IdB = 200;
            const int IdC = 300;
            hat.Insert( "2", 100.0f, IdA );
            hat.Insert( "3", 100.0f, IdB );
            hat.Insert( "1", 100.0f, IdA );
            hat.Insert( "1", 100.0f, IdB );

            // Act
            bool wasRemoved = hat.Remove( "1", IdC );

            // Assert
            Assert.IsFalse( wasRemoved );
        }
        
        [TestMethod]
        public void Remove_NullItemWithId_ThatIsKnown_ReturnsTrue()
        {
            // Arrange
            var hat = new Hat<string>( new SIRand() );

            const int IdA = 100;
            const int IdB = 200;
            hat.Insert( "2", 100.0f, IdA );
            hat.Insert( "3", 100.0f, IdB );
            hat.Insert( "1", 100.0f, IdA );
            hat.Insert( "1", 100.0f, IdB );
            hat.Insert( null, 100.0f, IdB );

            // Act
            bool wasRemoved = hat.Remove( null, IdB );

            // Assert
            Assert.IsTrue( wasRemoved );
        }

        [TestMethod]
        public void GetItem_ReturnsHatEntryInOrdner()
        {
            // Arrange
            var hat = new Hat<string>( new SIRand() );

            var a = hat.Insert( "1", 100.0f );
            var b = hat.Insert( "2", 100.0f ); ;

            // Act & Assert
            Assert.AreEqual( a, hat[0] );
            Assert.AreEqual( b, hat[1] );
        }

        [TestMethod]
        public void GetEnumerator_ReturnsEnumeratorThatIteratesOverAllEntries()
        {
            // Arrange
            var hat = new Hat<string>( new SIRand() );

            var a = hat.Insert( "1", 100.0f );
            var b = hat.Insert( "2", 100.0f );

            var enumerable = (System.Collections.IEnumerable)hat;

            // Act
            var enumerator = enumerable.GetEnumerator();

            // Assert
            enumerator.MoveNext();
            Assert.AreEqual( a, enumerator.Current );

            enumerator.MoveNext();
            Assert.AreEqual( b, enumerator.Current );
        }
    }
}
