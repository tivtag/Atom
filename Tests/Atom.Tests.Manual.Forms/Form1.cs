using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Atom.Tests.Manual.Forms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click( object sender, EventArgs e )
        {
            throw new ArgumentException( "Feck!", new NotSupportedException( "lol", new NotImplementedException( "hah" ) ) );
        }
    }
}
