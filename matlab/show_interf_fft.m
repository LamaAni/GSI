cc=0;

for x=11:1:20
cc=cc+1;
DATA=squeeze(data(9,x,:));
FF=fft(DATA,128);
subplot(10,4,(cc-1)*4+1)
plot(DATA)
hold on
plot(round(Np.*[0.5 0.5]),[-50 50],'--k')
title(['data, ',num2str(sum(DATA-min(DATA)))])
axis([1 Np -50 50])

subplot(10,4,(cc-1)*4+2)
plot(real(FF))
axis([1 128,-500 500])
title('real')

subplot(10,4,(cc-1)*4+3)
plot(imag(FF))
axis([1 128,-500 500])
title('imaginary')

subplot(10,4,(cc-1)*4+4)
plot(abs(FF))
axis([1 128,0 500])
title(['abs, ',num2str(round(sum(abs(FF))))])
end
