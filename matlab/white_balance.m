clear all
close all

fpath='C:\Documents and Settings\ph206\My Documents\spectral imaging\';
fpath='\\132.70.33.111\garini\MOSHE\asi\measurements\';

[fname,fpath]=uigetfile({'*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff';'*.jpg;*.jpeg';'*.png';'*.bmp';'*.tif;*.tiff'},'Load image',fpath);

rgb=imread([fpath,fname]);
%%
imshow(rgb)
rgb=single(rgb);
rect=round(getrect);
close(gcf)
R=rgb(rect(2)+[1:rect(4)],rect(1)+[1:rect(3)],1);
G=rgb(rect(2)+[1:rect(4)],rect(1)+[1:rect(3)],2);
B=rgb(rect(2)+[1:rect(4)],rect(1)+[1:rect(3)],3);

r=mean(R(:));
g=mean(G(:));
b=mean(B(:));
A=max(rgb(:));
rgb(:,:,1)=A*rgb(:,:,1)./r;
rgb(:,:,2)=A*rgb(:,:,2)./g;
rgb(:,:,3)=A*rgb(:,:,3)./b;
rgb=uint8(rgb);
imshow(rgb)
[fname, fpath]=uiputfile([fpath,'*.jpg']);
imwrite(rgb,[fpath,fname],'jpg')