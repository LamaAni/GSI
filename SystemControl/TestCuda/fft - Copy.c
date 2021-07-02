#define SWAP(a,b) ftemp=(a);(a)=(b);(b)=ftemp
#define TWOPI 6.28318530717959

// Defining radix 2 fft. Using the Danielson-Lanczos algorithem.
// The data is copied to an internal vector of data values and then written back
// to the same vector. Negative values of the forier transform are ignored.
// Needless to say that this is not a very efficient algorithem
// but it will do for the time being.
///////////////////////////////////////////
// [Press et al, Numerical Recipes in C, CAMBRIDGE UNIVERSITY PRESS, P 504], implementation differs from original.
///////////////////////////////////////////
kernel void doFFT(
	const int l, // vector lengths, dose not have to be size of 2^p. 
	global float* data, // the working data set, ordered by [img. real][image rea]....
	const int isign // the direction of the the forier transform, -1 to 
	)
{
	/* get_global_id(0) returns the ID of the thread in execution.
	As many threads are launched at the same time, executing the same kernel,
	each one will receive a different ID, and consequently perform a different computation.*/
	const int idx = get_global_id(0);
	if (idx > 0)
		return;

	// Defs.
	float dtemp = 0; // float temp value.
	float ftemp = 0; // float temp value.

	// int nn. Is the length of the float vector.
	// the data is arragned as follows, data[even] = real values, data[odd] - imaginary.
	int nn = pow(2, ceil(log2(l * 1.0))) * 2;

	// define the processing data structure. 
	//float data[nn * 2];

	//// prpulating the data structure. TODO: merge with bit reversal.
	//int idx = 0;
	//int offsetIndex = get_global_id(0)*l;
	//for (int i = 0; i < nn; i += 2)
	//{
	//	idx = i / 2;
	//	data[i] = real[offsetIndex + idx];
	//	data[i + 1] = img[offsetIndex + idx];
	//}

	// first part of the forier transform is the selecting the correct 
	// bit reversal for the indexing.
	int n = nn << 1;
	int j = 1;
	int m = 0;
	for (int i = 0; i < n; i += 2)
	{
		if (j > i)
		{
			SWAP(data[j], data[i]);
			SWAP(data[j + 1], data[i + 1]);
		}
		m = nn;
		while (m >= 2 && j > m)
		{
			j -= m;
			m >>= 1;
		}
		j += m;
	}

	// Here begins the Danielson-Lanczos section of the routine. We now
	// do the actual FFT algorithem on the bit ordered vector data.
	int mmax = 2; // 
	int istep = 0; // the step size inside the vector.
	float theta = 0; // the angle.
	float wpr = 0; // the current W for the loop. W^i.
	float wpi = 0; // calculates the current W.
	float wr = 0, wi = 0;
	float temp_real = 0, temp_imag = 0, temp_w = 0; // temp values.
	j = 0; // reset the looping parameter.
	while (n > mmax)
	{
		// step size. changes with mmax.
		istep = mmax << 1;

		// calculate current W^i.
		theta = isign * (TWOPI / mmax);
		temp_w = sin(0.5 * theta);
		wpr = -2.0 * temp_w * temp_w;
		wpi = sin(theta);

		wr = 1;
		wi = 0;

		// looping over the matrix locations.
		for (int m = 0; m < mmax; m += 2)
		{
			// looping over the steps.
			for (int i = 0; i < n; i += istep)
			{
				// The algorithem's fomula.
				j = i + mmax;

				// calculating values.
				temp_real = wr * data[j] - wi * data[j + 1];
				temp_imag = wr * data[j + 1] + wi * data[j];

				// setting new values ontop of the vector.
				data[j] = data[i] - temp_real;
				data[j + 1] = data[i + 1] - temp_imag;
				data[i] += temp_real;
				data[i + 1] += temp_imag;
			}

			// Trigonometric recurrence.
			wr = (temp_w = wr) * wpr - wi * wpi + wr;
			wi = wi * wpr + temp_w * wpi + wi;
		}

		// change the current matix size.
		mmax = istep;
	}

	//for (int i = 0; i < nn; i += 2)
	//{
	//	data[i] = 0;
	//	data[i + 1] = 0;
	//}
		
	//// write the data back on the result vectors.
	//for (int i = 0; i < nn; i += 2)
	//{
	//	idx = i / 2;
	//	real[offsetIndex + idx] = data[i];
	//	img[offsetIndex + idx] = data[i + 1];
	//}
}