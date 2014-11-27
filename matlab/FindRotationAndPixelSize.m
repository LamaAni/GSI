% Calculates the rotation and pixels size as seen in the camera from the 
% image gathered as compared to the orientation of the state.
% imagewidth - toallow parsing of the image vectors.
% imga - a vector with the firt image pixels. grayscale single value per pixel.
% imgb - a vector with the second image pixles . grayscale single value per pixel.
% deltax,y - the shift in x,y in microns.
function [ang,pixsize]=FindRotationAndPixelSize(imga, imgb,imgwidth, deltax, deltay)
    % converting the images to matrix form.
    mimga=reshape(imga,[length(imga)/imgwidth,imgwidth]);
    mimgb=reshape(imgb,[length(imgb)/imgwidth,imgwidth]);
    corr=fast_xcorr2(mimga,mimgb);
    delta=sqrt(deltax^2+deltay^2);
    [x,y]=find_shift(corr);
    r=sqrt(x.^2 + y.^2);
    pixsize=delta/r;
    ang=atan(-y/x);
end

% calls to do a fast forier xcorrelation of the images.
function [out]=fast_xcorr2(im1,im2)
    F1=fftshift(fft2(im1));
    F2=fftshift(fft2(im2));
    F12=(conj(F1)).*F2;
    out=abs(ifftshift(ifft2(F12)));
end