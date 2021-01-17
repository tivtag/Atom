// <copyright file="ErrorReportDialog.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.Dialogs.ErrorReportDialog class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.ErrorReporting.Dialogs
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// Represents the default <see cref="IErrorReportDialog"/> implementation for Windows Forms.
    /// This class can't be inherited.
    /// </summary>
    internal sealed partial class ErrorReportDialog : Form, IErrorReportDialog
    {
        /// <summary>
        /// Initializes a new instance of the ErrorReportForm class.
        /// </summary>
        /// <param name="mailReporter">
        /// The optional reporter that allows users to send an error report by e-mail.
        /// </param>
        public ErrorReportDialog( IErrorReporter mailReporter = null )
        {
            this.InitializeComponent();
            this.InitializeTooltips();

            this.mailReporter = mailReporter;
            this.buttonSendReport.Visible = mailReporter != null;
        }

        /// <summary>
        /// Adds all the tooltips.
        /// </summary>
        private void InitializeTooltips()
        {
            new ToolTip().SetToolTip( this.buttonCopyReport, "Copies the error report to the clipboard." );
            new ToolTip().SetToolTip( this.buttonSendReport, "Opens the default mail application and creates a new e-mail filled with the error report." );
            new ToolTip().SetToolTip( this.buttonShowStackTrace, "Opens a new window that shows where within the source code the error has occurred." );
            new ToolTip().SetToolTip( this.buttonClose, "Closes this window." );
        }

        /// <summary>
        /// Shows this IErrorReportDialog.
        /// </summary>
        /// <param name="error">
        /// The <see cref="IError"/> to report to the user.
        /// </param>
        public void Show( IError error )
        {
            this.error = error;
            {
                this.buttonShowStackTrace.Visible = error is IExceptionError;
                this.InitializeErrorInfo( error );
                this.ShowDialog();
            }
            this.error = null;
        }

        /// <summary>
        /// Initializes the text show in <see cref="textBoxErrorInfo"/>.
        /// </summary>
        /// <param name="error">
        /// The <see cref="IError"/> to report to the user.
        /// </param>
        private void InitializeErrorInfo( IError error )
        {            
            var sb = new StringBuilder();
            sb.AppendFormat(
                CultureInfo.InvariantCulture,
                "{0}: {1}",
                error.Name,
                error.Description
            );
            sb.AppendLine();
            
            var exceptionError = error as IExceptionError;
            
            if( exceptionError != null )
            {
                int depth = 1;
                var exception = exceptionError.Exception.InnerException;

                while( exception != null )
                {
                    for( int i = 0; i < depth; ++i )
                    {
                        sb.Append( "    " );
                    }

                    sb.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "{0}: {1}",
                        exception.GetType().Name,
                        exception.Message
                    );

                    sb.AppendLine();

                    ++depth;
                    exception = exception.InnerException;
                }
            }

            this.textBoxErrorInfo.Text = sb.ToString();
        }

        /// <summary>
        /// Called when the user has clicked on the 'Close' button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The EventArgs that contain the event data.
        /// </param>
        private void OnCloseButtonClicked( object sender, EventArgs e )
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Called when the user has clicked on the 'Details..' button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The EventArgs that contain the event data.
        /// </param>
        private void OnShowDetailsButtonClicked( object sender, EventArgs e )
        {
            var exceptionError = this.error as IExceptionError;
            
            using( var dialog = new ExceptionDetailsDialog( exceptionError.Exception ) )
            {
                dialog.ShowDialog();
            }
        }

        /// <summary>
        /// Called when the user has clicked on the 'Send Report' button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The EventArgs that contain the event data.
        /// </param>
        private void OnSendReportButtonClicked( object sender, EventArgs e )
        {
            if( this.mailReporter != null )
            {
                this.mailReporter.Report( this.error );
            }

        }
        /// <summary>
        /// Called when the user has clicked on the 'Send Report' button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The EventArgs that contain the event data.
        /// </param>
        private void OnCopyReportButtonClicked( object sender, EventArgs e )
        {
            try
            {
                Clipboard.SetDataObject( error.ToString(), true );
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message, "Error copying error report to clipboard" );
            }
        }

        /// <summary>
        /// The currently shown error.
        /// </summary>
        private IError error;

        /// <summary>
        /// The optional reporter that allows users to send an error report by e-mail. Might be null.
        /// </summary>
        private readonly IErrorReporter mailReporter;
    }
}
