using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Security.Principal;
using AspNetForums;

namespace SwenetDev {
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global : System.Web.HttpApplication {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public Global() {
			InitializeComponent();
		}	
		
		/// <summary>
		/// Initialize application state objects.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_Start(Object sender, EventArgs e) {
			Application["SEEKAreas"] = SwenetDev.DBAdapter.Categories.getSEEKAreas();
			Application["BloomLevels"] = SwenetDev.DBAdapter.Modules.getBloomLevels();
		}
 
		protected void Session_Start(Object sender, EventArgs e) {

		}

		protected void Application_BeginRequest(Object sender, EventArgs e) {

		}

		protected void Application_EndRequest(Object sender, EventArgs e) {

		}

		/// <summary>
		/// Application_AuthenticateRequest Event
		/// If the client is authenticated with the application, then determine
		/// which security roles he/she belongs to and replace the "User" intrinsic
		/// with a custom IPrincipal security object that permits "User.IsInRole"
		/// role checks within the application
		///
		/// Roles are cached in the browser in an in-memory encrypted cookie.  If the
		/// cookie doesn't exist yet for this session, create it.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
 
		//
		// 
		protected void Application_AuthenticateRequest(Object sender, EventArgs e) {
			// SWEnet roles

			// Extract the forms authentication cookie
			string cookieName = FormsAuthentication.FormsCookieName;
			HttpCookie authCookie = Context.Request.Cookies[cookieName];

			if( authCookie == null ) {
				// There is no authentication cookie.
				return;
			}

			FormsAuthenticationTicket authTicket = null;

			try {
				authTicket = FormsAuthentication.Decrypt( authCookie.Value );
			} catch( Exception ex ) {
				throw;
			}

			if (null == authTicket) {
				// Cookie failed to decrypt.
				throw new Exception( "failed to decrypt" );
				return; 
			}

			// When the ticket was created, the UserData property was assigned a
			// pipe delimited string of role names.
			string[] roles = authTicket.UserData.Split(new char[]{'|'});
          
			// Create an Identity object
			FormsIdentity id = new FormsIdentity( authTicket );

			// This principal will flow throughout the request.
			GenericPrincipal principal = new GenericPrincipal(id, roles);

			// Attach the new principal object to the current HttpContext object
			Context.User = principal;
		}

		protected void Application_Error(Object sender, EventArgs e) {
			try {
				// Set default values
				Session["ErrorMsg"] = "No error information was available";
				Session["ExceptionType"] = String.Empty;
				Session["PageErrorOccured"] = Request.RawUrl;
				Session["StackTrace"] = String.Empty;

				// Catch the last exception thrown
				Exception lastError = Server.GetLastError();

				// Pull out the InnerException
				if ( lastError != null ) {
					lastError = lastError.InnerException;

					// Place a few key values in the session for retrieval on the custom error page
					Session["ErrorMsg"] = lastError.Message;
					Session["ExceptionType"] = lastError.GetType().ToString();
					Session["StackTrace"] = lastError.StackTrace;
				}

				// Clear the error from the server
				Server.ClearError();

				// Redirect to the custom error page
				Response.Redirect("error.aspx");
			} catch ( Exception ex ) {
				// if we end up here, 
				// error handling has thrown an error.
				// do nothing - don't want to create an infinite loop
				Response.Write( "We apologize, but an unrecoverable error has " +
					"occurred. Please click the back button on your browser and try again." );
			}
		}

		protected void Session_End(Object sender, EventArgs e) {

		}

		protected void Application_End(Object sender, EventArgs e) {

		}
			
		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {    
			this.components = new System.ComponentModel.Container();
		}
		#endregion
	}
}

