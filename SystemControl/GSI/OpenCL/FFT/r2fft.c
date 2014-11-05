#pragma OPENCL EXTENSION cl_khr_fp64: enable
#define MATH_PI 3.14159265359

// the function subtract the average from the data (remove DC frequency)
void SubtractAverage(int offset, // the vector offset to the location of the data.
	int l, // the vector length, in power of 2.
	global float* real, // the working real nums.
	global float* imag // the working image nums.
	)
{
	float temp1 = 0;
	float temp2 = 0;
	int i = 0;

	for (i = 0; i < l - 1; ++i)
	{
		temp1 += real[i + offset];
		temp2 += imag[i + offset];
	}
	temp1 /= (1.0*l);
	temp2 /= (1.0*l);
	for (i = 0; i < l - 1; ++i)
	{
		real[i + offset] -= temp1;
		imag[i + offset] -= temp2;
	}

}

// Bit reorder (see algorithem)
void R2Reorder(int offset, // the vector offset to the location of the data.
	int l, // the vector length, in power of 2.
	global float* real, // the working real nums.
	global float* imag // the working image nums.
	)
{
	float temp1 = 0;
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

// Step of the R2 algorithem.
void R2Step(int offset, // the vector offset to the location of the data.
	int l , // the vector length, in power of 2.
	global float* real, // the working real nums.
	global float* imag, // the working image nums.
	int k, // step inside the matrix. (See algorithem).
	int levelSize, // the internal matrix size. (See algorithem).
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

void ApplayMask(int offset, // the vector offset to the location of the data.
	int l, // the vector length, in power of 2.
	global float* real, // the working real nums.
	global float* imag, // the working image nums.
	global float* mask // the mask
	)
{
	int i = 0;
	for (i = 0; i < l; i++)
	{
		real[offset + i] = real[offset + i] * mask[i];
		imag[offset + i] = imag[offset + i] * mask[i];
	}
}

//void CalculateAvarage(int sampleNum, int l, int offset, // the sample number.
//	global double* real, // the working real nums.
//	global double* imag, // the working iamg nums.
//	global double* mag, // the magnitude if calculated.
//	global double* av, // the avarages
//	const int doMag // if 1 then assume magnitude.
//	)
//{
//	int i = 0;
//	av[sampleNum] = 0;
//	if (doMag == 1)
//	{
//		for (i = 0; i < l; i++)
//		{
//			av[sampleNum] += mag[offset + i];
//		}
//	}
//	else
//	{
//		for (i = 0; i < l; i++)
//		{
//			av[sampleNum] += real[offset + i];
//		}
//	}
//	
//	av[sampleNum] = av[sampleNum] * 1.0 / l;
//}

void CalculateMag(
	int offset, // the vector offset to the location of the data.
	int l, // the vector length, in power of 2.
	global float* real, // the working real nums.
	global float* imag, // the working image nums.
	global float* mag // the magnitude to calculate.
	)
{
	int i = 0;
	double val;
	for (i = 0; i < l; i++)
	{
		val = sqrt(pow(real[offset + i], 2) + pow(imag[offset + i], 2));
		mag[offset + i] = val;
	}
}

// Radix 2 fft (pad zeros if in source).
kernel void R2FFT(
	const int logn, // the vector length, in power of 2.
	const int isign, // the direction of the the forier transform, -1 to  (Bool dose not exist)
	global float* real, // the working real nums.
	global float* imag, // the working image nums.
	global float* mag, // the magnitude.
	// global double* av, // the avarage of the vector removed from code due to error!.
	const int doMask, // if true then do apodization mask. (Bool dose not exist)
	const int substractAvarageLength, // if true the do avarage substract. (Bool dose not exist)
	global float* mask // the apodization mask, must be in the length of 2^logn.
	)
{
	// active vector index.
	const int sampleNum = get_global_id(0); // read the index.
	// get the vector length.
	int l = pow(2.0, logn);
	int offset = sampleNum*l;

	// removed due to error.
	//CalculateAvarage(sampleNum, l, offset, real, imag, mag, av, 0);

	// removing the avarage value of the vector.
	if (substractAvarageLength > 0)
		SubtractAverage(offset, substractAvarageLength, real, imag);

	// Doing appodization of the matrix.
	if (doMask == 1)
		ApplayMask(offset, l, real, imag, mask);

	// bit reorder.
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

	// finished, calculating magnitude.
	CalculateMag(offset, l, real, imag, mag);
}
