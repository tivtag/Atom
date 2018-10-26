////// <copyright file="IEntityComponentCollection.Contracts.cs" company="federrot Software">
//////     Copyright (c) federrot Software. All rights reserved.
////// </copyright>
////// <summary>Defines the contracts for the Atom.Components.IEntityComponentCollection interface.</summary>
////// <author>Paul Ennemoser (Tick)</author>

////namespace Atom.Components
////{
////    using System;
////    using System.Collections.Generic;
////    using Atom.Diagnostics.Contracts;

////    /// <summary>
////    /// Defines the code contracts for the IEntityComponentCollection interface.
////    /// </summary>
////    [ContractClassFor( typeof( IEntityComponentCollection ) )]
////    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
////    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
////        Justification = "Contract classes for interfaces aren't required to be documented." )] 
////    internal abstract class IEntityComponentCollectionContracts : IEntityComponentCollection
////    {
////        event RelaxedEventHandler<IComponent> IEntityComponentCollection.Added
////        {
////            add { }
////            remove { }
////        }

////        event RelaxedEventHandler<IComponent> IEntityComponentCollection.Removed
////        {
////            add {  }
////            remove { }
////        }

////        IEntity IEntityComponentCollection.Owner
////        {
////            get
////            {
////                Contract.Ensures( Contract.Result<IEntity>() != null );

////                return default( IEntity );
////            }
////        }

////        bool IEntityComponentCollection.IsReadOnly
////        {
////            get
////            {
////                return default( bool );
////            }

////            set
////            {
////            }
////        }

////        int IEntityComponentCollection.Count
////        {
////            get
////            {
////                Contract.Ensures( Contract.Result<int>() >= 0 );
////                return default( int );
////            }
////        }

////        bool IEntityComponentCollection.BindingNotificationEnabled
////        {
////            get
////            {
////                return default( bool );
////            }

////            set
////            {
////            }
////        }

////        IComponent IEntityComponentCollection.this[Type key]
////        {
////            get 
////            { 
////                return default( IComponent );
////            }
////        }

////        void IEntityComponentCollection.BeginSetup()
////        {
////        }

////        void IEntityComponentCollection.EndSetup()
////        {
////        }

////        T IEntityComponentCollection.Get<T>()
////        {
////            return default( T );
////        }

////        T IEntityComponentCollection.Find<T>()
////        {
////            return default( T );
////        }

////        IList<T> IEntityComponentCollection.FindAll<T>()
////        {
////            return default( IList<T> );
////        }

////        bool IEntityComponentCollection.Contains<T>()
////        {
////            return default( bool );
////        }

////        bool IEntityComponentCollection.Contains( Type componentType )
////        {
////            return default( bool );
////        }

////        bool IEntityComponentCollection.Contains( IComponent component )
////        {
////            Contract.Ensures( !(component == null) || (Contract.Result<bool>() == false) );

////            return default( bool );
////        }

////        void IEntityComponentCollection.Add( IComponent component )
////        {
////            IEntityComponentCollection @this = this;
////            Contract.Requires<ArgumentNullException>( component != null );
////            Contract.Requires<InvalidOperationException>( !@this.IsReadOnly, ErrorStrings.ComponentCollectionIsReadOnly );

////            Contract.Ensures(
////                @this.Count > Contract.OldValue<int>( @this.Count )
////            );
////        }

////        void IEntityComponentCollection.AddRange( IEnumerable<IComponent> components )
////        {         
////            Contract.Requires<ArgumentNullException>( components != null );
////            Contract.Requires<ArgumentException>( 
////                Contract.ForAll<IComponent>( components, component => component != null )
////            );
////        }

////        bool IEntityComponentCollection.Remove<T>()
////        {
////            return default( bool );
////        }

////        bool IEntityComponentCollection.Remove( IComponent component )
////        {
////            IEntityComponentCollection @this = this;
////            Contract.Requires<InvalidOperationException>( !@this.IsReadOnly, ErrorStrings.ComponentCollectionIsReadOnly );
////            Contract.Requires<ArgumentNullException>( component != null );

////            return default( bool );
////        }

////        void IEntityComponentCollection.Clear()
////        {
////            IEntityComponentCollection @this = this;

////            Contract.Requires<InvalidOperationException>( !@this.IsReadOnly, ErrorStrings.ComponentCollectionIsReadOnly );
////            Contract.Ensures( @this.Count == 0 );
////        }

////        #region - IUpdateable -

////        void IUpdateable.Update( IUpdateContext updateContext )
////        {
////        }

////        void IPreUpdateable.PreUpdate( IUpdateContext updateContext )
////        {
////        }

////        #endregion

////        #region - IEnumerable -

////        IEnumerator<IComponent> IEnumerable<IComponent>.GetEnumerator()
////        {
////            return default( System.Collections.Generic.IEnumerator<IComponent> );
////        }

////        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
////        {
////            return default( System.Collections.IEnumerator );
////        }

////        #endregion
////    }
////}
