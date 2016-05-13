// <copyright file="RandExtenionsTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Tests.RandExtenionsTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math.Tests
{
    using System;
    using Atom.Math.Moles;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the RandExtenions class.
    /// </summary>
    [TestClass]
    public partial class RandExtenionsTests
    {
        [TestMethod]
        public void RandomDirection4_WithSpecificRandomValues_ReturnsAssociatedDirection()
        {
            // Expect
            var expectedValues = new [] {
                Tuple.Create( 0.0f, Direction4.None ),
                Tuple.Create( 0.25f, Direction4.Left ),
                Tuple.Create( 0.5f, Direction4.Right ),
                Tuple.Create( 0.75f, Direction4.Up ),
                Tuple.Create( 1.0f, Direction4.Down )
            };

            // Arrange
            int index = 0;
            var randStub = new SIRand();
            randStub.RandomSingleGet = () => expectedValues[index].Item1;

            IRand rand = randStub;
            
            for( index = 0; index < expectedValues.Length; ++index )
            {
                // Act
                Direction4 direction = rand.RandomDirection4();

                // Assert
                Assert.AreEqual( expectedValues[index].Item2, direction );
            }
        }

        [TestMethod]
        public void RandomDirection4But_ReturnsAnyDirection_ButSpecified()
        {
            // Arrange
            var directions = (Direction4[])Enum.GetValues( typeof( Direction4 ) );

            var randStub = new SIRand();

            float randomValue = 0.0f;
            randStub.RandomSingleGet = () => {
                randomValue += 0.1f;

                if( randomValue > 1.0f )
                {
                    randomValue = 0.0f;
                }

                return randomValue;
            };

            IRand rand = randStub;

            // Act
            for( int i = 0; i < directions.Length; ++i )
            {
                Direction4 directionToExclude = directions[i];

                for( int j = 0; j < 10; ++j )
                {
                    Direction4 direction = rand.RandomDirection4But( directionToExclude );

                    // Assert
                    Assert.AreNotEqual( directionToExclude, direction );
                }
            }
        }

        [TestMethod]
        public void RandomActualDirection4But_ReturnsAnyDirection_ButSpecifiedAndNone()
        {
            // Arrange
            var directions = (Direction4[])Enum.GetValues( typeof( Direction4 ) );

            var randStub = new SIRand();

            float randomValue = 0.0f;
            randStub.RandomSingleGet = () =>
            {
                randomValue += 0.1f;

                if( randomValue > 1.0f )
                {
                    randomValue = 0.0f;
                }

                return randomValue;
            };

            IRand rand = randStub;

            // Act
            for( int i = 0; i < directions.Length; ++i )
            {
                Direction4 directionToExclude = directions[i];

                for( int j = 0; j < 10; ++j )
                {
                    Direction4 direction = rand.RandomActualDirection4But( directionToExclude );

                    // Assert
                    Assert.AreNotEqual( Direction4.None, direction );
                    Assert.AreNotEqual( directionToExclude, direction );
                }
            }
        }

        [TestMethod]
        public void RandomDirection8But_ReturnsAnyDirection_ButSpecified()
        {
            // Arrange
            var directions = (Direction8[])Enum.GetValues( typeof( Direction8 ) );

            var randStub = new SIRand();

            float randomValue = 0.0f;
            randStub.RandomSingleGet = () => {
                randomValue += 0.1f;

                if( randomValue > 1.0f )
                {
                    randomValue = 0.0f;
                }

                return randomValue;
            };

            IRand rand = randStub;

            // Act
            for( int i = 0; i < directions.Length; ++i )
            {
                Direction8 directionToExclude = directions[i];

                for( int j = 0; j < 10; ++j )
                {
                    Direction8 direction = rand.RandomDirection8But( directionToExclude );

                    // Assert
                    Assert.AreNotEqual( directionToExclude, direction );
                }
            }
        }

        [TestMethod]
        public void RandomActualDirection8But_ReturnsAnyDirection_ButSpecifiedAndNone()
        {
            // Arrange
            var directions = (Direction8[])Enum.GetValues( typeof( Direction8 ) );

            var randStub = new SIRand();

            float randomValue = 0.0f;
            randStub.RandomSingleGet = () => {
                randomValue += 0.1f;

                if( randomValue > 1.0f )
                {
                    randomValue = 0.0f;
                }

                return randomValue;
            };

            IRand rand = randStub;

            // Act
            for( int i = 0; i < directions.Length; ++i )
            {
                Direction8 directionToExclude = directions[i];

                for( int j = 0; j < 10; ++j )
                {
                    Direction8 direction = rand.RandomActualDirection8But( directionToExclude );

                    // Assert
                    Assert.AreNotEqual( Direction8.None, direction );
                    Assert.AreNotEqual( directionToExclude, direction );
                }
            }
        }
    }
}
