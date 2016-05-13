// <copyright file="EntityComponentCollection.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Components.EntityComponentCollection class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Reflection;
    
    /// <summary>
    /// Represents a list of <see cref="IComponent"/>s that compose an <see cref="IEntity"/>.
    /// This class can't be inherited.
    /// </summary>
    public sealed class EntityComponentCollection : IEntityComponentCollection
    {
        #region [ Events ]

        /// <summary>
        /// Raised when an <see cref="IComponent"/> has been added to this EntityComponentCollection.
        /// </summary>
        public event RelaxedEventHandler<IComponent> Added;

        /// <summary>
        /// Raised when an <see cref="IComponent"/> has been removed from this EntityComponentCollection.
        /// </summary>
        public event RelaxedEventHandler<IComponent> Removed;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the <see cref="IEntity"/> that owns this EntityComponentCollection.
        /// </summary>
        /// <value>
        /// The <see cref="IEntity"/> that owns this EntityComponentCollection.
        /// </value>
        public IEntity Owner
        {
            get
            {
                return this.owner; 
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this EntityComponentCollection is read-only.
        /// </summary>
        /// <value>If this value is <see langword="true"/> then this EntityComponentCollection can't be modified.</value>
        public bool IsReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this EntityComponentCollection
        /// notifies its <see cref="IComponent"/>s by calling <see cref="IComponent.InitializeBindings"/>
        /// when an <see cref="IComponent"/> gets added or removed.
        /// </summary>
        /// <remarks>
        /// It may be useful to temporarily disable notification for perfomance reasons.
        /// For example before adding a punch of <see cref="IComponent"/>s. 
        /// </remarks>
        /// <value>The default value is true.</value>
        public bool BindingNotificationEnabled
        {
            get 
            { 
                return this._bindingNotificationEnabled; 
            }

            set
            {
                if( value == this.BindingNotificationEnabled )
                    return;

                this._bindingNotificationEnabled = value;

                // If required refresh the bindings between the components:
                if( value )
                {
                    if( this.isBindingRefreshRequired )
                    {
                        this.InitializeBindings();
                        this.isBindingRefreshRequired = false;
                    }
                }
            }
        }

        /// <summary>
        /// Receives a <see cref="IComponent"/> by exact <see cref="Type"/> key.
        /// </summary>
        /// <param name="key">The type of IComponent to get.</param>
        /// <returns>
        /// The requested <see cref="IComponent"/> or null if not found.
        /// </returns>
        public IComponent this[Type key]
        {
            get
            {
                IComponent component;

                if( this.dictionary.TryGetValue( key, out component ) )
                    return component;

                return null;
            }
        }

        /// <summary>
        /// Gets the number of <see cref="IComponent"/>s in this <see cref="EntityComponentCollection"/>.
        /// </summary>
        /// <value>The number of <see cref="IComponent"/>s in this <see cref="EntityComponentCollection"/>.</value>
        public int Count
        {
            get
            {
                return this.dictionary.Count;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityComponentCollection"/> class.
        /// </summary>
        /// <param name="owner">
        /// The <see cref="IEntity"/> that owns the new EntityComponentCollection.
        /// </param>
        /// <param name="capacity">
        /// The initial number of <see cref="IComponent"/>s the new EntityComponentCollection
        /// will be able to store without needing to reallocate memory.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="owner"/> is null.
        /// </exception>
        public EntityComponentCollection( IEntity owner, int capacity )
        {
            Contract.Requires<ArgumentNullException>( owner != null );

            this.owner      = owner;
            this.dictionary = new Dictionary<Type, IComponent>( capacity );
            this.components = new List<IComponent>( capacity );
        }

        #endregion

        #region [ Methods ]

        #region - Add -

        /// <summary>
        /// Adds the specified <see cref="IComponent"/> to this <see cref="EntityComponentCollection"/>.
        /// </summary>
        /// <param name="component">
        /// The IComponent to add.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// If this EntityComponentCollection is read-only.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="component"/> is null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If this EntityComponentCollection already contains an <see cref="IComponent"/> of the given type.
        /// </exception>
        public void Add( IComponent component )
        {
            //if( components.Count >= components.Capacity )
            //{
            //    throw new ArgumentException( "Ugh: " + components.Capacity + " count=" + components.Count + " " + this.owner.GetType() );
            //}

            this.components.Add( component );
            this.dictionary.Add( component.GetType(), component );
            
            component.Owner = this.Owner;
            component.Initialize();

            this.OnComponentAdded( component );
        }

        /// <summary>
        /// Adds the specified <see cref="IComponent"/>s to this IEntityComponentCollection.
        /// </summary>
        /// <param name="components">
        /// The <see cref="IComponent"/>s to add.
        /// </param>
        public void AddRange( IEnumerable<IComponent> components )
        {
            bool oldNotificationState = this.BindingNotificationEnabled;
            this.BindingNotificationEnabled = false;

            foreach( var component in components )
            {
                this.Add( component );
            }

            this.BindingNotificationEnabled = oldNotificationState;
        }

        #endregion

        #region - Remove -

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
        /// If this EntityComponentCollection is read-only.
        /// </exception>
        public bool Remove<T>() where T : class, IComponent
        {
            for( int i = 0; i < this.components.Count; ++i )
            {
                var component = this.components[i];                

                if( component is T )
                    return this.Remove( component );
            }

            return false;
        }

        /// <summary>
        /// Tries to remove the specified <see cref="IComponent"/> from this IEntityComponentCollection.
        /// </summary>
        /// <param name="component">
        /// The <see cref="IComponent"/> to remove.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the <see cref="Component"/> has been removed;
        /// otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified <see cref="IComponent"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// If this IEntityComponentCollection is read-only.
        /// </exception>
        public bool Remove( IComponent component )
        {
            if( this.dictionary.Remove( component.GetType() ) )
            {
                this.components.Remove( component );
                this.OnComponentRemoved( component );
                return true;
            }

            return false;
        }

        #endregion

        #region - Get -

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
        /// Get&lt;TheComponent&gt;;  // This will receive the IComponent.
        /// Get&lt;ITheComponent&gt;; // Try to receive base interface/class - this will FAIL!
        /// </code> 
        /// You must use Find for more complex IComponent searches.
        /// </remarks>
        /// <returns>
        /// The requested IComponent; 
        /// or null if there exists no IComponent of the specified Type.
        /// </returns>
        public T Get<T>() where T : class, IComponent
        {
            IComponent component;

            if( dictionary.TryGetValue( typeof( T ), out component ) )
            {
                return (T)component;
            }

            return null;
        }

        #endregion

        #region - Find -

        /// <summary>
        /// Finds the first occurence of an <see cref="IComponent"/> of type <typeparamref name="T"/>.
        /// </summary>
        /// <remarks>
        /// This operation has a complexity of O(N), there N is the number of components present in the collection.
        /// </remarks>
        /// <typeparam name="T">The type of <see cref="IComponent"/> to find.</typeparam>
        /// <returns>
        /// The found <see cref="IComponent"/>;
        /// or null if no matching <see cref="IComponent"/> could be found in this IEntityComponentCollection.
        /// </returns>
        public T Find<T>() where T : class, IComponent
        {
            for( int i = 0; i < this.components.Count; ++i )
            {
                T t = this.components[i] as T;
                if( t != null )
                    return t;
            }

            return null;
        }

        /// <summary>
        /// Finds all <see cref="IComponent"/>s that are of type <typeparamref name="T"/> or implement it.
        /// </summary>
        /// <typeparam name="T">
        /// The type of <see cref="IComponent"/>s to find.
        /// </typeparam>
        /// <returns>
        /// A list of <see cref="IComponent"/>s that are of type <typeparamref name="T"/> or implement it.
        /// </returns>
        public IList<T> FindAll<T>() where T : class, IComponent
        {
            var matches = new List<T>();

            for( int i = 0; i < this.components.Count; ++i )
            {
                var match = this.components[i] as T;

                if( match != null )
                {
                    matches.Add( match );
                }
            }

            return matches;
        }

        #endregion

        #region - Contains -

        /// <summary>
        /// Determines whether this <see cref="EntityComponentCollection"/> contains 
        /// a <see cref="IComponent"/> of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of <see cref="IComponent"/>s to find.
        /// </typeparam>
        /// <returns>
        /// Returns <see langword="true"/> if this EntityComponentCollection
        /// contains a <see cref="IComponent"/> of type <typeparamref name="T"/>;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Contains<T>() where T : class, IComponent
        {
            for( int i = 0; i < this.components.Count; ++i )
            {
                var component = this.components[i];
                if( component is T )
                    return true;
            }

            return false;
        }
        
        /// <summary>
        /// Determines whether this EntityComponentCollection contains 
        /// a <see cref="IComponent"/> of the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="componentType">
        /// The type of component to look for.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if this EntityComponentCollection contains 
        /// a <see cref="Component"/> of the specified <see cref="Type"/>;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Contains( Type componentType )
        {
            if( componentType == null )
                return false;

            for( int i = 0; i < this.components.Count; ++i )
            {
                var component = this.components[i];
                if( componentType.IsAssignableFrom( component.GetType() ) )
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether this <see cref="EntityComponentCollection"/> contains the specified <see cref="IComponent"/>.
        /// </summary>
        /// <param name="component">
        /// The <see cref="IComponent"/> to look for.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if this <see cref="EntityComponentCollection"/> 
        /// contains the specified <see cref="IComponent"/>; otherwise <see langword="false"/>.
        /// </returns>
        public bool Contains( IComponent component )
        {
            return this.dictionary.ContainsValue( component );
        }

        #endregion

        #region - Misc -

        /// <summary>
        /// Removes all <see cref="IComponent"/>s from this <see cref="EntityComponentCollection"/>.
        /// </summary>
        public void Clear()
        {
            // Temporarily disable binding notification
            // for perfomance reasons.
            bool changedNotification = false;
            if( this.BindingNotificationEnabled )
            {
                changedNotification             = true; 
                this.BindingNotificationEnabled = false;
            }

            // Remove all components.
            for( int i = 0; i < this.components.Count; ++i )
            {
                var component = this.components[i];
                this.OnComponentRemoved( component );
            }

            this.dictionary.Clear();
            this.components.Clear();

            // Reenable notification.
            if( changedNotification )
                this.BindingNotificationEnabled = true;
        }

        /// <summary>
        /// Gets an enumerator that iterates over the <see cref="Components"/>
        /// of this <see cref="EntityComponentCollection"/>.
        /// </summary>
        /// <returns>
        /// A new enumerator.
        /// </returns>
        public IEnumerator<IComponent> GetEnumerator()
        {
            return components.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator that iterates over the <see cref="Components"/>
        /// of this <see cref="EntityComponentCollection"/>.
        /// </summary>
        /// <returns>
        /// A new enumerator.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return components.GetEnumerator();
        }

        #endregion

        #region - Update -

        /// <summary>
        /// Updates the <see cref="IComponent"/>s of this EntityComponentCollection.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        public void Update( IUpdateContext updateContext )
        {
            for( int i = 0; i < this.components.Count; ++i )
            {
                this.components[i].Update( updateContext );
            }
        }

        /// <summary>
        /// Pre-updates the <see cref="IComponent"/>s of this EntityComponentCollection.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        public void PreUpdate( IUpdateContext updateContext )
        {
            for( int i = 0; i < this.components.Count; ++i )
            {
                this.components[i].PreUpdate( updateContext );
            }
        }

        #endregion
        
        /// <summary>
        /// Begins the region in which <see cref="IComponent"/>s are added to this IEntityComponentCollection.
        /// </summary>
        public void BeginSetup()
        {
            // Remarks: Use field instead of property for optimization reasons.
            this._bindingNotificationEnabled = false;
        }

        /// <summary>
        /// Ends the setup process initiated by BeginSetup.
        /// </summary>
        public void EndSetup()
        {
            // Remarks: Use fields/manual initialization for optimization reasons.
            this._bindingNotificationEnabled = true;
            this.InitializeBindings();
            this.isBindingRefreshRequired = false;
        }

        /// <summary>
        /// Initialize the bindings between components in this EntityComponentCollection
        /// by calling <see cref="IComponent.InitializeBindings"/> 
        /// if <see cref="BindingNotificationEnabled"/> is true.
        /// </summary>
        private void InitializeBindings()
        {
            if( this.BindingNotificationEnabled )
            {
                for( int i = 0; i < this.components.Count; ++i )
                {
                    this.components[i].InitializeBindings();
                }
            }
            else
            {
                this.isBindingRefreshRequired = true;
            }
        }

        /// <summary>
        /// Raises the ComponentAdded event.
        /// </summary>
        /// <param name="component">
        /// The IComponent that was added.
        /// </param>
        private void OnComponentAdded( IComponent component )
        {
            this.InitializeBindings();

            if( this.Added != null )
            {
                this.Added( this, component );
            }
        }

        /// <summary>
        /// Raises the ComponentRemoved event.
        /// </summary>
        /// <param name="component">
        /// The IComponent that was removed.
        /// </param>
        private void OnComponentRemoved( IComponent component )
        {
            this.InitializeBindings();

            if( this.Removed != null )
            {
                this.Removed( this, component );
            }
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Identifies the <see cref="IEntity"/> that owns this EntityComponentCollection.
        /// </summary>
        private readonly IEntity owner;

        /// <summary>
        /// The components that compose the <see cref="IEntity"/>, sorted by their type.
        /// </summary>
        private readonly Dictionary<Type, IComponent> dictionary;

        /// <summary>
        /// The collection that contains the components that compose this <see cref="Entity"/>.
        /// </summary>
        private readonly List<IComponent> components;

        /// <summary>
        /// States whether this EntityComponentCollection notifies its <see cref="IComponent"/>s 
        /// by calling <see cref="IComponent.InitializeBindings"/> when an <see cref="IComponent"/> 
        /// gets added or removed.
        /// </summary>
        private bool _bindingNotificationEnabled = true;

        /// <summary>
        /// States whether this EntityComponentCollection should update the bindings between its <see cref="IComponent"/>s.
        /// </summary>
        /// <remarks>
        /// This field is modified by the <see cref="BindingNotificationEnabled"/> property.
        /// </remarks>
        private bool isBindingRefreshRequired;

        #endregion
    }
}
