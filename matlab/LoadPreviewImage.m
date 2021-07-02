%% returns the preview RGB matrix.
% LoadPreviewImage(file, resolution)
% LoadPreviewImage(file, targetWidth,targetHeight)
% LoadPreviewImage(file, resolution,x,y,width,height)
% LoadPreviewImage(file, targetWidth,targetHeight,x,y,width,height)
function [prev,w,h]=LoadPreviewImage(prevfile,a,b,c,d,e,f)
    if(~exist(prevfile,'file'))
        disp('file not found.');
        return;
    end
    
    % checking for conditional parameters.
    if(~exist('a')) 
        res=-1;x=-1;y=-1;width=-1; height=-1;targetwidth=-1;targetheight=-1; 
    end
    if(exist('a')) 
        res=a;x=-1;y=-1;width=-1; height=-1;targetwidth=-1;targetheight=-1; 
    end
    if(exist('b')) 
        res=-1;x=-1;y=-1;width=-1; height=-1;targetwidth=a;targetheight=b; 
    end
    if(exist('c') && (~exist('d') || ~exist('e'))) 
        disp('not enouph parameters. See parametrs options');
        return;
    end
    if(exist('e')) 
        res=a;x=b;y=c;width=d;height=e;targetwidth=-1;targetheight=-1; 
    end
    if(exist('f')) 
        res=-1;x=c;y=d;width=e;height=f;targetwidth=a;targetheight=b; 
    end

    %% calling the .NET to load.
    ValidateAndLoadGSI();
    strm=GSI.IP.PreviewStream.Open(prevfile);
    
    %% Calcilating conditional parameters
    if(x<0) x=0; end
    if(y<0) y=0; end
    if(width<0) width=strm.Width; end
    if(height<0) height=strm.Height; end
    
    if(res>0)
        % assume some resolution is provided.
        if(width>height)
            targetwidth = res;
            targetheight =res*height/width;
        else
            targetheight = res;
            targetwidth=res*width/height;
        end
    end
    h=round(targetheight);
    w=round(targetwidth);
    
    %% actual loading of the previw
    prev=strm.MakePreview(x,y,width,height,w,h);
    prev=prev.single;
    strm.Dispose();
    prev=shiftdim(reshape(prev,[3,w,h]),1);
end