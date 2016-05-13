
// <copyright file="LogSeverityTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Diagnostics.Tests.LogSeverityTests class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Diagnostics.Filters.Tests
{
    using System;
    using Microsoft.Pex.Framework;
    using Microsoft.Pex.Framework.Validation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Asserts the behaviour of the <see cref="SeverityLogFilter"/> class.
    /// </summary>
    [TestClass]
    [PexClass( typeof( SeverityLogFilter ) )]
    public sealed partial class SeverityLogFilterTests
    {
        [TestMethod]
        public void Default_AllowedSeverity_Is_All()
        {
            // Arrange
            var filter = new SeverityLogFilter();

            // Assert
            Assert.IsTrue( filter.Allowed == LogSeverities.All );
        }

        [PexMethod]
        public void Default_Doesnt_Filter( LogSeverities messageSeverity )
        {
            // Assume
            PexAssume.EnumIsDefined<LogSeverities>( messageSeverity );

            // Arrange
            var filter = new SeverityLogFilter();

            // Act
            bool isFiltered = filter.Filters( messageSeverity, string.Empty );

            // Assert
            Assert.IsFalse( isFiltered );
        }

        [PexMethod]
        public void All_AreFiltered_When_FilterSeverityIsNone( LogSeverities messageSeverity )
        {
            // Assume
            PexAssume.EnumIsDefined<LogSeverities>( messageSeverity );

            // Arrange
            var filter = new SeverityLogFilter() {
                Allowed = LogSeverities.None
            };

            // Act
            bool isFiltered = filter.Filters( messageSeverity, string.Empty );

            // Assert
            Assert.IsTrue( isFiltered );
        }

        [PexMethod]
        public void Filters_Expected_Severity( LogSeverities messageSeverity, LogSeverities filterSevarity )
        {
            // Assume
            PexAssume.EnumIsDefined<LogSeverities>( messageSeverity );
            PexAssume.EnumIsDefined<LogSeverities>( filterSevarity );

            // Arrange
            var filter = new SeverityLogFilter() {
                Allowed = filterSevarity
            };

            // Act
            bool isFiltered = filter.Filters( messageSeverity, string.Empty );

            // Assert
            PexAssert
                .Case( isFiltered )
                    .Implies( () => !filterSevarity.Contains( messageSeverity ) )
                .Case( isFiltered == false )
                    .Implies( () => messageSeverity == LogSeverities.None || filterSevarity.Contains( messageSeverity ) )
                .ExpectAtLeastOne();
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) )]
        public void Allowing_Severity_WorksAs_Expected( LogSeverities initialFilterSevarity, LogSeverities sevarityToAllow )
        {
            // Assume
            PexAssume.EnumIsDefined<LogSeverities>( initialFilterSevarity );
            PexAssume.EnumIsDefined<LogSeverities>( sevarityToAllow );

            // Arrange
            var filter = new SeverityLogFilter() {
                Allowed = initialFilterSevarity 
            };

            // Act
            filter.Allow( sevarityToAllow );

            // Assert
            Assert.IsTrue( filter.Allowed.Contains( sevarityToAllow ) );
        }

        [PexMethod]
        public void Disallow_Severity_WorksAs_Expected( LogSeverities initialFilterSevarity, LogSeverities sevarityToDisallow )
        {
            // Assume
            PexAssume.EnumIsDefined<LogSeverities>( initialFilterSevarity );
            PexAssume.EnumIsDefined<LogSeverities>( sevarityToDisallow );

            // Arrange
            var filter = new SeverityLogFilter() {
                Allowed = initialFilterSevarity
            };

            // Act
            filter.Disallow( sevarityToDisallow );

            // Assert
            Assert.IsFalse( filter.Allowed.Contains( sevarityToDisallow ) );
        }

        [PexMethod]
        public void Disallow_Severity_WorksAs_Expected_When_DisablingAlreadyExisting( LogSeverities severity )
        {   
            // Assume
            PexAssume.IsTrue( severity != LogSeverities.None );
            PexAssume.EnumIsDefined<LogSeverities>( severity );

            // Arrange
            var filter = new SeverityLogFilter() {
                Allowed = severity
            };

            // Act
            filter.Disallow( severity );

            // Assert
            Assert.IsFalse( filter.Allowed.Contains( severity ) );
        }
    }
}
