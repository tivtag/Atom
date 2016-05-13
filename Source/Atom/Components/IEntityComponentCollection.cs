// <copyright file="IEntityComponentCollection.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Components.IEntityComponentCollection interface.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Components
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Encapsulates the list of <see cref="IComponent"/>s that compose an <see cref="IEntity"/>.
    /// </summary>
    [ContractClass( typeof( IEntityComponentCollectionContracts ) )]
    public interface IEntityComponentCollection : IEnumerable<IComponent>, IUpdateable, IPreUpdateable
    {
        /// <summary>
        /// Raised when an <see cref="IComponent"/> has been added to this IEntityComponentCollection.
        /// </summary>
        event RelaxedEventHandler<IComponent> Added;

        /// <summary>
        /// Raised when an <see cref="IComponent"/> has been removed from this IEntityComponentCollection.
        /// </summary>
        event RelaxedEventHandler<IComponent> Removed;

        /// <summary>
        /// Gets the <see cref="IEntity"/> that owns this IEntityComponentCollection.
        /// </summary>
        /// <value>
        /// The <see cref="IEntity"/> that owns this IEntityComponentCollection.
        /// </value>
        IEntity Owner
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this IEntityComponentCollection is read-only.
        /// </summary>
        /// <value>
        /// This IEntityComponentCollection can't be modified if this property has been set to <see langword="true"/>.
        /// The default value is false.
        /// </value>
        bool IsReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the number of <see cref="IComponent"/>s in this <see cref="IEntityComponentCollection"/>.
        /// </summary>
        /// <value>The number of <see cref="IComponent"/>s in this <see cref="IEntityComponentCollection"/>.</value>
        int Count
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this IEntityComponentCollection
        /// notifies its <see cref="IComponent"/>s by calling <see cref="IComponent.InitializeBindings"/>
        /// when an <see cref="IComponent"/> gets added or removed.
        /// </summary>
        /// <remarks>
        /// It may be useful to temporarily disable notification for perfomance reasons.
        /// For example before adding a punch of <see cref="IComponent"/>s. 
        /// </remarks>
        /// <value>The default value is true.</value>
        bool BindingNotificationEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a <see cref="IComponent"/> by exact <see cref="Type"/> key.
        /// </summary>
        /// <param name="key">The type of component to get.</param>
        /// <returns>
        /// The requested <see cref="IComponent"/> or null if not found.
        /// </returns>
        IComponent this[Type key]
        {
            get;
        }

        /// <summary>
        /// Begins the region in which <see cref="IComponent"/>s are added to this IEntityComponentCollection.
        /// </summary>
        void BeginSetup();

        /// <summary>
        /// Ends the setup process initiated by BeginSetup.
        /// </summary>
        void EndSetup();

        /// <summary>
        /// Tries to get the <see cref="IComponent"/> of the specified Type.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the <see cref="IComponent"/> to get.
        /// </typeparam>
        /// <remarks>
        /// This method searches only for the exact specified type.
        /// This method has a complexity of O(1).
        /// <code>
        /// components.Add( new TheComponent() ); 
        /// Get&lt;TheComponent&gt;;  // This will receive the component.
        /// Get&lt;ITheComponent&gt;; // Try to receive base interface/class - this will FAIL!
        /// </code> 
        /// You must use Find for more complex component searches.
        /// </remarks>
        /// <returns>
        /// The requested <see cref="IComponent"/>; 
        /// or null if there exists no <see cref="IComponent"/> of the specified type <typeparamref name="T"/>.
        /// </returns>
        T Get<T>() where T : class, IComponent;

        /// <summary>
        /// Finds the first occurence of an <see cref="IComponent"/> of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IComponent"/> to find.</typeparam>
        /// <returns>
        /// The found <see cref="IComponent"/>;
        /// or null if no matching <see cref="IComponent"/> could be found in this IEntityComponentCollection.
        /// </returns>
        T Find<T>() where T : class, IComponent;

        /// <summary>
        /// Finds all <see cref="IComponent"/>s that are of type <typeparamref name="T"/> or implement it.
        /// </summary>
        /// <typeparam name="T">
        /// The type of <see cref="IComponent"/>s to find.
        /// </typeparam>
        /// <returns>
        /// A list of <see cref="IComponent"/>s that are of type <typeparamref name="T"/> or implement it.
        /// </returns>
        IList<T> FindAll<T>() where T : class, IComponent;

        /// <summary>
        /// Determines whether this IEntityComponentCollection contains 
        /// a <see cref="IComponent"/> of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of <see cref="IComponent"/>s to find.
        /// </typeparam>
        /// <returns>
        /// Returns <see langword="true"/> if this EntityComponentCollection
        /// contains a <see cref="Component"/> of type <typeparamref name="T"/>;
        /// otherwise <see langword="false"/>.
        /// </returns>
        bool Contains<T>() where T : class, IComponent;

        /// <summary>
        /// Determines whether this IEntityComponentCollection contains 
        /// a <see cref="IComponent"/> of the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="componentType">
        /// The type of component to look for.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if this IEntityComponentCollection contains 
        /// a <see cref="Component"/> of the specified <see cref="Type"/>;
        /// otherwise <see langword="false"/>.
        /// </returns>
        bool Contains( Type componentType );

        /// <summary>
        /// Determines whether this <see cref="IEntityComponentCollection"/> contains the specified <see cref="IComponent"/>.
        /// </summary>
        /// <param name="component">
        /// The <see cref="IComponent"/> to look for.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if this <see cref="IEntityComponentCollection"/> 
        /// contains the specified <see cref="IComponent"/>; otherwise <see langword="false"/>.
        /// </returns>
        bool Contains( IComponent component );

        /// <summary>
        /// Adds the specified <see cref="IComponent"/> to this IEntityComponentCollection.
        /// </summary>
        /// <param name="component">
        /// The <see cref="IComponent"/> to add.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If the <paramref name="component"/> is null
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// If this IEntityComponentCollection is read-only.
        /// </exception>
        void Add( IComponent component );

        /// <summary>
        /// Adds the specified <see cref="IComponent"/>s to this IEntityComponentCollection.
        /// </summary>
        /// <param name="components">
        /// The <see cref="IComponent"/>s to add.
        /// </param>
        void AddRange( IEnumerable<IComponent> components );

        /// <summary>
        /// Removes the first occurence of a <see cref="IComponent"/> of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of <see cref="IComponent"/> to remove.
        /// </typeparam>
        /// <returns>
        /// Returns <see langword="true"/> if the <see cref="Component"/> has been removed;
        /// otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// If this IEntityComponentCollection is read-only.
        /// </exception>
        bool Remove<T>() where T : class, IComponent;

        /// <summary>
        /// Tries to remove the specified <see cref="IComponent"/> from this IEntityComponentCollection.
        /// </summary>
        /// <param name="component">
        /// The <see cref="IComponent"/> to remove.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the <see cref="IComponent"/> has been removed;
        /// otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified <see cref="IComponent"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// If this IEntityComponentCollection is read-only.
        /// </exception>
        bool Remove( IComponent component );

        /// <summary>
        /// Removes all IComponents from this IEntityComponentCollection.
        /// </summary>
        void Clear();
    }
}
