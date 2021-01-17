// <copyright file="IMailMessageModifier.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Mail.Modifiers.IMailMessageModifier interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Mail.Modifiers
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Provides a mechanism for modifying an existing <see cref="IMailMessage"/>.
    /// </summary>
    // [ContractClass( typeof( IMailMessageModifierContract ) )]
    public interface IMailMessageModifier
    {
        /// <summary>
        /// Modifies the specified IMailMessage.
        /// </summary>
        /// <param name="mail">
        /// The mail to modify.
        /// </param>
        void Apply( IMailMessage mail );
    }

    /////// <summary>
    /////// Defines the contract for the <see cref="IMailMessageModifier"/> interface.
    /////// </summary>
    ////[ContractClassFor( typeof( IMailMessageModifier ) )]
    ////[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
    ////    Justification = "Contract classes for interfaces aren't required to be documented." )] 
    ////internal abstract class IMailMessageModifierContract : IMailMessageModifier
    ////{
    ////    public void Apply( IMailMessage mail )
    ////    {
    ////        Contract.Requires<ArgumentNullException>( mail != null );
    ////    }
    ////}
}
