using System.Drawing;

namespace RgbComposer
{
	public struct Route
	{
		public Channel SourceChannel;
		public Bitmap SourceBitmap;

		public Route(Channel sourceChannel, Bitmap sourceBitmap)
		{
			SourceChannel = sourceChannel;
			SourceBitmap = sourceBitmap;
		}
	}
}