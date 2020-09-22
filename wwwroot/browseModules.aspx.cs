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
using SwenetDev.DBAdapter;
using SwenetDev.Comparers;
using System.Collections;

namespace SwenetDev {
	/// <summary>
	/// Summary description for browseModules.
	/// </summary>
	public class browseModules : System.Web.UI.Page {
		protected System.Web.UI.WebControls.HyperLink HyperLink1;
		protected System.Web.UI.WebControls.Label InfoMessage;
		protected System.Web.UI.WebControls.Label DiscussLbl;
		protected System.Web.UI.WebControls.DataGrid ModuleGrid;
		protected System.Web.UI.WebControls.HyperLink ModArrowLink;
		protected System.Web.UI.WebControls.HyperLink SubArrowLink;
		protected System.Web.UI.WebControls.HyperLink DateArrowLink;
		protected String categoryParamStr;
		protected String sortingParamStr;
	
		/// <summary>
		/// Perform necessary functions upon the loading of the page.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		private void Page_Load(object sender, System.EventArgs e) {
			if ( !IsPostBack ) {
				SubArrowLink = new HyperLink();
				DateArrowLink = new HyperLink();
				categoryParamStr = Request.Params[ "categoryID" ];
				sortingParamStr = Request.Params[ "SortedBy" ];
				ArrayList resultModules = null;
				String categoryText = "All";
				try {
					// If no category is specified in the request params,
					// obtain all modules.  Otherwise obtain the modules
					// under the specified category.
					if ( categoryParamStr == null || categoryParamStr == "" ) {
						resultModules = (ArrayList)Modules.getAll( ModuleStatus.Approved );
					} else {
						int categoryParamInt = int.Parse( categoryParamStr );

						Categories.CategoryInfo category = Categories.getCategoryInfo( categoryParamInt );

					
						resultModules =
							(ArrayList)Modules.getModulesByCategory( categoryParamInt, ModuleStatus.Approved );
						categoryText =
							category.LongText;
						DiscussLbl.Text = "<a href=\"" +
							AspNetForums.Components.Globals.UrlShowForum + category.ForumID +
							"\">Discuss modules in this category</a> in the forums.";

					}

					// If there were no resulting modules, display an
					// appropriate message.
					if ( resultModules == null || resultModules.Count == 0 ) {
						InfoMessage.Text = "<p>No modules found in category \""
							+ categoryText + "\".</p>";
					} else {
						InfoMessage.Text = "<h2>SEEK Category: " + categoryText + "</h2>";
						if ( sortingParamStr == "Date" ) {

							resultModules.Sort( new DateComparer() );
							resultModules.Reverse();

						} else if ( sortingParamStr == "DatRev" ) {

							resultModules.Sort( new DateComparer() );
                            
						} else if ( sortingParamStr == "Submitter" ) {

							resultModules.Sort( new SubmitterComparer() );

						} else if ( sortingParamStr == "SubRev" ) {

							resultModules.Sort( new SubmitterComparer() );
							resultModules.Reverse();

						} else if ( sortingParamStr == "Reverse" ) {

							resultModules.Sort();
							resultModules.Reverse();

						} else {

							resultModules.Sort();

						}
						
						ModuleGrid.DataSource = resultModules;
						ModuleGrid.DataBind();
					}
				} catch ( Exception ex ) {
					InfoMessage.Text = "An error occurred while obtaining the "
						+ "desired modules from the database.  Try again.";
				}
			}
		}

		#region WebControl Load Methods
		protected void ModArrowLink_Load( Object sender, EventArgs e ) 
		{
			ModArrowLink = (HyperLink)sender;
			int sortIndex = Request.Url.PathAndQuery.IndexOf( "SortedBy" );

			if( sortIndex != -1 ) 
			{

//				if( Request.Url.PathAndQuery.IndexOf( "Reverse" ) != -1 ) {

//				}

				ModArrowLink.NavigateUrl = Request.Url.PathAndQuery.Substring( 0, sortIndex - 1 );

			} else {

				ModArrowLink.ImageUrl = "DotWiki/images/uparrow.jpg";

				ModArrowLink.NavigateUrl = Request.Url.PathAndQuery;

				if ( ( categoryParamStr == null || categoryParamStr == "" ) ) {
					
					ModArrowLink.NavigateUrl += "?SortedBy=Reverse";

				} else {

					ModArrowLink.NavigateUrl += "&SortedBy=Reverse";

				}

			}
		}

		protected void SubArrowLink_Load( Object sender, EventArgs e ) 
		{
			SubArrowLink = (HyperLink)sender;
			int paramIndex;

			if( sortingParamStr == null || sortingParamStr == "" ) {

				if ( ( categoryParamStr == null || categoryParamStr == "" ) ) {

					SubArrowLink.NavigateUrl = Request.Url.PathAndQuery + "?SortedBy=Submitter";

				} else {

					SubArrowLink.NavigateUrl = Request.Url.PathAndQuery + "&SortedBy=Submitter";

				}

			} else if( sortingParamStr.IndexOf( "Submitter" ) == -1 ) {

				paramIndex = Request.Url.PathAndQuery.IndexOf( "SortedBy" );
				SubArrowLink.NavigateUrl = Request.Url.PathAndQuery.Substring( 0, paramIndex )
											+ "SortedBy=Submitter";
			
			} else {

				SubArrowLink.ImageUrl = "DotWiki/images/uparrow.jpg";

				paramIndex = Request.Url.PathAndQuery.IndexOf( "SortedBy" );
				SubArrowLink.NavigateUrl = Request.Url.PathAndQuery.Substring( 0, paramIndex )
											+ "SortedBy=SubRev";

			}
		}

		protected void DateArrowLink_Load( Object sender, EventArgs e ) 
		{
			DateArrowLink = (HyperLink)sender;
			int paramIndex;

			if( sortingParamStr == null || sortingParamStr == "" ) 
			{

				if ( ( categoryParamStr == null || categoryParamStr == "" ) ) 
				{

					DateArrowLink.NavigateUrl = Request.Url.PathAndQuery + "?SortedBy=Date";

				} 
				else 
				{

					DateArrowLink.NavigateUrl = Request.Url.PathAndQuery + "&SortedBy=Date";

				}

			} else if( sortingParamStr.IndexOf( "Date" ) == -1 ) {

				paramIndex = Request.Url.PathAndQuery.IndexOf( "SortedBy" );
				DateArrowLink.NavigateUrl = Request.Url.PathAndQuery.Substring( 0, paramIndex )
					+ "SortedBy=Date";
			
			} else {

				DateArrowLink.ImageUrl = "DotWiki/images/uparrow.jpg";

				paramIndex = Request.Url.PathAndQuery.IndexOf( "SortedBy" );
				DateArrowLink.NavigateUrl = Request.Url.PathAndQuery.Substring( 0, paramIndex )
					+ "SortedBy=DatRev";

			}

		}

		#endregion

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
		private void InitializeComponent() {    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
