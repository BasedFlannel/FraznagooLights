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

		protected override Color[] CreateRGBSpectrumInternal(float[] fftBuffer) {
			SpectrumPointData[] spectrumPoints = CalculateSpectrumPoints(768, fftBuffer);
			Color[] arrColors = Color.createEmptyArray(spectrumPoints.Length);

			for (int i = 0; i < spectrumPoints.Length; i++) {
				SpectrumPointData p = spectrumPoints[i];
				arrColors[i].setTo(wheel(clamp((int)p.Value + wheelOffset,wheelOffset + 512)));
			}
			return arrColors;
		}
	}
}
