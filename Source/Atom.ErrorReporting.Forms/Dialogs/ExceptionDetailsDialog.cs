// <copyright file="ExceptionDetailsDialog.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.Dialogs.ExceptionDetailsDialog class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.ErrorReporting.Dialogs
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Forms;

    /// <summary>
    /// Used to visualize the stack trace of an exception.
    /// This class can't be inherited.
    /// </summary>
    internal sealed partial class ExceptionDetailsDialog : Form
    {
        /// <summary>
        /// Initializes a new instance of the ExceptionDetailsDialog class.
        /// </summary>
        /// <param name="exception">
        /// The exception whose details should be visualized.
        /// </param>
        public ExceptionDetailsDialog( Exception exception )
        {
            Contract.Requires<ArgumentNullException>( exception != null );

            this.InitializeComponent();
            this.InitializeTitle( exception );
            this.InitializeTreeView( exception );
        }

        /// <summary>
        /// Initializes the titile of this ExceptionDetailsDialog.
        /// </summary>
        /// <param name="exception">
        /// The exception whose details should be visualized.
        /// </param>
        private void InitializeTitle( Exception exception )
        {
            this.Text = "Stack Trace: " + exception.GetType().Name;

            exception = exception.InnerException;
            while( exception != null )
            {
                this.Text += " -> " + exception.GetType().Name;

                exception = exception.InnerException;
            }
        }

        /// <summary>
        /// Initializes the tree view control of this ExceptionDetailsDialog.
        /// </summary>
        /// <param name="exception">
        /// The exception whose details should be visualized.
        /// </param>
        private void InitializeTreeView( Exception exception )
        {
            this.AddExceptionInformation( exception );
            this.AddStackTrace( exception );

            this.treeViewStackTrace.ExpandAll();
        }
        
        /// <summary>
        /// Adds information about the exception to the tree view control of this ExceptionDetailsDialog.
        /// </summary>
        /// <param name="exception">
        /// The exception whose details should be visualized.
        /// </param>
        private void AddExceptionInformation( Exception exception )
        {
            TreeNode exceptionNode = null;

            while( exception != null )
            {
                string name = exception.GetType().FullName;

                if( exceptionNode == null )
                {
                    exceptionNode = this.treeViewStackTrace.Nodes.Add( name );
                }
                else
                {
                    exceptionNode = exceptionNode.Nodes.Add( name );
                }

                exceptionNode.Nodes.Add( exception.Message );

                if( !string.IsNullOrWhiteSpace( exception.StackTrace ) )
                {
                    exceptionNode.Nodes.Add( exception.StackTrace );
                }

                exception = exception.InnerException;
            }
        }

        /// <summary>
        /// Adds the stack trace ofthe exception to the tree view control of this ExceptionDetailsDialog.
        /// </summary>
        /// <param name="exception">
        /// The exception whose details should be visualized.
        /// </param>
        private void AddStackTrace( Exception exception )
        {
            var stackTrace = exception.StackTrace;
            var stackFrames = stackTrace.Split( new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries );

            TreeNode parentNode = null;

            foreach( string frame in stackFrames )
            {
                if( parentNode == null )
                {
                    parentNode = this.treeViewStackTrace.Nodes.Add( frame );
                }
                else
                {
                    parentNode = parentNode.Nodes.Add( frame );
                }
            }
        }
    }
}
