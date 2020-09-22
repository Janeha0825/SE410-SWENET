using System;
using AspNetForums;
using AspNetForums.Components;
using System.Web;

namespace SwenetDev {
	using DBAdapter;
	/// <summary>
	/// Summary description for ModuleRatingsControl.
	/// </summary>
	public class ModuleRatingsControl {

		public const int FORUMS_ID = 5;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="currentInfo"></param>
		/// <param name="post"></param>
		/// <param name="rating"></param>
		/// <returns></returns>
		public static ModuleRatingInfo addRating( ModuleRatingInfo currentInfo, Post post, Rating rating ) {
			// If post is not null, add the post and rating to forums database.
			// Even if the post is empty, still add it.

			if ( post != null ) {
				post.PostType = Posts.PostType.Rating;
				post.ParentID = currentInfo.ThreadID;
				post.IsLocked = true;
				post = Posts.AddPost( post );
				rating.PostID = post.PostID;
				Ratings.AddRating( rating );
			}

			// Calculate the new rating and construct a new ModuleRatingInfo
			// object to be returned.

			float newRating = 0;

			if ( currentInfo.NumRatings == 0 ) {
				newRating = rating.Value;
			} else {
				newRating = (currentInfo.Rating * currentInfo.NumRatings + rating.Value) /
					(currentInfo.NumRatings + 1);
			}

			ModuleRatingInfo newInfo = new ModuleRatingInfo();
			newInfo.ModuleID = currentInfo.ModuleID;
			newInfo.ThreadID = currentInfo.ThreadID;
			newInfo.Rating = newRating;
			newInfo.NumRatings = currentInfo.NumRatings + 1;

			// Update the module rating in the module database.

			ModuleRatings.updateRating( newInfo );
			
			return newInfo;
		}

		public static ModuleRatingInfo initModuleRating( int moduleID ) {
			// Construct a new thread post for ratings for the given module.

			Modules.ModuleInfo module = Modules.getModuleInfo( moduleID );
			Post newPost = new Post();
			newPost.Username = "Admin";
			newPost.Subject = module.Title;
			newPost.Body = "Ratings for <a href=\"" +  HttpContext.Current.Request.ApplicationPath +
				"/viewModule.aspx?moduleID=" + module.Id + "\">" + module.Title + "</a>.";
			newPost.IsLocked = true;
			newPost.ForumID = FORUMS_ID;

			// Add the post to the forums database.

			Post returnedPost = Posts.AddPost( newPost );

			// Construct a ModuleRatingInfo object with the thread ID of the 
			// actual post created.

			ModuleRatingInfo modRating = new ModuleRatingInfo();
			modRating.ModuleID = module.Id;
			modRating.ThreadID = returnedPost.ThreadID;

			ModuleRatings.createRating( modRating );

			return modRating;
		}

		public static Rating getRatingForUser( string username, ModuleRatingInfo ratingInfo ) {
			return Ratings.GetRatingForUser( username, ratingInfo.ThreadID );
		}
	}
}
