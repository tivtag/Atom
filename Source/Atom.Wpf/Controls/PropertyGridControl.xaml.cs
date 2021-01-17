// <copyright file="PropertyGridControl.xaml.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Controls.PropertyGridControl class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Wpf.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Implements a custom UserControl that wrapps the <see cref="System.Windows.Forms.PropertyGrid"/> 
    /// control for use with the Windows Presentation Foundation (WPF). 
    /// </summary>
    public partial class PropertyGridControl : UserControl
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the object which is displayed within the PropertyGrid.
        /// </summary>
        public object SelectedObject
        {
            get
            {
                return (object)GetValue( SelectedObjectProperty );
            }

            set
            {
                this.propertyGrid.SelectedObject = value;
                SetValue( SelectedObjectProperty, value );
            }
        }

        #endregion
        
        #region [ Dependency Properties ]

        /// <summary>
        /// Indentifies the <see cref="SelectedObject"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register(
            "SelectedObject",
            typeof( object ),
            typeof( PropertyGridControl ),
            new UIPropertyMetadata( 0, new PropertyChangedCallback( PropertyGridControl.OnSelectedObjectChanged ) )
        );

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyGridControl"/> class.
        /// </summary>
        public PropertyGridControl()
        {
            InitializeComponent();
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Called when the selected object changes.
        /// </summary>
        /// <param name="d">The PropertyGridControl whose SelectedObject property has changed.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnSelectedObjectChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            ((PropertyGridControl)d).SelectedObject = e.NewValue;
        }

        #endregion
    }
}
