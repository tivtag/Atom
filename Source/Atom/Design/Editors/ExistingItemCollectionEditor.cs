// <copyright file="ExistingItemCollectionEditor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Design.ExistingItemCollectionEditor{TItem} class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Design
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// Represents an <see cref="UITypeEditor"/> that allows one to add existing
    /// objects to a collection using the property grid.
    /// </summary>
    /// <remarks>
    /// This is similiar to the System.ComponentModel.Design.CollectionEditor, but
    /// instead of adding new items the user can select existing items.
    /// </remarks>
    /// <example>
    /// To use this class simply inherit from it:
    /// <code>
    /// public class MyListEditor : Acid.Design.ExistingItemCollectionEditor{MyItemType}
    /// {
    ///    public MyListEditor()
    ///    {
    ///         protected override IItemSelectionDialog{MyItemType} CreateSelectionDialog()
    ///         {
    ///             return new MySelectionDialog();
    ///         }
    ///    }
    /// }
    /// </code>
    /// and then apply the editor on your property: 
    /// <code>
    /// [Editor( typeof( MyListEditor ), typeof( UITypeEditor ) )]
    /// </code>
    /// </example>
    /// <typeparam name="TItem">The type of item the collection contains.</typeparam>
    public abstract class ExistingItemCollectionEditor<TItem> : UITypeEditor
        where TItem : class, IReadOnlyNameable
    {
        /// <summary>
        /// Creates the <see cref="IItemSelectionDialog{TItem}"/> this ExistingItemCollectionEditor{TItem}
        /// uses internally. Must be overriden by users of this class.
        /// </summary>
        /// <returns>
        /// The <see cref="IItemSelectionDialog{TItem}"/> that should be used by this ExistingItemCollectionEditor{TItem}.
        /// </returns>
        protected abstract IItemSelectionDialog<TItem> CreateSelectionDialog();

        /// <summary>
        /// Overriden. Edits the value of the specified object.
        /// </summary>
        /// <param name="context">
        /// An System.ComponentModel.ITypeDescriptorContext that can be used to gain additional context information.
        /// </param>
        /// <param name="provider">An System.IServiceProvider that this editor can use to obtain services.</param>
        /// <param name="value">The object to edit.</param>
        /// <returns>
        /// The new value of the object. If the value of the object has not changed,
        /// this should return the same object it was passed.
        /// </returns>
        public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value )
        {
            // Create.
            using( var form = new EditorForm( value, this ) )
            {
                this.selectionDialog = this.CreateSelectionDialog();

                // :)))
                form.ShowDialog();

                return value;
            }
        }

        /// <summary>
        /// Overriden. Gets the editor style used by the EditValue method. 
        /// </summary>
        /// <param name="context">
        /// An System.ComponentModel.ITypeDescriptorContext that can be used to gain additional context information.
        /// </param>
        /// <returns>
        /// Returns UITypeEditorEditStyle.Modal.
        /// </returns>
        public override UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context )
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// Gets the items in the given collection.
        /// </summary>
        /// <param name="editValue">The input value to the editor.</param>
        /// <returns>The array of items.</returns>
        protected virtual TItem[] GetItems( object editValue )
        {
            var input = editValue as IEnumerable<TItem>;

            if( input == null )
                return new TItem[0];
            return input.ToArray();
        }

        /// <summary>
        /// Sets the items in the given collection.
        /// </summary>
        /// <param name="editValue">The input value to the editor.</param>
        /// <param name="value">The array of items to set.</param>
        protected virtual void SetItems( object editValue, TItem[] value )
        {
            if( editValue != null )
            {
                var list = editValue as ICollection<TItem>;

                if( list != null )
                {
                    list.Clear();
                    for( int i = 0; i < value.Length; ++i )
                        list.Add( value[i] );
                }
            }
        }

        /// <summary>
        /// The IItemSelectionDialog that allows the user of the class to plugin
        /// their custom dialog that selects an item.
        /// </summary>
        private IItemSelectionDialog<TItem> selectionDialog;

        /// <summary>
        /// Represents the form that gets displayed when the user wants to edit his collection.
        /// This class can't be inherited.
        /// </summary>
        private sealed class EditorForm : Form
        {
            #region [ Initialization ]

            /// <summary>
            /// Initializes a new instance of the EditorForm class.
            /// </summary>
            /// <param name="editValue">The collection value beeing edited.</param>
            /// <param name="editor">The editor that owns the new EditorForm.</param>
            public EditorForm( object editValue, ExistingItemCollectionEditor<TItem> editor )
            {
                this.InitializeComponent();

                this.editor    = editor;
                this.editValue = editValue;
                var items      = this.Items;

                // Fill initial list box:
                for( int i = 0; i < items.Length; ++i )
                    this.listBox.Items.Add( items[i] );

                // Select first item:
                if( this.listBox.Items.Count > 0 )
                    this.listBox.SelectedIndex = 0;
            }

            #endregion

            #region [ Properties ]

            /// <summary>
            /// Gets or sets the items the edited collection contains.
            /// </summary>
            private TItem[] Items
            {
                get
                {
                    return this.editor.GetItems( this.editValue );
                }

                set
                {
                    this.editor.SetItems( this.editValue, value );
                }
            }

            #endregion

            #region [ Methods ]

            /// <summary>
            /// Adds the given item to the collection.
            /// </summary>
            /// <param name="item">
            /// The item to add.
            /// </param>
            private void AddItem( TItem item )
            {
                this.listBox.Items.Add( item );

                // Refresh selecton:
                this.listBox.ClearSelected();
                this.listBox.SelectedIndex = this.listBox.Items.Count - 1;

                RefreshUserCollection();
            }

            /// <summary>
            /// Refreshes the collection of the user by
            /// copying the current state of the list box.
            /// </summary>
            private void RefreshUserCollection()
            {
                TItem[] newItems = new TItem[this.listBox.Items.Count];

                for( int i = 0; i < newItems.Length; ++i )
                    newItems[i] = (TItem)this.listBox.Items[i];

                this.Items = newItems;
            }

            /// <summary>
            /// Swaps the items at the given indices.
            /// </summary>
            /// <param name="indexA">The first index.</param>
            /// <param name="indexB">The second index.</param>
            private void SwapItems( int indexA, int indexB )
            {
                var itemA = listBox.Items[indexA];
                var itemB = listBox.Items[indexB];

                listBox.Items[indexA] = itemB;
                listBox.Items[indexB] = itemA;

                this.RefreshUserCollection();
            }

            /// <summary>
            /// Called when the user clicks on the OK button.
            /// </summary>
            /// <param name="sender">
            /// The sender of the event.
            /// </param>
            /// <param name="e">
            /// The EventArgs that contain the event data.
            /// </param>
            private void OnOkButtonClicked( object sender, EventArgs e )
            {
                this.DialogResult = DialogResult.OK;
            }

            /// <summary>
            /// Called when the user clicks on the Add button.
            /// </summary>
            /// <param name="sender">
            /// The sender of the event.
            /// </param>
            /// <param name="e">
            /// The EventArgs that contain the event data.
            /// </param>
            private void OnAddButtonClicked( object sender, EventArgs e )
            {
                var dialog = this.editor.CreateSelectionDialog();

                if( dialog.ShowDialog() )
                {
                    AddItem( dialog.SelectedItem );
                }
            }

            /// <summary>
            /// Called when the user clicks on the Remove button.
            /// </summary>
            /// <param name="sender">
            /// The sender of the event.
            /// </param>
            /// <param name="e">
            /// The EventArgs that contain the event data.
            /// </param>
            private void OnRemoveButtonClicked( object sender, EventArgs e )
            {
                this.DialogResult = DialogResult.None;
                int index = this.listBox.SelectedIndex;
                if( index < 0 || index >= this.listBox.Items.Count )
                    return;

                this.listBox.Items.RemoveAt( index );

                // Refresh selecton:
                this.listBox.ClearSelected();
                
                if( this.listBox.Items.Count >= 1 )
                {
                    int value = index - 1;
                    int max = this.listBox.Items.Count - 1;
                    value = (value > max) ? max : value;
                    value = (value < 0) ? 0 : value;

                    this.listBox.SelectedIndex = value;
                }

                this.RefreshUserCollection();
            }

            /// <summary>
            /// Called when the user clicks on the Cancel button.
            /// </summary>
            /// <param name="sender">
            /// The sender of the event.
            /// </param>
            /// <param name="e">
            /// The EventArgs that contain the event data.
            /// </param>
            private void OnCancelButtonClicked( object sender, EventArgs e )
            {
                this.DialogResult = DialogResult.Cancel;
            }

            /// <summary>
            /// Called when the user clicks on the Up button.
            /// </summary>
            /// <param name="sender">
            /// The sender of the event.
            /// </param>
            /// <param name="e">
            /// The EventArgs that contain the event data.
            /// </param>
            private void OnUpButtonClicked( object sender, EventArgs e )
            {
                int index = this.listBox.SelectedIndex;
                if( index <= 0 || this.listBox.Items.Count <= 1 )
                    return;

                int index2 = index - 1;
                SwapItems( index, index2 );

                this.listBox.ClearSelected();
                this.listBox.SelectedIndex = index2;
            }

            /// <summary>
            /// Called when the user clicks on the Down button.
            /// </summary>
            /// <param name="sender">
            /// The sender of the event.
            /// </param>
            /// <param name="e">
            /// The EventArgs that contain the event data.
            /// </param>
            private void OnDownButtonClicked( object sender, EventArgs e )
            {
                int index = this.listBox.SelectedIndex;
                int count = this.listBox.Items.Count;
                if( index >= count - 1 || count <= 1 )
                    return;

                int index2 = index + 1;
                SwapItems( index, index2 );

                this.listBox.ClearSelected();
                this.listBox.SelectedIndex = index2;
            }

            #endregion

            #region [ Fields ]

            /// <summary>
            /// The collection value currently beeing edited.
            /// </summary>
            private readonly object editValue;

            /// <summary>
            /// The editor that owns this EditorForm.
            /// </summary>
            private readonly ExistingItemCollectionEditor<TItem> editor;

            #endregion

            #region [ Design Related ]

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
                this.buttonAdd = new System.Windows.Forms.Button();
                this.buttonRemove = new System.Windows.Forms.Button();
                this.listBox = new System.Windows.Forms.ListBox();
                this.buttonCancel = new System.Windows.Forms.Button();
                this.buttonUp = new System.Windows.Forms.Button();
                this.buttonDown = new System.Windows.Forms.Button();
                this.buttonOK = new System.Windows.Forms.Button();

                this.SuspendLayout();

                this.buttonAdd.Location = new System.Drawing.Point( 14, 295 );
                this.buttonAdd.Name = "buttonAdd";
                this.buttonAdd.Size = new System.Drawing.Size( 84, 23 );
                this.buttonAdd.TabIndex = 0;
                this.buttonAdd.Text = "Add";
                this.buttonAdd.UseVisualStyleBackColor = true;
                this.buttonAdd.Click += new System.EventHandler( this.OnAddButtonClicked );

                this.buttonRemove.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.buttonRemove.Location = new System.Drawing.Point( 104, 295 );
                this.buttonRemove.Name = "buttonRemove";
                this.buttonRemove.Size = new System.Drawing.Size( 84, 23 );
                this.buttonRemove.TabIndex = 1;
                this.buttonRemove.Text = "Remove";
                this.buttonRemove.UseVisualStyleBackColor = true;
                this.buttonRemove.Click += new System.EventHandler( this.OnRemoveButtonClicked );

                this.listBox.FormattingEnabled = true;
                this.listBox.Location = new System.Drawing.Point( 13, 12 );
                this.listBox.Name = "listBox";
                this.listBox.Size = new System.Drawing.Size( 175, 277 );
                this.listBox.TabIndex = 2;

                this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.buttonCancel.Location = new System.Drawing.Point( 333, 330 );
                this.buttonCancel.Name = "buttonCancel";
                this.buttonCancel.Size = new System.Drawing.Size( 75, 23 );
                this.buttonCancel.TabIndex = 3;
                this.buttonCancel.Text = "Cancel";
                this.buttonCancel.UseVisualStyleBackColor = true;
                this.buttonCancel.Click += new System.EventHandler( this.OnCancelButtonClicked );
               
                this.buttonUp.Location = new System.Drawing.Point( 194, 10 );
                this.buttonUp.Name = "buttonUp";
                this.buttonUp.Size = new System.Drawing.Size( 43, 23 );
                this.buttonUp.TabIndex = 4;
                this.buttonUp.Text = "Up";
                this.buttonUp.UseVisualStyleBackColor = true;
                this.buttonUp.Click += new System.EventHandler( this.OnUpButtonClicked );

                this.buttonDown.Location = new System.Drawing.Point( 194, 39 );
                this.buttonDown.Name = "buttonDown";
                this.buttonDown.Size = new System.Drawing.Size( 43, 23 );
                this.buttonDown.TabIndex = 5;
                this.buttonDown.Text = "Down";
                this.buttonDown.UseVisualStyleBackColor = true;
                this.buttonDown.Click += new System.EventHandler( this.OnDownButtonClicked );

                this.buttonOK.Location = new System.Drawing.Point( 252, 330 );
                this.buttonOK.Name = "buttonOK";
                this.buttonOK.Size = new System.Drawing.Size( 75, 23 );
                this.buttonOK.TabIndex = 6;
                this.buttonOK.Text = "OK";
                this.buttonOK.UseVisualStyleBackColor = true;
                this.buttonOK.Click += new System.EventHandler( this.OnOkButtonClicked );

                this.AcceptButton = this.buttonOK;
                this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.CancelButton = this.buttonCancel;
                this.ClientSize = new System.Drawing.Size( 420, 365 );

                this.Controls.Add( this.buttonOK );
                this.Controls.Add( this.buttonDown );
                this.Controls.Add( this.buttonUp );
                this.Controls.Add( this.buttonCancel );
                this.Controls.Add( this.listBox );
                this.Controls.Add( this.buttonRemove );
                this.Controls.Add( this.buttonAdd );

                this.Cursor = System.Windows.Forms.Cursors.Default;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.Name = "ExistingItemCollectionEditorForm";
                this.ShowIcon = false;
                this.ShowInTaskbar = false;
                this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

                this.Text = typeof( TItem ).Name + " Collection-Editor";
                this.ResumeLayout( false );
            }

            /// <summary>
            /// The ListBox that visualized the collection of items.
            /// </summary>
            private ListBox listBox;

            /// <summary>
            /// The button when clicked allows the user to add a new item to the collection.
            /// </summary>
            private Button buttonAdd;

            /// <summary>
            /// The button when clicked allows the user to remove the currently selected item from the collection.
            /// </summary>
            private Button buttonRemove;

            /// <summary>
            /// The button when clicked allows the user to move the currently selected item
            /// up by one index.
            /// </summary>
            private Button buttonUp;

            /// <summary>
            /// The button when clicked allows the user to move the currently selected item
            /// down by one index.
            /// </summary>
            private Button buttonDown;

            /// <summary>
            /// The button when clicked closes the editor without applying the changes.
            /// </summary>
            private Button buttonCancel;

            /// <summary>
            /// The button when clicked closes the editor while applying the changes to the underyling collection.
            /// </summary>
            private Button buttonOK;

            #endregion
        }
    }
}
