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
	///	A control for adding and editing materials for a SWEnet module.
	/// </summary>
	public class PrerequisitesControl : System.Web.UI.UserControl, IEditControl {
		
		public static int MaxLength {
			get{ return DBAdapter.Modules.getColumnMaxLength("Prereqs","PrerequisiteText"); }
		}
		
		protected DataEditControl PrereqsEditor;

		private void Page_Load(object sender, System.EventArgs e) {
			PrereqsEditor.Text = "";
			PrereqsEditor.displayReOrgColumn();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			PrereqsEditor.UpdateCommand += new DataGridCommandEventHandler(PrereqsEditor_UpdateCommand);
			PrereqsEditor.NewItemEvent += new EventHandler(PrereqsEditor_NewItemEvent);
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
		private void PrereqsEditor_UpdateCommand(object source, DataGridCommandEventArgs e) {
			TextBox prereqBox = (TextBox)e.Item.Cells[1].FindControl( "PrereqTextbox" );
			string newText = Globals.parseTextInput(prereqBox.Text);
			((Prerequisites.PrereqInfo)PrereqsEditor.DataList[(int)e.Item.ItemIndex]).Text = newText;

			if( hasDuplicates() ) {
				PrereqsEditor.Text = "That prerequisite has already been added.";
				PrereqsEditor.DataList.RemoveAt((int)e.Item.ItemIndex);
			} else {
				PrereqsEditor.Text = "";
			}

		}

		/// <summary>
		/// Checks the Prerequisites control for duplicates
		/// </summary>
		/// <returns>False if any duplicates exist, true otherwise.</returns>
		public bool hasDuplicates() {
			
			bool retval = false;
			ArrayList knownPrereqs = new ArrayList();

			foreach( Prerequisites.PrereqInfo pInfo in PrereqsEditor.DataList ) {
				if( knownPrereqs.Contains( pInfo.Text ) ) {
					retval = true;
					break;
				} else {
					knownPrereqs.Add( pInfo.Text );
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
		private void PrereqsEditor_NewItemEvent(object sender, EventArgs e) 
		{
			Prerequisites.PrereqInfo pi = new Prerequisites.PrereqInfo( "" );
			PrereqsEditor.DataList.Add( pi );
		}

		#region IEditControl Members

		/// <summary>
		/// Initialize the list of prerequisites.
		/// </summary>
		/// <param name="moduleID">The module identifier for the
		/// initial list of prrequisites or 0 if it should be initially empty.</param>
		public void initList( int moduleID ) {
			if ( moduleID == 0 ) {
				PrereqsEditor.DataList = new ArrayList();
				PrereqsEditor.DataBind();
			} else if ( moduleID > 0 ) {
				PrereqsEditor.DataList = Prerequisites.getAll( moduleID );
				PrereqsEditor.DataBind();
			} else {
				throw new ArgumentException( "Invalid module identifier.", "moduleID" );
			}
		}

		/// <summary>
		/// Insert all of the prerequisites for the given module.
		/// </summary>
		/// <param name="moduleID">The module to add the prerequisites for.</param>
		public void insertAll( int moduleID, bool removePrevious ) {
			if ( removePrevious ) {
				Prerequisites.removeAll( moduleID );
			}
			
			Prerequisites.addAll( moduleID, PrereqsEditor.DataList );
		}

		/// <summary>
		/// Validate the prereqs list.
		/// </summary>
		/// <returns>True if all input is valid.</returns>
		public bool validate() {
			return PrereqsEditor.validate() & !hasDuplicates();
		}

		#endregion
	}
}
