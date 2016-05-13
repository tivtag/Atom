// <copyright file="RandRangeExtensionsTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tests.RandRangeExtensionsTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math.Tests
{
    using System.Linq;
    using Atom.Math.Moles;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="RandRangeExtensions"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class RandRangeExtensionsTests
    {
        [PexMethod]
        public void GetRandomRange_ReturnsValueInExpectedRanges( [PexAssumeNotNull]float[] randomValues, bool invertMinMax )
        {
            PexAssume.IsTrue( randomValues.Length > 5 );
            PexAssume.AreDistinctValues( randomValues );
            PexAssume.IsTrue( randomValues.All( randomValue => 0.0f <= randomValue && randomValue <= 1.0f ) );

            foreach( var randomValue in randomValues )
            {
                GetRandomRange_ReturnsValueInExpectedRange( randomValue, invertMinMax );
            }
        }

        [PexMethod]
        public void GetRandomRangeInteger_ReturnsValueInExpectedRanges( [PexAssumeNotNull]float[] randomValues, bool invertMinMax )
        {
            PexAssume.IsTrue( randomValues.Length > 5 );
            PexAssume.AreDistinctValues( randomValues );
            PexAssume.IsTrue( randomValues.All( randomValue => 0.0f <= randomValue && randomValue <= 1.0f ) );

            foreach( var randomValue in randomValues )
            {
                GetRandomRangeInteger_ReturnsValueInExpectedRange( randomValue, invertMinMax );
            }
        }
        
        [PexMethod]
        public void GetRandomRangeInteger_ReturnsValueInExpectedRanges( [PexAssumeNotNull]double[] randomValues, bool invertMinMax )
        {
            PexAssume.IsTrue( randomValues.Length > 5 );
            PexAssume.AreDistinctValues( randomValues );
            PexAssume.IsTrue( randomValues.All( randomValue => 0.0 <= randomValue && randomValue <= 1.0 ) );

            foreach( var randomValue in randomValues )
            {
                GetRandomRangeLong_ReturnsValueInExpectedRange( randomValue, invertMinMax );
            }
        }

        [PexMethod]
        public void GetRandomRangeUnsignedInteger_ReturnsValueInExpectedRanges( [PexAssumeNotNull]double[] randomValues, bool invertMinMax )
        {
            PexAssume.IsTrue( randomValues.Length > 5 );
            PexAssume.AreDistinctValues( randomValues );
            PexAssume.IsTrue( randomValues.All( randomValue => 0.0 <= randomValue && randomValue <= 1.0 ) );

            foreach( var randomValue in randomValues )
            {
                GetRandomRangeUnsignedInteger_ReturnsValueInExpectedRange( randomValue, invertMinMax );
            }
        }

        [PexMethod]
        public void GetRandomRangeDouble_ReturnsValueInExpectedRanges( [PexAssumeNotNull]double[] randomValues, bool invertMinMax )
        {
            PexAssume.IsTrue( randomValues.Length > 5 );
            PexAssume.AreDistinctValues( randomValues );
            PexAssume.IsTrue( randomValues.All( randomValue => 0.0 <= randomValue && randomValue <= 1.0 ) );

            foreach( var randomValue in randomValues )
            {
                GetRandomRangeDouble_ReturnsValueInExpectedRange( randomValue, invertMinMax );
            }
        }

        [PexMethod]
        public void GetRandomRangeDecimal_ReturnsValueInExpectedRanges( bool invertMinMax )
        {
            decimal[] randomValues = new decimal[] { 0.0m, 0.2m, 0.4m, 0.6m, 0.8m, 0.9m, 1.0m };

            foreach( var randomValue in randomValues )
            {
                GetRandomRangeDecimal_ReturnsValueInExpectedRange( randomValue, invertMinMax );
            }
        }      

        private void GetRandomRange_ReturnsValueInExpectedRange( float randomValue, bool invertMinMax )
        {
            // Arrange
            IRand rand = new SIRand() {
                RandomSingleGet = () => randomValue
            };

            float Minimum = 10.0f;
            float Maximum = 100000.0f;
            if( invertMinMax )
                Swap.Them( ref Minimum, ref Maximum );

            // Act
            float value = rand.RandomRange( Minimum, Maximum );

            // Assert 
            if( invertMinMax )
                Swap.Them( ref Minimum, ref Maximum );

            Assert.IsTrue( Minimum <= value && value <= Maximum );
        }

        private void GetRandomRangeInteger_ReturnsValueInExpectedRange( float randomValue, bool invertMinMax )
        {
            // Arrange
            IRand rand = new SIRand() {
                RandomSingleGet = () => randomValue
            };

            int Minimum = 10;
            int Maximum = 100000;
            if( invertMinMax )
                Swap.Them( ref Minimum, ref Maximum );

            // Act
            int value = rand.RandomRange( Minimum, Maximum );

            // Assert
            if( invertMinMax )
                Swap.Them( ref Minimum, ref Maximum );

            Assert.IsTrue( Minimum <= value && value <= Maximum );
        }

        private void GetRandomRangeLong_ReturnsValueInExpectedRange( double randomValue, bool invertMinMax )
        {
            // Arrange
            IRand rand = new SIRand() {
                RandomDoubleGet = () => randomValue
            };

            long Minimum = 0;
            long Maximum = 10000000000;
            if( invertMinMax )
                Swap.Them( ref Minimum, ref Maximum );

            // Act
            long value = rand.RandomRange( Minimum, Maximum );

            // Assert
            if( invertMinMax )
                Swap.Them( ref Minimum, ref Maximum );

            Assert.IsTrue( Minimum <= value && value <= Maximum );
        }

        private void GetRandomRangeUnsignedInteger_ReturnsValueInExpectedRange( double randomValue, bool invertMinMax )
        {
            // Arrange
            IRand rand = new SIRand()
            {
                RandomDoubleGet = () => randomValue
            };

            uint Minimum = 0;
            uint Maximum = 100000000;
            if( invertMinMax )
                Swap.Them( ref Minimum, ref Maximum );

            // Act
            long value = rand.RandomRange( Minimum, Maximum );

            // Assert
            if( invertMinMax )
                Swap.Them( ref Minimum, ref Maximum );

            Assert.IsTrue( Minimum <= value && value <= Maximum );
        }

        private void GetRandomRangeDouble_ReturnsValueInExpectedRange( double randomValue, bool invertMinMax )
        {
            // Arrange
            IRand rand = new SIRand()
            {
                RandomDoubleGet = () => randomValue
            };

            double Minimum = 0.0;
            double Maximum = 10000000000.0;
            if( invertMinMax )
                Swap.Them( ref Minimum, ref Maximum );

            // Act
            double value = rand.RandomRange( Minimum, Maximum );

            // Assert
            if( invertMinMax )
                Swap.Them( ref Minimum, ref Maximum );

            Assert.IsTrue( Minimum <= value && value <= Maximum );
        }

        private void GetRandomRangeDecimal_ReturnsValueInExpectedRange( decimal randomValue, bool invertMinMax )
        {
            // Arrange
            IRand rand = new SIRand() {
                RandomDecimalGet = () => randomValue
            };

            decimal Minimum = 0.0m;
            decimal Maximum = 10000000000.0m;
            if( invertMinMax )
                Swap.Them( ref Minimum, ref Maximum );

            // Act
            decimal value = rand.RandomRange( Minimum, Maximum );

            // Assert
            if( invertMinMax )
                Swap.Them( ref Minimum, ref Maximum );

            Assert.IsTrue( Minimum <= value && value <= Maximum );
        }

        [PexMethod]
        public void GetUnsafeRandomRangeSingle_ReturnsValueInExpectedRanges( [PexAssumeNotNull]float[] randomValues )
        {
            PexAssume.IsTrue( randomValues.Length > 5 );
            PexAssume.AreDistinctValues( randomValues );
            PexAssume.IsTrue( randomValues.All( randomValue => 0.0 <= randomValue && randomValue <= 1.0 ) );

            foreach( var randomValue in randomValues )
            {
                GetUnsafeRandomRangeSingle_ReturnsValueInExpectedRange( randomValue );
            }
        }

        [PexMethod]
        public void GetUnsafeRandomRangeInteger_ReturnsValueInExpectedRanges( [PexAssumeNotNull]float[] randomValues )
        {
            PexAssume.IsTrue( randomValues.Length > 5 );
            PexAssume.AreDistinctValues( randomValues );
            PexAssume.IsTrue( randomValues.All( randomValue => 0.0 <= randomValue && randomValue <= 1.0 ) );

            foreach( var randomValue in randomValues )
            {
                GetUnsafeRandomRangeInteger_ReturnsValueInExpectedRange( randomValue );
            }
        }

        private void GetUnsafeRandomRangeInteger_ReturnsValueInExpectedRange( float randomValue )
        {
            // Arrange
            IRand rand = new SIRand() {
                RandomSingleGet = () => randomValue
            };

            const int Minimum = 0;
            const int Maximum = 10000000;

            // Act
            int value = rand.UncheckedRandomRange( Minimum, Maximum );

            // Assert
            Assert.IsTrue( Minimum <= value && value <= Maximum );
        }

        private void GetUnsafeRandomRangeSingle_ReturnsValueInExpectedRange( float randomValue )
        {
            // Arrange
            IRand rand = new SIRand() {
                RandomSingleGet = () => randomValue
            };

            const float Minimum = 0.0f;
            const float Maximum = 10000000000.0f;

            // Act
            float value = rand.UncheckedRandomRange( Minimum, Maximum );

            // Assert
            Assert.IsTrue( Minimum <= value && value <= Maximum );
        }
    }
}
