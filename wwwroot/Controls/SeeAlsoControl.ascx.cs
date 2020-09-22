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
	public class SeeAlsoControl : System.Web.UI.UserControl, IEditControl {

		public static int MaxLength {
			get { return DBAdapter.Modules.getColumnMaxLength("SeeAlso","Description"); }
		}

		protected System.Web.UI.WebControls.TextBox DescriptionTextbox;
		protected System.Web.UI.WebControls.TextBox ModuleIDTextbox;
		protected System.Web.UI.WebControls.RequiredFieldValidator Requiredfieldvalidator1;
		protected System.Web.UI.WebControls.Button Button4;
		protected System.Web.UI.WebControls.Button Button5;
		protected System.Web.UI.WebControls.CustomValidator ModuleIdentifierValidator;
		protected System.Web.UI.WebControls.LinkButton Linkbutton1;
		protected System.Web.UI.WebControls.LinkButton Linkbutton2;
		protected System.Web.UI.WebControls.LinkButton Linkbutton3;
		protected SwenetDev.Controls.DataEditControl AuthorsEditor;
		protected System.Web.UI.WebControls.Label ErrorMessage;
		protected DataEditControl SeeAlsoEditor;

		/// <summary>
		/// Respond to the page load event by initializing the list property
		/// of the edit control.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments</param>
		private void Page_Load(object sender, System.EventArgs e) {
			ErrorMessage.Text = "";
			SeeAlsoEditor.Text = "";
			SeeAlsoEditor.displayReOrgColumn();
		}

		/// <summary>
		/// Respond to the new item event, when the user elects to create a
		/// new item, by creating the specific info class and adding it to
		/// the editor control's data list.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments</param>
		private void SeeAlsoEditor_NewItemEvent(object sender, System.EventArgs e) {
			SeeAlsoEditor.DataList.Add( new SeeAlso.SeeAlsoInfo( "", "" ) );
		}

		/// <summary>
		/// Respond to the update event, when an item is being edited and
		/// subsequently updated, by updating the info object of the
		/// item being edited.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments</param>
		private void SeeAlsoEditor_UpdateCommand(object source, DataGridCommandEventArgs e) {

			TextBox tb1 = (TextBox)e.Item.FindControl("DescriptionTextbox");
			TextBox tb2 = (TextBox)e.Item.FindControl("ModuleIDTextbox");
			SeeAlso.SeeAlsoInfo ri =
				((SeeAlso.SeeAlsoInfo)SeeAlsoEditor.DataList[(int)e.Item.ItemIndex]);
			ri.Description = tb1.Text;
			ri.AltModule = tb2.Text;

			if( hasDuplicates() ) {
				SeeAlsoEditor.Text = "That alternate module has already been added.";
				SeeAlsoEditor.DataList.RemoveAt((int)e.Item.ItemIndex);
			} /*else if( Modules.getBaseIDfromIdentifier( tb2.Text ) == -1 ) {
				// If the identifier wasn't already in the list, see if it exists in the DB
				SeeAlsoEditor.Text = "That module identifier does not exist.";
				SeeAlsoEditor.DataList.RemoveAt((int)e.Item.ItemIndex);
			}*/ else {
				SeeAlsoEditor.Text = "";

			}

		}

		/// <summary>
		/// Checks the See Also control for duplicates
		/// </summary>
		/// <returns>False if any duplicates exist, true otherwise.</returns>
		public bool hasDuplicates() 
		{
			
			bool retval = false;
			ArrayList knownSAs = new ArrayList();

			foreach( SeeAlso.SeeAlsoInfo saInfo in SeeAlsoEditor.DataList ) {
				if( knownSAs.Contains( saInfo.AltModule ) ) {
					retval = true;
					break;
				} else {
					knownSAs.Add( saInfo.AltModule );
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
			SeeAlsoEditor.UpdateCommand += new DataGridCommandEventHandler(SeeAlsoEditor_UpdateCommand);
			SeeAlsoEditor.NewItemEvent += new EventHandler(SeeAlsoEditor_NewItemEvent);
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

		#region IEditControl Members

		/// <summary>
		/// Initialize the list of resources.
		/// </summary>
		/// <param name="moduleID">The module identifier for the
		/// initial list of resources or 0 if it should be initially empty.</param>
		public void initList( int moduleID ) {
			if ( moduleID == 0 ) {
				SeeAlsoEditor.DataList = new ArrayList();
				SeeAlsoEditor.DataBind();
			} else if ( moduleID > 0 ) {
				SeeAlsoEditor.DataList = SeeAlso.getAll( moduleID );
				SeeAlsoEditor.DataBind();
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
				SeeAlso.removeAll( moduleID );
			}

			SeeAlso.addAll( moduleID, SeeAlsoEditor.DataList );
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
