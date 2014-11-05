clear all
close all
data=csvread('D:\Code\SystemControl\TestCuda\bin\Debug\multirslt.csv');
samplenum=size(data);
samplenum=samplenum(2)/3;

startsamp=20;
endsamp=22;
plotnum=(endsamp-startsamp+1);
pidx=1;
for i=startsamp:endsamp
    sidx=(i-1)*3+1;
    subplot(plotnum,2,pidx);
    plot(data(:,sidx));
    subplot(plotnum,2,pidx+1);
    plot((data(:,sidx+1).^2+data(:,sidx+2).^2).^0.5);
    pidx=pidx+2;
end