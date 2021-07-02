%% calibrates the given forier data, from 3 filters, to allow forier index to wavelegnth calibration
% filterInfo, 3d matrix composed of [filter index, sample index, forier amp vector]
% filterLocation, 1d matrix composed of filter locations (arranged according to the filter index)
% zeroFill, the zero filling that was preformed on the data.
function [rslt]=GetForierCalibration(zeroFill, filterLocation, filterInfo)
	% finding averages in filter info, to allow peak finding.
	rvec=mean(squeeze(filterInfo(1,:,:)));
	gvec=mean(squeeze(filterInfo(2,:,:)));
	bvec=mean(squeeze(filterInfo(3,:,:)));
	
end