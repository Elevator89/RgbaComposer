using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using TGASharpLib;

namespace RgbComposer
{
	class Program
	{
		static void Main(string[] args)
		{
			string outputPath = args[0];
			string[] pipeStrings = args.Skip(1).ToArray();

			Pipe[] pipes = pipeStrings.Select(LoadPipe).ToArray();

			if (pipes.Length == 0)
				return;

			Dictionary<Channel, Route> routeMap = BuildRouteMap(pipes);

			int width = pipes[0].InputBitmap.Width;
			int height = pipes[0].InputBitmap.Height;

			Bitmap result = new Bitmap(width, height, PixelFormat.Format32bppArgb);

			for (int y = 0; y < height; ++y)
			{
				for (int x = 0; x < width; ++x)
				{
					result.SetPixel(x, y, ComposePixel(x, y, routeMap));
				}
			}

			result.Save(outputPath, ImageFormat.Png);
		}

		private static Color ComposePixel(int x, int y, Dictionary<Channel, Route> routeMap)
		{
			byte[] color = { 0, 0, 0, 0 };

			foreach (KeyValuePair<Channel, Route> mapItem in routeMap)
			{
				Channel destChannel = mapItem.Key;
				Color pixel = mapItem.Value.SourceBitmap.GetPixel(x, y);

				switch (mapItem.Value.SourceChannel)
				{
					case Channel.Red:
						color[(int)destChannel] = pixel.R;
						break;
					case Channel.Green:
						color[(int)destChannel] = pixel.G;
						break;
					case Channel.Blue:
						color[(int)destChannel] = pixel.B;
						break;
					case Channel.Alpha:
						color[(int)destChannel] = pixel.A;
						break;
				}
			}

			return Color.FromArgb(color[3], color[0], color[1], color[2]);
		}

		//r-ag:"asdfas.sdf"
		//rgb-"jghsdfag.sdf":a
		//rg-agbg:"aghjs.sdf"
		private static Dictionary<Channel, Route> BuildRouteMap(Pipe[] pipes)
		{
			Dictionary<Channel, Route> map = new Dictionary<Channel, Route>();

			foreach (Pipe pipe in pipes)
			{
				for (int outputIndex = 0; outputIndex < pipe.OutputChannels.Length; outputIndex++)
				{
					if (pipe.InputChannels.Length == 1)
					{
						map[pipe.OutputChannels[outputIndex]] = new Route(pipe.InputChannels[0], pipe.InputBitmap);
					}
					else if (outputIndex < pipe.InputChannels.Length)
					{
						map[pipe.OutputChannels[outputIndex]] = new Route(pipe.InputChannels[outputIndex], pipe.InputBitmap);
					}
				}
			}

			return map;
		}

		//rgb-"jghsdfag.sdf":a
		private static Pipe LoadPipe(string pipeString)
		{
			int hyphenIndex = pipeString.IndexOf('-');
			int colonIndex = pipeString.IndexOf(':');

			string outputChannelsStr = pipeString.Substring(0, hyphenIndex);
			string path = pipeString.Substring(hyphenIndex + 1, colonIndex - hyphenIndex - 1).Replace("\"", string.Empty);
			string inputChannelsStr = pipeString.Substring(colonIndex + 1, pipeString.Length - colonIndex - 1);

			Bitmap bitmap = (Bitmap)TGA.FromFile(path);

			return new Pipe(ParseChannels(inputChannelsStr), ParseChannels(outputChannelsStr), bitmap);
		}

		private static Channel[] ParseChannels(string channelsStr)
		{
			List<Channel> channels = new List<Channel>();

			for (int c = 0; c < channelsStr.Length; c++)
			{
				switch (channelsStr[c])
				{
					case 'r':
						channels.Add(Channel.Red);
						break;
					case 'g':
						channels.Add(Channel.Green);
						break;
					case 'b':
						channels.Add(Channel.Blue);
						break;
					case 'a':
						channels.Add(Channel.Alpha);
						break;
				}
			}

			return channels.ToArray();
		}
	}
}
