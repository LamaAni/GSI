// predeifinitions for the values
#define SWAP(a,b) tempr=(a);(a)=(b);(b)=tempr

// test kernal to do vector adding.
kernel void VectorAdd( global const float* src_a,
	global const float* src_b,
	global float* res)
{
	/* get_global_id(0) returns the ID of the thread in execution.
	As many threads are launched at the same time, executing the same kernel,
	each one will receive a different ID, and consequently perform a different computation.*/
	const int idx = get_global_id(0);

	/* Now each work-item asks itself: "is my ID inside the vector's range?"
	If the answer is YES, the work-item performs the corresponding computation*/
	res[idx] = src_a[idx] + src_b[idx];
}

// example for kernal generated code.
kernel void DoComplex(const int internalN, global const float* src_a,
	global const float* src_b,
	global float* res)
{
	const int idx = get_global_id(0); // read the index.
	int a = 0;
	int b = 12;
	int SWAP(a, b);
	
	// Do something big.
	for (int i = 0; i < internalN; i++)
	{
		res[idx] = cos(src_a[idx]) + cos(src_b[idx]);
	}
}