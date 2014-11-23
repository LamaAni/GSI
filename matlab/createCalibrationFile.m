fclose('all');

clear all
close all

% ZF=256; %zero filling factor


clr={'b';'g';'r'};
fpath='G:\Measurements\';


for t=1:3
    [fname{t},fpath]=uigetfile([fpath,'*.sdat']);
    wl(t)=str2num(cell2mat(inputdlg('Enter Wavelength [nm]',fname{t})));
    lgnd{t}=[num2str(wl(t)),' nm'];
end

oredered_wl=sort(wl);

for t=1:3
    file_name=[fpath,fname{t}];
    %     fInfo=dir(file_name);
    %     fileSize=fInfo.bytes;
    %
    %     fid=fopen(file_name,'r');
    %     if (fid==-1),
    %         error('file not found');
    %     end;
    %     p=fread(fid,3,'int32');
    %     W=p(1);
    %     H=p(2);
    %     Np=p(3);
    %
    %     p=fread(fid,2,'double');
    %     dx=p(1);
    %     pixelSize=p(2);
    %
    %
    %     data=zeros(H,W,Np,'uint8');
    %
    %     for i=1:W
    %         vec=fread(fid,Np*H,'uint8');
    %         vec=reshape(vec,Np,H);
    %         vec=permute(vec,[2,3,1]);
    %         data(:,i,:)=vec;
    %     end
    %
    %     data=double(data);
    %     avData=mean(data,3);
    %     avData=repmat(avData,[1,1,Np]);
    %     data=data-avData;
    %     F=abs(fft(data,ZF,3));
    %     F=F(:,:,1:round(ZF./2));
    %
    %     F=reshape(F,H*W,size(F,3));
    F=ReadSpec(file_name);
    F=reshape(F,size(F,1)*size(F,2),size(F,3));
    F(isnan(F))=0;
    avSpec(:,t)=mean(F);
    [peaks idx]=sort(avSpec(:,t));
    YN='No';
    k=length(idx);
    while strcmp(YN,'No');
        wl_peak(t)=idx(k);
        plot(avSpec(:,t),'color',clr{find(oredered_wl==wl(t))})
        hold on
        plot(wl_peak(t),avSpec(wl_peak(t),t),'o','color',clr{find(oredered_wl==wl(t))},'markersize',14,'linewidth',3)
        xlabel('FFT channel')
        ylabel('Spectrum')
        title({fname{t};[num2str(wl(t)),' nm']},'interpreter','none')
        YN=questdlg('Peak position OK?',[num2str(wl(t)),' nm'],'Yes','No','Yes');
        k=k-1;
        close (gcf)
    end
end

ch2Lambda = @(x) [x(1)./(wl_peak(1)+x(2)) - x(3) - wl(1);...
    x(1)./(wl_peak(2)+x(2)) - x(3) - wl(2);...
    x(1)./(wl_peak(3)+x(2)) - x(3) - wl(3)];
%%
coeff_0=[2700,-1,-60];
options=optimset('Display','off');
calibrationCoefficient = fsolve(ch2Lambda,coeff_0,options);

% chnls=1:round(ZF./2);
chnls=1:size(F,2);
lambda=calibrationCoefficient(1)./(chnls + calibrationCoefficient(2)) - calibrationCoefficient(3);
%%
subplot(2,1,1)
for t=1:3
    hold on
    plot(chnls,avSpec(:,t),'color',clr{find(oredered_wl==wl(t))})
    xlabel('FFT channel')
    ylabel('Spectrum')
    title('FFT Channel')
end
legend(lgnd)

for t=1:3
    plot(chnls(wl_peak(t)),avSpec(wl_peak(t),t),'o','color',clr{find(oredered_wl==wl(t))},'markersize',14,'linewidth',3)
end

subplot(2,1,2)
for t=1:3
    hold on
    plot(lambda,avSpec(:,t),'color',clr{find(oredered_wl==wl(t))})
    xlabel('\Lambda [nm]')
    ylabel('Spectrum')
    title('Wave Length')
end
legend(lgnd)
for t=1:3
    plot(lambda(wl_peak(t)),avSpec(wl_peak(t),t),'o','color',clr{find(oredered_wl==wl(t))},'markersize',14,'linewidth',3)
end
axis([400 750 0 max(avSpec(:))])
%%
[fname,fpath]=uiputfile([fpath,'*.csv']);
csvwrite([fpath,fname],calibrationCoefficient)