using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace SwenetDev {
	using DBAdapter;
	/// <summary>
	/// User Management section of the Admin Page.
	/// </summary>
	public class UserManagement : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label PageMessageLbl;
		protected System.Web.UI.WebControls.DataGrid UsersGrid;
	
		private void Page_Load(object sender, System.EventArgs e) 
		{
			if ( User.Identity.IsAuthenticated && User.IsInRole( UserRole.Admin.ToString() ) ) 
			{
				if ( !IsPostBack ) 
				{
					bindGrid();
				} 
				else 
				{
					PageMessageLbl.Text = "";
				}
			} 
			else 
			{
				if ( !User.Identity.IsAuthenticated && User.IsInRole( UserRole.Admin.ToString() ) ) 
				{
					throw new Exception( "Your session has expired." );
				} 
				else 
				{
					throw new Exception( "You are not authorized to view the requested page." );
				}
			}
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() 
		{    
			this.UsersGrid.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.UsersGrid_ItemCreated);
			this.UsersGrid.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.UsersGrid_PageIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		protected void SelectionChanged(object source, System.EventArgs e) 
		{
			// Get a reference to the DropDownList;
			DropDownList ddl = (DropDownList)source;
			DataGridItem dgi = (DataGridItem)ddl.Parent.Parent;
			UserAccounts.UserInfo user = new UserAccounts.UserInfo();
			user.Username = ((HyperLink)dgi.Cells[0].FindControl("UserLink")).Text;
			user.Role = (UserRole)(Convert.ToInt32( ddl.SelectedValue ));
			UsersControl.updateUserRole( user );
			bindGrid();
		}

		private void UsersGrid_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e) 
		{
			if ( e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem ) 
			{
				DropDownList ddl = (DropDownList)e.Item.Cells[1].FindControl( "RolesDrop" );
				ddl.Items.Add( new ListItem( "User", ((int)UserRole.User).ToString() ) );
				ddl.Items.Add( new ListItem( "Faculty", ((int)UserRole.Faculty).ToString() ) );
				ddl.Items.Add( new ListItem( "Submitter", ((int)UserRole.Submitter).ToString() ) );
				ddl.Items.Add( new ListItem( "Editor", ((int)UserRole.Editor).ToString() ) );
				ddl.Items.Add( new ListItem( "Admin", ((int)UserRole.Admin).ToString() ) );
				ddl.Items.Add( new ListItem( "Canceled", ((int)UserRole.Canceled).ToString() ) );
				ddl.Items.Add( new ListItem( "Disabled", ((int)UserRole.Disabled).ToString() ) );
			}
		}

		private void bindGrid() 
		{
			UsersGrid.DataSource = UserAccounts.getAll();
			UsersGrid.DataBind();
		}

		private void UsersGrid_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e) 
		{
			UsersGrid.CurrentPageIndex = e.NewPageIndex;
			bindGrid();
		}
	}
}