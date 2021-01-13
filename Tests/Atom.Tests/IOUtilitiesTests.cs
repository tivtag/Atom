// <copyright file="IOUtilitiesTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Tests.IOUtilitiesTests class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Tests
{
    using System;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="IOUtilities"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class IOUtilitiesTests
    {
        [TestMethod]
        public void CopyStream_Throws_WhenPassed_NullInput()
        {
            CustomAssert.Throws<ArgumentNullException>(
                () => IOUtilities.CopyStream( null, new MemoryStream() )
            );
        }
        
        [TestMethod]
        public void CopyStream_Throws_WhenPassed_NullOutput()
        {
            CustomAssert.Throws<ArgumentNullException>(
                () => IOUtilities.CopyStream( new MemoryStream(), null )
            );
        }
    }
}