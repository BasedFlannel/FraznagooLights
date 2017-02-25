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

		public int[] CreateRGBSpectrum() {

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
		protected virtual int[] CreateRGBSpectrumInternal(float[] fftBuffer) {
			SpectrumPointData[] spectrumPoints = CalculateSpectrumPoints(512, fftBuffer);
			int[] arrRGB = new int[spectrumPoints.Length * 3];
			for (int i = 0; i < spectrumPoints.Length; i++) {
				SpectrumPointData p = spectrumPoints[i];
				int lightIndex = p.SpectrumPointIndex;
				double lightVal = p.Value;
				arrRGB[i * 3] = (int)lightVal;
				arrRGB[i * 3 + 1] = (int)lightVal;
				arrRGB[i * 3 + 2] = (int)lightVal;
			}
			
			return arrRGB;
		}

		protected virtual int[] wheel(int pos) {
			int[] arrRGB = { 0, 0, 0 };
			int val;
			clamp(pos, 768);
			val = getSinedValue(pos);
			if (pos < 256) {
				
				arrRGB[0] = 0;
				arrRGB[1] = val;
				arrRGB[2] = 255-val;
			}
			else if(pos < 512){
				pos -= 256;
				arrRGB[0] = val;
				arrRGB[1] = 255 - val;
				arrRGB[2] = 0;
			}
			else {
				pos -= 512;
				pos &= 255;
				arrRGB[0] = 255-val;
				arrRGB[1] = 0;
				arrRGB[2] = val;
			}

			return arrRGB;
		}
		protected int getSinedValue(int value) {
			value %= 256;
			return (int)(128 * (Math.Sin(Math.PI * ((value-128)/ 256.0)) + 1));
		}

		protected int clamp(int num, int max = 255, int min = 0) {
			return Math.Min(Math.Max(num, min), max);
		}
	}
}
