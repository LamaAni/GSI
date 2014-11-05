#pragma OPENCL EXTENSION cl_khr_fp64: enable
#define RGBA_IMAGE_N_BYTES 4

int getImageOffset(
	const int vectorIndex, // the index of the vector.
	const int numberOfLines, // total number of lines in the sample.
	const int lineSize, // the number of vectors in a line,
	const int vectorSize // the number of pixels in a vector.
	)
{
	// getting the correct line. (Sets the y offset to write to).
	int xoffset = (vectorIndex / numberOfLines) * vectorSize;
	int yoffset = (vectorIndex % lineSize) * vectorSize * numberOfLines;
	int pixelIndex = yoffset + xoffset;
	return pixelIndex;
}

unsigned char getGrayscaleColor(
	const int vpi, // the pisition within the data of the vector pixel.
	const int vectorDataPixelLength, // the number of bytes in a pixel.
	global unsigned char* vectors // the data source to read from.
	)
{
	int i = 0;
	float avarage = 0;
	for (i = 0; i < vectorDataPixelLength; i++)
	{
		avarage += vectors[vpi + i];
	}
	avarage /= vectorDataPixelLength;
	if (avarage>255)
		return 255;
	return (unsigned char)avarage;
}

unsigned char calculateMaskAvarage(
	const int maskIndex, // the index of the mask value (R=0, G=1, B=2).
	const int vpi, // the pisition within the data of the vector pixel.
	const int vectorDataPixelLength, // the number of bytes in a pixel.
	global float* colormask, // the mask to apply to the colors. 
	// [RGBA_IMAGE_N_BYTES * vectorDataPixelLength * 3  or null] 
	// the three diffrent data sets are for RGB values.
	global unsigned char* vectors // the data source to read from.
	)
{
	int i = 0;
	float avarage = 0;
	int maskOffset = maskIndex*vectorDataPixelLength;
	for (i = 0; i < vectorDataPixelLength; i++)
	{
		avarage += vectors[vpi + i] * colormask[maskOffset + i];
	}

	// normalizing.
	avarage /= vectorDataPixelLength;
	if (avarage>255)
		return 255;
	return (unsigned char)avarage;
}

//void ApplyColorMaskAndPopulateImage(
//	const int vpi, // the pisition within the data of the vector pixel.
//	const int imgi, // the image poisition.
//	const int vectorDataPixelLength, // the number of bytes in a pixel.
//	float* colormask, // the mask to apply to the colors. 
//							// [RGBA_IMAGE_N_BYTES * vectorDataPixelLength * 3  or null] 
//							// the three diffrent data sets are for RGB values.
//	unsigned char* vectors, // the data source to read from.
//	unsigned char* imagedata // the image data to write to.
//	)
//{
//	imgi
//}

// Radix 2 fft (pad zeros if in source).
kernel void makeimagedata(
	const int numberOfLines, // total number of lines in the sample.
	const int lineSize, // the number of vectors in a line,
	const int vectorSize, // the number of pixels in a vector.
	const int vectorDataPixelLength, // the length of a single pixel data.
	const int usecolormask, // if 1 then use the colormask. 
	global float* colormask, // the mask to apply to the colors. 
	// [RGBA_IMAGE_N_BYTES * vectorDataPixelLength * 3  or null] 
	// the three diffrent data sets are for RGB values.
	global unsigned char* vectors, // the data source to read from.
	global unsigned char* imagedata // the image data to write to.
	)
{
	// active vector index.
	const int vectorIndex = get_global_id(0); // read the index.
	const int vecOffset = vectorIndex*vectorSize*vectorDataPixelLength;
	const int imgOffset = getImageOffset(vectorIndex, numberOfLines, lineSize, vectorSize);
	int veci = 0;
	int pixi = 0;

	// reading the vector data into the image data.
	for (veci = 0; veci < vectorSize; veci++)
	{
		int vpi = vecOffset + veci*vectorDataPixelLength; // the vector pixel index.
		int imgi = imgOffset + veci*RGBA_IMAGE_N_BYTES;
		if (usecolormask == 1)
		{
			// Set the colormask and populate the image.
			//ApplyColorMaskAndPopulateImage(vpi, imgi, vectorDataPixelLength, colormask, vectors, imagedata);
			imagedata[imgi] = calculateMaskAvarage(0, vpi, vectorDataPixelLength, colormask, vectors);
			imagedata[imgi + 1] = calculateMaskAvarage(1, vpi, vectorDataPixelLength, colormask, vectors);;
			imagedata[imgi + 2] = calculateMaskAvarage(2, vpi, vectorDataPixelLength, colormask, vectors);;
			imagedata[imgi + 3] = 255;
		}
		else
		{
			// using avarage to calculate.
			unsigned char color = getGrayscaleColor(vpi, vectorDataPixelLength, vectors);
			imagedata[imgi] = color;
			imagedata[imgi + 1] = color;
			imagedata[imgi + 2] = color;
			imagedata[imgi + 3] = 255;
		}
	}
}
