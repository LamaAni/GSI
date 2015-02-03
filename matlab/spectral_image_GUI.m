function varargout = spectral_image_GUI(varargin)
% SPECTRAL_IMAGE_GUI MATLAB code for spectral_image_GUI.fig
%      SPECTRAL_IMAGE_GUI, by itself, creates a new SPECTRAL_IMAGE_GUI or raises the existing
%      singleton*.
%
%      H = SPECTRAL_IMAGE_GUI returns the handle to a new SPECTRAL_IMAGE_GUI or the handle to
%      the existing singleton*.
%
%      SPECTRAL_IMAGE_GUI('CALLBACK',hObject,eventData,handles,...) calls the local
%      function named CALLBACK in SPECTRAL_IMAGE_GUI.M with the given input arguments.
%
%      SPECTRAL_IMAGE_GUI('Property','Value',...) creates a new SPECTRAL_IMAGE_GUI or raises the
%      existing singleton*.  Starting from the left, property value pairs are
%      applied to the GUI before spectral_image_GUI_OpeningFcn gets called.  An
%      unrecognized property name or invalid value makes property application
%      stop.  All inputs are passed to spectral_image_GUI_OpeningFcn via varargin.
%
%      *See GUI Options on GUIDE's Tools menu.  Choose "GUI allows only one
%      instance to run (singleton)".
%
% See also: GUIDE, GUIDATA, GUIHANDLES

% Edit the above text to modify the response to help spectral_image_GUI

% Last Modified by GUIDE v2.5 02-Feb-2015 12:07:50

% Begin initialization code - DO NOT EDIT
gui_Singleton = 1;
gui_State = struct('gui_Name',       mfilename, ...
    'gui_Singleton',  gui_Singleton, ...
    'gui_OpeningFcn', @spectral_image_GUI_OpeningFcn, ...
    'gui_OutputFcn',  @spectral_image_GUI_OutputFcn, ...
    'gui_LayoutFcn',  [] , ...
    'gui_Callback',   []);
if nargin && ischar(varargin{1})
    gui_State.gui_Callback = str2func(varargin{1});
end

if nargout
    [varargout{1:nargout}] = gui_mainfcn(gui_State, varargin{:});
else
    gui_mainfcn(gui_State, varargin{:});
end
% End initialization code - DO NOT EDIT


% --- Executes just before spectral_image_GUI is made visible.
function spectral_image_GUI_OpeningFcn(hObject, eventdata, handles, varargin)
% This function has no output args, see OutputFcn.
% hObject    handle to figure
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)
% varargin   command line arguments to spectral_image_GUI (see VARARGIN)

% Choose default command line output for spectral_image_GUI
handles.output = hObject;
axes(handles.im_ax)
axis off
axes(handles.txt_ax)
axis off
handles.txt=text(.5,.5,'','fontsize',14,'VerticalAlignment','middle','horizontalalignment','center');
axes(handles.sp_ax)
title('Spectrum','fontsize',10)
xlabel('\lambda (nm)','fontsize',10)
ylabel('Intensity','fontsize',10)
grid on
set(handles.sp_ax,'xlim',[400 700],'xtick',400:25:700,'fontsize',6,'box','on')

handles.fpath='f:\Measurements\';


% Update handles structure
guidata(hObject, handles);

% UIWAIT makes spectral_image_GUI wait for user response (see UIRESUME)
% uiwait(handles.figure1);


% --- Outputs from this function are returned to the command line.
function varargout = spectral_image_GUI_OutputFcn(hObject, eventdata, handles)
% varargout  cell array for returning output args (see VARARGOUT);
% hObject    handle to figure
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Get default command line output from handles structure
varargout{1} = handles.output;


% --- Executes on button press in load_btn.
function load_btn_Callback(hObject, eventdata, handles)
% hObject    handle to load_btn (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)
[handles.fname,handles.fpath]=uigetfile([handles.fpath,'*.mat'],'Load spectral image file');
handles.filename=[handles.fpath,handles.fname];
set(handles.txt,'string','Loading...')
drawnow
handles.sp_data=load(handles.filename);
axes(handles.im_ax);
imshow(handles.sp_data.RGB);
set(handles.txt,'string','')

