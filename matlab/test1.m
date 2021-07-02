r1=rand(20,4e4);
r2=rand(20,4e4);

tic
a=fast_xcorr2(r1,r2);
toc