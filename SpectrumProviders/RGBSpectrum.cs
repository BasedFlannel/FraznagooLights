using CSCore.DSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCorePlaying {
	public class RGBSpectrum : SpectrumBase{
		private int lightCount;

		public RGBSpectrum(FftSize objFFTSize) {
			FftSize = objFFTSize;
		}

		public int LightCount {
			get { return lightCount; }
			set {
				if (value <= 0)
					throw new ArgumentOutOfRangeException("value");
				lightCount = value;
				SpectrumResolution = value;
				UpdateFrequencyMapping();
			}
		}

		public Color[] CreateRGBSpectrum() {

			var fftBuffer = new float[(int)FftSize];
			
			if (SpectrumProvider.GetFftData(fftBuffer, this)) {
				return CreateRGBSpectrumInternal(fftBuffer);
			}
			else {
				Console.WriteLine("No FFT Performed");
				return null;
			}
		}

		//internal method to create the actual spectrum
		protected virtual Color[] CreateRGBSpectrumInternal(float[] fftBuffer) {
			SpectrumPointData[] spectrumPoints = CalculateSpectrumPoints(512, fftBuffer);
			Color[] arrColors = Color.createEmptyArray(spectrumPoints.Length);
			for (int i = 0; i < spectrumPoints.Length; i++) {
				int lightVal = (int)spectrumPoints[i].Value;
				arrColors[i].setTo(new Color(lightVal, lightVal, lightVal));
			}
			
			return arrColors;
		}

		protected virtual Color wheel(int pos) {
			Color c;
			int val;
			clamp(pos, 768);
			val = getSinedValue(pos);
			if (pos < 256) {
				c = new CSCorePlaying.Color(0, val, 255 - val);
			}
			else if(pos < 512){
				c = new CSCorePlaying.Color(val, 255 - val, 0);
			}
			else {
				c = new CSCorePlaying.Color(255 - val, 0, val);
			}

			return c;
		}
		protected int getSinedValue(int value) {
			value %= 256;
			return (int)(128 * (Math.Sin(Math.PI * ((value-128)/ 256.0)) + 1));
		}

		protected int clamp(int num, int max = 255, int min = 0) {
			return Math.Min(Math.Max(num, min), max);
		}

		protected double clampDouble(double num, double max = 1.0, double min = 0)
		{
			return Math.Min(Math.Max(num, min), max);
		}

		//given two colors, returns a color that is a mixture of the two.
		//position is a number representing the strength of c2 in the interpolation.
		//e.g. if position is 0.8, you will recieve a color that's 80% of the way between c1 and c2, being noticably closer to c2.
		// c1-------*-c2
		//at position=0 the returned color will be identical to c1, and position=1 the returned color will be identical to c2.
		protected Color interpolateColor(Color c1, Color c2, double position = 0.5) {
			return new Color(
				(int)(c1.red*(1-position) + c2.red*position), 
				(int)(c1.green * (1 - position) + c2.green * position), 
				(int)(c1.blue * (1 - position) + c2.blue * position)
			);
		}
	}
}
