namespace SwenetDev.Controls {
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using SwenetDev.DBAdapter;
	using System.Collections;

	/// <summary>
	///	A control for adding and editing objectives for a SWEnet module.
	/// </summary>
	public class ObjectivesControl : System.Web.UI.UserControl, IEditControl {
		
		public static int MaxLength {
			get{ return DBAdapter.Modules.getColumnMaxLength("Objectives","ObjectiveText"); }
		}

		protected DataEditControl ObjectivesEditor;

		private void Page_Load(object sender, System.EventArgs e) {
			ObjectivesEditor.Text = "";
			ObjectivesEditor.displayReOrgColumn();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.ObjectivesEditor.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.ObjectivesEditor_UpdateCommand);
			this.ObjectivesEditor.NewItemEvent += new System.EventHandler(this.ObjectivesEditor_NewItemEvent);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		/// <summary>
		/// Respond to the update event, when an item is being edited and
		/// subsequently updated, by updating the info object of the
		/// item being edited.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments</param>
		private void ObjectivesEditor_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e) {
			DropDownList ddl = (DropDownList)(e.Item.Cells[1].FindControl("BloomDrop"));
			TextBox tb = (TextBox)(e.Item.Cells[1].FindControl("ObjectiveTextbox"));
            int selected = e.Item.ItemIndex;
			string newText = SwenetDev.Globals.parseTextInput( tb.Text );
			
			Objectives.ObjectiveInfo oi = ((Objectives.ObjectiveInfo)ObjectivesEditor.DataList[selected]);
			oi.Text = newText;
			oi.BloomLevel = ddl.SelectedValue;
			ObjectivesEditor.DataList[selected] = oi;
			ObjectivesEditor.DataBind();

			if( hasDuplicates() ) {
				ObjectivesEditor.Text = "That objective has already been added.";
				ObjectivesEditor.DataList.RemoveAt(selected);
			} else if( oi.Text.Length > MaxLength ) {
				ObjectivesEditor.Text = "That entry exceeds the maximum length of: " + MaxLength;
				ObjectivesEditor.DataList.RemoveAt(selected);
			} else {
				ObjectivesEditor.Text = "";
			}
			
		}

		/// <summary>
		/// Checks the Objectives control for duplicates
		/// </summary>
		/// <returns>False if any duplicates exist, true otherwise.</returns>
		public bool hasDuplicates() {
			
			bool retval = false;
			ArrayList knownObjectives = new ArrayList();

			foreach( Objectives.ObjectiveInfo oInfo in ObjectivesEditor.DataList ) {
				
				string temp = oInfo.BloomLevel + oInfo.Text;
				if( knownObjectives.Contains( temp ) ) {
					retval = true;
					break;
				} else {
					knownObjectives.Add( temp );
				}
			}

			return retval;
		}


		/// <summary>
		/// Respond to the new item event, when the user elects to create a
		/// new item, by creating the specific info class and adding it to
		/// the editor control's data list.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments</param>
		private void ObjectivesEditor_NewItemEvent(object sender, System.EventArgs e) 
		{
			ObjectivesEditor.DataList.Add( new Objectives.ObjectiveInfo( "", "" ) );
		}

		#region IEditControl Members

		/// <summary>
		/// Initialize the list of objectives.
		/// </summary>
		/// <param name="moduleID">The module identifier for the
		/// initial list of objectives or 0 if it should be initially empty.</param>
		public void initList( int moduleID ) {
			if ( moduleID == 0 ) {
				ObjectivesEditor.DataList = new ArrayList();
				ObjectivesEditor.DataBind();
			} else if ( moduleID > 0 ) {
				ObjectivesEditor.DataList = Objectives.getAll( moduleID );
				ObjectivesEditor.DataBind();
			} else {
				throw new ArgumentException( "Invalid module identifier.", "moduleID" );
			}
		}

		/// <summary>
		/// Insert all of the objectives for the given module.
		/// </summary>
		/// <param name="moduleID">The module to add the objectives for.</param>
		/// <param name="removePrevious">Whether to remove any existing
		/// objectives for the given module.</param>
		public void insertAll( int moduleID, bool removePrevious ) {
			if ( removePrevious ) {
				Objectives.removeAll( moduleID );
			}

			Objectives.addAll( moduleID, ObjectivesEditor.DataList );
		}

		/// <summary>
		/// Validate the objectives list.
		/// </summary>
		/// <returns>True if all input is valid.</returns>
		public bool validate() {
			return ObjectivesEditor.validate() & !hasDuplicates();
		}

		#endregion

		/// <summary>
		/// Given the string of the chosen Bloom Level, this method returns the
		/// appropriate index for the selection.
		/// </summary>
		/// <param name="bloomLevel">The string representation of the chosen
		/// Bloom Level</param>
		/// <returns>The appropriate index of the Bloom Level.</returns>
		protected int getSelectedIndex( string bloomLevel ) {
			IList levels = Globals.BloomLevels;
			int index = -1;

			for ( int i = 0; i < levels.Count && index == -1; i++ ) {
				if ( bloomLevel == (string)levels[i] ) {
					index = i;
				}
			}

			return index;
		}
	}
}
