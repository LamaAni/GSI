% test load preview
ValidateAndLoadGSI('..\SystemControl\TestRun\bin\Debug\GSI.dll');
%[fname,pathname]=uigetfile('*.prevdat');
%previewfilename=[pathname,'\',fname];
previewfilename='D:\GSIMeasurments\\sample_100_1.rawstack.forier.sdat.prevdat';

% Show preview.
prev=LoadPreviewImage(previewfilename,1000,0,0,200,200);

% normalizing
prev_norm=1/abs(max(prev(:))-min(prev(:)));
prev_img=(prev-min(prev(:))).*prev_norm;
image(prev_img);
axis equal;
% imgstor=zeros(w,h);
% imgstor(:)=prev(1,:,:)+prev(2,:,:)+prev(3,:,:);
% imagesc(imgstor)
% axis equal