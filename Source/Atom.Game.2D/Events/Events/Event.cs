// <copyright file="Event.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.Event class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Events
{
    using System;
    using System.ComponentModel;
    using Atom.Diagnostics.Contracts;
    using System.Globalization;
    using Atom.Events.Design;

    /// <summary>
    /// Defines the abstract base class of all events.
    /// An event can be any (triggerable) action.
    /// </summary>
    [TypeConverter( typeof( System.ComponentModel.ExpandableObjectConverter ) )]
    public abstract class Event : INotifyPropertyChanged, IReadOnlyNameable
    {
        #region [ Events ]

        /// <summary>
        /// Fired when a property of this <see cref="Event"/> has changed.
        /// Not all properties have to implement this kind of notification.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets an <see cref="Event"/> instance that does nothing.
        /// </summary>
        /// <value>An Event object that does nothing.</value>
        public static Event Empty
        {
            get
            {
                return NullEvent.Instance; 
            }
        }

        /// <summary>
        /// Gets the type-name of the <see cref="Event"/>
        /// that is used when serializing and deserializing.
        /// </summary>
        /// <value>The type-name of this Event.</value>
        [LocalizedDisplayName( "PropDisp_TypeName" )]
        [LocalizedCategory( "PropCate_Identification" )]
        [LocalizedDescription( "PropDesc_TypeName" )]
        public string TypeName
        {
            get
            {
                return this.GetType().GetTypeName();
            }
        }
        
        /// <summary>
        /// Gets or sets the (unique and bindable) name of the <see cref="Event"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Set: If the given name is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        /// <value>The name that uniquely identifies this Event.</value>
        [LocalizedDisplayName( "PropDisp_Name" )]
        [LocalizedCategory( "PropCate_Identification" )]
        [LocalizedDescription( "PropDesc_E_Name" )]
        public string Name
        {
            get 
            { 
                return this.name; 
            }

            set
            {
                Contract.Requires<ArgumentNullException>( value != null );

                if( value == this.name )
                    return;

                if( this.EventManager != null )
                {
                    if( this.EventManager.ContainsEvent( value ) )
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                CultureInfo.CurrentUICulture,
                                EventStrings.Error_CantChangeNameOfEventXToYThereAlreadyExistsSuchAnEvent,
                                this.name ?? string.Empty,
                                value
                            )
                        );
                    }

                    string oldName = name;
                    this.name = value;
                    this.EventManager.InternalInformEventNameHasChanged( this, oldName );
                }
                else
                {
                    this.name = value;
                }

                this.OnPropertyChanged( "Name" );
            }
        }

        /// <summary>
        /// Gets the <see cref="EventManager"/> that manages the
        /// <see cref="Event"/>.
        /// </summary>
        /// <value>
        /// This value is null if the event has not been added to
        /// an EventManager.
        /// </value>
        /// <remarks>
        /// An Event can only be active in a single EventManager at a time.
        /// Use cloning and syncing to have the 'same' events in different Managers.
        /// </remarks>
        [Browsable( false )]
        public EventManager EventManager
        {
            get;
            internal set;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets a value that indicates whether the 
        /// specified <see cref="Object"/> can trigger this <see cref="Event"/>.
        /// </summary>
        /// <param name="obj">
        /// The object to test.
        /// </param>
        /// <returns>
        /// Returns true if the object can trigger it; otherwise false.
        /// </returns>
        public virtual bool CanBeTriggeredBy( object obj )
        {
            return true;
        }

        /// <summary>
        /// Triggers this <see cref="Event"/>.
        /// </summary>
        /// <param name="obj">
        /// The object which is triggering this event.
        /// </param>
        public abstract void Trigger( object obj );

        #region > Storage <

        /// <summary>
        /// Serializes this Event using the specified IEventSerializationContext.
        /// </summary>
        /// <remarks>
        /// When overriding this function a call to base.Serialize 
        /// should always be the first action.
        /// Reason is that it writes the TypeName and Name of the <see cref="Event"/>
        /// which is needed to create an instance of the <see cref="Event"/>'s class.
        /// </remarks>
        /// <param name="context">
        /// The context under which the serialization process occurs.
        /// </param>
        public virtual void Serialize( IEventSerializationContext context )
        {
            context.Write( this.TypeName );
            context.Write( this.name ?? string.Empty );
        }

        /// <summary>
        /// Deserializes this Event using the specified IEventDeserializationContext.
        /// </summary>
        /// <remarks>
        /// When overriding this function a call to base.Deserialize 
        /// should always be the first action.
        /// When using this function from outside be sure to read the type-name and name
        /// by using
        /// <code>
        /// string typeName = context.ReadString();
        /// string name     = context.ReadString();
        /// </code>
        /// before calling Deserialize.
        /// </remarks>
        /// <param name="context">
        /// The context under which the deserialization process occurs.
        /// </param>
        public virtual void Deserialize( IEventDeserializationContext context )
        {
        }

        #endregion

        #region > Misc <

        /// <summary>
        /// Overriden to return a short human-readable description name of the <see cref="Event"/>.
        /// </summary>
        /// <returns>
        /// A string representation of the Event.
        /// </returns>
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder( 20 );

            sb.Append( name ?? string.Empty );
            sb.Append( " (" );
            sb.Append( this.GetType().Name );
            sb.Append( ')' );

            return sb.ToString();
        }

        /// <summary>
        /// Fires the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property that has changed.
        /// </param>
        protected void OnPropertyChanged( string propertyName )
        {
            if( this.PropertyChanged != null )
            {
                this.PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
            }
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The name of the <see cref="Event"/>. May be null.
        /// </summary>
        private string name;

        #endregion
    }
}
