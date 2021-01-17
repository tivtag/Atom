// <copyright file="EventHandlerExtensionTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tests.EventHandlerExtensionTests class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="EventHandlerExtension"/> class.
    /// </summary>
    [TestClass]
    public sealed class EventHandlerExtensionTests
    {
        [TestMethod]
        public void Raise_Calls_EventHandlers_AsExpected()
        {
            const int BooArgument = 100;
            bool fooWasCalled = false;
            bool zooWasCalled = false;
            bool booWasCalled = false;  

            var testClass = new TestClass();
            testClass.Foo();
            testClass.Zoo();
            testClass.Boo( 100 );
            testClass.Shroom();

            testClass.FooCalled += (sender, e) => {
                Assert.AreEqual( testClass, sender );
                Assert.IsFalse( fooWasCalled );
                fooWasCalled = true;
            };

            testClass.ZooCalled += ( sender, e ) => {
                Assert.AreEqual( testClass, sender );
                Assert.IsFalse( zooWasCalled );
                zooWasCalled = true;
            };

            testClass.BooCalled += (sender, value ) => {
                Assert.AreEqual( testClass, sender );
                Assert.AreEqual( BooArgument, value ); 
                Assert.IsFalse( booWasCalled );
                booWasCalled = true;
            };

            Assert.IsFalse( fooWasCalled );
            Assert.IsFalse( zooWasCalled );
            Assert.IsFalse( booWasCalled );

            testClass.Foo();
            Assert.IsTrue( fooWasCalled );
            Assert.IsFalse( zooWasCalled );
            Assert.IsFalse( booWasCalled );

            testClass.Zoo();
            Assert.IsTrue( fooWasCalled );
            Assert.IsTrue( zooWasCalled );
            Assert.IsFalse( booWasCalled );

            testClass.Boo( BooArgument );
            Assert.IsTrue( fooWasCalled );
            Assert.IsTrue( zooWasCalled );
            Assert.IsTrue( booWasCalled );
        }

        [TestMethod]
        public void Raise_WithNoEventArgs_UsesEmptyEventArgs()
        {
            var testClass = new TestClass();

            testClass.LooCalled += (sender, e) => {
                Assert.AreEqual( testClass, sender );
                Assert.AreEqual( EventArgs.Empty, e );
            };

            testClass.Loo();
        }

        [TestMethod]
        public void Raise_SimpleEventHandler_WorksAsExpected()
        {
            // Arrange
            bool wasCalled = false;
            var testClass = new TestClass();

            testClass.ShroomCalled += ( sender ) => {
                Assert.AreEqual( testClass, sender );
                wasCalled = true;
            };

            // Act
            testClass.Shroom();

            // Assert
            Assert.IsTrue( wasCalled );
        }

        [TestMethod]
        public void Raise_RelaxedEventHandler2_WorksAsExpected()
        {
            // Arrange
            const int Argument = 33;
            bool wasCalled = false;
            var testClass = new TestClass();

            testClass.RoomCalled += (sender, e) => {
                Assert.AreEqual( Argument, e );
                Assert.AreEqual( testClass, sender );
                wasCalled = true;
            };

            // Act
            testClass.Room( Argument );

            // Assert
            Assert.IsTrue( wasCalled );
        }

        private sealed class TestClass
        {
            public event EventHandler FooCalled;
            public event EventHandler LooCalled;
            public event EventHandler<EventArgs> ZooCalled;
            public event RelaxedEventHandler<int> BooCalled;
            public event RelaxedEventHandler<TestClass, int> RoomCalled;
            public event SimpleEventHandler<TestClass> ShroomCalled;

            public void Foo()
            {
                this.FooCalled.Raise( this, EventArgs.Empty );
            }

            public void Zoo()
            {
                this.ZooCalled.Raise( this, EventArgs.Empty );
            }

            public void Boo( int value )
            {
                this.BooCalled.Raise( this, value );
            }

            public void Room( int value )
            {
                this.RoomCalled.Raise( this, value );
            }

            public void Loo()
            {
                this.LooCalled.Raise( this );
            }

            public void Shroom()
            {
                this.ShroomCalled.Raise( this );
            }
        }
    }
}