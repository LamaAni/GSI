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
    disp(i)
end
%%
data=double(data);
avData=mean(data,3);
avData=repmat(avData,1,1,Np);
data=data-avData;
F=abs(fft(data,256,3));
% F=F(:,:,1:64);

%%
f1=figure('units','normalized','position',[0 0 1 1]);

C=colormap(hsv(ceil(dx/2)));
colorbar
ax(1)=subplot(3,2,[1]);
imagesc(mean(avData+data,3));
hold on
h1=plot([1,dx],[1,1],'b','marker','s');
colormap gray(256)
title('from interferogram')
axis equal
axis off

ax(2)=subplot(3,2,[2]);
imagesc(mean(F,3));
title('from FFT')
colormap gray(256)
axis equal
axis off
linkaxes(ax)
h=imline(ax(2),[1,dx],[1,1]);

f2=figure;

while 1
    %     [x,y]=ginput(1);
    position = round(wait(h));
    y=position(1,2);
    set(h1,'Xdata',position(:,1),'Ydata',position(:,2));
    cc=0;
    figure(f1);
    for x=position(1,1):2:position(2,1)
        
        cc=cc+1;
        pixel_inter=squeeze(data(round(y),round(x),:));
        pixel_spec=squeeze(F(round(y),round(x),:));
        
        subplot(3,2,[3 5])
        if cc==1
            cla;
        end
        hold on
        plot(pixel_inter,'color',C(cc,:))
        xlabel('');
        ylabel('');
        title(['Interferogram at x=',num2str(position(1,1)),':',num2str(position(2,1)),', y=',num2str(round(y))])
        
        
        subplot(3,2,[4 6])
        if cc==1
            cla;
        end
        hold on
        plot(pixel_spec,'color',C(cc,:))
        xlabel('channel');
        ylabel('Spectrum');
        title(['Spectrum at x=',num2str(position(1,1)),':',num2str(position(2,1)),', y=',num2str(round(y))])
    end
    
    cc=0;
    for x=position(1,1):2:position(2,1)
        
        cc=cc+1;
        pixel_inter=squeeze(data(round(y),round(x),:));
        pixel_spec=squeeze(F(round(y),round(x),:));
        
        figure(f2)
        hold on
        plot(pixel_inter,'color',C(cc,:))
        xlabel('');
        ylabel('');
        title(['Interferogram at x=',num2str(position(1,1)),':',num2str(position(2,1)),', y=',num2str(round(y))])
        
    end
end