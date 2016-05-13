// <copyright file="EntityComponentCollectionTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Components.Tests.EntityComponentCollectionTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Components.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Atom.Components.Moles;

    /// <summary>
    /// Tests the usage of the <see cref="Component"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class EntityComponentCollectionTests
    {
        /// <summary>
        /// Creates a new IEntityComponentCollection instance to be used
        /// by an unit tests.
        /// </summary>
        /// <returns></returns>
        private static IEntityComponentCollection CreateEntityComponentCollection()
        {
            return new FakeEntity().Components;
        }
        
        [TestMethod]
        public void BindingNotificationEnabled_IsTrue_ByDefault()
        {
            // Arrange
            var components = CreateEntityComponentCollection();

            // Assert
            Assert.IsTrue( components.BindingNotificationEnabled );
        }

        [TestMethod]
        public void InitializeBindings_Is_Called_OnNewlyAddedComponents_If_BindingNotificationEnabled()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
            components.BindingNotificationEnabled = true;

            // Act
            components.Add( new AnotherComponent() );

            // Assert    
            Assert.IsTrue( components.Get<AnotherComponent>().InitializeBindingsCalled );
        }

        [TestMethod]
        public void InitializeBindings_Is_NotCalled_OnNewlyAddedComponents_If_BindingNotificationEnabled_IsFalse()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
            components.BindingNotificationEnabled = false;

            // Act
            components.Add( new MasterComponent() );
            components.Add( new SubComponent() );

            // Assert    
            Assert.IsFalse( components.Get<SubComponent>().InitializeBindingsCalled );
            Assert.IsFalse( components.Get<MasterComponent>().InitializeBindingsCalled );
        }

        [TestMethod]
        public void InitializeBindings_Is_Called_After_Reenabling_BindingNotification()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
            components.BindingNotificationEnabled = false;
            components.Add( new MasterComponent() );
            components.Add( new SubComponent() );

            // Pre-Assert
            Assert.IsFalse( components.Get<SubComponent>().InitializeBindingsCalled );
            Assert.IsFalse( components.Get<MasterComponent>().InitializeBindingsCalled );

            // Act
            components.BindingNotificationEnabled = true;

            // Assert    
            Assert.IsTrue( components.Get<SubComponent>().InitializeBindingsCalled );
            Assert.IsTrue( components.Get<MasterComponent>().InitializeBindingsCalled );
        }

        [TestMethod]
        public void IsNot_ReadOnly_ByDefault()
        {
            // Arrange
            var components = CreateEntityComponentCollection();

            // Assert
            Assert.IsFalse( components.IsReadOnly );
        }

        [TestMethod]
        public void Throws_WhenTryingTo_AddComponent_If_ReadOnly()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
            components.IsReadOnly = true;
            
            // Assert
            CustomAssert.Throws<System.InvalidOperationException>(
                () => {
                    components.Add( new SubComponent() );
                }
            );
        }

        [TestMethod]
        public void Throws_WhenTryingTo_RemoveComponent_If_ReadOnly()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
            var component = new MasterComponent();
            components.Add( component );

            // Act
            components.IsReadOnly = true;

            // Assert
            CustomAssert.Throws<System.InvalidOperationException>(
                () => {
                    components.Remove( component );
                }
            );
        }

        [TestMethod]
        public void Contains_Returns_False_OnEmptyCollection()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
            var master = new MasterComponent();
            var sub = new SubComponent();

            // Assert
            Assert.IsFalse( components.Contains( (IComponent)null ) );
            Assert.IsFalse( components.Contains( (Type)null ) );
            Assert.IsFalse( components.Contains( master ) );
            Assert.IsFalse( components.Contains( sub ) );
            Assert.IsFalse( components.Contains<MasterComponent>() );
            Assert.IsFalse( components.Contains<SubComponent>() );
            Assert.IsFalse( components.Contains<TestComponent>() );
            Assert.IsFalse( components.Contains( typeof( MasterComponent ) ) );
            Assert.IsFalse( components.Contains( typeof( SubComponent ) ) );
            Assert.IsFalse( components.Contains( typeof( TestComponent ) ) );
        }

        [TestMethod]
        public void Contains_Returns_True_When_AddedComponent()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
            var master = new MasterComponent();
            var sub = new SubComponent();

            components.Add( master );

            // Act & Assert
            Assert.IsTrue( components.Contains( master ) );
            Assert.IsTrue( components.Contains<MasterComponent>() );
            Assert.IsTrue( components.Contains( typeof( MasterComponent ) ) );
            Assert.IsTrue( components.Contains( typeof( TestComponent ) ) );

            Assert.IsFalse( components.Contains( (IComponent)null ) );
            Assert.IsFalse( components.Contains( (Type)null ) );
            Assert.IsFalse( components.Contains( sub ) );
            Assert.IsFalse( components.Contains<SubComponent>() );
            Assert.IsFalse( components.Contains( typeof( SubComponent ) ) );
        }

        [TestMethod]
        public void Contains_Returns_False_When_AddedComponentWasRemoved()
        {  
            // Arrange
            var components = CreateEntityComponentCollection();
            var master = new MasterComponent();
            var sub = new SubComponent();
            
            components.Add( master );
            components.Remove( master );

            // Act & Assert
            Assert.IsFalse( components.Contains( (IComponent)null ) );
            Assert.IsFalse( components.Contains( (Type)null ) );

            Assert.IsFalse( components.Contains( master ) );
            Assert.IsFalse( components.Contains( sub ) );
            Assert.IsFalse( components.Contains<MasterComponent>() );
            Assert.IsFalse( components.Contains<SubComponent>() );
            Assert.IsFalse( components.Contains<TestComponent>() );
            Assert.IsFalse( components.Contains( typeof( MasterComponent ) ) );
            Assert.IsFalse( components.Contains( typeof( SubComponent ) ) );
            Assert.IsFalse( components.Contains( typeof( TestComponent ) ) );
        }

        [TestMethod]
        public void Adding_NullComponent_Throws()
        {
            // Arrange
            var components = CreateEntityComponentCollection();

            // Assert
            CustomAssert.Throws<System.ArgumentNullException>(
                () => {
                    components.Add( null );
                }
            );
        }

        [TestMethod]
        public void Adding_SameComponentTwice_Throws()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
            components.Add( new MasterComponent() );

            // Act & Assert  
            CustomAssert.Throws<System.ArgumentException>(
                () => {
                    components.Add( new MasterComponent() );
                }
            );
        }

        [TestMethod]
        public void Removing_NullComponent_Throws()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
            components.Add( new MasterComponent() );

            // Act & Assert  
            CustomAssert.Throws<System.ArgumentNullException>(
                () => {
                    components.Remove( null );
                }
            );
        }

        [TestMethod]
        public void Remove_ReturnsFalse_WhenCollectionIsEmpty()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
            var master = new MasterComponent();
            
            // Act & Assert
            Assert.IsFalse( components.Remove( master ) );
            Assert.IsFalse( components.Remove<TestComponent>() );
            Assert.IsFalse( components.Remove<MasterComponent>() );
        }

        [TestMethod]
        public void Remove_RemovesComponent_AsExpected_AndCallsRemoved()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
            var master = new MasterComponent();
            var sub = new SubComponent();
            components.Add( new AnotherComponent() );
            components.Add( master );
            components.Add( sub );

            int removeCount = 0;
            components.Removed += ( sender, c ) => {
                Assert.AreEqual( components, sender );
                Assert.IsTrue( c == sub || c == master );
                ++removeCount;
            };

            // Act
            bool wasRemoved = components.Remove( master );

            // Assert   
            Assert.IsTrue( wasRemoved );
            Assert.AreEqual( 1, removeCount );
            Assert.IsTrue( components.Contains( sub ) );
            Assert.IsFalse( components.Contains( master ) );
        }

        [TestMethod]
        public void RemoveT_RemovesComponent_WhenPassed_ParentTypeInsteadOfConcrete()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
            var master = new MasterComponent();
            components.Add( new SIComponent() { Initialize = () => {}, InitializeBindings = () => {}, OwnerSetIEntity = owner => {} } );
            components.Add( master );

            int removeCount = 0;
            components.Removed += ( sender, c ) => {
                Assert.AreEqual( components, sender );
                Assert.IsTrue( c == master );
                ++removeCount;
            };

            // Act
            bool wasRemoved = components.Remove<TestComponent>();

            // Assert
            Assert.IsTrue( wasRemoved );
            Assert.IsFalse( components.Contains( master ) );
            Assert.AreEqual( 1, removeCount );
        }

        [TestMethod]
        public void RemoveT_RemovesComponent_WhenPassed_ConcreteType()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
            var master = new MasterComponent();
            components.Add( master );

            int removeCount = 0;
            components.Removed += ( sender, c ) => {
                Assert.AreEqual( components, sender );
                Assert.IsTrue( c == master );
                ++removeCount;
            };

            // Act
            bool wasRemoved = components.Remove<MasterComponent>();

            // Assert
            Assert.IsTrue( wasRemoved );
            Assert.IsFalse( components.Contains( master ) );
            Assert.AreEqual( 1, removeCount );
        }

        [TestMethod]
        public void Clear_Removes_All_Components()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
            components.Add( new MasterComponent() );
            components.Add( new SubComponent() );

            // Pre - Assert
            Assert.AreEqual( 2, components.Count );
            Assert.IsTrue( components.Contains<MasterComponent>() );
            Assert.IsTrue( components.Contains<SubComponent>() );
            Assert.IsTrue( components.Contains<TestComponent>() );

            // Act
            components.Clear();

            // Post - Assert
            Assert.AreEqual( 0, components.Count );
            Assert.IsFalse( components.Contains<MasterComponent>() );
            Assert.IsFalse( components.Contains<SubComponent>() );
            Assert.IsFalse( components.Contains<TestComponent>() );
        }

        [TestMethod]
        public void BeginSetup_Disables_BindingNotification()
        {
            // Arrange
            var components = CreateEntityComponentCollection();

            // Act && Assert
            components.BeginSetup();
            {
                Assert.IsFalse( components.BindingNotificationEnabled );
            }
        }

        [TestMethod]
        public void EndSetup_Disables_BindingNotification_AfterBeginSetup()
        {
            // Arrange
            var components = CreateEntityComponentCollection();

            // Act && Assert
            components.BeginSetup();
            {
                Assert.IsFalse( components.BindingNotificationEnabled );
            }
            components.EndSetup();
            Assert.IsTrue( components.BindingNotificationEnabled );
        }

        [TestMethod]
        public void BeginEndSetup_InitializesBindings()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
            var component = new AnotherComponent();

            // Act
            components.BeginSetup();
            {
                components.Add( component );
            }
            components.EndSetup();

            // Assert
            Assert.IsTrue( component.InitializeBindingsCalled );
        }

        [TestMethod]
        public void Find_Returns_Expected_Component()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
         
            var master = new MasterComponent();
            var sub    = new SubComponent();
            components.Add( master );
            components.Add( sub );

            // Pre - Assert
            Assert.AreEqual( sub, components.Find<SubComponent>() );
            Assert.AreEqual( master, components.Find<MasterComponent>() );
            Assert.IsNotNull( components.Find<TestComponent>() );
            Assert.IsNull( components.Find<AnotherComponent>() );

            // Act
            Assert.IsTrue( components.Remove( sub ) );

            // Assert
            Assert.IsNull( components.Find<SubComponent>() );
            Assert.AreEqual( master, components.Find<MasterComponent>() );
            Assert.AreEqual( master, components.Find<TestComponent>() );
            Assert.IsNull( components.Find<AnotherComponent>() );

            // Act
            Assert.IsTrue( components.Remove( master ) );

            //  Assert
            Assert.IsNull( components.Find<SubComponent>() );
            Assert.IsNull( components.Find<MasterComponent>() );
            Assert.IsNull( components.Find<TestComponent>() );
            Assert.IsNull( components.Find<AnotherComponent>() );
        }

        [TestMethod]
        public void Find_Returns_NoComponent_OnEmptyCollection()
        {
            // Arrange
            var components = CreateEntityComponentCollection();

            // Assert   
            Assert.IsNull( components.Find<SubComponent>() );
            Assert.IsNull( components.Find<MasterComponent>() );
            Assert.IsNull( components.Find<TestComponent>() );
            Assert.IsNull( components.Find<AnotherComponent>() );
        }

        [TestMethod]
        public void FindAll_Returns_Expected_Components()
        {
            // Arrange
            var components = CreateEntityComponentCollection();            
            var master = new MasterComponent();
            var sub    = new SubComponent();

            components.Add( master );
            components.Add( sub );

            // Act
            var foundTestComponents = components.FindAll<TestComponent>();
            var foundSubComponents = components.FindAll<SubComponent>();
            var foundMasterComponents = components.FindAll<MasterComponent>();
            var foundAnotherComponents = components.FindAll<AnotherComponent>();

            // Assert
            Assert.AreEqual( 2, foundTestComponents.Count );
            Assert.AreEqual( 1, foundSubComponents.Count );
            Assert.AreEqual( 1, foundMasterComponents.Count );
            Assert.AreEqual( 0, foundAnotherComponents.Count );

            CustomAssert.Contains( sub, foundTestComponents );
            CustomAssert.Contains( master, foundTestComponents );
            CustomAssert.Contains( sub, foundSubComponents );
            CustomAssert.Contains( master, foundMasterComponents );
        }

        [TestMethod]
        public void FindAll_Returns_NoComponents_OnEmptyCollection()
        {
            // Arrange
            var components = CreateEntityComponentCollection();

            // Act & Assert
            Assert.AreEqual( 0, components.FindAll<SubComponent>().Count );
            Assert.AreEqual( 0, components.FindAll<MasterComponent>().Count );
            Assert.AreEqual( 0, components.FindAll<TestComponent>().Count );
            Assert.AreEqual( 0, components.FindAll<AnotherComponent>().Count );
        }

        [TestMethod]
        public void Get_AlwaysReturnsNull_OnEmptyCollection()
        {
            // Arrange
            var components = CreateEntityComponentCollection();

            // Assert
            Assert.IsNull( components.Get<MasterComponent>() );
            Assert.IsNull( components.Get<SubComponent>() );
            Assert.IsNull( components.Get<TestComponent>() );
            Assert.IsNull( components.Get<AnotherComponent>() );
        }

        [TestMethod]
        public void Get_Returns_Expected_Components()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
            var master = new MasterComponent();
            var sub = new SubComponent();

            components.Add( master );
            components.Add( sub );

            // Assert
            Assert.AreEqual( master, components.Get<MasterComponent>() );
            Assert.AreEqual( sub, components.Get<SubComponent>() );
            Assert.IsNull( components.Get<TestComponent>() );
            Assert.IsNull( components.Get<AnotherComponent>() );
        }

        [TestMethod]
        public void Indexer_Returns_Expected_Components()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
            var master = new MasterComponent();
            var sub = new SubComponent();

            components.Add( master );
            components.Add( sub );

            // Assert
            Assert.AreEqual( master, components[typeof( MasterComponent )] );
            Assert.AreEqual( sub, components[typeof( SubComponent )] );
            Assert.IsNull( components[typeof( TestComponent )] );
            Assert.IsNull( components[typeof( AnotherComponent )] );
        }

        [TestMethod]
        public void Get_Returns_Null_AfterClearing_FilledCollection()
        {     
            // Arrange
            var components = CreateEntityComponentCollection();
            components.Add( new SubComponent() );

            // Act
            components.Clear();

            // Arrange
            Assert.IsNull( components.Get<MasterComponent>() );
            Assert.IsNull( components.Get<SubComponent>() );
            Assert.IsNull( components.Get<TestComponent>() );
            Assert.IsNull( components.Get<AnotherComponent>() );
        }

        [TestMethod]
        public void GetEnumerator_Returns_AllComponents()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
            var master = new MasterComponent();
            var sub = new SubComponent();

            components.Add( master );
            components.Add( sub );

            // Assert
            foreach( var component in components )
            {
                Assert.IsTrue( component == master || component == sub );
            }

            foreach( var component in (System.Collections.IEnumerable)components )
            {
                Assert.IsTrue( component == master || component == sub );
            }
        }

        [TestMethod]
        public void AddRange_Throws_WhenPassed_Null()
        {
            // Arrange
            var components = CreateEntityComponentCollection();

            // Assert
            CustomAssert.Throws<ArgumentNullException>(
               () => {
                   components.AddRange( null );
               }
            );
        }

        [TestMethod]
        public void AddRange_Adds_All_Components()
        {
            // Arrange
            var components = CreateEntityComponentCollection();

            // Act
            components.AddRange(
                new IComponent[]{
                    new MasterComponent(),
                    new SubComponent(),
                    new AnotherComponent()
                }
            );

            // Assert
            Assert.IsTrue( components.Contains<SubComponent>() );
            Assert.IsTrue( components.Contains<MasterComponent>() );
            Assert.IsTrue( components.Contains<AnotherComponent>() );
        }

        [TestMethod]
        public void Add_RaisesAddedEvent()
        {
            IComponent componentToAdd = new AnotherComponent();
            IComponent receivedComponent = null;

            // Arrange
            var components = new EntityComponentCollection( new SIEntity(), 0 );
            components.Added += (sender, component) => {
                Assert.AreEqual( components, sender );
                receivedComponent = component;
            };
            
            // Act
            components.Add( componentToAdd );

            // Assert
            Assert.AreEqual( componentToAdd, receivedComponent );
        }

        [TestMethod]
        public void Add_WhenEntityComponentCollectionIsReadOnly_Throws()
        {
            // Arrange
            var components = CreateEntityComponentCollection();
            components.IsReadOnly = true;

            // Act & Assert
            CustomAssert.Throws<InvalidOperationException>( () => {
                components.Add( new SIComponent() );
            } );
        }

        [TestMethod]
        public void Throws_WhenTryingTo_CreateNew_EntityComponentCollection_WithNull_Entity()
        {
            CustomAssert.Throws<ArgumentNullException>(
               () => {
                   new EntityComponentCollection( null, 0 );
               }
            );
        }
    }
}
