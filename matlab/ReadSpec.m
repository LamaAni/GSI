function spec=ReadSpec(fname)

gsiasm=NET.addAssembly('D:\Code\SystemControl\GSI\bin\Debug\GSI.dll');
reader=GSI.Storage.Spectrum.SpectrumStreamReader.Open(fname);
H=reader.Settings.Height;
W=reader.Settings.Width;
k=reader.Settings.FftDataSize;

im_size=double(H*W);
spec=zeros(W-1,H-1,k);
h = waitbar(0,'Loading spectrum') ;
set(h,'Position', [345 356.25 270 76.25])
tic
q=0;
for i=1:W-1
    for j=1:H-1
        q=q+1;
        pix_spec=reader.ReadSpectrumPixel(i-1,j-1);
        spec(i,j,:)=reshape(pix_spec.double,[1 1 k]);
        
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
