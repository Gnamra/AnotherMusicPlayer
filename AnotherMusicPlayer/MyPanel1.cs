using System;
using Eto.Forms;
using Eto.Drawing;

namespace AnotherMusicPlayer
{
	public partial class MyPanel1 : Panel
	{
		public MyPanel1()
		{

			Content = new StackLayout
			{
				Padding = 10,
				Items =
				{
					"Hello World!",
					// add more controls here
				}
			};

		}
	}
}
