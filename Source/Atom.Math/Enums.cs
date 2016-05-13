// <copyright file = "Enums.cs" company = "federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines various enumerations provided by the Atom.Math libary.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    /// <summary>
    /// Enumerates the different turn direction.
    /// </summary>
    public enum TurnDirection
    {
        /// <summary>
        /// No specified direction.
        /// </summary>
        None = 0,

        /// <summary>
        /// A clockwise turn.
        /// </summary>
        Clockwise,

        /// <summary>
        /// A anti(counter) clockwise turn.
        /// </summary>
        AntiClockwise
    }

    /// <summary>
    /// Indicates the extent to which bounding volumes intersect or contain one another.
    /// </summary>
    public enum ContainmentType
    {
        /// <summary>
        /// Indicates there is no overlap between the bounding volumes.
        /// </summary>
        Disjoint = 0,

        /// <summary>
        /// Indicates that one bounding volume completely contains the other.
        /// </summary>
        Contains,

        /// <summary>
        /// Indicates that the bounding volumes partially overlap.
        /// </summary>
        Intersects
    }

    /// <summary>
    /// Enumerates the four main direction on the 2d plane.
    /// </summary>
    public enum Direction4
    {
        /// <summary> No specified direction. </summary>
        None = 0,

        /// <summary> Left (west). </summary>
        Left,

        /// <summary> Right (east). </summary>
        Right,

        /// <summary> Up (north). </summary>
        Up,

        /// <summary> Down (south). </summary>
        Down
    }

    /// <summary>
    /// Enumerates the four main direction on the 2d plane.
    /// This enumeration can be combined.
    /// </summary>
    [System.Flags]
    public enum Directions
    {
        /// <summary> No specified direction. </summary>
        None = 0,

        /// <summary> Left (west). </summary>
        Left = 2,

        /// <summary> Right (east). </summary>
        Right = 4,

        /// <summary> Up (north). </summary>
        Up = 16,

        /// <summary> Down (south). </summary>
        Down = 32
    }

    /// <summary>
    /// Enumerates the horizontal directions on the 2d plane.
    /// </summary>
    public enum HorizontalDirection
    {
        /// <summary> No specified direction. </summary>
        None = 0,

        /// <summary> Left (west). </summary>
        Left,

        /// <summary> Right (east). </summary>
        Right
    }

    /// <summary>
    /// Enumerates the vertical directions on the 2d plane.
    /// </summary>
    public enum VerticalDirection
    {
        /// <summary> No specified direction. </summary>
        None = 0,

        /// <summary> Up (north). </summary>
        Up,

        /// <summary> Down (south). </summary>
        Down
    }

    /// <summary>
    /// Enumerates the eight direction on the 2d plane.
    /// </summary>
    public enum Direction8
    {
        /// <summary> No specified direction. </summary>
        None = 0,

        /// <summary> Left (west). </summary>
        Left,

        /// <summary> Left and Up (west north). </summary>
        LeftUp,

        /// <summary> Left and Down (west south). </summary>
        LeftDown,

        /// <summary> Right (east). </summary>
        Right,

        /// <summary> Right and Up (east north). </summary>
        RightUp,

        /// <summary> Right and Down (east south). </summary>
        RightDown,

        /// <summary> Up (north). </summary>
        Up,

        /// <summary> Down (south). </summary>
        Down
    }

    /// <summary>
    /// Enumerates the four corners of a quad or rectangle.
    /// </summary>
    public enum QuadCorner
    {
        /// <summary> The upper left corner. </summary>
        UpperLeft,

        /// <summary> The upper right corner. </summary>
        UpperRight,

        /// <summary> The bottom left corner. </summary>
        BottomLeft,

        /// <summary> The bottom right corner. </summary>
        BottomRight
    }

    /// <summary>
    /// Enumerates the different types of Triangular Matrices.
    /// </summary>
    public enum TriangularMatrixType
    {
        /// <summary>
        /// A non-triangular Matrix.
        /// </summary>
        None,

        /// <summary>
        /// An upper-triangular Matrix.
        /// </summary>
        Upper,

        /// <summary>
        /// A lower-triangular Matrix.
        /// </summary>
        Lower,

        /// <summary>
        /// A diagonal Matrix.
        /// </summary>
        Diagonal
    }

    /// <summary>
    /// Enumerates the possible intersection types
    /// between a Plane and any bounding volume.
    /// </summary>
    public enum PlaneIntersectionType
    {
        /// <summary>
        /// The bounding volume is infront of the Plane.
        /// </summary>
        Front,

        /// <summary>
        /// The bounding volume is behind of the Plane.
        /// </summary>
        Back,

        /// <summary>
        /// The plane is directly intersecting with the bounding volume.
        /// </summary>
        Intersecting
    }

    /// <summary>
    /// Enumerates the different signs.
    /// </summary>
    public enum Sign
    {
        /// <summary>
        /// Represents no specific sign.
        /// </summary>
        None,

        /// <summary>
        /// Represents the plus sign.
        /// </summary>
        Plus,

        /// <summary>
        /// Represents the minus sign.
        /// </summary>
        Minus
    }

    /// <summary>
    /// Enumerates the alignments on x-axis.
    /// </summary>
    public enum HorizontalAlignment
    {
        /// <summary>
        /// No specific alignment.
        /// </summary>
        None = 0,

        /// <summary>
        /// Left alignment.
        /// </summary>
        Left,

        /// <summary>
        /// Center alignment.
        /// </summary>
        Center,

        /// <summary>
        /// Right alignment.
        /// </summary>
        Right
    }

    /// <summary>
    /// Enumerates the alignments on y-axis.
    /// </summary>
    public enum VerticalAlignment
    {
        /// <summary>
        /// No specific alignment.
        /// </summary>
        None = 0,

        /// <summary>
        /// Top alignment.
        /// </summary>
        Top,

        /// <summary>
        /// Center alignment.
        /// </summary>
        Center,

        /// <summary>
        /// Bottom alignment.
        /// </summary>
        Bottom
    }    

    /// <summary>
    /// Enumerates the various orientations on the 2D-plane.
    /// </summary>
    public enum Orientation
    {
        /// <summary>
        /// No specific orientation.
        /// </summary>
        None = 0,

        /// <summary>
        /// The object is oriented vertically.
        /// </summary>
        Vertical,
        
        /// <summary>
        /// The object is oriented horizontally.
        /// </summary>
        Horizontal
    }

    /// <summary>
    /// Enumerates the axes of 2D-space.
    /// </summary>
    public enum Axis2
    {
        /// <summary>
        /// No specific axis.
        /// </summary>
        None,

        /// <summary>
        /// The x-axis. (horizontal)
        /// </summary>
        X,

        /// <summary>
        /// The y-axis. (vertical)
        /// </summary>
        Y
    }

    /// <summary>
    /// Enumerates the axes of 3D-space.
    /// </summary>
    public enum Axis3
    {
        /// <summary>
        /// No specific axis.
        /// </summary>
        None,

        /// <summary>
        /// The x-axis. (horizontal)
        /// </summary>
        X,

        /// <summary>
        /// The y-axis. (vertical)
        /// </summary>
        Y,

        /// <summary>
        /// The z-axis. (in-/outwards)
        /// </summary>
        Z
    }
}
