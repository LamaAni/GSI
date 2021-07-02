function test1
% set (gcf, 'WindowButtonMotionFcn', @mouseMove);
% 
% 
% function mouseMove (object, eventdata)
% C = get (gca, 'CurrentPoint');
% title(gca, ['(X,Y) = (', num2str(C(1,1)), ', ',num2str(C(1,2)), ')']);

f = figure('ButtonDownFcn',@exampleSelectionType_Callback);

end

function exampleSelectionType_Callback(figHdl,varargin)
    selType = get(figHdl,'SelectionType');
    switch lower(selType)
        case 'normal'
            disp('Left Click');
        case 'extend'
            disp('Shift - click left mouse button or click both left and right mouse buttons');
        case 'alt'
            disp('Control - click left mouse button or click right mouse button.');
        case 'open'
            disp('Double-click any mouse button')
        otherwise
            disp(selType)
    end %switch
end %exampleSelectionType_Callback
