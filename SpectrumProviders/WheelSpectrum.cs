using CSCore.DSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCorePlaying {
	public class WheelSpectrum : RGBSpectrum{
		private int wheelOffset = 0;
		public WheelSpectrum (FftSize objFFTSize, int wheelOffset = 0 ) : base(objFFTSize) {this.wheelOffset = wheelOffset%768;}

		protected override int[] CreateRGBSpectrumInternal(float[] fftBuffer) {
			SpectrumPointData[] spectrumPoints = CalculateSpectrumPoints(768, fftBuffer);
			int[] arrRGB = new int[spectrumPoints.Length * 3];
			int[] rgbVals;

			for (int i = 0; i < spectrumPoints.Length; i++) {
				SpectrumPointData p = spectrumPoints[i];
				rgbVals = wheel(clamp((int)p.Value + wheelOffset,wheelOffset + 512));
				arrRGB[i * 3] = rgbVals[0];
				arrRGB[i * 3 + 1] = rgbVals[1];
				arrRGB[i * 3 + 2] = rgbVals[2];
			}
			return arrRGB;
		}

		protected override int[] wheel(int pos) {
			return base.wheel(pos);
		}
	}
}
