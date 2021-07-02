function [x,y]=ShiftCalc(im1,im2,imSize,runl)

% im1 - vector of 1st image
% im2 - vector of 2nd image
% imSize - vector of the image size (rows, columns)
% runl - the distance between the images (in microns)

Snap1=double(reshape(im1,imSize(1),imSize(2)));
Snap2=double(reshape(im2,imSize(1),imSize(2)));
corr=fast_xcorr2(Snap1,Snap2);
[x,y]=find_shift(corr);
