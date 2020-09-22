namespace SwenetDev.Controls {
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Collections;
	using DBAdapter;

	/// <summary>
	///	The control for editing and adding SWEnet module categories.
	/// </summary>
	public class CategoriesControl : System.Web.UI.UserControl, IEditControl {
		protected System.Web.UI.WebControls.Button Button1;
		protected System.Web.UI.WebControls.Button Button2;
		protected SwenetDev.Controls.DataEditControl CategoriesEditor;

		private void Page_Load(object sender, System.EventArgs e) {
			CategoriesEditor.Text = "";
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
			this.CategoriesEditor.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.CategoriesEditor_UpdateCommand);
			this.CategoriesEditor.NewItemEvent += new System.EventHandler(this.CategoriesEditor_NewItemEvent);
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion

		#region IEditControl Members

		/// <summary>
		/// Initialize the list of categories.
		/// </summary>
		/// <param name="moduleID">The module identifier for the
		/// initial list of categories or 0 if it should be initially empty.</param>
		public void initList( int moduleID ) {
			if ( moduleID == 0 ) {
				CategoriesEditor.DataList = new ArrayList();
				CategoriesEditor.DataBind();
			} else if ( moduleID > 0 ) {
				CategoriesEditor.DataList = Categories.getAll( moduleID );
				CategoriesEditor.DataBind();
			} else {
				throw new ArgumentException( "Invalid module identifier.", "moduleID" );
			}
		}

		/// <summary>
		/// Insert all categories that have been added for the module to the
		/// database.
		/// </summary>
		/// <param name="moduleID">The module identifier that the categories
		/// classify.</param>
		public void insertAll( int moduleID, bool removePrevious ) {
			if ( removePrevious ) {
				Categories.removeAll( moduleID );
			}

			Categories.addAll( moduleID, CategoriesEditor.DataList );
		}

		/// <summary>
		/// Tells whether or not this CategoriesControl object is valid.
		/// </summary>
		/// <returns>True if the control is valid.</returns>
		public bool validate() {
			bool valid = true;
			CategoriesEditor.Text = "";

			if ( CategoriesEditor.DataList.Count == 0 ) {
				CategoriesEditor.Text = "At least one SEEK category is required.";
				valid = false;
			} else if( !(valid = !hasDuplicates()) ) {
				CategoriesEditor.Text = "Duplicate SEEK categories exist.";
			}
			
			return valid;
		}

		#endregion

		/// <summary>
		/// Respond to the new item event, when the user elects to create a
		/// new item, by creating the specific info class and adding it to
		/// the editor control's data list.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments</param>
		private void CategoriesEditor_NewItemEvent(object sender, System.EventArgs e) {
			Categories.CategoryInfo catInfo = new Categories.CategoryInfo();
			CategoriesEditor.DataList.Add( catInfo );
		}

		/// <summary>
		/// Respond to the update event, when an item is being edited and
		/// subsequently updated, by updating the info object of the
		/// item being edited.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments</param>
		private void CategoriesEditor_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e) {
			CategoriesSelect catSelect = (CategoriesSelect)e.Item.Cells[1].FindControl( "CatSelect" );
			Categories.CategoryInfo selected = catSelect.getSelectedCategory();
			CategoriesEditor.DataList[e.Item.ItemIndex] = selected;
			CategoriesEditor.DataBind();

			if( hasDuplicates() ) {
				CategoriesEditor.Text = "That SEEK area/unit combination has already been added.";
				CategoriesEditor.DataList.RemoveAt(e.Item.ItemIndex);
			} else {
				CategoriesEditor.Text = "";
			}

		}

		/// <summary>
		/// This method determines whether or not this CategoriesControl object contains
		/// any duplicate categories.
		/// </summary>
		/// <returns>True if duplicates exist.</returns>
		public bool hasDuplicates() {
			
			bool retval = false;
			ArrayList knownCategories = new ArrayList();

			foreach( Categories.CategoryInfo cInfo in CategoriesEditor.DataList ) {
				if( knownCategories.Contains( cInfo.Abbreviation ) ) {
					retval = true;
					break;
				} else {
					knownCategories.Add( cInfo.Abbreviation );
				}
			}

			return retval;
		}
	}
}
