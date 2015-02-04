function ValidateAndLoadGSI(asm)
    if(~exist('asm'))
        asm='..\SystemControl\TestRun\bin\Debug\GSI.dll';
    end
    global gsi_was_loaded;
    if(gsi_was_loaded==1)
        return;
    end
    if(~exist('asm'))
        asm='..\SystemControl\TestRun\bin\Debug\GSI.dll';
    end
    asm=GetFullPath(asm);
    
    % copy the assembly to current directory, so it wont block recompile.
    [pathstr,name,ext]=fileparts(asm); 
    nasm=[pathstr,'\',name,'.matlab.copy',ext];
    copyfile(asm,nasm);
    % adding the assemble.
    NET.addAssembly(nasm);
    disp('GSI assembly loaded. Need to close matlab to renew.');
    gsi_was_loaded=1;
end