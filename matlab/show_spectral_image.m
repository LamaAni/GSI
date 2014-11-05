function []=show_spectral_image(filename)
fpath='D:\Measurments\20141027\';
if nargin==0
    [fname,fpath]=uigetfile([fpath,'*.mat'],'Load spectral image file');
    filename=[fpath,fname];
end

load(filename);

figure('units','normalized','position',[0 0 1 1])
ax=subplot(3,4,[1:3,5:7,9:11]);
imshow(RGB);
axis equal
axis off

w_balance=questdlg('Do you want to do a white balancing?','White Balance','Yes','No','Yes');

if strcmp('Yes',w_balance)
    subplot(3,4,4)
    txt_h=text(0.5,0.5,{'Choose a white rectangle';'area using the mouse'}...
        ,'HorizontalAlignment','center','fontsize',13,'fontweight','bold');
    axis off
    axes(ax)
    rect=round(getrect);
    delete(txt_h);
    R=RGB(rect(2)+[1:rect(4)],rect(1)+[1:rect(3)],1);
    G=RGB(rect(2)+[1:rect(4)],rect(1)+[1:rect(3)],2);
    B=RGB(rect(2)+[1:rect(4)],rect(1)+[1:rect(3)],3);
    
    r=mean(R(:));
    g=mean(G(:));
    b=mean(B(:));
    A=.98*max(RGB(:));
    RGB(:,:,1)=A*RGB(:,:,1)./r;
    RGB(:,:,2)=A*RGB(:,:,2)./g;
    RGB(:,:,3)=A*RGB(:,:,3)./b;
    
    subplot(3,4,[1:3,5:7,9:11])
    imshow(RGB);
    axis equal
    axis off
    
end

saveRGB=questdlg('Do you want Save the RGB image?','Save','Yes','No','No');

if strcmp(saveRGB,'Yes')
    [fname,fpath]=uiputfile([filename(1:end-3),'jpg'],'Save RGB image');
    imwrite(RGB,[fpath,fname],'jpg');
end


subplot(3,4,4)
text(0.5,0.5,{'Click on a pixle';'to see its spectrum'}...
    ,'HorizontalAlignment','center','fontsize',13,'fontweight','bold')
axis off




while 1
    [X,Y]=ginput(1);
    X=round(X);
    Y=round(Y);
    pixel_spec=squeeze(spec(Y,X,:));
    peak=find(pixel_spec==max(pixel_spec),1);
    subplot(3,4,8)
    plot(lambda,pixel_spec,'b',[lambda(peak),lambda(peak)],[0,max(spec(:)).*1.2],':k')
    axis([400,750,0,1.2.*max(spec(:))]);
    xlabel('wavelength');
    ylabel('Intensity');
    title({['Spectrum at x=',num2str(X),', y=',num2str(Y)];['Peak -> ',num2str(round(lambda(peak))),' nm']})
    
end





