using System;

namespace SwenetDev.DBAdapter {
	/// <summary>
	/// Summary description for Info.
	/// </summary>
	[Serializable()]
	public abstract class Info {
		protected int id = 0;

		public virtual int Id {
			get {
				return id;
			}
			set {
				id = value;
			}
		}
	}
}
