clear all
close all
fclose('all');

nheader=3;
dx=10;

[fname,fpath]=uigetfile('D:\Code\SystemControl\TestRun\bin\Debug\*.dat');

file_name=[fpath,fname];
fInfo=dir(file_name);
fileSize=fInfo.bytes;

fid=fopen(file_name,'r');
if (fid==-1),
    error('file not found');
end;
p=fread(fid,nheader,'int32');
W=p(1);
H=p(2);
Np=p(3);
%%
vecHeader=4;
data=zeros(H,W,Np,'uint8');

for i=1:W
    p=fread(fid,vecHeader,'int32');
    pixOffset=p(1);
    vec=fread(fid,Np*H,'uint8');
    vec=reshape(vec,Np,H);
    vec=permute(vec,[2,3,1]);
    data(:,i,:)=vec;
end
%%
newSize=dx*floor(size(data,2)./dx);

data=double(data(:,1:newSize,:));
avData=mean(data,3);

F=abs(fft(data-repmat(avData,1,1,Np),128,3));
F=F(:,:,1:64);
avF=mean(F,3);

imagesc(avF);
axis equal
axis off
colormap gray
h=imrect;
rect=round(wait(h));
delete(h)
rect(1)=dx*ceil(rect(1)./dx);
rect(3)=dx*floor(rect(3)./dx);

%%

av=mean(avF(rect(2)+[1:rect(4)],rect(1)+[1:rect(3)]),1);

av=reshape(av,dx,rect(3)/dx);
minAv=min(av,[],1);
minAv=repmat(minAv,dx,1);
av=av-minAv;

av=mean(av,2);

av=repmat(av',size(data,1),newSize/dx);



im_F=avF;
im_F=im_F-min(im_F(:));
im_F=im_F./max(im_F(:));

im_F1=avF-av;
im_F1=im_F1-min(im_F1(:));
im_F1=im_F1./max(im_F1(:));


ax(1)=subplot(1,2,1);
imshow(im_F)
axis equal
title('original FFT')
ax(2)=subplot(1,2,2);
imshow(im_F1)
title('"clean" FFT')
axis equal
linkaxes(ax)



av_F=mean(im_F,1);
av_F=av_F-min(av_F(:));
av_F=av_F./max(av_F(:));

av_F1=mean(im_F1,1);
av_F1=av_F1-min(av_F1(:));
av_F1=av_F1./max(av_F1(:));

figure

ax1(1)=subplot(1,2,1);
plot(av_F)
title('original FFT')
ax1(2)=subplot(1,2,2);
plot(av_F1)
title('"clean" FFT')

linkaxes(ax1)