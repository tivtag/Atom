// <copyright file="VoltageData.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Data.VoltageData class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math.Graph.Data
{
    /// <summary>
    /// Defines a basic implementation of the <see cref="IVoltageData"/> interface.
    /// </summary>
    public class VoltageData : IVoltageData
    {
        /// <summary>
        /// Gets or sets the associated voltage value.
        /// </summary>
        /// <value>The voltage value.</value>
        public int Voltage
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VoltageData"/> class.
        /// </summary>
        public VoltageData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VoltageData"/> class.
        /// </summary>
        /// <param name="voltage">The associated voltage value.</param>
        public VoltageData( int voltage )
        {
            this.Voltage = voltage;
        }

        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString()
        {
            return "Voltage: " + this.Voltage.ToString( System.Globalization.CultureInfo.CurrentCulture );
        }
    }
}
