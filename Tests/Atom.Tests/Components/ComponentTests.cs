// <copyright file="ComponentTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Components.Tests.ComponentTests class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Components.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="Component"/> class.
    /// </summary>
    [TestClass]
    public sealed class ComponentTests
    {
        [TestMethod]
        public void Component_CanBe_EnabledAndDisabled()
        {
            Component component = new MasterComponent();
            Assert.IsTrue( component.IsEnabled );

            component.IsEnabled = false;
            Assert.IsFalse( component.IsEnabled );

            component.IsEnabled = true;
            Assert.IsTrue( component.IsEnabled );
        }
    }
}
