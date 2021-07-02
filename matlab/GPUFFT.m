%GPUFFt
clear all
close all
data=csvread('D:\Code\SystemControl\TestCuda\bin\Debug\rslt.csv');
maxval=1e3;
minval=-1e3;
data(data>maxval)=maxval;
data(data<minval)=minval;
idxs=data(:,1);
source=data(:,2);
magGPU=(data(:,3).^2+data(:,4).^2).^0.5;
magGPU2=(data(:,11).^2+data(:,12).^2).^0.5;
magCPU=(data(:,5).^2+data(:,6).^2).^0.5;
magCPU2=(data(:,7).^2+data(:,8).^2).^0.5;
magCPU3=(data(:,9).^2+data(:,10).^2).^0.5;
magMAT=abs(fft(source));
nplot=7;
subplot(nplot,1,1);
plot(idxs, source);
subplot(nplot,1,2);
plot(idxs, magGPU2);
subplot(nplot,1,3);
plot(idxs, magGPU);
subplot(nplot,1,4);
plot(idxs,magCPU);
subplot(nplot,1,5);
plot(idxs,magCPU2);
subplot(nplot,1,6);
plot(idxs,magCPU3);
subplot(nplot,1,7);
plot(idxs,magMAT);