fclose('all');

clear all
close all

ZF=256; %zero filling factor



fpath='D:\Measurments\20141027\';
[fname,fpath]=uigetfile([fpath,'*.rawstack']);


file_name=[fpath,fname];
fInfo=dir(file_name);
fileSize=fInfo.bytes;

fid=fopen(file_name,'r');
if (fid==-1),
    error('file not found');
end;
p=fread(fid,3,'int32');
W=p(1);
H=p(2);
Np=p(3);

p=fread(fid,2,'double');
dx=p(1);
pixelSize=p(2);


data=zeros(H,W,Np,'uint8');

for i=1:W
    vec=fread(fid,Np*H,'uint8');
    vec=reshape(vec,Np,H);
    vec=permute(vec,[2,3,1]);
    data(:,i,:)=vec;
end

data=double(data);
avData=mean(data,3);
avData=repmat(avData,[1,1,Np]);
F_org=abs(fft(data,ZF,3));
F_org=F_org(:,:,1:round(ZF./2));
F_org=reshape(F_org,H*W,size(F_org,3));
avSpec_org=mean(F_org);

F_DC=abs(fft(data-avData,ZF,3));
F_DC=F_DC(:,:,1:round(ZF./2));
F_DC=reshape(F_DC,H*W,size(F_DC,3));
avSpec_DC=mean(F_DC);

n=linspace(-Np,Np,Np);
HG(1,1,:)=.54+.46.*cos(pi.*n./Np);
hg=repmat(HG,[H,W]);

F_apod=abs(fft((data-avData).*hg,ZF,3));
F_apod=F_apod(:,:,1:round(ZF./2));
F_apod=reshape(F_apod,H*W,size(F_apod,3));
avSpec_apod=mean(F_apod);

F_Zav=ReadSpec([file_name,'.forier.sdat']);
F_Zav=reshape(F_Zav,size(F_Zav,1)*size(F_Zav,2),size(F_Zav,3));
F_Zav(isnan(F_Zav))=0;
avSpec_Zav=mean(F_Zav);


hold on
plot(avSpec_org,'b')
plot(avSpec_DC,'r')
plot(avSpec_apod,'k')
plot(avSpec_Zav,'g')

legend('Original','DC remove','Apodization','Zav')
