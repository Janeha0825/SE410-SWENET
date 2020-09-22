namespace SwenetDev.Controls {
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Collections;
	using DBAdapter;

	/// <summary>
	///	A control encapsulating the selection of a SEEK category, consisting of
	///	a SEEK area and optionally a SEEK unit.  SEEK units are dynamically updated
	///	to reflect changes in the SEEK area selection.
	/// </summary>
	public class CategoriesSelect : System.Web.UI.UserControl {
		protected System.Web.UI.WebControls.DropDownList AreaDrop;
		protected System.Web.UI.WebControls.DropDownList UnitDrop;

		/// <summary>
		/// The SEEK Unit list.  This contains an empty unit, Categories.CategoryInfo.Empty,
		/// as the first item in the list.
		/// </summary>
		protected IList unitsList {
			get {
				// Get the list of units from the database for the selected area.
				IList list = null;
				if ( AreaDrop.Items.Count > 0 ) {
					list = Categories.getSEEKUnits( Int32.Parse( AreaDrop.SelectedValue ) );
				} else {
					list = Categories.getSEEKUnits( 1 );
				}

				// Add the Empty unit to the top of the list.
				list.Insert( 0, Categories.CategoryInfo.Empty );

				return list;
			}
		}

		/// <summary>
		/// Respond to the page load event.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		private void Page_Load(object sender, System.EventArgs e) {
			if (!IsPostBack) {
				AreaDrop.DataSource = Globals.SEEKAreas;
				AreaDrop.DataBind();
				populateUnits();
			}
		}

		/// <summary>
		/// Obtain the selected category.
		/// </summary>
		/// <returns>The selected SEEK area if the unit selection is None, otherwise the
		/// selected SEEK unit.</returns>
		public Categories.CategoryInfo getSelectedCategory() {
			DropDownList drop = null;
			Categories.CategoryInfo selected = null;
			IList list = null;

			if ( UnitDrop.SelectedIndex == 0 ) {
				drop = AreaDrop;
				list = Globals.SEEKAreas;
			} else {
				drop = UnitDrop;
				list = unitsList;
			}
			
			selected = (Categories.CategoryInfo)(list[drop.SelectedIndex]);

			return selected;
		}

		/// <summary>
		/// When the selected area changes, update the units DropDownList.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		private void AreaDrop_SelectedIndexChanged(object sender, System.EventArgs e) {
			populateUnits();
		}

		/// <summary>
		/// Populate the SEEK Units drop down list with the units for the selected area.
		/// </summary>
		private void populateUnits() {
			//unitList = categoryAdapter.getSEEKUnits( Int32.Parse( AreaDrop.SelectedValue ) );
			UnitDrop.DataSource = unitsList;
			UnitDrop.DataBind();
		}
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
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
		private void InitializeComponent()
		{
			this.AreaDrop.SelectedIndexChanged += new System.EventHandler(this.AreaDrop_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
