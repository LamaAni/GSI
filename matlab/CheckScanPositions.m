clear all
close all
pos=csvread('D:\Code\SystemControl\TestRun\bin\Debug\pos.csv');
imageTimes=csvread('D:\Code\SystemControl\TestRun\bin\Debug\imagedt.csv');

starVals= mean(pos(:,2)).*ones(size(imageTimes));
imagelocs= interp1(pos(:,1),pos(:,2),imageTimes);
plot(imageTimes, imagelocs,'r*');