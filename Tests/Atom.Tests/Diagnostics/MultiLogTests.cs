// <copyright file="MultiLogTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Diagnostics.Tests.MultiLogTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Diagnostics.Tests
{
    using System.Collections.Generic;
    using Atom.Collections;
    using Atom.Diagnostics.Moles;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="MultiLog"/> class.
    /// </summary>
    [TestClass]
    public sealed class MultiLogTests
    {
        [TestMethod]
        public void DefaultSeverity_ShouldBe_Info()
        {
            var log = new MultiLog();
            Assert.AreEqual( LogSeverities.Info, log.DefaultSeverity );
        }

        [TestMethod]
        public void WriteOfSubLog_IsNotCalled_WhenMessageFilterReturnsTrue()
        {
            bool writeCalled = false;

            // Arrange
            var stubLog = new SILog();
            stubLog.WriteLogSeveritiesString = (severity, message) => { writeCalled = true; };
            
            var stubFilter = new Filters.Moles.SILogFilter();
            stubFilter.FiltersLogSeveritiesString = ( severity, message ) => { return true; };

            var multiLog = new MultiLog();
            multiLog.MessageFilter = stubFilter;
            multiLog.Add( stubLog );

            // Act
            multiLog.Write( string.Empty );

            // Assert
            Assert.IsFalse( writeCalled );
        }

        [TestMethod]
        public void WriteOfSubLog_IsCalled_WhenMessageFilterReturnsFalse()
        {
            bool writeCalled = false;

            // Arrange
            var stubLog = new SILog();
            stubLog.WriteLogSeveritiesString = (severity, message) => { writeCalled = true; };
            
            var stubFilter = new Filters.Moles.SILogFilter();
            stubFilter.FiltersLogSeveritiesString = ( severity, message ) => { return false; };

            var multiLog = new MultiLog();
            multiLog.MessageFilter = stubFilter;
            multiLog.Add( stubLog );

            // Act
            multiLog.Write( string.Empty );

            // Assert
            Assert.IsTrue( writeCalled );
        }

        [TestMethod]
        public void WriteOfSubLog_IsCalled_WhenMessageFilterIsNull()
        {
            bool writeCalled = false;

            // Arrange
            var stubLog = new SILog();
            stubLog.WriteLogSeveritiesString = (severity, message) => { writeCalled = true; };

            var multiLog = new MultiLog();
            multiLog.Add( stubLog );

            // Act
            multiLog.Write( string.Empty );

            // Assert
            Assert.IsTrue( writeCalled );
        }

        [TestMethod]
        public void WriteLineOfSubLog_IsNotCalled_WhenMessageFilterReturnsTrue()
        {
            bool writeCalled = false;

            // Arrange
            var stubLog = new SILog();
            stubLog.WriteLogSeveritiesString = (severity, message) => { writeCalled = true; };

            var stubFilter = new Filters.Moles.SILogFilter();
            stubFilter.FiltersLogSeveritiesString = ( severity, message ) => { return true; };

            var multiLog = new MultiLog();
            multiLog.MessageFilter = stubFilter;
            multiLog.Add( stubLog );

            // Act
            multiLog.WriteLine( string.Empty );

            // Assert
            Assert.IsFalse( writeCalled );
        }

        [TestMethod]
        public void WriteLineOfSubLog_IsCalled_WhenMessageFilterReturnsFalse()
        {
            bool writeCalled = false;

            // Arrange
            var stubLog = new SILog();
            stubLog.WriteLineLogSeveritiesString = (severity, message) => { writeCalled = true; };

            var stubFilter = new Filters.Moles.SILogFilter();
            stubFilter.FiltersLogSeveritiesString = ( severity, message ) => { return false; };

            var multiLog = new MultiLog();
            multiLog.MessageFilter = stubFilter;
            multiLog.Add( stubLog );

            // Act
            multiLog.WriteLine( string.Empty );

            // Assert
            Assert.IsTrue( writeCalled );
        }

        [TestMethod]
        public void WriteLineOfSubLog_IsCalled_WhenMessageFilterIsNull()
        {
            bool writeCalled = false;

            // Arrange
            var stubLog = new SILog();
            stubLog.WriteLineLogSeveritiesString = (severity, message) => { writeCalled = true; };

            var multiLog = new MultiLog();
            multiLog.Add( stubLog );

            // Act
            multiLog.WriteLine( string.Empty );

            // Assert
            Assert.IsTrue( writeCalled );
        }

        [TestMethod]
        public void Clear_RemovesAllSubLogs()
        {
            // Arrange
            var multiLog = new MultiLog();

            multiLog.Add( new SILog() );
            multiLog.Add( new SILog() );
            multiLog.Add( new SILog() );

            // Act
            multiLog.Clear();

            // Assert
            Assert.AreEqual( 0, multiLog.Count );
        }

        [TestMethod]
        public void Contains_WithEmptyMultiLog_ReturnsFalse()
        {
            // Arrange
            var multiLog = new MultiLog();
            var logToSearch = new SILog();

            // Act
            bool wasRemoved = multiLog.Remove( logToSearch );

            // Assert
            Assert.IsFalse( wasRemoved );
        }

        [TestMethod]
        public void Contains_ExistingLog_ReturnsTrue()
        {
            // Arrange
            var multiLog = new MultiLog();
            var logToSearch = new SILog();
            multiLog.Add( logToSearch );

            // Act
            bool wasRemoved = multiLog.Remove( logToSearch );

            // Assert
            Assert.IsTrue( wasRemoved );
        }

        [TestMethod]
        public void GetEnumerator_ReturnsAllLogs()
        {
            // Arrange
            const int LogCount = 5;
            var multiLog = new MultiLog();
            var logs = new ILog[LogCount];

            for( int i = 0; i < LogCount; ++i )
            {
                logs[i] = new SILog();
                multiLog.Add( logs[i] );
            }

            // Act
            Assert.IsTrue( multiLog.ElementsEqual( logs ) );
        }

        [TestMethod]
        public void Contains_WithKnownLog_ReturnsTrue()
        {
            // Arrange
            var log = new SILog();
            var multiLog = new MultiLog();

            multiLog.Add( log );

            // Act
            bool contains = multiLog.Contains( log );

            // Assert
            Assert.IsTrue( contains );
        }
        
        [TestMethod]
        public void Contains_WithUnknownLog_ReturnsFalse()
        {
            // Arrange
            var log = new SILog();
            var multiLog = new MultiLog();

            // Act
            bool contains = multiLog.Contains( log );

            // Assert
            Assert.IsFalse( contains );
        }

        [TestMethod]
        public void WriteLineNoArgs_CallsNativeWriteLineNoArgs_OfSubLogs()
        {
            int callCount = 0;

            // Arrange
            var log = new SILog() {
                WriteLine = () => {
                    ++callCount;
                }
            };

            var multiLog = new MultiLog();
            multiLog.Add( log );
            multiLog.Add( log );
            multiLog.Add( log );

            // Act
            multiLog.WriteLine();

            // Assert
            Assert.AreEqual( multiLog.Count, callCount );
        }

        [TestMethod]
        public void GetItem_WithValidIndex_ReturnsExpectedLog()
        {
            // Arrange
            var log = new SILog();
            var multiLog = new MultiLog();
            multiLog.Add( log );

            IList<ILog> list = multiLog;
            
            // Assert
            Assert.AreEqual( log, list[0] );
        }

        [TestMethod]
        public void RemoveAt_WithValidIndex_RemovesLog()
        {
            // Arrange
            var log = new SILog();
            var multiLog = new MultiLog();
            multiLog.Add( log );
            multiLog.Add( new SILog() );

            IList<ILog> list = multiLog;

            // Act
            list.RemoveAt( 0 );

            // Assert
            Assert.IsFalse( multiLog.Contains( log ) );
        }

        [TestMethod]
        public void IsReadOnly_ReturnsFalse()
        {
            // Arrange
            var multiLog = new MultiLog();
            IList<ILog> list = multiLog;

            // Assert
            Assert.IsFalse( list.IsReadOnly );
        }

        [TestMethod]
        public void IndexOf_KnownLog_ReturnsExpectedIndex()
        {
            // Arrange
            var log = new SILog();

            var multiLog = new MultiLog();
            multiLog.Add( new SILog() );
            multiLog.Add( log );
            multiLog.Add( new SILog() );

            IList<ILog> list = multiLog;

            // Act
            int index = list.IndexOf( log );

            // Assert
            Assert.AreEqual( 1, index );
        }
    }
}
