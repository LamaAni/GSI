clear all
close all
fclose('all');
nheader=4;
[fname,fpath]=uigetfile('D:\Code\SystemControl\TestRun\bin\Debug\*.dat');

file_name=[fpath,fname];
fInfo=dir(file_name);
fileSize=fInfo.bytes;

fid=fopen(file_name,'r');
if (fid==-1),
    error('file not found');
end;
p=fread(fid,nheader,'int32');
Np=p(4);
W=p(2);
H=p(3);
numOfLines=p(1);
%%
% nfloats=(fileSize-nheader*4)/4;
nfloats=W*H*Np;
partial=[];
data=zeros(Np,H*numOfLines,W);
for i=1:numOfLines
    partial=fread(fid,nfloats,'float');
    partial=reshape(partial,Np,H,W);
    hidx=(i-1)*H+(1:H);
    data(:,hidx,:)=partial;
end
fclose(fid);

data=shiftdim(data,1);


%%
figure('units','normalized','position',[0 0 1 1])

subplot(3,1,[1 2])
imagesc(mean(data,3));
colormap gray
axis equal
axis off

while 1
    [x,y]=ginput(1);
    pixel_spec=squeeze(data(round(y),round(x),:));
    subplot(3,1,3)
    plot(pixel_spec)
    xlabel('channel');
    ylabel('Spectrum');
    title(['Spectrum at x=',num2str(round(x)),', y=',num2str(round(y))])    
    
end