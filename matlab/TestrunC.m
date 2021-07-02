gen=GSI.IP.SpectrumPreviewGenerator.Create(fn,[fn,'.prevval.blat'],w,h);
gen.Make(true,true);
pause(2);
while(gen.Processor.IsRunning)
    disp(gen.CurCount*100/gen.ExpectedDoneCount);
    pause(0.5);
end
gen.Close();