namespace SwenetDev.Controls {
	using System;
	using System.Data;
	using System.Data.SqlClient;
	using System.Configuration;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using SwenetDev.DBAdapter;
	using System.Collections;

	/// <summary>
	///	The CodeBehind class for the control to add and edit SWENET authors.
	/// </summary>
	public class AuthorsControl : System.Web.UI.UserControl, IEditControl {
		protected DataEditControl AuthorsEditor;
		protected System.Web.UI.WebControls.DropDownList existAuthLst;
		protected System.Web.UI.WebControls.Label ErrorMessage;

		public AuthorsControl() {
			existAuthLst = new DropDownList();
			existAuthLst.ID = "existAuthLst";
			existAuthLst.DataTextField = "Name";
			existAuthLst.DataValueField = "UserName";
		}

		private void Page_Load(Object sender, EventArgs e) {
			ErrorMessage.Text = "";
			AuthorsEditor.Text = "";
			AuthorsEditor.displayReOrgColumn();
			//AuthorsEditor.disableButtons();

			if ( !IsPostBack ) 
			{
				existAuthLst.DataSource = Authors.getAll();
				existAuthLst.DataBind();

			}
			
		}

		/// <summary>
		/// Respond to the click event of the button for adding an existing
		/// author by adding the selected author to the list of authors
		/// for the module being created.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		public void AddExistAuthBtn_Click(object sender, EventArgs e) {
			if ( !AuthorsEditor.Editing ) {
				Authors.AuthorInfo ai = Authors.getAuthorInfo( existAuthLst.SelectedValue );
				AuthorsEditor.DataList.Add( ai );
				AuthorsEditor.DataBind();
			} 
			else {
				AuthorsEditor.Text = "Click <strong>Apply</strong> or <strong>Cancel</strong> before attempting to add an entry.";
			}

		}

		/// <summary>
		/// Respond to the update event, when an item is being edited and
		/// subsequently updated, by updating the info object of the
		/// item being edited.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments</param>
		private void AuthorsEditor_UpdateCommand(object source, DataGridCommandEventArgs e) {
			DropDownList ddl = (DropDownList)e.Item.Cells[1].FindControl( "existAuthLst" );
			Authors.AuthorInfo ai = Authors.getAuthorInfo( ddl.SelectedValue );
			AuthorsEditor.DataList[(int)e.Item.ItemIndex] = ai;
			AuthorsEditor.DataBind();

			if( hasDuplicates() ) {
				AuthorsEditor.Text = "That author has already been added.";
				AuthorsEditor.DataList.RemoveAt((int)e.Item.ItemIndex);
			} else {
				AuthorsEditor.Text = "";
			}

		}

		/// <summary>
		/// This method determines whether or not this AuthorsControl object contains
		/// any duplicate authors.
		/// </summary>
		/// <returns>True if duplicates exist.</returns>
		public bool hasDuplicates() {

			bool retval = false;
			ArrayList knownAuthors = new ArrayList();

			foreach( Authors.AuthorInfo aInfo in AuthorsEditor.DataList ) {
				if( knownAuthors.Contains( aInfo.Name ) ) {
					retval = true;
					break;
				} else {
					knownAuthors.Add( aInfo.Name );
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
		private void AuthorsEditor_NewItemEvent(object sender, EventArgs e) {
			Authors.AuthorInfo ai = new Authors.AuthorInfo( "", "", "" );
			AuthorsEditor.DataList.Add( ai );

		}

		#region IEditControl Members

		/// <summary>
		/// Insert all of the authors for the given module.
		/// </summary>
		/// <param name="moduleID">The module to add the authors for.</param>
		/// <param name="removePrevious">Whether to remove any existing
		/// authors for the given module.</param>
		public void insertAll( int moduleID, bool removePrevious ) {
			if ( removePrevious ) {
				Authors.removeAll( moduleID );
			}

			Authors.addAll( moduleID, this.AuthorsEditor.DataList );
		}

		/// <summary>
		/// Initialize the list of authors.
		/// </summary>
		/// <param name="moduleID">The module identifier for the
		/// initial list of authors or 0 if it should be initially empty.</param>
		public void initList( int moduleID ) {
			if ( moduleID == 0 ) {
				AuthorsEditor.DataList = new ArrayList();
				string username = Context.User.Identity.Name;
				Authors.AuthorInfo ai = Authors.getAuthorInfo( username );
				AuthorsEditor.DataList.Add( ai );
				AuthorsEditor.DataBind();
			} else if ( moduleID > 0 ) {
				AuthorsEditor.DataList = Authors.getAll( moduleID );
				AuthorsEditor.DataBind();
			} else {
				throw new ArgumentException( "Invalid module identifier.", "moduleID" );
			}
		}

		/// <summary>
		/// Validate the author list.
		/// </summary>
		/// <returns>True if all input is valid.</returns>
		public bool validate() {
			return AuthorsEditor.validate() & !hasDuplicates();
		}

		#endregion

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			AuthorsEditor.UpdateCommand += new DataGridCommandEventHandler(AuthorsEditor_UpdateCommand);
			AuthorsEditor.NewItemEvent += new EventHandler(AuthorsEditor_NewItemEvent);
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
	}
}
