// <copyright file="Particle.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Particle structure.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Xna.Particles
{
    using System.Runtime.InteropServices;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Represents a single Particle.
    /// </summary>
    /// <remarks>
    /// The Particle structure is designed in such a way that it can be directly drawn
    /// by using the Particle itself as a Vertex.
    /// </remarks>
    [StructLayout( LayoutKind.Sequential )]
    public struct Particle
    {
        #region [ Vertex Definitions ]

        /// <summary>
        /// Initializes static members of the Particle structure.
        /// </summary>
        static Particle()
        {
            // Position Vertex Element:
            var positionElement = new VertexElement() {
                VertexElementFormat = VertexElementFormat.Vector2,
                VertexElementUsage = VertexElementUsage.Position
            };

            // Scale Vertex Element:
            var scaleElement = new VertexElement() {
                Offset = 8,
                VertexElementFormat = VertexElementFormat.Single,
                VertexElementUsage = VertexElementUsage.PointSize
            };

            // Rotation Vertex Element:
            var rotationElement = new VertexElement() {
                Offset = 12,
                VertexElementFormat = VertexElementFormat.Single,
                VertexElementUsage = VertexElementUsage.TextureCoordinate
            };

            // Color Vertex Element:
            var colorElement = new VertexElement() {
                Offset = 16,
                VertexElementFormat = VertexElementFormat.Vector4,
                VertexElementUsage = VertexElementUsage.Color
            };

            // Vertex Element Array:
            Particle.VertexElements = new VertexElement[] {
                positionElement,
                scaleElement,
                rotationElement,
                colorElement
            };
        }

        /// <summary>
        /// An array of four vertex elements describing the position, scale, rotation
        /// and color of this Particle Vertex.
        /// </summary>
        public static readonly VertexElement[] VertexElements;

        /// <summary>
        /// Gets the size of the Particle structure in bytes.
        /// </summary>
        public static int SizeInBytes 
        {
            get { return 56; } 
        }

        #endregion

        #region [ Data Fields ]

        #region > Drawing <

        /// <summary>
        /// Gets or sets the position of this Particle.
        /// </summary>
        /// <remarks>This value is directly used when drawing this Particle.</remarks>
        public Vector2 Position;

        /// <summary>
        /// Gets or sets the scale of this Particle.
        /// </summary>
        /// <remarks>This value is directly used when drawing this Particle.</remarks>
        public float Scale;

        /// <summary>
        /// Gets or sets the rotation of this Particle.
        /// </summary>
        /// <remarks>This value is directly used when drawing this Particle.</remarks>
        public float Rotation;

        /// <summary>
        /// Gets or sets the color value of this Particle.
        /// </summary>
        /// <remarks>This value is directly used when drawing this Particle.</remarks>
        public Vector4 ColorValue;

        #endregion

        #region > Simulation <

        /// <summary>
        /// Gets or sets the momentum of this Particle.
        /// </summary>
        /// <remarks>This value is only used while simulating this Particle.</remarks>
        public Vector2 Momentum;

        /// <summary>
        /// Gets or sets the velocity of this Particle.
        /// </summary>
        /// <remarks>This value is only used while simulating this Particle.</remarks>
        public Vector2 Velocity;

        /// <summary>
        /// Gets or sets the inception of this Particle.
        /// </summary>
        /// <remarks>This value is only used while simulating this Particle.</remarks>
        public float Inception;

        /// <summary>
        /// Gets or sets the age of this Particle.
        /// </summary>
        /// <remarks>This value is only used while simulating this Particle.</remarks>
        public float Age;

        #endregion

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the <see cref="Color"/> of this Particle.
        /// </summary>
        public Color Color
        {
            get 
            {
                return new Color( this.ColorValue );
            }

            set
            {
                this.ColorValue = value.ToVector4();
            }
        }

        /// <summary>
        /// Gets or sets the opacity of this Particle.
        /// </summary>
        public float Opacity
        {
            get { return this.ColorValue.W; }
            set { this.ColorValue.W = value; }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Applies a force to this Particle.
        /// </summary>
        /// <param name="force">
        /// The force to apply to this Particle.
        /// </param>
        public void ApplyForce( ref Vector2 force )
        {
            Vector2.Add( ref this.Velocity, ref force, out this.Velocity );
        }

        /// <summary>
        /// Updates this Particle.
        /// </summary>
        /// <param name="frameTime">
        /// The time (in seconds) the last update took to execute.
        /// </param>
        public void Update( float frameTime )
        {
            // Add velocity to momentum...
            Vector2.Add( ref this.Velocity, ref this.Momentum, out this.Momentum );

            // Reset the velocity...
            this.Velocity = Vector2.Zero;

            // Calculate momentum for this time-step...
            Vector2 deltaMomentum;
            Vector2.Multiply( ref this.Momentum, frameTime, out deltaMomentum );

            // Add momentum to the particles position...
            Vector2.Add( ref this.Position, ref deltaMomentum, out this.Position );
        }

        /// <summary>
        /// Gets the hashcode of this Particle instance.
        /// </summary>
        /// <returns>
        /// The hashcode.
        /// </returns>
        public override int GetHashCode()
        {
            var hashBuilder = new Atom.HashCodeBuilder();

            hashBuilder.AppendStruct( this.Age );
            hashBuilder.AppendStruct( this.ColorValue );
            hashBuilder.AppendStruct( this.Inception );
            hashBuilder.AppendStruct( this.Momentum );
            hashBuilder.AppendStruct( this.Position );
            hashBuilder.AppendStruct( this.Rotation );
            hashBuilder.AppendStruct( this.Scale );
            hashBuilder.AppendStruct( this.Velocity );

            return hashBuilder.GetHashCode();
        }

        /// <summary>
        /// Gets a string representation of this Particle instance.
        /// </summary>
        /// <returns>
        /// A human readable string representation.
        /// </returns>
        public override string ToString()
        {
            return string.Format( 
                System.Globalization.CultureInfo.CurrentCulture,
                "{{Position:{0} Scale:{1} Rotation:{2} Color:{3}}}",
                this.Position,
                this.Scale, 
                this.Rotation, 
                this.ColorValue        
            );
        }

        /// <summary>
        /// Returns a value indicating whether this Particle equals
        /// the given <see cref="System.Object"/>.
        /// </summary>
        /// <param name="obj">
        /// The Object to compare against.
        /// </param>
        /// <returns>
        /// Returns true if they are equal;
        /// otherwise false.
        /// </returns>
        public override bool Equals( object obj )
        {
            if( obj is Particle )
            {
                return this == ((Particle)obj);
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the given Particles are equal.
        /// </summary>
        /// <param name="left">The Particle on the left side.</param>
        /// <param name="right">The Particle on the right side.</param>
        /// <returns>
        /// Returns true if they are equal;
        /// otherwise false.
        /// </returns>
        public static bool operator ==( Particle left, Particle right )
        {
            return (left.Position == right.Position) && (left.Scale == right.Scale) && 
                   (left.Rotation == right.Rotation) && (left.Color == right.Color);
        }

        /// <summary>
        /// Gets a value indicating whether the given Particles are not equal.
        /// </summary>
        /// <param name="left">The Particle on the left side.</param>
        /// <param name="right">The Particle on the right side.</param>
        /// <returns>
        /// Returns true if they are not equal;
        /// otherwise false.
        /// </returns>
        public static bool operator !=( Particle left, Particle right )
        {
            return !(left == right);
        }

        #endregion
    }
}