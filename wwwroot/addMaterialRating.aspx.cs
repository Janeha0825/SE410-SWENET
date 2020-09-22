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
	/// Summary description for addMaterialRating.
	/// </summary>
	public class addMaterialRating : System.Web.UI.Page {

		protected System.Web.UI.WebControls.Button SubmitButton;
		protected System.Web.UI.WebControls.RadioButtonList RButtonList;
		protected System.Web.UI.WebControls.Label Comments;
		protected System.Web.UI.WebControls.TextBox CommentText;
		protected System.Web.UI.WebControls.TextBox SubjectText;
		protected System.Web.UI.WebControls.Button CancelButton;
		protected System.Web.UI.WebControls.Label SubjectLabel;
		protected System.Web.UI.WebControls.Label CommentLabel;
		protected System.Web.UI.WebControls.Label MaterialIdent;
		protected System.Web.UI.WebControls.HyperLink MaterialLink;
		protected System.Web.UI.WebControls.Panel HeaderPanel;
		protected System.Web.UI.WebControls.Label TitleLabel;
		protected System.Web.UI.WebControls.RegularExpressionValidator CommentingValidator;
		protected System.Web.UI.WebControls.RegularExpressionValidator SubjectValidator;
		protected System.Web.UI.WebControls.Panel Panel1;
		private int materialID = 0;

		private void Page_Load(object sender, System.EventArgs e)
		{
			//Get the materialID for the material being rated
			string strMaterialID = Request.Params["materialID"];

			if (strMaterialID != null) 
			{
				//Obtain the material that is associated with the materialID and setup the header
				materialID = int.Parse(Request.Params["materialID"]);
				Materials.MaterialInfo mi = Materials.getMaterialInfo(materialID);
				MaterialIdent.Text = mi.IdentInfo;
				MaterialLink.NavigateUrl = "Materials/" + Materials.getModuleOfMaterial(mi.MatID) + "/" + mi.Link;

				//Identify the user and their access level
				int role = -1;
				if (Context.User.Identity.IsAuthenticated) {

					UserAccounts.UserInfo cui = UserAccounts.getUserInfo(Context.User.Identity.Name);
					role = (int) cui.Role;

				}
				//If the user does not have the needed access level then do not allow them to download the material
				if ((role < mi.AccessFlag) || (role > 5)) MaterialLink.Enabled = false;

				//Setup the material download link and then setup the display format for it
				int position = mi.Link.LastIndexOf('.');
				if (position == -1) MaterialLink.Text = " (" + mi.Link + ")";
				else MaterialLink.Text = "(" + mi.Link.Substring((position + 1)) + ")";
				MaterialLink.Text = MaterialLink.Text.ToUpper();
			}
		}

		/// <summary>
		/// Handles when the "Submit" button is clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void Submit_Btn(Object sender, EventArgs e) 
		{
			int materialRating = 0;
			int numberOfRatings = 0;
			int locationOfSpace = 0;
			string comments = null;
			string subject = null;
			string date = null;
			string author = null;
			string selected = null;

			numberOfRatings = ((Materials.getNumberOfRatings(materialID)) + 1);

			//Get the radio button the user selected for the rating
			selected = RButtonList.SelectedValue;
			materialRating = int.Parse(selected);

			//If the user added a comment then construct all the comment information and add it to the database
			if ((CommentText.Text != String.Empty) && (SubjectText.Text != String.Empty)) 
			{
				
				comments = CommentText.Text;
				subject  = SubjectText.Text;
				date     = "" + DateTime.Today; 
				author   = Context.User.Identity.Name;

				locationOfSpace = date.IndexOf(" ");
				date = date.Substring(0, locationOfSpace);

				MaterialComments.addComment(new MaterialComments.MaterialCommentsInfo(materialID, comments, subject, date, materialRating, "", author)); 
			}
			
			Materials.setNumberOfRatings(numberOfRatings, materialID);
			Materials.setMaterialRating(materialRating, materialID);

			//Return to viewing the module
			Response.Redirect("viewModule.aspx?moduleID=" + Materials.getModuleOfMaterial(materialID));
			
		}

		/// <summary>
		/// Handles when the "Cancel" button is clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void Cancel_Btn(Object sender, EventArgs e) {

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
