using System;
using Xamarin.Forms;

namespace TestApp2
{
	public class App
	{

		public static HomePage Home;
		public static Page GetMainPage ()
		{	
			return Home ?? (Home = new HomePage ());
		}
	}
}

