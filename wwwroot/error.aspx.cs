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
	/// <summary>
	/// Summary description for error.
	/// </summary>
	public class error : System.Web.UI.Page {
		protected System.Web.UI.WebControls.Label lblMessage;
	
		private void Page_Load(object sender, System.EventArgs e) {
			try {
				if ( !IsPostBack ) {
   
					// Grab the exception information from the session
					string errorMsg = (string)Session["ErrorMsg"];
					string pageErrorOccured = (string)Session["PageErrorOccured"];
					string exceptionType = (string)Session["ExceptionType"];
					string stackTrace = (string)Session["StackTrace"];

					// Clear the values from the session
					Session.Remove( "ErrorMsg" );
					Session.Remove( "PageErrorOccured" );
					Session.Remove( "ExceptionType" );
					Session.Remove( "StackTrace" );

					// Display a generic error message to the user
					lblMessage.Text = "An error has occurred: " + errorMsg;

					lblMessage.Text = 
						String.Format( "{0}<br/><br/>To try again, click <a href=\"{1}\">here</a>.<br/><br/>",
						lblMessage.Text, pageErrorOccured );

					// Add specific error information as HTML comments for you
					// to view during development.  You could also log the
					// error to the Windows event log here.
					lblMessage.Text = lblMessage.Text + "<!--\n" +
						"Error Message: " + errorMsg +
						"\nPage Error Occurred: " + pageErrorOccured +
						"\nExceptionType: " + exceptionType +
						"\nStack Trace: " + stackTrace +
						"\n-->";

				}
			} catch ( Exception ex ) {
				// If an exception is thrown in the above code output the
				// message and stack trace to the screen
				lblMessage.Text = ex.Message + " " + ex.StackTrace;
			}
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
