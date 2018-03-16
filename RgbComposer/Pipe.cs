using System.Collections.Generic;
using System.Drawing;
using TGASharpLib;

namespace RgbComposer
{
	public struct Pipe
	{
		public Channel[] InputChannels;
		public Channel[] OutputChannels;
		public Bitmap InputBitmap;

		public Pipe(Channel[] inputChannels, Channel[] outputChannels, Bitmap inputBitmap)
		{
			InputChannels = inputChannels;
			OutputChannels = outputChannels;
			InputBitmap = inputBitmap;
		}
	}
}