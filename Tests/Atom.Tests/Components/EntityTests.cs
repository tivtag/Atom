// <copyright file="EntityTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Components.Tests.EntityTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Components.Tests
{
    using Atom.Components.Moles;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="Entity"/> class.
    /// </summary>
    [TestClass]
    [PexClass(typeof(Entity))]
    public sealed partial class EntityTests
    {
        [TestMethod]
        public void Construction_WithNull_EntityComponentCollection_Throw()
        {
            CustomAssert.Throws<System.ArgumentNullException>(
               () => {
                   new Entity( null );
               }
            );
        }

        [TestMethod]
        public void Construction_WithNonNull_EntityComponentCollection_DoesNotThrow()
        {
            CustomAssert.DoesNotThrow( () => {
                new Entity( new SIEntityComponentCollection() );
            });
        }

        [PexMethod]
        public void Update_Calls_Update_OnComponents( int timesToCallUpdate )
        {
            // Assume
            PexAssume.IsTrue( timesToCallUpdate >= 0 );
            PexSymbolicValue.Minimize( timesToCallUpdate );

            // Arrange
            var entity = new Entity();
            entity.Components.Add( new MasterComponent() );
            entity.Components.Add( new SubComponent() );

            var sub    = entity.Components.Get<SubComponent>();
            var master = entity.Components.Get<MasterComponent>();

            // Pre - Assert
            Assert.AreEqual( 0, sub.UpdateCallCount );
            Assert.AreEqual( 0, master.UpdateCallCount );
            Assert.AreEqual( 0, sub.PreUpdateCallCount );
            Assert.AreEqual( 0, master.PreUpdateCallCount );
                        
            // Act
            for( int i = 0; i < timesToCallUpdate; ++i )
            {
                entity.Update( null );
            }

            // Assert
            Assert.AreEqual( 0, sub.PreUpdateCallCount );
            Assert.AreEqual( 0, master.PreUpdateCallCount );
            Assert.AreEqual( timesToCallUpdate, sub.UpdateCallCount );
            Assert.AreEqual( timesToCallUpdate, master.UpdateCallCount );
        }

        [PexMethod]
        public void PreUpdate_Calls_PreUpdate_OnComponents( int timesToCallPreUpdate )
        {
            // Assume
            PexAssume.IsTrue( timesToCallPreUpdate >= 0 );
            PexSymbolicValue.Minimize( timesToCallPreUpdate );

            // Arrange
            var entity = new Entity();
            entity.Components.Add( new MasterComponent() );
            entity.Components.Add( new SubComponent() );

            var sub    = entity.Components.Get<SubComponent>();
            var master = entity.Components.Get<MasterComponent>();

            // Pre - Assert
            Assert.AreEqual( 0, sub.UpdateCallCount );
            Assert.AreEqual( 0, master.UpdateCallCount );
            Assert.AreEqual( 0, sub.PreUpdateCallCount );
            Assert.AreEqual( 0, master.PreUpdateCallCount );

            // Act
            for( int i = 0; i < timesToCallPreUpdate; ++i )
            {
                entity.PreUpdate( null );
            }

            // Assert
            Assert.AreEqual( 0, sub.UpdateCallCount );
            Assert.AreEqual( 0, master.UpdateCallCount );
            Assert.AreEqual( timesToCallPreUpdate, sub.PreUpdateCallCount );
            Assert.AreEqual( timesToCallPreUpdate, master.PreUpdateCallCount );
        }
    }
}
