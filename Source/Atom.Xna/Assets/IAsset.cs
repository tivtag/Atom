// <copyright file="IAsset.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.IAsset interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    using System.ComponentModel;
    using Atom.Storage;

    /// <summary>
    /// Represents a custom asset that can be used by xna applications.
    /// </summary>
    /// <remarks>
    /// Usually IAssets don't use the xna Content Pipeline for greater
    /// customization.
    /// </remarks>
    [TypeConverter( typeof( ExpandableObjectConverter ) )]
    public interface IAsset : IReadOnlyNameable
    {
    }
}
