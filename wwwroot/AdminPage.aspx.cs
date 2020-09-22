using System;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SwenetDev {
	using DBAdapter;
	/// <summary>
	/// A page for SWEnet administrators.
	/// </summary>
	public class AdminPage : System.Web.UI.Page 
	{
		protected System.Web.UI.WebControls.Label PageMessageLbl;
	
		private void Page_Load(object sender, System.EventArgs e) 
		{
			if ( User.Identity.IsAuthenticated && User.IsInRole( UserRole.Admin.ToString() ) ) 
			{
				if ( IsPostBack ) 
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
	}
}