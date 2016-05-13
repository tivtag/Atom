using System;
using System.Windows.Forms;
using Atom.Diagnostics;
using Atom.ErrorReporting.Errors;
using Atom.ErrorReporting.Formatters;
using Atom.ErrorReporting.Reporters;
using Atom.Mail.Mapi;
using System.IO;
using Atom.ErrorReporting;

namespace Atom.Tests.Manual.Forms
{
    static class Program
    {
        public class CustomReporter : IErrorReporter
        {
            public CustomReporter()
            {
                var mailReporter = new MailErrorReporter(
                    "TLoZ-BC",
                    "tick@federrot.at",
                    new MailBodyErrorFormatter(),
                    new MapiMailSender() );

                mailReporter.MailModifier = new Mail.Modifiers.AddAttachmentModifier() {
                    FilePath = Path.GetFullPath( "MyLog.log" )
                };

                this.mailReporter = mailReporter;
            }

            public void Report( IError error )
            {
                using( var dialog = new Atom.ErrorReporting.Dialogs.WinFormsErrorReportDialogFactory( this.mailReporter ).Build() )
                {
                    dialog.Show( error );
                }

                Application.Exit();
            }

            private readonly IErrorReporter mailReporter;
        }
        

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode( UnhandledExceptionMode.Automatic, false );
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            
            var log = new FileLog( "MyLog.log" );
            log.WriteLine( "Hello, you!" );
            log.WriteLine( DateTime.Now.ToString() );
            log.WriteLine( "Hi" );

                        
            var hook = new Atom.ErrorReporting.Hooks.ApplicationThreadExceptionHook(
                new CustomReporter(),
                new ExceptionErrorFactory()
            );

            hook.Hook();

            Application.Run( new Form1() );
        }
    }
}
