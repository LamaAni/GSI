clear all
close all

f_path='G:\Measurements\';

[cal_file,f_path]=uigetfile([f_path,'*.csv'],'Load calibration file');
[spec_file,f_path]=uigetfile([f_path,'*.sdat'],'Load spectrum file');
def_name=[spec_file(1:find(spec_file=='.',1,'first'))];
fname=cell2mat(inputdlg('Enter File name','Save',1,{def_name(1:end-1)}));


gsiasm=NET.addAssembly('D:\Code\SystemControl\GSI\bin\Debug\GSI.dll');
reader=GSI.Storage.Spectrum.SpectrumStreamReader.Open([f_path,spec_file]);
H=double(reader.Settings.Height);
W=double(reader.Settings.Width);
K=double(reader.Settings.FftDataSize);

h=2000;
w=2000;

Ww=floor(W./w);
Hh=floor(H./h);


calibrationCoefficient=csvread([f_path,cal_file]);

chnls=double(1:K);
lambda=calibrationCoefficient(1)./(chnls + calibrationCoefficient(2)) - calibrationCoefficient(3);

idx=find((lambda>=400)&(lambda<=760));
lambda=lambda(idx);



tic
p=waitbar(0,'Loading...');
q=0;
Q=((Ww+1)*(Hh+1));
for k=1:Ww+1
    for l=1:Hh+1
        q=q+1;
        waitbar(q./Q,p,['Loading ',num2str(q),' of ',num2str(Q)])
        
        if k==(Ww+1)
            ww=W-w*Ww-2;
        else
            ww=w;
        end
        
        if l==(Hh+1)
            hh=H-h*Hh-2;
        else
            hh=h;
        end
        
        spec=zeros(ww,hh,K);
        
        initialW=(k-1)*w;
        initialH=(l-1)*h;
        
        for i=0:ww-1
            for j=0:hh-1
                pix_spec=reader.ReadSpectrumPixel(initialW+i,initialH+j);
                spec(i+1,j+1,:)=reshape(pix_spec.double,[1 1 K]);
            end %j
        end %i
        spec=spec(:,:,idx);
        RGB=spec2image(spec,lambda);
        RGB=RGB-min(RGB(:));
        RGB=RGB./max(RGB(:));
        save([f_path,fname,'_',num2str(k,'%.2d'),'x',num2str(l,'%.2d')],'spec','RGB','lambda')
        
        
    end %l
end %k
reader.Close;
close(p)
toc