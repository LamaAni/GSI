% test function to try and load zero parameters.
function TestFunction(a)
    if(~exist('a'))
        a=0;
    end
    disp(a);
end