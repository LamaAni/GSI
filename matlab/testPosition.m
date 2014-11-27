clear all
close all
fpath='\\132.70.33.111\garini\MOSHE\';
[fname1,fpath1]=uigetfile([fpath,'\*.bmp'],'1st snapshot');
[fname2,fpath2]=uigetfile([fpath1,'\*.bmp'],'2nd snapshot');

Snap1=double(imread([fpath1,fname1]));
Snap1=Snap1(:,:,1);
Snap2=double(imread([fpath2,fname2]));
Snap2=Snap2(:,:,1);
runl=100;
corr=fast_xcorr2(Snap1,Snap2);
[x,y]=find_shift(corr);
r=sqrt(x.^2 + y.^2)
mag=r*5.5/runl
pixsize=runl/r
ang=atan(-y/x)
ssize=size(Sanp1);

FindRotationAndPixelSize(Snap1(:),Snap2(:),ssize(2),100,0);

proj=zeros(size(Snap1,1)+abs(y),size(Snap1,2)+abs(x),3);

w=size(Snap1,2);
h=size(Snap1,1);
if x>=0 & y>=0
    proj(y+1:y+h,x+1:x+w,1)=Snap1;
    proj(1:h,1:w,2)=Snap2;
    proj(1:h,1:w,3)=Snap2;
elseif x>=0 & y<0
    y=abs(y);
    proj(1:h,x+1:x+w,1)=Snap1;
    proj(y+1:y+h,1:w,2)=Snap2;
    proj(y+1:y+h,1:w,3)=Snap2;
elseif x<0 & y<0
    y=abs(y);
    x=abs(x);
    proj(1:h,1:w,1)=Snap1;
    proj(y+1:y+h,x+1:x+w,2)=Snap2;
    proj(y+1:y+h,x+1:x+w,3)=Snap2;
elseif x<0 & y>=0
    x=abs(x);
    proj(y+1:y+h,1:w,1)=Snap1;
    proj(1:h,x+1:x+w,2)=Snap2;
    proj(1:h,x+1:x+w,3)=Snap2;
end
proj=uint8(proj);
imshow(proj)