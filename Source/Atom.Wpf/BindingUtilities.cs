// <copyright file="BindingUtilities.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.BindingUtilities class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Provides utility methods related to Windows Presentation Foundation (WPF) data-binding.
    /// </summary>
    public static class BindingUtilities
    {
        /// <summary>
        /// Recursively processes a given dependency object and all its
        /// children, and updates sources of all objects that use a
        /// binding expression on a given property.
        /// </summary>
        /// <param name="target">
        /// The <see cref="DependencyObject"/> that marks a starting point.
        /// This could be a dialog window or a panel control that hosts bound controls.
        /// </param>
        /// <param name="properties">
        /// The properties to be updated if <paramref name="target"/> or one of its childrens
        /// provide it along with a binding expression.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="target"/>, <paramref name="properties"/> or any of the given DependencyProperties is null.
        /// </exception>
        public static void UpdateBindingSources( this DependencyObject target, params DependencyProperty[] properties )
        {
            Contract.Requires<ArgumentNullException>( target != null );
            Contract.Requires<ArgumentNullException>( properties != null );
            Contract.Requires<ArgumentException>( Contract.ForAll( properties, ( propertery ) => propertery != null ) );

            foreach( DependencyProperty property in properties )
            {
                // Check whether the submitted object provides a bound property
                // that matches the property parameters:
                var bindingExpression = BindingOperations.GetBindingExpression( target, property );
                
                if( bindingExpression != null )
                {
                    bindingExpression.UpdateSource();
                }
            }

            int childCount = VisualTreeHelper.GetChildrenCount( target );

            for( int i = 0; i < childCount; ++i )
            {
                DependencyObject child = VisualTreeHelper.GetChild( target, i );
                child.UpdateBindingSources( properties );
            }
        }
    }
}
