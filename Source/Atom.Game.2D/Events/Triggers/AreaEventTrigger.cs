// <copyright file="AreaEventTrigger.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.AreaEventTrigger class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Events
{
    using Atom.Events.Design;
    using Atom.Math;

    /// <summary>
    /// Enumerates the various modes an <see cref="AreaEventTrigger"/> can be triggered.
    /// </summary>
    public enum AreaEventTriggerMode : byte
    {
        /// <summary>
        /// The object must merely touch the area to trigger it.
        /// This is the default mode.
        /// </summary>
        Intersection,

        /// <summary>
        /// The object must be fully contained by the area to trigger it.
        /// </summary>
        Containment
    }

    /// <summary>
    /// Defines an <see cref="EventTrigger"/> which triggers 
    /// when an object is within a specified rectangular area.
    /// </summary>
    public class AreaEventTrigger : EventTrigger
    {
        /// <summary> 
        /// Gets or sets the Rectangle that defines the area this <see cref="AreaEventTrigger"/> is active in.
        /// </summary>
        /// <value>
        /// The area this AreaEventTrigger is active in.
        /// </value>
        [LocalizedDisplayName( "PropDisp_AET_Area" )]
        [LocalizedCategory( "PropCate_Settings" )]
        [LocalizedDescription( "PropDesc_AET_Area" )]
        public Rectangle Area
        {
            get
            {
                return this.area;
            }

            set
            {
                this.area = value; 
            }
        }

        /// <summary>
        /// Gets or sets the area this ZeldaTileAreaEventTrigger triggers in tile-space.
        /// </summary>
        [LocalizedDisplayName( "PropDisp_AET_TileArea" )]
        [LocalizedCategory( "PropCate_Settings" )]
        [LocalizedDescription( "PropDesc_AET_TileArea" )]
        public Rectangle TileArea
        {
            get
            {
                return new Rectangle( area.X / TileWidth, area.Y / TileHeight, area.Width / TileWidth, area.Height / TileHeight );
            }

            set
            {
                this.area = new Rectangle(
                    value.X * TileWidth,
                    value.Y * TileHeight,
                    value.Width * TileWidth,
                    value.Height * TileHeight
                );
            }
        }

        /// <summary>
        /// Gets or sets a value indicating how this AreaEventTrigger is triggered.
        /// </summary>
        [LocalizedCategory( "PropCate_Settings" )]
        public AreaEventTriggerMode TriggerMode 
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets the width of a single tile.
        /// </summary>
        /// <value>
        /// The default value is 16.
        /// </value>
        protected virtual int TileWidth
        {
            get
            {
                return 16;
            }
        }

        /// <summary>
        /// Gets the height of a single tile.
        /// </summary>
        /// <value>
        /// The default value is 16.
        /// </value>
        protected virtual int TileHeight
        {
            get
            {
                return 16;
            }
        }

        /// <summary>
        /// Tests whether the specified <see cref="Rectangle"/> intersects the trigger's trigger-area.
        /// </summary>
        /// <param name="rectangle">The rectangle to test against.</param>
        /// <returns>
        /// Returns true if the rectangles intersect; otherwise false.
        /// </returns>
        public bool Intersects( ref Rectangle rectangle )
        {
            bool result;
            this.area.Intersects( ref rectangle, out result );
            return result;
        }

        /// <summary>
        /// Tests whether the specified <see cref="Rectangle"/> intersects the trigger's trigger-area.
        /// </summary>
        /// <param name="rectangle">The rectangle to test against.</param>
        /// <returns>
        /// Returns true if the rectangles intersect; otherwise false.
        /// </returns>
        public bool Intersects( Rectangle rectangle )
        {
            bool result;
            this.area.Intersects( ref rectangle, out result );
            return result;
        }

        /// <summary>
        /// Gets whether the specified <see cref="Point2"/> is contained within the
        /// trigger's area.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <returns>
        /// Returns true if the given <paramref name="point"/> is within the trigger's area;
        /// otherwise false.
        /// </returns>
        public bool Contains( Point2 point )
        {
            return this.area.Contains( point );
        }

        /// <summary>
        /// Gets whether the specified <see cref="Rectangle"/> is contained within the
        /// trigger's area.
        /// </summary>
        /// <param name="area">The area to test.</param>
        /// <returns>
        /// Returns true if the given <paramref name="area"/> is within the trigger's area;
        /// otherwise false.
        /// </returns>
        public bool Contains( ref Rectangle area )
        {
            bool result;
            this.area.Contains( ref area, out result );
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether the given object would be triggered
        /// by this AreaEventTrigger.
        /// </summary>
        /// <param name="context">
        /// The execution context.
        /// </param>
        /// <param name="area">
        /// The area of the object.
        /// </param>
        /// <returns>
        /// true if it can be triggered;
        /// -or- otherwise false.
        /// </returns>
        public bool WouldTriggerBy( TriggerContext context, ref Rectangle area )
        {
            if( CanBeTriggeredBy( context ) )
            {
                if( this.TriggerMode == AreaEventTriggerMode.Intersection )
                {
                    return Intersects( ref area );
                }
                else
                {
                    return Contains( ref area );
                }
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        /// Serializes this AreaEventTrigger using the specified IEventSerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context under which the serialization process occurs.
        /// </param>
        public override void Serialize( IEventSerializationContext context )
        {
            base.Serialize( context );

            const int Version = 1;
            context.Write( Version );    
       
            context.Write( this.area.X );
            context.Write( this.area.Y );
            context.Write( this.area.Width );
            context.Write( this.area.Height );
            context.Write( (byte)this.TriggerMode );
        }

        /// <summary>
        /// Deserializes this AreaEventTrigger using the specified IEventDeserializationContext.
        /// </summary>
        /// <param name="context">
        /// The context under which the deserialization process occurs.
        /// </param>
        public override void Deserialize( IEventDeserializationContext context )
        {
            base.Deserialize( context );

            int version = context.ReadInt32();
            if( version != 1 )
                throw new InvalidVersionException();
            
            int x = context.ReadInt32();
            int y = context.ReadInt32();
            int width  = context.ReadInt32();
            int height = context.ReadInt32();
            this.area = new Rectangle( x, y, width, height );

            this.TriggerMode = (AreaEventTriggerMode)context.ReadByte();
        }
                
        /// <summary>
        /// The Rectangle that defines the trigger-area.
        /// </summary>
        private Rectangle area;
    }
}