handles.BG_spec=nan;
handles.smooth_BG=nan;

handles.save_data=handles.sp_data.lambda(:);
handles.spec_num=1;
handles.save_header{handles.spec_num}='Lambda (nm)';

guidata(hObject, handles);

% --- Executes on button press in save_btn.
function save_btn_Callback(hObject, eventdata, handles)
% hObject    handle to save_btn (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

[handles.fname,handles.fpath]=uiputfile([handles.filename(1:end-3),'jpg'],'Save RGB image');
set(handles.txt,'string','Saving...')
drawnow
imwrite(handles.sp_data.RGB,[handles.fpath,handles.fname],'jpg');
set(handles.txt,'string','')
guidata(hObject, handles);

% --- Executes on button press in zoomin.
function zoomin_Callback(hObject, eventdata, handles)
% hObject    handle to zoomin (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)
set(handles.txt,'string',{'Use the mouse to';'define an area';'For Zoom-In'})
axes(handles.im_ax)
rect=round(getrect);
axis([rect(1),rect(1)+rect(3),rect(2),rect(2)+rect(4)]);

set(handles.txt,'string','')
guidata(hObject, handles);

% --- Executes on button press in zoomout.
function zoomout_Callback(hObject, eventdata, handles)
% hObject    handle to zoomout (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

axes(handles.im_ax)
axis([1,size(handles.sp_data.RGB,2),1,size(handles.sp_data.RGB,1)]);
guidata(hObject, handles);


% --- Executes on button press in show_px_spec.
function show_px_spec_Callback(hObject, eventdata, handles)
% hObject    handle to show_px_spec (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)


set(handles.txt,'string',...
    {'Use the mouse to see spectra:';'Left click - red line';'Right click - green line';
    'Middle click - blue line';'Press ESC to exit'},...
    'fontsize',10)
m_btn=1;
axes(handles.sp_ax);
cla;

spec1=1;
spec2=1;
spec3=1;
handles.SPEC1=[];
handles.SPEC2=[];
handles.SPEC3=[];

while m_btn~=27
    hold on
    w=get(handles.av_1,'value')*0+get(handles.av_3,'value')*1+get(handles.av_5,'value')*2;
    axes(handles.im_ax);
    [X,Y,m_btn]=ginput2(1);
    X=round(X);
    Y=round(Y);
    handles.pixel_spec=(handles.sp_data.spec((Y-w):(Y+w),(X-w):(X+w),:));
    handles.pixel_spec=mean(handles.pixel_spec,1);
    handles.pixel_spec=mean(handles.pixel_spec,2);
    handles.pixel_spec=handles.pixel_spec(:);
    
    switch get(handles.pl_type,'value')
        case 1
        case 2
            handles.pixel_spec=handles.pixel_spec./handles.smooth_BG(:);
        case 3
            handles.pixel_spec=log10(handles.smooth_BG(:)./handles.pixel_spec);
    end
    
    peak=find(handles.pixel_spec==max(handles.pixel_spec),1);
    axes(handles.sp_ax);
    if m_btn==1
        try
            delete(plt1)
        catch
        end
        plt1=plot(handles.sp_data.lambda,handles.pixel_spec,'r',[handles.sp_data.lambda(peak),handles.sp_data.lambda(peak)],[0,max(handles.sp_data.spec(:)).*1.2],'m--','linewidth',2);
        handles.SPEC1(:,spec1)=handles.pixel_spec;
        spec1=spec1+1;
    elseif m_btn==2
        try
            delete(plt2)
        catch
        end
        plt2=plot(handles.sp_data.lambda,handles.pixel_spec,'b',[handles.sp_data.lambda(peak),handles.sp_data.lambda(peak)],[0,max(handles.sp_data.spec(:)).*1.2],'m--','linewidth',2);
        handles.SPEC2(:,spec2)=handles.pixel_spec;
        spec2=spec2+1;
    elseif m_btn==3
        try
            delete(plt3)
        catch
        end
        plt3=plot(handles.sp_data.lambda,handles.pixel_spec,'g',[handles.sp_data.lambda(peak),handles.sp_data.lambda(peak)],[0,max(handles.sp_data.spec(:)).*1.2],'m--','linewidth',2);
        handles.SPEC3(:,spec3)=handles.pixel_spec;
        spec3=spec3+1;
    end
    title(['Spectrum, peak @ ',num2str(round(handles.sp_data.lambda(peak))),'nm'],'fontsize',10)
    xlabel('\lambda (nm)','fontsize',10)
    switch get(handles.pl_type,'value')
        case 1
            ylabel('Intensity I(\lambda)','fontsize',10)
        case 2
            ylabel('Normalized intensity I/I_0','fontsize',10)
        case 3
            ylabel('Absorbance log_{10}(I_0/I)','fontsize',10)
    end
    set(handles.sp_ax,'ylim',[0,1.2.*max(handles.pixel_spec)]);
    
    
end
set(handles.txt,'string','','fontsize',14)
guidata(hObject, handles);

% --- Executes on button press in whitebalance.
function whitebalance_Callback(hObject, eventdata, handles)
% hObject    handle to whitebalance (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)
set(handles.txt,'string',{'Use the mouse to';'define a rectangle';'white area in the image'})
axes(handles.im_ax)
rect=round(getrect);
R=handles.sp_data.RGB(rect(2)+[1:rect(4)],rect(1)+[1:rect(3)],1);
G=handles.sp_data.RGB(rect(2)+[1:rect(4)],rect(1)+[1:rect(3)],2);
B=handles.sp_data.RGB(rect(2)+[1:rect(4)],rect(1)+[1:rect(3)],3);

r=mean(R(:));
g=mean(G(:));
b=mean(B(:));
A=max(handles.sp_data.RGB(:));
handles.sp_data.RGB(:,:,1)=A*handles.sp_data.RGB(:,:,1)./r;
handles.sp_data.RGB(:,:,2)=A*handles.sp_data.RGB(:,:,2)./g;
handles.sp_data.RGB(:,:,3)=A*handles.sp_data.RGB(:,:,3)./b;
imshow(handles.sp_data.RGB)
set(handles.txt,'string','')
guidata(hObject, handles);


% --- Executes on button press in save_spec.
function save_spec_Callback(hObject, eventdata, handles)
% hObject    handle to save_spec (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

[handles.fname,handles.fpath]=uiputfile([handles.filename(1:end-4),'_specs.txt'],'Save Spectra data');

set(handles.txt,'string','Saving...')
drawnow

fid = fopen([handles.fpath, handles.fname],'wt');

for i=1:length(handles.save_header)
    fprintf(fid,'%s',handles.save_header{i});
    if i~=length(handles.save_header)
        fprintf(fid,'\t');
    else
        fprintf(fid,'\n');
    end
end

for i=1:size(handles.save_data,1)
    for j=1:size(handles.save_data,2)
        
        fprintf(fid,'%f',handles.save_data(i,j));
        if j~=size(handles.save_data,2)
            fprintf(fid,'\t');
        else
            fprintf(fid,'\n');
        end
    end
end

fclose(fid)


% 
% sp1=handles.SPEC1;
% sp2=handles.SPEC2;
% sp3=handles.SPEC3;
% lambda=handles.sp_data.lambda(:);
% 
% save([handles.fpath,handles.fname],'sp1','sp2','sp3','lambda')
% 
set(handles.txt,'string','')
guidata(hObject, handles);


% --- Executes on button press in show_av_spec.
function show_av_spec_Callback(hObject, eventdata, handles)
% hObject    handle to show_av_spec (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

set(handles.txt,'string',{'Use the mouse to define an area';'Double-click to exit'},'fontsize',10)

while 1
    
    axes(handles.im_ax);
    
    rect=round(getrect);
    
    
    try
        delete(h_rect)
    catch
    end
    
    
    if (rect(3)==0)|(rect(4)==0)
        break
    end
    hold on
    
    h_rect=rectangle('Position',rect);
    
    spec_roi=handles.sp_data.spec(rect(2):(rect(2)+rect(4)), rect(1):(rect(1)+rect(3)),:);
    spec_roi=reshape(spec_roi,size(spec_roi,1)*size(spec_roi,2),size(spec_roi,3));
    spec_roi=mean(spec_roi);
    
    
    
    switch get(handles.pl_type,'value')
        case 1
            handles.pixel_spec=spec_roi(:);
        case 2
            handles.pixel_spec=spec_roi(:)./handles.smooth_BG(:);
        case 3
            handles.pixel_spec=log10(handles.smooth_BG(:)./spec_roi(:));
    end
    
    peak=find(handles.pixel_spec==max(handles.pixel_spec),1);
    axes(handles.sp_ax);
    cla
    hold on
    try
        delete(plt)
    catch
    end
    plt=plot(handles.sp_data.lambda,handles.pixel_spec,'r',[handles.sp_data.lambda(peak),handles.sp_data.lambda(peak)],[0,max(handles.sp_data.spec(:)).*1.2],'m--','linewidth',2);
    
    title(['Spectrum, peak @ ',num2str(round(handles.sp_data.lambda(peak))),'nm'],'fontsize',10)
    xlabel('\lambda (nm)','fontsize',10)
    switch get(handles.pl_type,'value')
        case 1
            ylabel('Intensity I(\lambda)','fontsize',10)
        case 2
            ylabel('Normalized intensity I/I_0','fontsize',10)
        case 3
            ylabel('Absorbance log_{10}(I_0/I)','fontsize',10)
    end
    set(handles.sp_ax,'ylim',[0,1.2.*max(handles.pixel_spec)]);
    
end %while

set(handles.txt,'string','')

guidata(hObject, handles);


% --- Executes on button press in load_ref.
function load_ref_Callback(hObject, eventdata, handles)
% hObject    handle to load_ref (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

[handles.fname,handles.fpath]=uigetfile([handles.fpath,'*.mat'],'Load reference image');
handles.BG_spec=ref_spec([handles.fpath, handles.fname]);
handles.smooth_BG=smooth(handles.BG_spec,3);

guidata(hObject, handles);



% --- Executes on selection change in pl_type.
function pl_type_Callback(hObject, eventdata, handles)
% hObject    handle to pl_type (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: contents = cellstr(get(hObject,'String')) returns pl_type contents as cell array
%        contents{get(hObject,'Value')} returns selected item from pl_type

if get(handles.pl_type,'value')~=1
    if isnan(handles.BG_spec)
        msgbox('You must load reference spectrum!','Reference spectrum');
        set(handles.pl_type,'value',1);
    end
end
guidata(hObject, handles);


% --- Executes during object creation, after setting all properties.
function pl_type_CreateFcn(hObject, eventdata, handles)
% hObject    handle to pl_type (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: listbox controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end


% --- Executes on button press in add_spec.
function add_spec_Callback(hObject, eventdata, handles)
% hObject    handle to add_spec (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

handles.spec_num=handles.spec_num+1;
handles.save_data=[handles.save_data,handles.pixel_spec];
handles.save_header{handles.spec_num}=['Spec. #',num2str(handles.spec_num-1,'%.2u')];
guidata(hObject, handles);



% --- Executes on button press in lambda_range.
function lambda_range_Callback(hObject, eventdata, handles)
% hObject    handle to lambda_range (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

wl_range=inputdlg({['min value (',num2str(ceil(min(handles.sp_data.lambda))),' nm)'];...
    ['max value (',num2str(floor(max(handles.sp_data.lambda))),' nm)'];['Ticks interval']},...
    'Wavelength Range',1,{'400';'700';'25'});
minWL=str2num(wl_range{1});
maxWL=str2num(wl_range{2});
tickWL=str2num(wl_range{3});

set(handles.sp_ax,'xlim',[minWL maxWL],'xtick',minWL:tickWL:maxWL);

guidata(hObject, handles);


% --- Executes on button press in smooting.
function smooting_Callback(hObject, eventdata, handles)
% hObject    handle to smooting (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

smth=str2num(cell2mat(inputdlg('Enter smoothing value:','Smoothing',1,{'3'})));
handles.smooth_BG=smooth(handles.BG_spec,smth);

figure
plot(handles.sp_data.lambda,handles.BG_spec,'r:',handles.sp_data.lambda,handles.smooth_BG,'g','linewidth',2)
legend('Reference spectrum','Smoothed spectrum','location','NorthWest')

guidata(hObject, handles);


