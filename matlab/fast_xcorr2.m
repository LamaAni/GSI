function [out]=fast_xcorr2(im1,im2)
F1=fftshift(fft2(im1));
F2=fftshift(fft2(im2));
F12=(conj(F1)).*F2;

out=abs(ifftshift(ifft2(F12)));