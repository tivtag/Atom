// <copyright file="FakeComponents.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the fake Component classes that are used for testing.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Components.Tests
{
    using System;
    using Atom;

    public abstract class TestComponent : Component
    {
        public event EventHandler UpdateCalled;
        public event EventHandler PreUpdateCalled;

        public override void InitializeBindings()
        {
            this.InitializeBindingsCalled = true;
            base.InitializeBindings();
        }

        public override void Initialize()
        {
            this.InitializeCalled = true;
            base.Initialize();
        }

        public override void Update( IUpdateContext updateContext )
        {
            ++this.UpdateCallCount;
            this.UpdateCalled.Raise( this );

            base.Update( updateContext );
        }

        public override void PreUpdate( IUpdateContext updateContext )
        {
            ++this.PreUpdateCallCount;
            this.PreUpdateCalled.Raise( this );

            base.PreUpdate( updateContext );
        }

        public int UpdateCallCount { get; private set; }
        public int PreUpdateCallCount { get; private set; }
        public bool InitializeCalled { get; private set; }
        public bool InitializeBindingsCalled { get; private set; }
    }

    public class MasterComponent : TestComponent
    {
    }

    public class AnotherComponent : TestComponent
    {
    }

    [System.Serializable] // to have two attributes.
    public class SubComponent : TestComponent
    {
        public override void InitializeBindings()
        {
            base.InitializeBindings();

            master = this.Owner.Components.Get<MasterComponent>();
        }

        MasterComponent master;
    }
}
