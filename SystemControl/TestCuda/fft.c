#pragma OPENCL EXTENSION cl_khr_fp64: enable

// Defining radix 2 fft. Using the Danielson-Lanczos algorithem.
// The data is copied to an internal vector of data values and then written back
// to the same vector. Negative values of the forier transform are ignored.
// Needless to say that this is not a very efficient algorithem
// but it will do for the time being.
///////////////////////////////////////////
// [Press et al, Numerical Recipes in C, CAMBRIDGE UNIVERSITY PRESS, P 504], implementation differs from original.
///////////////////////////////////////////
kernel void doFFT(
	const unsigned int nn, // vector lengths, dose not have to be size of 2^p. 
	global double* data, // the working data set, ordered by [img. real][image rea]....
	const int isign // the direction of the the forier transform, -1 to 
	)
{
	// int nn. Is the length of the float vector.
	// the data is arragned as follows, data[even] = real values, data[odd] - imaginary.
	unsigned long n = 0, mmax = 0, m = 0, j = 0, istep = 0, i = 0;
	double wtemp = 0, wr = 0, wpr = 0, wpi = 0, wi = 0, theta = 0;
	double tempr = 0, tempi = 0;

	data[nn] = 1;
	n = nn << 1;
	j = 1;
	for (i = 1; i<n; i += 2) {
		if (j > i) {
			tempr = data[j];
			data[j] = data[i];
			data[i] = tempr;

			tempr = data[j];
			data[j + 1] = data[i + 1];
			data[i + 1] = tempr;
		}
		m = nn;
		while (m >= 2 && j > m) {
			j -= m;
			m >>= 1;
		}
		j += m;
	}
	int markerIndex = nn * 2;
	data[markerIndex] = 2;
	mmax = 2;
	tempr = 0;
	tempi = 0;
	int encounter = 0;
	while (n > mmax) {
		istep = mmax << 1;
		theta = isign*(6.28318530717959 / mmax);
		wtemp = sin(0.5*theta);
		wpr = -2.0*wtemp*wtemp;
		wpi = sin(theta);
		wr = 1.0;
		wi = 0.0;
		for (m = 1; m < mmax; m += 2) {
			for (i = m; i <= n; i += istep)
			{
				j = i + mmax;
				tempr = wr*data[j] - wi*data[j + 1];
				tempi = wr*data[j + 1] + wi*data[j];
				data[j] = data[i] - tempr;
				data[j + 1] = data[i + 1] - tempi;
				data[i] += tempr;
				data[i + 1] += tempi;
			}
			wr = (wtemp = wr)*wpr - wi*wpi + wr;
			wi = wi*wpr + wtemp*wpi + wi;
		}
		mmax = istep;
	}

	data[markerIndex] = -1;
}