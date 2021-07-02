clear all
close all
fclose('all');

nheader=3;
dx=10;

[fname,fpath]=uigetfile('D:\Code\SystemControl\TestRun\bin\Debug\*.dat');

file_name=[fpath,fname];
fInfo=dir(file_name);
fileSize=fInfo.bytes;
vecHeader=4;

fid=fopen(file_name,'r');
if (fid==-1),
    error('file not found');
end;
p=fread(fid,nheader,'int32');
W=p(1);
H=p(2);
Np=p(3);
numberOfVectors=(fileSize-4*nheader)/(H*Np+vecHeader*4);
numberOfLines=ceil(numberOfVectors/W);

fid2=fopen([fpath,fname,'.converted.rawstack'],'w');
fwrite(fid2,p,'int32')
%%
data=zeros(H*numberOfLines,W,Np,'uint8');
lidx=1;
vidx=1;
for i=1:numberOfVectors
    p=fread(fid,vecHeader,'int32');
    pixOffset=p(1);
    vec=fread(fid,Np*H,'uint8');
    fwrite(fid2,vec,'uint8');
    vec=reshape(vec,Np,H);
    vec=permute(vec,[2,3,1]);
    vidxs=((lidx-1)*H+1):(lidx)*H;
    data(vidxs,vidx,:)=vec;
    if(vidx==W)
        vidx=1;
        lidx=lidx+1;
        if(lidx>numberOfLines)
            break;
        end
    end
    vidx=vidx+1;
end
fclose(fid);
fclose(fid2);
% %%
% data=double(data);
% avData=mean(data,3);
% avData=repmat(avData,1,1,Np);
% data=data-avData;

%%
im=sum(data,3);
figure
imagesc(im)
axis equal
colormap gray