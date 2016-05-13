// <copyright file="StringUtilitiesTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tests.StringUtilitiesTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Moles;
    using System.Globalization;
    using System.Linq;
    using Atom.Collections;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="StringUtilities"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class StringUtilitiesTests
    {
        [TestMethod]
        public void Reverse_WithPalindroms_ReturnsSameString()
        {
            var palindroms = new string[] {
                "neozoen",
                "lagerregal",
                "otto",
                "reittier",
                "reliefpfeiler",
                "rentner",
                "rotor"
            };

            foreach( var palindrom in palindroms )
            {
                Assert.AreEqual( palindrom, StringUtilities.Reverse( palindrom ) );
            }
        }

        [PexMethod]
        public void IncrementTrailingInteger_WorkAsExpected( [PexAssumeNotNull]string str, int value )
        {
            // Assume
            PexAssume.IsTrue( str.Length > 0 );
            PexAssume.IsFalse( char.IsDigit( str[str.Length-1] ) );

            // Arrange
            str += value.ToString( CultureInfo.InvariantCulture );

            // Act
            string result = StringUtilities.IncrementTrailingInteger( str );

            // Assert
            Assert.IsTrue( result.EndsWith( (value + 1).ToString() ) );
        }

        [PexMethod]
        public void IncrementTrailingInteger_WithStringThatHasNumbers_WorkAsExpected( [PexAssumeNotNull]string str, int value )
        {
            // Assume
            PexAssume.IsTrue( ((IEnumerable<char>)str).Any( ch => char.IsDigit( ch ) ) );

            IncrementTrailingInteger_WorkAsExpected( str, value );
        }

        [PexMethod]
        public void DecrementTrailingInteger_WorkAsExpected( [PexAssumeNotNull]string str, int value )
        {
            // Assume
            PexAssume.IsTrue( value > 0 );
            PexAssume.IsTrue( str.Length > 0 );
            PexAssume.IsFalse( char.IsDigit( str[str.Length-1] ) );

            // Arrange
            str += value.ToString( CultureInfo.InvariantCulture );

            // Act
            string result = StringUtilities.DecrementTrailingInteger( str );

            // Assert
            Assert.IsTrue( result.EndsWith( (value - 1).ToString() ) );
        }

        [PexMethod]
        public void DecrementTrailingInteger_WithStringThatHasNumbers_WorkAsExpected( [PexAssumeNotNull]string str, int value )
        {
            // Assume
            PexAssume.IsTrue( ((IEnumerable<char>)str).Any( ch => char.IsDigit( ch ) ) );

            DecrementTrailingInteger_WorkAsExpected( str, value );
        }

        [PexMethod]
        public void ConvertFromValues_Returns_String_That_Contains_All_Inputs<T>( [PexAssumeNotNull]T[] values )
        {
            // Assume
            PexAssume.IsTrue( values.HasDistinctElements() );

            // Act
            string str = StringUtilities.ConvertFromValues<T>( values );

            // Assert
            foreach( var value in values )
            {
                CustomAssert.Contains( value.ToString(), str );
            }
        }

        [PexMethod]
        public void ConvertToValues_Returns_Array_WithExpected_Values<T>( 
            [PexAssumeNotNull]T[] inputValues )
        {
            // Assume
            PexAssume.IsTrue( inputValues.HasDistinctElements() );

            // Arrange
            string separator = ", ";
            string inputString = inputValues.Aggregate<T, string, string>(
                string.Empty,
                (x, y) => x + separator + y.ToString(),
                (x) => x
            );

            // Act
            T[] values = StringUtilities.ConvertToValues<T>( null, null, inputString, inputValues.Length, string.Empty, separator );

            // Assert
            Assert.IsNotNull( values );
            Assert.IsTrue( values.ElementsEqual( inputValues ) );
        }

        [TestMethod]
        public void ConvertFromValues_Returns_EmptyString_WhenPassed_NoValues()
        {
            // Act
            string str = StringUtilities.ConvertFromValues<int>( null, null, new int[]{} );

            // Assert
            Assert.AreEqual( string.Empty, str );
        }

        [TestMethod]
        public void ConvertFromValues_Throws_WhenPassed_NullInput()
        {
            CustomAssert.Throws<System.ArgumentNullException>(
                () => {
                    StringUtilities.ConvertFromValues<int>( null, CultureInfo.CurrentCulture, null );
                }
            );
        }

        [TestMethod]
        public void ConvertToValues_Throws_WhenPassed_NullInput()
        {
            CustomAssert.Throws<System.ArgumentNullException>(
                () => {
                    StringUtilities.ConvertToValues<int>( null, CultureInfo.CurrentCulture, null, 0, string.Empty );
                }
            );
        }

        [TestMethod]
        public void ConvertToValues_Throws_WhenPassed_InputWithInvalidFormat()
        {
            CustomAssert.Throws<System.ArgumentException>(
                () => {
                    StringUtilities.ConvertToValues<int>( null, CultureInfo.CurrentCulture, "invalid", 1, string.Empty );
                }
            );
        }
        
        [TestMethod]
        public void ConvertToValues_Throws_WhenPassed_InputWith_UnexpectedValueCount()
        {
            CustomAssert.Throws<System.ArgumentException>(
                () => {
                    StringUtilities.ConvertToValues<int>( null, CultureInfo.CurrentCulture, "100", 0, string.Empty );
                }
            );
        }

        [TestMethod]
        public void ConvertFromValues_Throws_WhenPassed_NullValues()
        {
            CustomAssert.Throws<System.ArgumentNullException>(
                () => {
                    StringUtilities.ConvertFromValues<int>( null, null, null );
                }
            );
        }
        
        [PexMethod]
        public void ConvertFromValues_IntegratesWith_ConvertToValues<T>( [PexAssumeNotNull]T[] values )
        {
            // Assume
            PexAssume.IsTrue( values.HasDistinctElements() );

            // Act
            string stringFromValues = StringUtilities.ConvertFromValues<T>( values );
            T[] valuesFromString = StringUtilities.ConvertToValues<T>( stringFromValues, values.Length, string.Empty );

            // Assert
            Assert.IsTrue( values.ElementsEqual( valuesFromString ) );
        }

        [PexMethod]
        public void ConvertFromValues_IntegratesWith_ConvertToValues_AndBackWith_ConvertFromValues<T>( [PexAssumeNotNull]T[] values )
        {        
            // Assume
            PexAssume.IsTrue( values.HasDistinctElements() );

            // Act
            string @string            = StringUtilities.ConvertFromValues<T>( values );
            T[]    valuesFromString   = StringUtilities.ConvertToValues<T>( @string, values.Length, string.Empty );
            string roundTrippedString = StringUtilities.ConvertFromValues<T>( valuesFromString );

            // Assert
            Assert.AreEqual( @string, roundTrippedString );
        }

        [TestMethod]
        [HostType( "Moles" )]
        public void ConvertToValues_RethrowsOutOfMemoryException()
        {
            // Mole
            MTypeDescriptor.GetConverterType = type => {
                return new STypeConverter() {
                    CanConvertFromITypeDescriptorContextType = 
                        (context, objectType) => true,
                    ConvertFromITypeDescriptorContextCultureInfoObject =
                        ( descriptor, context, obj ) => {
                            throw new OutOfMemoryException();
                        }
                };
            };

            // Act & Assert
            CustomAssert.Throws<OutOfMemoryException>( () => {
                StringUtilities.ConvertToValues<int>(
                    null,
                    CultureInfo.InvariantCulture,
                    "1,2",
                    2, 
                    "{x},{y}",
                    ","
                );
            } );
        }

        [TestMethod]
        [HostType( "Moles" )]
        public void ConvertToValues_RethrowsStackOverflowException()
        {
            // Mole
            MTypeDescriptor.GetConverterType = type =>
            {
                return new STypeConverter()
                {
                    CanConvertFromITypeDescriptorContextType = 
                        ( context, objectType ) => true,
                    ConvertFromITypeDescriptorContextCultureInfoObject =
                        (descriptor, context, obj) => {
                            throw new StackOverflowException();
                        }
                };
            };

            // Act & Assert
            CustomAssert.Throws<StackOverflowException>( () => {
                StringUtilities.ConvertToValues<int>(
                    null,
                    CultureInfo.InvariantCulture,
                    "1,2",
                    2,
                    "{x},{y}",
                    ","
                );
            } );
        }

        [TestMethod]
        [HostType( "Moles" )]
        public void ConvertToValues_ThrowsArgumentException_WhenConverterThrowsNonSystemException()
        {
            var exceptionToThrow = new InvalidOperationException();

            // Mole
            MTypeDescriptor.GetConverterType = type => {
                return new STypeConverter() {
                    CanConvertFromITypeDescriptorContextType = 
                        ( context, objectType ) => true,
                    ConvertFromITypeDescriptorContextCultureInfoObject =
                        ( descriptor, context, obj ) => {
                            throw exceptionToThrow;
                        }
                };
            };

            // Act
            var thrownException = CustomAssert.Throws<ArgumentException>( () => {
                StringUtilities.ConvertToValues<int>(
                    null,
                    CultureInfo.InvariantCulture,
                    "1,2",
                    2,
                    "{x},{y}",
                    ","
                );
            } );

            // Assert
            Assert.IsNotNull( thrownException );
            Assert.AreEqual( "input", thrownException.ParamName );
            Assert.AreEqual( exceptionToThrow, thrownException.InnerException );
        }

        [TestMethod]
        public void ConvertToValues_Throws_WhenExpectedValueCountAndActualValueCountDiffer()
        {
            // Act & Assert
            var exception = CustomAssert.Throws<ArgumentException>( () => {
                StringUtilities.ConvertToValues<int>(
                    null,
                    CultureInfo.InvariantCulture,
                    "1,2",
                    3,
                    "{x},{y}",
                    ","
                );
            } );

            Assert.AreEqual( "expectedValueCount", exception.ParamName );
        }
    }
}
