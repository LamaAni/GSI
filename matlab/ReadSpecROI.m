function spec=ReadSpecROI(fname)

gsiasm=NET.addAssembly('D:\Code\SystemControl\GSI\bin\Debug\GSI.dll');
reader=GSI.Storage.Spectrum.SpectrumStreamReader.Open(fname);
H=reader.Settings.Height;
W=reader.Settings.Width;
k=reader.Settings.FftDataSize;

ROI=inputdlg({'start at W=?';'lenght:';'start at H=?';'lenght:'},['W=',num2str(W),'  ,  H=',num2str(H)],[1 50;1 50;1 50;1 50],{'1','1000','1','1000'});
initialW=str2num(ROI{1});
newW=str2num(ROI{2});
initialH=str2num(ROI{3});
newH=str2num(ROI{4});
im_size=double(newH*newW);
spec=zeros(newW,newH,k);
h = waitbar(0,'Loading spectrum') ;
set(h,'Position', [345 356.25 270 76.25])
tic
q=0;
for i=0:newW-1
    for j=0:newH-1
        q=q+1;
        pix_spec=reader.ReadSpectrumPixel(initialW+i,initialH+j);
        spec(i+1,j+1,:)=reshape(pix_spec.double,[1 1 k]);
        
        if (q./100)==ceil(q./100)
            time_left=toc*(1-(q./im_size))./(q./im_size);
            tlS=floor(rem(time_left,60));
            tlM=floor(rem(time_left/60,60));
            tlH=floor(time_left/3600);
            t_left=[num2str(tlH,'%.2d'),':',num2str(tlM,'%.2d'),':',num2str(tlS,'%.2d')];
            etS=floor(rem(toc,60));
            etM=floor(rem(toc/60,60));
            etH=floor(toc/3600);
            elapsed_time=[num2str(etH,'%.2d'),':',num2str(etM,'%.2d'),':',num2str(etS,'%.2d')];
            waitbar(q./im_size,h,{['Loading spectrum, ',num2str(round(100*q./im_size)),'%'];['Elapsed time: ',elapsed_time];['Time left:        ',t_left]});
        end
    end
end
reader.Close;
close(h)
toc
