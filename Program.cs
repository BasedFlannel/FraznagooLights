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

namespace CSCorePlaying {
	public class Program {
		private static BasicSpectrumProvider spectrumProvider;
		private static RGBSpectrum rgbSpectrizer;

		//persistent modifiers
		private static int numLights = 30;
		private static Queue<SpectrumModifiers.IRGBSpectrumModifier> modifierQueue;
		static void Main(string[] args) {

			//set effect queue
			modifierQueue = new Queue<SpectrumModifiers.IRGBSpectrumModifier>();
			//add modifiers to the queue
			//modifierQueue.Enqueue(new SpectrumModifiers.RollingAverager(30, 9));
			//modifierQueue.Enqueue(new SpectrumModifiers.SpectrumReverser());
			//modifierQueue.Enqueue(new SpectrumModifiers.ReflectedSpectrum(false,false));
			//modifierQueue.Enqueue(new SpectrumModifiers.Bars.BaseBarModifier(0,5));


			const FftSize objFFTSize = FftSize.Fft4096;

			System.Timers.Timer timer = new System.Timers.Timer();
			
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
				timer.Interval = 17;


				//create RGB spectrum object
				rgbSpectrizer = new OpacityRainbowSpectrum(objFFTSize, 1, 0, 1, 2.0)
                {
                    SpectrumProvider = spectrumProvider,
					UseAverage = false,
					LightCount = numLights,
					IsXLogScale = true,
					ScalingStrategy = ScalingStrategy.Linear
				};

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
			String server = "192.168.1.174";
			//set up connection
			Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(server);
			System.Net.IPEndPoint remoteEP = new IPEndPoint(ipAdd, 21012);
			soc.Connect(remoteEP);
			
			//send
			int[] data = rgbSpectrizer.CreateRGBSpectrum();
			Queue<SpectrumModifiers.IRGBSpectrumModifier> modifierQueueDuplicate = new Queue<SpectrumModifiers.IRGBSpectrumModifier>(modifierQueue);
			if (data != null) {
				while (modifierQueueDuplicate.Count > 0) {
					modifierQueueDuplicate.Dequeue().modifySpectrum(data);
				}
			}
			//Console.WriteLine(JsonConvert.SerializeObject(data));
			byte[] byData = System.Text.Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data));
			soc.Send(byData);
		}
	}

	/*quad chromatic function
	 * Staple colors: blue, green, orange, pink
	 * secondary colors: cyan, yellow, red, purple
	 * */
}
