clear all
close all

fpath='D:\Code\SystemControl\TestRun\bin\Release\'
[fname,fpath]=uigetfile([fpath,'*.bmp'],'Load filter');

im=imread([fpath,fname]);

wave=sum(im(:,:,1),1);
wave=wave-mean(wave);
wave=wave./max(wave(:));
x=1:length(wave);
%%

F=abs(fft(wave));
F=F(1:round(0.5.*length(wave)));
wavelength=length(wave).*(1./find(F==max(F)));

dx=round(wavelength./3.5)

W=wave(1:dx:end);
X=x(1:dx:end);

plot(x,wave,'b',X,W,'.b')