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

% Last Modified by GUIDE v2.5 30-Oct-2014 13:01:28

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
title('Spectrum')
xlabel('\lambda (nm)')
ylabel('Intensity')
set(handles.sp_ax,'xlim',[400 700])


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
handles.fpath='G:\Measurements\';
[handles.fname,handles.fpath]=uigetfile([handles.fpath,'*.mat'],'Load spectral image file');
handles.filename=[handles.fpath,handles.fname];
set(handles.txt,'string','Loading...')
drawnow
handles.sp_data=load(handles.filename);
axes(handles.im_ax);
imshow(handles.sp_data.RGB);
set(handles.txt,'string','')

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


% --- Executes on button press in show_spec.
function show_spec_Callback(hObject, eventdata, handles)
% hObject    handle to show_spec (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)


set(handles.txt,'string',...
    {'Use the mouse to see spectra:';'Left click - blue line';'Right click - green line';
    'Middle click - red line';'Press ESC to exit'},...
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
    pixel_spec=(handles.sp_data.spec((Y-w):(Y+w),(X-w):(X+w),:));
    pixel_spec=mean(pixel_spec,1);
    pixel_spec=mean(pixel_spec,2);
    pixel_spec=pixel_spec(:);
    peak=find(pixel_spec==max(pixel_spec),1);
    axes(handles.sp_ax);
    if m_btn==1
        try
            delete(plt1)
        catch
        end
        plt1=plot(handles.sp_data.lambda,pixel_spec,'b',[handles.sp_data.lambda(peak),handles.sp_data.lambda(peak)],[0,max(handles.sp_data.spec(:)).*1.2],':k');
        handles.SPEC1(:,spec1)=pixel_spec;
        spec1=spec1+1;
    elseif m_btn==2
        try
            delete(plt2)
        catch
        end
        plt2=plot(handles.sp_data.lambda,pixel_spec,'r',[handles.sp_data.lambda(peak),handles.sp_data.lambda(peak)],[0,max(handles.sp_data.spec(:)).*1.2],':k');
        handles.SPEC2(:,spec2)=pixel_spec;
        spec2=spec2+1;        
    elseif m_btn==3
        try
            delete(plt3)
        catch
        end
        plt3=plot(handles.sp_data.lambda,pixel_spec,'g',[handles.sp_data.lambda(peak),handles.sp_data.lambda(peak)],[0,max(handles.sp_data.spec(:)).*1.2],':k');
        handles.SPEC3(:,spec3)=pixel_spec;
        spec3=spec3+1;    
    end
    title('Spectrum')
    xlabel('\lambda (nm)')
    ylabel('Intensity')
    axis([400,700,0,1.2.*max(handles.sp_data.spec(:))]);
    
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
[handles.fname,handles.fpath]=uiputfile([handles.filename(1:end-4),'_specs.mat'],'Save Spectra data');
set(handles.txt,'string','Saving...')
drawnow

sp1=handles.SPEC1;
sp2=handles.SPEC2;
sp3=handles.SPEC3;
lambda=handles.sp_data.lambda(:);

save([handles.fpath,handles.fname],'sp1','sp2','sp3','lambda')

set(handles.txt,'string','')
guidata(hObject, handles);
