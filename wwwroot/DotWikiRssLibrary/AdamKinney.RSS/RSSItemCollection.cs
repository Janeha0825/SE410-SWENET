using System;
using System.Collections;

namespace AdamKinney.RSS
{
	public class RSSItemCollection : CollectionBase
	{
		public RSSItem this[ int index ]  
		{
			get{ return( (RSSItem) List[index] ); }
			set{ List[index] = value; }
		}

		public int Add(RSSItem value)
		{
			return List.Add(value);
		}

		public int IndexOf( RSSItem value )  
		{
			return( List.IndexOf( value ) );
		}

		public void Insert( int index, RSSItem value )  
		{
			List.Insert( index, value );
		}

		public void Remove( RSSItem value )  
		{
			List.Remove( value );
		}

		public bool Contains( RSSItem value )  
		{
			return( List.Contains( value ) );
		}

		protected override void OnInsert( int index, Object value )  
		{
			if ( value.GetType() != Type.GetType("AdamKinney.RSS.RSSItem") )
				throw new ArgumentException( "value must be of type RSSItem.", "value" );
		}

		protected override void OnRemove( int index, Object value )  
		{
			if ( value.GetType() != Type.GetType("AdamKinney.RSS.RSSItem") )
				throw new ArgumentException( "value must be of type RSSItem.", "value" );
		}

		protected override void OnSet( int index, Object oldValue, Object newValue )  
		{
			if ( newValue.GetType() != Type.GetType("AdamKinney.RSS.RSSItem") )
				throw new ArgumentException( "newValue must be of type RSSItem.", "newValue" );
		}

		protected override void OnValidate( Object value )  
		{
			if ( value.GetType() != Type.GetType("AdamKinney.RSS.RSSItem") )
				throw new ArgumentException( "value must be of type RSSItem." );
		}
	}
}
