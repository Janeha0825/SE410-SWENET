using System;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Text.RegularExpressions;

namespace SwenetDev {
	/// <summary>
	/// Summary description for Globals.
	/// </summary>
	public class Globals {

		public static string ConnectionString {
			get { return ConfigurationSettings.AppSettings["ConnectionString"]; }
		}

		public static string UsersConnectionString {
			get { return ConfigurationSettings.AppSettings["UsersConnectionString"]; }
		}

		public static string SiteUrl {
			get { return ConfigurationSettings.AppSettings["urlWebSite"]; }
		}

		public static int Timeout {
			get { return Convert.ToInt32( ConfigurationSettings.AppSettings["Timeout"] ); }
		}

		public static string AdminsEmail {
			get { return ConfigurationSettings.AppSettings["AdminsEmail"]; }
		}

		public static string EditorsEmail {
			get { return ConfigurationSettings.AppSettings["EditorsEmail"]; }
		}

		public static string MaterialsDir {
			get { return ConfigurationSettings.AppSettings["MaterialsDir"]; }
		}

		public static string MaterialsTempDir {
			get { return ConfigurationSettings.AppSettings["MaterialsTempDir"]; }
		}

		public static IList SEEKAreas {
			get { return (IList)HttpContext.Current.Application["SEEKAreas"]; }
		}

		public static IList BloomLevels {
			get { return (IList)HttpContext.Current.Application["BloomLevels"]; }
		}

		public static bool EmailsEnabled {
			get { return Convert.ToBoolean( ConfigurationSettings.AppSettings["EmailsEnabled"] ); }
		}

		public static string FeedbackEmails {
			get { return ConfigurationSettings.AppSettings["FeedbackEmails"]; }
		}

		/// <summary>
		/// A utility method to parse textual input to remove any html formatting
		/// or scripting tags to prevent script injection attacks.  It also converts
		/// line breaks into html &lt;br&gt; tags to format larger text areas.
		/// </summary>
		/// <param name="text">The string to parse.</param>
		/// <returns>A "clean" string with tags removed and line breaks converted.</returns>
		public static string parseTextInput( string text ) {
			string retVal = Regex.Replace(text, @"<(.+?)>", "");
			retVal = System.Web.HttpUtility.HtmlEncode( retVal );
			retVal = Regex.Replace(retVal, @"\n", "<br>");

			return retVal;
		}

		/// <summary>
		/// A utility method to format text to be displayed in a text area for
		/// editing.  It removes any breaks and decodes the string.
		/// </summary>
		/// <param name="text">The string to parse.</param>
		/// <returns>A properly decoded string.</returns>
		public static string formatTextOutput( object text ) {
			string retVal = Regex.Replace(text.ToString(), @"<br>", "");
			retVal = System.Web.HttpUtility.HtmlDecode( retVal );

			return retVal;
		}
	}
}
