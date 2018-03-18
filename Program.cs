using System;
using System.Collections.Generic;
using CSCore;
using CSCore.SoundIn;
using CSCore.DSP;
using CSCore.CoreAudioAPI;
using CSCore.Streams;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;
using CSCorePlaying.SpectrumProviders;
using System.Threading;

namespace CSCorePlaying {
	public class Program {
		private static BasicSpectrumProvider spectrumProvider;
		private static RGBSpectrum rgbSpectrizer;
		private static System.Timers.Timer timer;

		//persistent modifiers
		private static int numLights = 30;
		private static Queue<SpectrumModifiers.IRGBSpectrumModifier> modifierQueue;
		static void Main(string[] args) {

			//set effect queue
			modifierQueue = new Queue<SpectrumModifiers.IRGBSpectrumModifier>();
			//add modifiers to the queue
			//modifierQueue.Enqueue(new SpectrumModifiers.RollingAverager(30, 5));
			modifierQueue.Enqueue(new SpectrumModifiers.RollingAverager(30, 9));
			//modifierQueue.Enqueue(new SpectrumModifiers.SpectrumReverser());
			modifierQueue.Enqueue(new SpectrumModifiers.ReflectedSpectrum(true,false));
			//modifierQueue.Enqueue(new SpectrumModifiers.Bars.BaseBarModifier(0,1));


			const FftSize objFFTSize = FftSize.Fft4096;

			timer = new System.Timers.Timer();
			timer.AutoReset = false;
			
			//This is where the magic mostly happens.
			using (WasapiCapture capture = new WasapiLoopbackCapture()) {
				//set loopback to capture from default audio device and initialize
				capture.Device = MMDeviceEnumerator.TryGetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
				capture.Initialize();
				
				//create spectrum provider object
				spectrumProvider = new BasicSpectrumProvider(capture.WaveFormat.Channels, capture.WaveFormat.SampleRate, objFFTSize);

				var notificationSource = new SingleBlockNotificationStream(new SoundInSource(capture).ToSampleSource());
				notificationSource.SingleBlockRead += (s, a) => spectrumProvider.Add(a.Left, a.Right);

				IWaveSource finalSource = notificationSource.ToWaveSource();
				byte[] buffer = new byte[finalSource.WaveFormat.BytesPerSecond / 2];
				capture.DataAvailable += (s, e) => {
					int read;
					while ((read = finalSource.Read(buffer, 0, buffer.Length)) > 0);
				};
				timer.Elapsed += new System.Timers.ElapsedEventHandler(onTimerTick);
				timer.Interval = 20;

				//create RGB spectrum object
				double brightnessModifier = 15;
				rgbSpectrizer = new OpacityRainbowSpectrum(objFFTSize, 1, 0.0, 1.0, brightnessModifier) {
					SpectrumProvider = spectrumProvider,
					UseAverage = false,
					LightCount = numLights,
					IsXLogScale = true,
					ScalingStrategy = ScalingStrategy.Linear
				};
				/*This is a preset that's good for music, combine with a 9 frame rolling average.
				double brightnessModifier = 15;
				rgbSpectrizer = new OpacityRainbowSpectrum(objFFTSize,1,0.0,1.0,brightnessModifier)
				{
					SpectrumProvider = spectrumProvider,
					UseAverage = false,
					LightCount = numLights,
					IsXLogScale = true,
					ScalingStrategy = ScalingStrategy.Linear
				};*/

				//This is an off switch essentially
				if (false){
					rgbSpectrizer = new OpacityRainbowSpectrum(objFFTSize, 1, 0.0, 0.0, 0.0)
					{
						SpectrumProvider = spectrumProvider,
						LightCount = numLights,
						ScalingStrategy = ScalingStrategy.Linear,
					};
				}

				Console.WriteLine("starting");
				capture.Start();
				timer.Start();
				//keep console alive
				while (Console.ReadKey().Key != ConsoleKey.Enter) {}
				capture.Stop();
				timer.Stop();
				Console.ReadKey();
			}
		}

		private static void onTimerTick(object source, System.Timers.ElapsedEventArgs e) {
			String server = "192.168.1.111";
			//set up connection
			Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(server);
			System.Net.IPEndPoint remoteEP = new IPEndPoint(ipAdd, 21012);
			soc.Connect(remoteEP);
			
			//send
			var colorData = rgbSpectrizer.CreateRGBSpectrum();
			Queue<SpectrumModifiers.IRGBSpectrumModifier> modifierQueueDuplicate = new Queue<SpectrumModifiers.IRGBSpectrumModifier>(modifierQueue);
			if (colorData != null) {
				while (modifierQueueDuplicate.Count > 0) {
					modifierQueueDuplicate.Dequeue().modifySpectrum(colorData);
				}
			}
			//Console.WriteLine(JsonConvert.SerializeObject(data));
			int[] colorArray = new int[90];
			for (int i = 0; i < colorData.Length; i++) {
				colorArray[i * 3] = colorData[i].red;
				colorArray[i * 3 + 1] = colorData[i].green;
				colorArray[i * 3 + 2] = colorData[i].blue;
			}
			byte[] byData = System.Text.Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(colorArray));
			soc.Send(byData);

			//Start Timer again, this prevents race conditions.
			//Future plan, Use the timer to enforce a MINIMUM frame time, not to do all processing.
			System.Timers.Timer t = (System.Timers.Timer)source;
			t.Start();
		}
	}

	/*quad chromatic function
	 * Staple colors: blue, green, orange, pink
	 * secondary colors: cyan, yellow, red, purple
	 * */
	 
}
