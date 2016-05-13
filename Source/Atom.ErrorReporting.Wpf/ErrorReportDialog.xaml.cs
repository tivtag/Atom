// <copyright file="ErrorReportDialog.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.Wpf.ErrorReportDialog class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.ErrorReporting.Wpf
{
    using System.Windows;

    /// <summary>
    /// </summary>
    public sealed partial class ErrorReportDialog : Window
    {
        /// <summary>
        /// Initializes a new instance of the ErrorReportDialog class.
        /// </summary>
        /// <param name="mailReporter"></param>
        public ErrorReportDialog( IErrorReporter mailReporter )
        {
            this.InitializeComponent();

            this.mailReporter = mailReporter;
            this.Initialize();
        }

        private void Initialize()
        {
            if( this.mailReporter == null )
            {
                this.buttonSendReport.IsEnabled = false;
                this.buttonSendReport.Visibility = Visibility.Hidden;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool? ShowDialog( IError error )
        {
            this.error = error;
            this.textBoxErrorMessage.Text = error.Description;

            return base.ShowDialog();
        }

        private void OnSendReportButtonClicked( object sender, RoutedEventArgs e )
        {
            this.mailReporter.Report( this.error );
            this.buttonSendReport.IsEnabled = false;

            MessageBox.Show( 
                ErrorResources.ReportSent,
                string.Empty,
                MessageBoxButton.OK,
                MessageBoxImage.Information,
                MessageBoxResult.OK, 
                MessageBoxOptions.None 
            );
        }

        private IError error;
        private readonly IErrorReporter mailReporter;
    }
}
