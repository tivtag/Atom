// <copyright file="IMutableGrouping.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.IMutableGrouping{Tkey, TElement} interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Collections
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a collection of objects that have a common key.
    /// </summary>
    /// <typeparam name="TKey">
    /// The type of the key of the IMutableGrouping{TKey, TElement}.
    /// </typeparam>
    /// <typeparam name="TElement">
    /// The type of the values in the IMutableGrouping{TKey, TElement}.
    /// </typeparam>
    public interface IMutableGrouping<TKey, TElement> : IGrouping<TKey, TElement>, ICollection<TElement>
    {
    }
}
