using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace SwenetDev.Controls {
	/// <summary>
	/// Summary description for WebCustomControl1.
	/// </summary>
	[ParseChildren(true),
	DefaultProperty("Text"),
	ToolboxData("<{0}:DataEditControl runat=server></{0}:DataEditControl>")]
	public class DataEditControl : WebControl, INamingContainer {
		protected System.Web.UI.WebControls.Button UpButton;
		protected System.Web.UI.WebControls.Label ErrorMessage;
		protected System.Web.UI.WebControls.Repeater Repeater1;
		protected System.Web.UI.WebControls.Button DownButton;
		protected System.Web.UI.WebControls.Button NewButton;
		protected System.Web.UI.WebControls.DataGrid DataGrid1;
		protected System.Web.UI.WebControls.Button DeleteButton;

		private IList dataList;
		private bool isEditing;
		private bool isAdding;

		public event DataGridCommandEventHandler UpdateCommand;
		public event EventHandler NewItemEvent;

		[Bindable(true),
		Category("Appearance"),
		DefaultValue("")]
		public string Text {
			get {
				EnsureChildControls();
				return ErrorMessage.Text;
			}

			set {
				EnsureChildControls();
				ErrorMessage.Text = value;
			}
		}

		[Bindable(true),
		DefaultValue(0)]
		public int Count {
			get {
				if ( dataList != null ) {
					return dataList.Count;
				} else {
					return 0;
				}
			}
		}

		[DefaultValue(true),
		Browsable(true)]
		public bool ReordingEnabled {
			set {
				EnsureChildControls();
				UpButton.Visible = value;
				DownButton.Visible = value;
				UpButton.Enabled = value;
				DownButton.Enabled = value;
			}
			get {
				EnsureChildControls();
				return UpButton.Visible && DownButton.Visible;
			}
		}

		[
		Browsable(false),
		DefaultValue(null),
		Description("The content to be shown in the header."),
		PersistenceMode(PersistenceMode.InnerProperty),
		TemplateContainer(typeof(DataGridItem))
		]
		public virtual ITemplate HeaderTemplate {
			get {
				EnsureChildControls();
				return ((TemplateColumn)(DataGrid1.Columns[1])).HeaderTemplate;
			}
			set {
				EnsureChildControls();
				((TemplateColumn)(DataGrid1.Columns[1])).HeaderTemplate = value;
			}
		}

		[
		Browsable(false),
		DefaultValue(null),
		Description("The content to be shown in each item."),
		PersistenceMode(PersistenceMode.InnerProperty),
		TemplateContainer(typeof(DataGridItem))
		]
		public virtual ITemplate ItemTemplate {
			get {
				EnsureChildControls();
				return ((TemplateColumn)(DataGrid1.Columns[1])).ItemTemplate;
			}
			set {
				EnsureChildControls();
				((TemplateColumn)(DataGrid1.Columns[1])).ItemTemplate = value;
			}
		}

		[
		Browsable(false),
		DefaultValue(null),
		Description("The content to be shown in each item in edit mode."),
		PersistenceMode(PersistenceMode.InnerProperty),
		TemplateContainer(typeof(DataGridItem))
		]
		public virtual ITemplate EditItemTemplate {
			get {
				EnsureChildControls();
				return ((TemplateColumn)(DataGrid1.Columns[1])).EditItemTemplate;
			}
			set {
				EnsureChildControls();
				((TemplateColumn)(DataGrid1.Columns[1])).EditItemTemplate = value;
			}
		}

		public IList DataList {
			get {
				return dataList;
			}
			set {
				dataList = value;
			}
		}

		public bool Editing {
			get {
				return isEditing;
			}
		}

		public DataEditControl() {
			isEditing = false;
			isAdding = false;
		}

		protected override void CreateChildControls()  {
			Controls.Add( new LiteralControl( "<p>" ) );

			ErrorMessage = new Label();
			ErrorMessage.ForeColor = Color.Red;
			Controls.Add( ErrorMessage );

			Controls.Add( new LiteralControl( "</p>" ) );
            
			DataGrid1 = new DataGrid();
			DataGrid1.EditItemStyle.BackColor = Color.WhiteSmoke;
			DataGrid1.HeaderStyle.BackColor = Color.FromArgb( 148, 175, 192 );
			DataGrid1.HeaderStyle.Font.Bold = true;
			DataGrid1.CellPadding = 5;
			DataGrid1.BorderWidth = 1;
			DataGrid1.BorderColor = Color.LightGray;
			DataGrid1.Width = 600;
			DataGrid1.AutoGenerateColumns = false;
			DataGrid1.ItemCommand += new DataGridCommandEventHandler( DataGrid1_Command );
			TemplateColumn c1 = new TemplateColumn();
			c1.HeaderTemplate = new CheckColumnTemplate( ListItemType.Header );
			c1.HeaderStyle.Width = 21;
			c1.ItemTemplate = new CheckColumnTemplate( ListItemType.Item );
			c1.ItemStyle.VerticalAlign = VerticalAlign.Top;
			c1.Visible = false;
			DataGrid1.Columns.Add( c1 );

			TemplateColumn c2 = new TemplateColumn();
			DataGrid1.Columns.Add( c2 );
			DataGrid1.ItemCreated += new DataGridItemEventHandler(DataGrid1_ItemCreated);
			DataGrid1.EditCommand += new DataGridCommandEventHandler(DataGrid1_EditCommand);
			DataGrid1.CancelCommand += new DataGridCommandEventHandler(DataGrid1_CancelCommand);
			DataGrid1.UpdateCommand += new DataGridCommandEventHandler(DataGrid1_UpdateCommand);

			ButtonColumn c3 = new ButtonColumn();
			c3.ButtonType = ButtonColumnType.PushButton;
			c3.HeaderStyle.Width = 5;
			c3.ItemStyle.Width = 5; 
			c3.HeaderText = "Shift Up";
			c3.CommandName = "UpClick";
			c3.Text = "\u2191";
			c3.Visible = false;
			DataGrid1.Columns.Add( c3 );

			ButtonColumn c4 = new ButtonColumn();
			c4.ButtonType = ButtonColumnType.PushButton;
			c4.ItemStyle.Width = 5;
			c4.HeaderStyle.Width = 5;
			c4.HeaderText = "Shift Down";
			c4.CommandName = "DownClick";
			c4.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
			c4.Text = "\u2193";
			c4.Visible = false;
			DataGrid1.Columns.Add( c4 );

			ButtonColumn c5 = new ButtonColumn();
			c5.ButtonType = ButtonColumnType.PushButton;
			c5.ItemStyle.Width = 25;
			c5.CommandName = "Delete";
			c5.HeaderText = "Delete";
			c5.Text = "Delete";
			c5.Visible = false;
			DataGrid1.Columns.Add( c5 );

			Controls.Add( DataGrid1 );

			Controls.Add( new LiteralControl( "<p>" ) );

			NewButton = new Button();
			NewButton.ID = "NewButton";
			NewButton.CssClass = "defaultButton";
			NewButton.Text = "New";
			NewButton.CausesValidation = false;
			NewButton.Click += new EventHandler( NewButton_Click );
			Controls.Add( NewButton );

			Controls.Add( new LiteralControl( "&nbsp;" ) );

			DeleteButton = new Button();
			DeleteButton.ID = "DeleteButton";
			DeleteButton.CssClass = "defaultButton";
			DeleteButton.Text = "Delete";
			DeleteButton.Enabled = false;
			DeleteButton.CausesValidation = false;
			DeleteButton.Click += new EventHandler(DeleteButton_Click);
			//Controls.Add( DeleteButton );

			Controls.Add( new LiteralControl( "&nbsp;" ) );

			UpButton = new Button();
			UpButton.ID = "UpButton";
			UpButton.CssClass = "defaultButton";
			UpButton.Text = "Up";
			UpButton.Enabled = false;
			UpButton.CausesValidation = false;
			UpButton.CommandName = "MoveUp";
			UpButton.Command += new CommandEventHandler( OnMoveCommand );
			//Controls.Add( UpButton );

			Controls.Add( new LiteralControl( "&nbsp;" ) );

			DownButton = new Button();
			DownButton.ID = "DownButton";
			DownButton.CssClass = "defaultButton";
			DownButton.Text = "Down";
			DownButton.Enabled = false;
			DownButton.CausesValidation = false;
			DownButton.CommandName = "MoveDown";
			DownButton.Command += new CommandEventHandler( OnMoveCommand );
			//Controls.Add( DownButton );

			Controls.Add( new LiteralControl( "</p>" ) );
		}

		protected override object SaveViewState() {
			object baseState = base.SaveViewState();
			object[] myState = new object[4];

			myState[0] = baseState;
			myState[1] = this.dataList;
			myState[2] = isEditing;
			myState[3] = isAdding;

			return myState;
		}

		protected override void LoadViewState(object savedState) {
			object[] myState = (object[])savedState;

			base.LoadViewState( myState[0] );
			dataList = (IList)myState[1];
			isEditing = (bool)myState[2];
			isAdding = (bool)myState[3];
		}

		public override void DataBind() {
			base.DataBind ();

			bindList();
		}

		#region Button Event Handlers
		/// <summary>
		/// Respond to the new button event.  This creates a temporarily empty item
		/// in the list, which will be updated when Apply is clicked or
		/// removed when Cancel is clicked.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments</param>
		private void NewButton_Click(object sender, System.EventArgs e) {
			if ( !isEditing ) {
				isEditing = true;
				isAdding = true;

				if ( NewItemEvent != null ) {
					NewItemEvent( this, e );
				}

				DataGrid1.EditItemIndex = dataList.Count - 1;
				bindList();
				DataGrid1.Columns[2].Visible = false;
				DataGrid1.Columns[3].Visible = false;
				DataGrid1.Columns[4].Visible = false;
		
			}
		}

	

		/// <summary>
		/// Respond to the delete button event.  Delete all checked items.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments</param>
		private void DeleteButton_Click(object sender, System.EventArgs e) {
			IList checkedList = getCheckedList();
			IList tempItems = new ArrayList( dataList );

			foreach ( int index in checkedList ) {
				dataList.Remove( tempItems[index] );
				if( tempItems[index].GetType() == typeof( DBAdapter.Materials.MaterialInfo ) ) {
					DBAdapter.Materials.MaterialInfo file = ((DBAdapter.Materials.MaterialInfo)(tempItems[index]));
					string tempDir = ConfigurationSettings.AppSettings["MaterialsTempDir"] +
										Context.User.Identity.Name + "\\";
					string permDir = ConfigurationSettings.AppSettings["MaterialsDir"] +
										Context.Request.Params["moduleID"] + "\\";
						
					// Remove the file from the hard drive
					if( File.Exists( tempDir + file.Link ) )
						File.Delete( tempDir + file.Link );
					else if( File.Exists( permDir + file.Link ) )
						File.Delete( permDir + file.Link );

					// Remove the material from the database
					int matID = file.MatID;
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = new SqlConnection( Globals.ConnectionString );
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.CommandText = "RemoveMaterial";
					cmd.Parameters.Add( new SqlParameter( "@MaterialID",  matID ) );

					try 
					{
						cmd.Connection.Open();
						cmd.ExecuteNonQuery();
					} 
					catch ( SqlException err ) 
					{
						throw;
					} 
					finally 
					{
						cmd.Connection.Close();
					}

				}
				
			}
			bindList();		
			if (dataList.Count <= 1) 
			{
				
				DataGrid1.Columns[2].Visible = false;
				DataGrid1.Columns[3].Visible = false;

			}

		}

		/// <summary>
		/// Respond to a move button command (up or down).
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		public void OnMoveCommand(object sender, System.Web.UI.WebControls.CommandEventArgs e) {
			IList newSelectedIndexes = new ArrayList( dataList.Count );
			bool changed = false;

			// List of checked indices.
			IList checkedList = getCheckedList();

			// If there are checked items...
			if ( checkedList.Count > 0 ) {
				// If command is move up...
				if ( e.CommandName.Equals( "MoveUp" ) ) {
					// If the first item is not checked, an up move is
					// allowed, so move all the checked items up.
					if ( !checkedList.Contains( 0 ) ) {
						foreach ( int i in checkedList ) {
							changed = true;
							//CommandEventArgs newE = new CommandEventArgs(e.CommandName, i );
							//this.MoveCommand( this, newE );
							swap( i, -1 );
							newSelectedIndexes.Add( i - 1 );
						}
					}
				} else {  // move down
					// If the last item is not checked, an down move is
					// allowed, so move all the checked items down.
					if ( !checkedList.Contains( dataList.Count - 1 ) ) {
						for ( int i = checkedList.Count - 1; i >= 0; i-- ) {
							changed = true;
//							CommandEventArgs newE = new CommandEventArgs(e.CommandName, (int)checkedList[i] );
//							this.MoveCommand( this, newE );
							swap( (int)checkedList[i], 1 );
							newSelectedIndexes.Add( (int)checkedList[i] + 1 );
						}
					}
				}

				// If a change occurred, update the display, ensure all
				// previously checked items are checked, and update the
				// button states.

				if ( changed ) {
					bindList();
					int j = 0;

					foreach ( DataGridItem dli in DataGrid1.Items ) {
						if ( newSelectedIndexes.Contains(j) ) {
							((CheckBox)(dli.Cells[0].FindControl("ItemCheckBox"))).Checked = true;
						} else {
							((CheckBox)(dli.Cells[0].FindControl("ItemCheckBox"))).Checked = false;
						}

						j++;
					}

					enableButtons();
				}
			}
		}

		/// <summary>
		/// Swap the item at the given index with the adjacent item in the
		/// given direction.
		/// </summary>
		/// <param name="index">The index of the item to swap.</param>
		/// <param name="direction">The direction of the adjacent item to swap
		/// with, -1 for the previous item and 1 for the next item.</param>
		private void swap( int index, int direction ) {
			object temp = null;

			temp = dataList[index + direction];
			dataList[index + direction] = dataList[index];
			//((Topic.TopicInfo)dataList[index + direction]).OrderId = index + direction + 1;
			dataList[index] = temp;
			//((Topic.TopicInfo)temp).OrderId = index + 1;
		}

		#endregion

		#region DataGrid Event Handlers
		/// <summary>
		/// Respond to the edit command of the data grid.
		/// </summary>
		/// <param name="source">The source of the event.</param>
		/// <param name="e">The event arguments.</param>
		private void DataGrid1_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e) {
			if ( !isEditing ) {
				isEditing = true;
				DataGrid1.EditItemIndex = (int)e.Item.ItemIndex;
				bindList();
			}
		}

		/// <summary>
		/// Respond to the cancel command of the data grid.
		/// </summary>
		/// <param name="source">The source of the event.</param>
		/// <param name="e">The event arguments.</param>
		private void DataGrid1_CancelCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e) {
			int index = e.Item.ItemIndex;

			if ( isAdding ) {
				DataList.RemoveAt( index );
			}

			isAdding = false;
			isEditing = false;

			DataGrid1.EditItemIndex = -1;
			bindList();
			if (DataGrid1.Items.Count == 0) DataGrid1.Columns[4].Visible = false;
			displayReOrgColumn();
		}

		/// <summary>
		/// Respond to the update command of the data grid.
		/// </summary>
		/// <param name="source">The source of the event.</param>
		/// <param name="e">The event arguments.</param>
		private void DataGrid1_UpdateCommand(object source, DataGridCommandEventArgs e) {
			if ( this.UpdateCommand != null ) {
				isEditing = false;
				isAdding = false;

				if ( UpdateCommand != null ) {
					UpdateCommand( this, e );
				}

				DataGrid1.EditItemIndex = -1;
				bindList();
			}
		}

		/// <summary>
		/// Respond to the event generated when an item is created in the
		/// data grid control.  This involves adding event handlers for the
		/// checkboxes.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		private void DataGrid1_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e) {
			ListItemType itemType = e.Item.ItemType;

			// Only look for checkbox control if an item or alternating item
			// is created.
			if ( itemType == ListItemType.Item
				|| itemType == ListItemType.AlternatingItem ) {
				CheckBox cb = (CheckBox)(e.Item.Cells[0].FindControl("ItemCheckBox"));
				cb.CheckedChanged += new EventHandler(OnCheckedChanged);

				// TODO: This is not needed because no authors are editable, and materials
				// are all new.
//				if ( ((SwenetDev.DBAdapter.Info)(dataList[e.Item.ItemIndex])).Id != 0 ) {
//					foreach ( Control c in e.Item.Cells[1].Controls ) {
//						if ( c.GetType() == typeof( LinkButton ) ) {
//							LinkButton lb = (LinkButton)c;
//							lb.Enabled = false;
//							lb.ForeColor = Color.Black;
//						}
//					}
//				}
			} else if ( itemType == ListItemType.Header ) {
				CheckBox cb = (CheckBox)(e.Item.Cells[0].FindControl("HeaderCheckBox"));
				cb.CheckedChanged += new EventHandler(OnCheckAll);
			}
		}

		#endregion

		#region Check Events
		/// <summary>
		/// Handle the event generated when an item checkbox is checked
		/// or unchecked.  Update the button states.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		private void OnCheckedChanged(object sender, EventArgs e) {
			enableButtons();
		}

		/// <summary>
		/// Handle the event generated when the select all checkbox
		/// is checked or unchecked.  This causes all of the item
		/// checkboxes to be checked or unchecked appropriately.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnCheckAll(object sender, EventArgs e) {
			CheckBox cb = (CheckBox)sender;

			setChecked( cb.Checked );
			enableButtons();
		}
		#endregion

		/// <summary>
		/// Bind the list of items to the web control.
		/// </summary>
		private void bindList() {
			DataGrid1.DataSource = dataList;
			DataGrid1.DataBind();
			enableButtons();

		}

		/// <summary>
		/// Obtain a list item indices that are checked.
		/// </summary>
		/// <returns>A list of checked item indices.</returns>
		private IList getCheckedList() {
			IList retVal = new ArrayList();

			for ( int i = 0; i < DataGrid1.Items.Count; i++ ) {
				CheckBox cb = (CheckBox)(DataGrid1.Items[i].Cells[0].FindControl("ItemCheckBox"));
				if ( cb != null ) {
					if ( cb.Checked ) {
						retVal.Add( i );
					}
				}
			}

			return retVal;
		}

		/// <summary>
		/// Set the state of all the item check boxes.
		/// </summary>
		/// <param name="checkState">The state desired, true for checked,
		/// false for unchecked.</param>
		private void setChecked( bool checkState ) {
			foreach ( DataGridItem dgi in DataGrid1.Items ) {
				((CheckBox)(dgi.Cells[0].FindControl("ItemCheckBox"))).Checked = checkState;
			}
		}

		/// <summary>
		/// Sets the CheckBox at the passed index to being selected
		/// </summary>
		/// <param name="index">The index of the CheckBox that is being selected</param>
		public  void setChecked(int index) {

			DataGridItem dgi = DataGrid1.Items[index];
			((CheckBox) (dgi.Cells[0].FindControl("ItemCheckBox"))).Checked = true;

		}

		/// <summary>
		/// Sets the CheckBox at the passed index to being un-selected
		/// </summary>
		/// <param name="index">The index of the CheckBox that is being selected</param>
		public void setUnChecked(int index) {

			DataGridItem dgi = DataGrid1.Items[index];
			((CheckBox) (dgi.Cells[0].FindControl("ItemCheckBox"))).Checked = false;

		}

		/// <summary>
		/// Enable or disable the command buttons as specified.
		/// </summary>
		/// <param name="add">Add button state.</param>
		/// <param name="delete">Delete button state.</param>
		/// <param name="up">Up button state.</param>
		/// <param name="down">Down button state.</param>
		private void enableButtons() {
			bool add = true;
			bool delete = true;
			bool up = true;
			bool down = true;

			if ( isEditing || isAdding ) {
				add = false;
				delete = false;
				up = false;
				down = false;
			} else if ( dataList.Count == 0 ) {
				delete = false;
				up = false;
				down = false;
			} else {
				IList checkedItems = this.getCheckedList();

				if ( checkedItems.Count == 0 ) {
					delete = false;
					up = false;
					down = false;
				} else {
					if ( checkedItems.Contains( 0 ) ) {
						up = false;
					}
					if ( checkedItems.Contains( dataList.Count - 1 ) ) {
						down = false;
					}
				}
			}

			NewButton.Enabled = add;
			DeleteButton.Enabled = delete;
			UpButton.Enabled = up;
			DownButton.Enabled = down;
		}

		public bool validate() {
			if ( dataList.Count == 0 ) {
				Text = "At least one entry is required.";
				return false;
			} else if ( isEditing ) {
				return false;
			} else {
				Text = "";
			}

			return true;
		}

		public class CheckColumnTemplate : ITemplate {
			private ListItemType type;

			public CheckColumnTemplate( ListItemType type ) {
				this.type = type;
			}

			#region ITemplate Members

			public void InstantiateIn(Control container) {
				CheckBox cb = new CheckBox();
				cb.AutoPostBack = true;
				cb.Visible = true;

				switch( type ) {
					case ListItemType.Header:
						cb.ID = "HeaderCheckBox";
						break;
					case ListItemType.Item:
						cb.ID = "ItemCheckBox";
						break;
				}

				container.Controls.Add( cb );
			}

			#endregion

		}

		public void DataGrid1_Command(object sender, System.Web.UI.WebControls.DataGridCommandEventArgs e) 
		{
			if (e.CommandName.Equals("UpClick") || e.CommandName.Equals("DownClick") || e.CommandName.Equals("Delete")) 
			{

				this.disableButtons();
				DataGrid buttonGrid = (DataGrid) sender;
				Button myButton = (Button) e.CommandSource;
				int index = 0;
				bool clickedButtonFound = false;

				if (e.CommandName.Equals("UpClick")) 
				{

					while (!clickedButtonFound) 
					{

						if (DataGrid1.Items[index].Cells[2].Controls.Contains(myButton)) clickedButtonFound = true;
						index++;

					}

					if (index != 1) 
					{

						setChecked((index - 1));
						CommandEventArgs ce = new CommandEventArgs("MoveUp", e);
						OnMoveCommand(sender, ce);
						setUnChecked((index - 2));

					}
					else DataGrid1.Items[0].Cells[2].Enabled = false;

				}

				if (e.CommandName.Equals("DownClick")) 
				{

					while (!clickedButtonFound) 
					{

						if (DataGrid1.Items[index].Cells[3].Controls.Contains(myButton)) clickedButtonFound = true;
						index++;

					}

					if (!(index > (DataGrid1.Items.Count - 1))) 
					{

						setChecked((index - 1));
						CommandEventArgs ce = new CommandEventArgs("MoveDown", e);
						OnMoveCommand(sender, ce);
						setUnChecked((index));

					}
					else DataGrid1.Items[(DataGrid1.Items.Count - 1)].Cells[3].Enabled = false;

				}

				if (e.CommandName.Equals("Delete")) 
				{

					while (!clickedButtonFound) 
					{

						if (DataGrid1.Items[index].Cells[4].Controls.Contains(myButton)) clickedButtonFound = true;
						index++;

					}

					setChecked((index - 1));
					DeleteButton_Click(sender, e);

					if (DataGrid1.Items.Count == 0) DataGrid1.Columns[4].Visible = false;
				}
				
			}
			else if (e.CommandName.Equals("edit")) 
			{

				DataGrid1.Columns[2].Visible = false;
				DataGrid1.Columns[3].Visible = false;
				DataGrid1.Columns[4].Visible = false;

			}


		}

		public void disableButtons() 
		{

			((Button) DataGrid1.Items[0].Cells[2].Controls[0]).Enabled = false;
			((Button) DataGrid1.Items[(DataGrid1.Items.Count - 1)].Cells[3].Controls[0]).Enabled = false;
			
		}			

		public void displayReOrgColumn() 
		{

			if (dataList.Count > 1) 
			{
				
				DataGrid1.Columns[2].Visible = true;
				DataGrid1.Columns[3].Visible = true;
				DataGrid1.Columns[4].Visible = true;


			}
			else if (dataList.Count == 1){
				
				DataGrid1.Columns[2].Visible = false;
				DataGrid1.Columns[3].Visible = false;
				DataGrid1.Columns[4].Visible = true;


			}

		}

		public void disableAllButtons() 
		{

			DataGrid1.Columns[2].Visible = false;
			DataGrid1.Columns[3].Visible = false;
			DataGrid1.Columns[4].Visible = false;

		}
	}
}
