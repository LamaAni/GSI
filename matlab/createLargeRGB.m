clear all
close all

f_path='G:\Measurements\';

[spec_file,f_path]=uigetfile([f_path,'*.sdat'],'Load spectrum file');
def_name=[spec_file(1:find(spec_file=='.',1,'first'))];
fname=def_name(1:end-1);


gsiasm=NET.addAssembly('D:\Code\SystemControl\GSI\bin\Debug\GSI.dll');
reader=GSI.Storage.Spectrum.SpectrumStreamReader.Open([f_path,spec_file]);
H=double(reader.Settings.Height);
W=double(reader.Settings.Width);
reader.Close;
m=2000;
Ww=ceil(W./m);
Hh=ceil(H./m);

largeRGB=uint8([]);
Q=Ww*Hh;
q=0;
WB=waitbar(0,'Creates RGB image...');
for j=1:Hh
    for i=1:Ww
        q=q+1;
        waitbar(q/Q,WB,['Creates RGB image, ',num2str(q),' of ',num2str(Q)]);
        data=load([f_path,fname,'_',num2str(i,'%.2d'),'x',num2str(j,'%.2d'),'.mat']);
        y_pos=(1:size(data.RGB,2))+(j-1)*m;
        x_pos=(1:size(data.RGB,1))+(i-1)*m;
        largeRGB(x_pos,y_pos,:)=uint8(255*data.RGB);
        imshow(largeRGB)
        drawnow;
    end %i=Ww
end %j=Hh
close(WB)

imwrite(largeRGB,[f_path,fname,'_RGB.jpg'],'jpg')
