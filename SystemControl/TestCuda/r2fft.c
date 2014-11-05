#pragma OPENCL EXTENSION cl_khr_fp64: enable
#define MATH_PI 3.14159265359

void R2Reorder(int offset, int l, // the vector length, in power of 2.
	global double* real, // the working real nums.
	global double* imag // the working image nums.
	)
{
	double temp1 = 0;
	int index1 = 0;
	int index2 = 0;
	for (index2 = 0; index2 < l - 1; ++index2)
	{
		if (index2 < index1)
		{
			// doing swaps.
			temp1 = real[offset + index2];
			real[offset + index2] = real[offset + index1];
			real[offset + index1] = temp1;

			temp1 = imag[offset + index2];
			imag[offset + index2] = imag[offset + index1];
			imag[offset + index1] = temp1;
		}
		int length = l;
		do
		{
			length >>= 1;
			index1 ^= length;
		} while ((index1 & length) == 0);
	}
}

// complex multiplication
//multiplicand._real * multiplier._real - multiplicand._imag* multiplier._imag
//multiplicand._real * multiplier._imag + multiplicand._imag * multiplier._real

void R2Step(int offset, int l , // the vector length, in power of 2.
	global double* real, // the working real nums.
	global double* imag, // the working image nums.
	int k, 
	int levelSize,
	int isign // the direction of the the forier transform, -1 to 
	)
{
	double rtemp1 = 0, rtemp2 = 0, rtemp3 = 0;
	double itemp1 = 0, itemp2 = 0, itemp3 = 0;

	double num1 = (double)(isign * k) * MATH_PI;
	num1 = num1 / (double)levelSize;

	rtemp1 = cos(num1);
	itemp1 = sin(num1);

	int num2 = levelSize << 1;
	int index = k;
	while (index < l)
	{
		rtemp2 = real[offset + index];
		itemp2 = imag[offset + index];

		rtemp3 = rtemp1*real[offset + index + levelSize] - itemp1*imag[offset + index + levelSize];
		itemp3 = rtemp1*imag[offset + index + levelSize] + itemp1*real[offset + index + levelSize];

		real[offset + index] = rtemp2 + rtemp3;
		imag[offset + index] = itemp2 + itemp3;

		real[offset + index + levelSize] = rtemp2 - rtemp3;
		imag[offset + index + levelSize] = itemp2 - itemp3;
		index += num2;
	}
}

// Radix 2 fft (pad zeros if in source).
kernel void R2FFT(
	const int logn, // the vector length, in power of 2.
	global double* real, // the working real nums.
	global double* imag, // the working image nums.
	const int isign // the direction of the the forier transform, -1 to 
	)
{
	// active vector index.
	const int sampleNum = get_global_id(0); // read the index.
	// get the vector length.
	int l = pow(2.0, logn);
	int offset = sampleNum*l;
	//for (int i = 0; i < l; i++)
	//{
	//	real[i] = 0;
	//}
	//return;
	// reoring the bits. (Lengistien).
	R2Reorder(offset, l, real, imag);

	// doing forier.
	int levelSize = 1;
	int k = 0;
	while (levelSize < l)
	{
		for (k = 0; k < levelSize; ++k)
			R2Step(offset, l, real, imag, k, levelSize, isign);
		levelSize *= 2;
	}
}
