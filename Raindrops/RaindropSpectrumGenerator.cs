using CSCore.DSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCorePlaying.Raindrops {
	class RaindropSpectrumGenerator : RGBSpectrum {
		public RaindropSpectrumGenerator(FftSize objFFTSize) : base(objFFTSize) {}
	}
}
