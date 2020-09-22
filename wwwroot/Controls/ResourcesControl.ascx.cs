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
	///	An imput control for other SWEnet resources.
	/// </summary>
	public class ResourcesControl : System.Web.UI.UserControl, IEditControl {

		public static int DescriptionLength {
			get { return DBAdapter.Modules.getColumnMaxLength("OtherResources","ResourceDescriptiveText"); }
		}

		public static int LinkLength {
			get { return DBAdapter.Modules.getColumnMaxLength("OtherResources","ResourceLink"); }
		}

		protected System.Web.UI.WebControls.RegularExpressionValidator LinkValidator;
		protected DataEditControl ResourcesEditor;

		/// <summary>
		/// Respond to the page load event by initializing the list property
		/// of the edit control.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments</param>
		private void Page_Load(object sender, System.EventArgs e) {
			ResourcesEditor.Text = "";
			ResourcesEditor.displayReOrgColumn();
		}

		/// <summary>
		/// Respond to the new item event, when the user elects to create a
		/// new item, by creating the specific info class and adding it to
		/// the editor control's data list.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments</param>
		private void ResourcesEditor_NewItemEvent(object sender, System.EventArgs e) {
			ResourcesEditor.DataList.Add( new Resources.ResourceInfo( "", "" ) );
		}

		/// <summary>
		/// Respond to the update event, when an item is being edited and
		/// subsequently updated, by updating the info object of the
		/// item being edited.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments</param>
		private void ResourcesEditor_UpdateCommand(object source, DataGridCommandEventArgs e) {
			TextBox tb1 = (TextBox)e.Item.FindControl("DescriptionTextbox");
			TextBox tb2 = (TextBox)e.Item.FindControl("LinkTextbox");
			string newText1 = SwenetDev.Globals.parseTextInput( tb1.Text );
			string newText2 = SwenetDev.Globals.parseTextInput( tb2.Text );
			Resources.ResourceInfo ri =
				((Resources.ResourceInfo)ResourcesEditor.DataList[(int)e.Item.ItemIndex]);
			ri.Text = newText1;
			ri.Link = newText2;

			if( hasDuplicates() ) {
				ResourcesEditor.Text = "That resource has already been added.";
				ResourcesEditor.DataList.RemoveAt((int)e.Item.ItemIndex);
			} else {
				ResourcesEditor.Text = "";
			}

		}

		/// <summary>
		/// Checks the Resources control for duplicates
		/// </summary>
		/// <returns>False if any duplicates exist, true otherwise.</returns>
		public bool hasDuplicates() 
		{
			
			bool retval = false;
			ArrayList knownResources = new ArrayList();

			foreach( Resources.ResourceInfo rInfo in ResourcesEditor.DataList ) {

				string temp = rInfo.Text + "|" + rInfo.Link;
				
				if( knownResources.Contains( temp ) ) {
					retval = true;
					break;
				} else {
					knownResources.Add( temp );
				}
			}

			return retval;
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
			this.ResourcesEditor.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.ResourcesEditor_UpdateCommand);
			this.ResourcesEditor.NewItemEvent += new System.EventHandler(this.ResourcesEditor_NewItemEvent);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		#region IEditControl Members

		/// <summary>
		/// Initialize the list of resources.
		/// </summary>
		/// <param name="moduleID">The module identifier for the
		/// initial list of resources or 0 if it should be initially empty.</param>
		public void initList( int moduleID ) {
			if ( moduleID == 0 ) {
				ResourcesEditor.DataList = new ArrayList();
				ResourcesEditor.DataBind();
			} else if ( moduleID > 0 ) {
				ResourcesEditor.DataList = Resources.getAll( moduleID );
				ResourcesEditor.DataBind();
			} else {
				throw new ArgumentException( "Invalid module identifier.", "moduleID" );
			}
		}

		/// <summary>
		/// Insert all of the resources added in the control to the database.
		/// </summary>
		/// <param name="moduleID">The module identifer to which the
		/// resources should be added.</param>
		public void insertAll( int moduleID, bool removePrevious ) {
			if ( removePrevious ) {
				Resources.removeAll( moduleID );
			}

			Resources.addAll( moduleID, ResourcesEditor.DataList );
		}
		
		/// <summary>
		/// Validate the resources list.
		/// </summary>
		/// <returns>True if all input is valid.</returns>
		public bool validate() {
			return !hasDuplicates();
		}

		#endregion
	}
}
