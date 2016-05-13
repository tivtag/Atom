// <copyright file="TileAreaEventTrigger.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.TileAreaEventTrigger class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Events
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using Atom.Events.Design;

    /// <summary>
    /// Defines an <see cref="AreaEventTrigger"/> that also
    /// stores the object depth layer the event is on.
    /// </summary>
    public class TileAreaEventTrigger : AreaEventTrigger, Atom.Scene.IFloorObject
    {
        /// <summary> 
        /// Gets or sets the (normalized) depth-layer the <see cref="TileAreaEventTrigger"/> is on.
        /// </summary>
        /// <value>
        /// A value that represents the floor this TileAreaEventTrigger is on.
        /// </value>
        [DefaultValue( 0 )]
        [LocalizedDisplayName( "PropDisp_Floor" )]
        [LocalizedCategory( "PropCate_Settings" )]
        [LocalizedDescription( "PropDesc_Floor" )]
        public int FloorNumber
        {
            get 
            {
                return this.floor; 
            }

            set
            {
                if( value == this.floor )
                    return;

                var tileEventManager = this.EventManager as TileEventManager;

                if( tileEventManager != null )
                {
                    if( !tileEventManager.IsValidFloor( value ) )
                    {
                        throw new ArgumentException( 
                            string.Format( 
                                CultureInfo.CurrentCulture,
                                EventStrings.Error_FloorXIsInvalidFloorCountY,
                                this.FloorNumber.ToString( CultureInfo.CurrentCulture ),
                                tileEventManager.FloorCount.ToString( CultureInfo.CurrentCulture ) 
                            ),
                            "value"
                         );
                    }

                    int oldFloor = this.floor;
                    this.floor   = value;

                    tileEventManager.InternalInformTileAreaTriggerFloorHasChanged( this, oldFloor );
                }
                else
                {
                    this.floor = value;
                }
            }
        }
        
        /// <summary>
        /// Serializes this TileAreaEventTrigger using the specified IEventSerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context under which the serialization process occurs.
        /// </param>
        public override void Serialize( IEventSerializationContext context )
        {
            base.Serialize( context );

            context.Write( this.FloorNumber );
        }

        /// <summary>
        /// Deserializes this TileAreaEventTrigger using the specified IEventDeserializationContext.
        /// </summary>
        /// <param name="context">
        /// The context under which the deserialization process occurs.
        /// </param>
        public override void Deserialize( IEventDeserializationContext context )
        {
            base.Deserialize( context );

            this.FloorNumber = context.ReadInt32();
        }
        
        /// <summary>
        /// The number of the floor this <see cref="TileAreaEventTrigger"/> is on.
        /// </summary>
        private int floor;
    }
}
