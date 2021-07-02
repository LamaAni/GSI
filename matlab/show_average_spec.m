clear all
close all
fclose('all');
nheader=4;
[fname,fpath]=uigetfile('D:\Code\SystemControl\TestRun\bin\Debug\*.dat','multiselect','on');

if ~iscell(fname)
    a=fname
    fname=cell(1);
    fname{1}=a;
end
for j=1:length(fname)
    file_name=[fpath,fname{j}];
    fInfo=dir(file_name);
    fileSize=fInfo.bytes;
    
    fid=fopen(file_name,'r');
    if (fid==-1),
        error('file not found');
    end;
    p=fread(fid,nheader,'int32');
    Np=p(4);
    W=p(2);
    H=p(3);
    numOfLines=p(1);

    nfloats=W*H*Np;
    partial=[];
    data=zeros(Np,H*numOfLines,W);
    for i=1:numOfLines
        partial=fread(fid,nfloats,'float');
        partial=reshape(partial,Np,H,W);
        hidx=(i-1)*H+(1:H);
        data(:,hidx,:)=partial;
    end
    fclose(fid);
    
    data=shiftdim(data,1);
    
    data=mean(data,1);
    spec{j}=squeeze(mean(data,2));
end
hold on;
c=colormap(hsv(length(spec)));
for i=1:length(spec)
    plot(spec{i},'color',c(i,:))
end
hold off;
legend(fname)
