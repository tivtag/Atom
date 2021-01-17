// <copyright file="StringUtilitiesTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tests.StringUtilitiesTests class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Tests
{
    using System;
    using System.Globalization;
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
            string[] palindroms = new string[] {
                "neozoen",
                "lagerregal",
                "otto",
                "reittier",
                "reliefpfeiler",
                "rentner",
                "rotor"
            };

            foreach( string palindrom in palindroms )
            {
                Assert.AreEqual( palindrom, StringUtilities.Reverse( palindrom ) );
            }
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

        [TestMethod]
        public void ConvertToValues_Throws_WhenExpectedValueCountAndActualValueCountDiffer()
        {
            // Act & Assert
            ArgumentException exception = CustomAssert.Throws<ArgumentException>( () => {
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
