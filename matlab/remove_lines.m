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

imagesc(avData);
axis equal
axis off
colormap gray
h=imrect;
rect=round(wait(h));
delete(h)
rect(1)=dx*ceil(rect(1)./dx);
rect(3)=dx*floor(rect(3)./dx);

%%

av=mean(avData(rect(2)+[1:rect(4)],rect(1)+[1:rect(3)]),1);

av=reshape(av,dx,rect(3)/dx);
minAv=min(av,[],1);
minAv=repmat(minAv,dx,1);
av=av-minAv;

av=mean(av,2);

av=repmat(av',size(data,1),newSize/dx,Np);
data1=data-av;


F=fft(data-repmat(mean(data,3),1,1,Np),128,3);
F1=fft(data1-repmat(mean(data1,3),1,1,Np),128,3);

im_data=mean(data,3);
im_data=im_data-min(im_data(:));
im_data=im_data./max(im_data(:));

im_data1=mean(data1,3);
im_data1=im_data1-min(im_data1(:));
im_data1=im_data1./max(im_data1(:));

im_F=mean(abs(F),3);
im_F=im_F-min(im_F(:));
im_F=im_F./max(im_F(:));

im_F1=mean(abs(F1),3);
im_F1=im_F1-min(im_F1(:));
im_F1=im_F1./max(im_F1(:));

ax(1)=subplot(2,2,1);
imshow(im_data)
axis equal
title('original data')
ax(2)=subplot(2,2,2);
imshow(im_data1)
title('"clean" data')
axis equal
ax(3)=subplot(2,2,3);
imshow(im_F)
axis equal
title('original FFT')
ax(4)=subplot(2,2,4);
imshow(im_F1)
title('"clean" FFT')
axis equal
linkaxes(ax)



av_data=mean(im_data,1);
av_data=av_data-min(av_data(:));
av_data=av_data./max(av_data(:));

av_data1=mean(im_data1,1);
av_data1=av_data1-min(av_data1(:));
av_data1=av_data1./max(av_data1(:));

av_F=mean(im_F,1);
av_F=av_F-min(av_F(:));
av_F=av_F./max(av_F(:));

av_F1=mean(im_F1,1);
av_F1=av_F1-min(av_F1(:));
av_F1=av_F1./max(av_F1(:));

figure
ax1(1)=subplot(2,2,1);
plot(av_data)
title('original data')
ax1(2)=subplot(2,2,2);
plot(av_data1)
title('"clean" data')
ax1(3)=subplot(2,2,3);
plot(av_F)
title('original FFT')
ax1(4)=subplot(2,2,4);
plot(av_F1)
title('"clean" FFT')

 linkaxes(ax1)