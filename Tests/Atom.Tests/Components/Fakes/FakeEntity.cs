using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atom.Components.Tests
{
    public sealed class FakeEntity : Entity
    {
        public FakeEntity()
        {
        }

        public FakeEntity( IEnumerable<IComponent> components )
        {
            this.Components.BeginSetup();
            {
                foreach( var component in components )
                {
                    this.Components.Add( component );
                }
            }
            this.Components.EndSetup();
        }
    }
}
