// <copyright file="TestPropertyWrappers.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the test implementations of the BaseObjectPropertyWrapper class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Design.Tests
{
    internal sealed class TestStringPropertyWrapper : BaseObjectPropertyWrapper<string>
    {
        public override IObjectPropertyWrapper Clone()
        {
            return new TestStringPropertyWrapper();
        }
    }

    internal sealed class TestIntegerPropertyWrapper : BaseObjectPropertyWrapper<int>
    {
        public override IObjectPropertyWrapper Clone()
        {
            return new TestIntegerPropertyWrapper();
        }
    }
}
