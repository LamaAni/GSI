function A=spec2image(spec,wl,a_mat)

if nargin==2
    a_mat=8;
end

wl=reshape(wl,1,1,length(wl));
wl=repmat(wl,size(spec,1),size(spec,2));

% nor=(3.5+max(spec(:))).*exp(-((wl-550)./202).^2)-3.5;
% spec=spec./nor;

% x, y, z - 10 deg color matching functions, based on: http://www.cvrl.org/cmfs.htm
x = 1.155.*exp(-((wl-593.1)./48.31).^2) + 0.4168.*exp(-((wl-444.1)./27.49).^2);
y = 1.015.*exp(-((wl-555.5)./65.89).^2);
z = 2.153.*exp(-((wl-447.9)./31.05).^2);

% a=M^-1 matrices
switch a_mat
    case 1 %http://www.cs.rit.edu/~ncs/color/t_spectr.html
        a=[...
            3.240479 -1.537150 -0.498535 ;...
            -0.969256  1.875992  0.041556 ;...
            0.055648 -0.204043  1.057311 ];
    case 2 % fundamentals of image processing, Ian T. Young et. al.
        a=[...
            1.9107  -0.5326  -0.2883 ;...
            -0.9843   1.9984  -0.0283 ;...
            0.0583  -0.1185   0.8986 ];
        
        % http://www.brucelindbloom.com/index.html?Equations.html
    case 3 % Bruce RGB, D65
        a=[...
            2.7454669 -1.1358136 -0.4350269 ;...
            -0.9692660  1.8760108  0.0415560 ;...
            0.0112723 -0.1139754  1.0132541 ];
        
    case 4 % AppleRGB, D65
        a=[...
            2.9515373 -1.2894116 -0.4738445;...
            -1.0851093  1.9908566  0.0372026;...
            0.0854934 -0.2694964  1.0912975 ];
        
    case 5 % Best RGB, D50
        a=[...
            1.7552599 -0.4836786 -0.2530000;...
            -0.5441336  1.5068789  0.0215528;...
            0.0063467 -0.0175761  1.2256959 ];
        
    case 6 % CIE RGB, E
        a=[...
            2.3706743 -0.9000405 -0.4706338;...
            -0.5138850  1.4253036  0.0885814;...
            0.0052982 -0.0146949  1.0093968];
        
    case 7 % Adobe RGB (1998), D65
        a=[...
            2.0413690 -0.5649464 -0.3446944;...
            -0.9692660  1.8760108  0.0415560;...
            0.0134474 -0.1183897  1.0154096];
    case 8 % sRGB, D50
        a=[...
            3.1338561 -1.6168667 -0.4906146;...
            -0.9787684  1.9161415  0.0334540;...
            0.0719453 -0.2289914  1.4052427];
        
    case 9 % NTSC RGB, D50
        a=[...
            1.8464881 -0.5521299 -0.2766458;...
            -0.9826630  2.0044755 -0.0690396;...
            0.0736477 -0.1453020  1.3018376 ];
end %switch

% sp=sum(spec,3);
% sp=max(sp(:));

% X=sum(x.*spec,3)./sp;
% Y=sum(y.*spec,3)./sp;
% Z=sum(z.*spec,3)./sp;
X=sum(x.*spec,3);
Y=sum(y.*spec,3);
Z=sum(z.*spec,3);

r = X.*a(1,1) + Y.*a(1,2) + Z.*a(1,3);
g = X.*a(2,1) + Y.*a(2,2) + Z.*a(2,3);
b = X.*a(3,1) + Y.*a(3,2) + Z.*a(3,3);

A(:,:,1)=r;
A(:,:,2)=g;
A(:,:,3)=b;

A(A<0)=0;
% A=A./max(A(:));