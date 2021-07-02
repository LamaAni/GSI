clear all
close all

load('cross_corr_test_data')

x1=x1-min(x1);
x2=x2-min(x2);

l=length(x1)-1;

idx=0;
a=zeros(1,1+2*l);
for i=-l:l
    idx=idx+1;
    if i<=0
        for j=1:(l+i+1)
            a(idx)=a(idx)+x1(j).*x2(j-i);
        end
    elseif i>0
        for j=1:(l-i+1)
            a(idx)=a(idx)+x1(j+i).*x2(j);
        end
        
    end
end

q=xcorr(x1,x2);

plot (a)
hold on
plot(q,':g')


t=find(a==max(a));
shft=length(x1)-t

t1=find(q==max(q));
shft1=length(x1)-t1

im1(1,:)=x1;
im1(2,:)=x2;

if shft<=0
    im2(1,:)=x1(shft:end);
    im2(2,:)=x2(1:end-shft+1);
else
    im2(2,:)=x2(shft:end);
    im2(1,:)=x1(1:end-shft+1);
end

ax(1)=subplot(2,1,1);
imagesc(im1)
axis off
axis equal
colormap gray

ax(2)=subplot(2,1,2);
imagesc(im2)
axis off
axis equal
colormap gray

linkaxes(ax)