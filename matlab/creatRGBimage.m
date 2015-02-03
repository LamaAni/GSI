close all
clear all

f_path='f:\Measurements\';

[cal_file,f_path]=uigetfile([f_path,'*.csv'],'Load calibration file');
[spec_file,f_path]=uigetfile([f_path,'*.sdat'],'Load spectrum file');
def_name=[spec_file(1:find(spec_file=='.',1,'first')),'mat'];
[f_name,f_path] = uiputfile([f_path,def_name],'Save spectrum');

[spec,chnls]=ReadSpec([f_path,spec_file]);
calibrationCoefficient=csvread([f_path,cal_file]);

% chnls=1:size(spec,3);
lambda=calibrationCoefficient(4)./(chnls + calibrationCoefficient(5)) - calibrationCoefficient(6);

% idx=find((lambda>=400)&(lambda<=760));
% lambda=lambda(idx);
% spec=spec(:,:,idx);

%%
RGB=spec2image(spec,lambda);
RGB=RGB-min(RGB(:));
RGB=RGB./max(RGB(:));



save([f_path,f_name],'spec','RGB','lambda')
clear spec RGB
show_spectral_image([f_path,f_name])