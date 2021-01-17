// <copyright file="EventTrigger.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.EventTrigger class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Events
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using Atom.Diagnostics.Contracts;
    using System.Globalization;
    using Atom.Events.Design;

    /// <summary>
    /// An <see cref="EventTrigger"/> triggers an <see cref="Event"/>.
    /// </summary>
    [TypeConverter( typeof( ExpandableObjectConverter ) )]
    public class EventTrigger : INotifyPropertyChanged, IReadOnlyNameable
    {
        /// <summary>
        /// Fired when a property of this <see cref="EventTrigger"/> has changed.
        /// Not all properties have to implement this kind of notification.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the type-name of the <see cref="EventTrigger"/>
        /// that is used when serializing and deserializing.
        /// </summary>
        /// <value>The type-name of this EventTrigger.</value>
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
        /// Gets or sets the name of the <see cref="EventTrigger"/>.
        /// </summary>
        /// <value>The name that uniquely identifies this EventTrigger.</value>
        [LocalizedDisplayName( "PropDisp_Name" )]
        [LocalizedCategory( "PropCate_Identification" )]
        [LocalizedDescription( "PropDesc_ET_Name" )]
        public string Name
        {
            get 
            {
                return this.name; 
            }

            set
            {
                Contract.Requires<ArgumentNullException>( value != null );

                if( this.EventManager == null )
                {
                    this.name = value;
                }
                else
                {
                    if( value == name )
                    {
                        return;
                    }

                    if( this.EventManager.ContainsTrigger( value ) )
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                CultureInfo.CurrentUICulture,
                                EventStrings.Error_CantChangeNameOfEventTriggerXToYThereAlreadyExistsSuchAnEventTrigger,
                                this.name, 
                                value
                            )
                        );
                    }

                    string oldName = name;
                    this.name      = value;

                    this.EventManager.InternalInformTriggerNameHasChanged( this, oldName );
                }

                this.OnPropertyChanged( nameof( Name ) );
            }
        }

        /// <summary>
        /// Gets or sets the underlying <see cref="Event"/> which is triggered by the <see cref="EventTrigger"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Event"/> which is triggered by the <see cref="EventTrigger"/>.
        /// </value>
        [DefaultValue( null )]
        [LocalizedDisplayName( "PropDisp_Event" )]
        [LocalizedCategory( "PropCate_Settings" )]
        [LocalizedDescription( "PropDesc_ET_Event" )]
        [Editor( "Atom.Events.Design.EventCreationEditor, Atom.Game.Design", "System.Drawing.Design.UITypeEditor, System.Windows.Forms" )]
        public Event Event
        {
            get 
            {
                return this.e;
            }

            set 
            {
                if( value == this.e )
                {
                    return;
                }

                this.e = value;
                this.OnPropertyChanged( nameof( Event ) );
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="EventTrigger"/> is currently active. 
        /// </summary>
        /// <value>The default value is true.</value>
        [DefaultValue( true )]
        [LocalizedDisplayName( "PropDisp_IsActive" )]
        [LocalizedCategory( "PropCate_Settings" )]
        [LocalizedDescription( "PropDesc_ET_IsActive" )]
        public bool IsActive
        {
            get 
            {
                return this.isActive;
            }

            set
            {
                if( value == this.isActive )
                {
                    return;
                }

                this.isActive = value;
                this.OnPropertyChanged( "IsActive" );
            }
        }

        /// <summary>
        /// Gets the <see cref="EventManager"/> that manages the
        /// <see cref="EventTrigger"/>.
        /// </summary>
        /// <value>
        /// This value is null if the event has not been added to
        /// an EventManager.
        /// </value>
        /// <remarks>
        /// An EventTrigger can only be active in a single EventManager at a time.
        /// Use cloning and syncing to have the 'same' EventTriggers in different Managers.
        /// </remarks>
        [Browsable( false )]
        public EventManager EventManager
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the custom tag applied to this <see cref="EventTrigger"/>.
        /// </summary>
        /// <remarks>
        /// The tag is not serialized by default.
        /// </remarks>
        /// <value>
        /// A custom user-setable object.
        /// </value>
        [Browsable( false )]
        public object Tag 
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTrigger"/> class.
        /// </summary>
        /// <param name="name"> 
        /// The name of the EventTrigger.</param>
        /// <param name="e">
        /// The underlying Event.
        /// </param>
        public EventTrigger( string name, Event e )
        {
            this.name = name;
            this.e = e;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTrigger"/> class.
        /// </summary>
        public EventTrigger()
        {
        }

        /// <summary>
        /// Gets whether the specified Object can trigger the <see cref="EventTrigger"/>.
        /// </summary>
        /// <param name="context">
        /// The context of execution.
        /// </param>
        /// <returns>
        /// true if the object can trigger it;
        /// otherwise false.
        /// </returns>
        public virtual bool CanBeTriggeredBy( TriggerContext context )
        {
            if( this.e == null )
            {
                return false;
            }

            return this.e.CanBeTriggeredBy( context.Object );
        }

        /// <summary>
        /// Triggers the underlying event if active.
        /// </summary>
        /// <param name="obj">
        /// The object which has triggered the event.
        /// </param>
        public virtual void Trigger( object obj )
        {
            if( this.isActive && this.e != null )
            {
                this.e.Trigger( obj );
            }
        }
        
        /// <summary>
        /// Serializes this EventTrigger using the specified IEventSerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context under which the serialization process occurs.
        /// </param>
        public virtual void Serialize( IEventSerializationContext context )
        {
            context.Write( this.TypeName );
            context.Write( this.Name );

            context.Write( this.isActive );

            if( this.e == null )
            {
                context.Write( string.Empty );
            }
            else
            {
                context.Write( e.Name );
            }
        }
        
        /// <summary>
        /// Deserializes this EventTrigger using the specified IEventDeserializationContext.
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
            this.isActive = context.ReadBoolean();
            string eventName = context.ReadString();

            if( eventName.Length == 0 )
            {
                this.e = null;
            }
            else
            {
                this.e = context.GetEvent( eventName );
                Debug.Assert( e != null, "An error occurred when trying to get the event named " + eventName + "." );
            }
        }

        /// <summary>
        /// Overriden to return a short human-readable description name of this <see cref="EventTrigger"/>.
        /// </summary>
        /// <returns>
        /// A string representation of this <see cref="EventTrigger"/>.
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

        /// <summary>
        /// The name of the <see cref="EventTrigger"/>. May be null.
        /// </summary>
        private string name;

        /// <summary>
        /// The underlying event.
        /// </summary>
        private Event e;

        /// <summary>
        /// States whether the <see cref="EventTrigger"/> is currently active.
        /// </summary>
        private bool isActive = true;
    }
}
