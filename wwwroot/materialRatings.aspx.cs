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
	/// Summary description for materialRatings.
	/// </summary>
	public class materialRatings : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Image RatingImage;
		protected System.Web.UI.WebControls.Button DoneButtonBottom;
		protected System.Web.UI.WebControls.Label NumberOfRatings;
		protected System.Web.UI.WebControls.Repeater CommentRepeater;
		protected System.Web.UI.WebControls.Button DoneButtonTop;
		protected System.Web.UI.WebControls.Panel DoneButtonTopPanel;
		protected System.Web.UI.WebControls.Label MaterialLabel;
		protected System.Web.UI.WebControls.HyperLink MaterialLink;
		protected System.Web.UI.WebControls.Label NumericalRating;
		private int materialID = 0;
	
		private void Page_Load(object sender, System.EventArgs e) {

			//Get the materialID for the material that is currently being viewed
			string strMaterialID = Request.Params["materialID"];

			if (strMaterialID != null) {

				materialID = int.Parse(Request.Params["materialID"]);

			}
		
			//Obtain the material that is associated with the materialID and setup the header
			Materials.MaterialInfo mi = Materials.getMaterialInfo(materialID);
			RatingImage.ImageUrl = mi.RatingImage;
			NumericalRating.Text = string.Format("{0:0.00}", mi.Rating); 
			MaterialLabel.Text = mi.IdentInfo;
			NumberOfRatings.Text = "" + Materials.getNumberOfRatings(materialID);
			MaterialLink.NavigateUrl = "Materials/" + Materials.getModuleOfMaterial(materialID) + "/" + mi.Link;

			//Identify the user and their access level
			int role = -1;
			if (Context.User.Identity.IsAuthenticated) 
			{

				UserAccounts.UserInfo cui = UserAccounts.getUserInfo(Context.User.Identity.Name);
				role = (int) cui.Role;

			}
			//If the user does not have the needed access level then do not allow them to download the material
			if ((role < mi.AccessFlag) || (role > 5)) MaterialLink.Enabled = false;

			//Setup the material download link and then display format of it
			int position = mi.Link.LastIndexOf('.');
			if (position == -1) MaterialLink.Text = "(" + mi.Link + ")";
			else MaterialLink.Text = "(" + mi.Link.Substring((position + 1)) + ")";
			MaterialLink.Text = MaterialLink.Text.ToUpper();

			IList tempComment = MaterialComments.getAll(materialID);
			CommentRepeater.DataSource = tempComment;
			CommentRepeater.DataBind();
		
		}

		/// <summary>
		/// Handles the "Done" button clicks
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void DoneButton_Click(Object sender, EventArgs e) {

			Response.Redirect("viewModule.aspx?moduleID=" + Materials.getModuleOfMaterial(materialID));

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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
