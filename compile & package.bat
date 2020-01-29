set vs_path=C:\"Program Files (x86)"\"Microsoft Visual Studio"\2019\Professional\Common7\IDE\devenv.exe
set out_path=d:\0.SC\MediaMux
set win_rar=C:\"Program Files"\WinRAR\WinRAR.exe

%vs_path% mediaMux.sln /Build "Release|x86"  

%vs_path% mediaMux.sln /Build "Release|Any CPU" 

XCOPY mediaMux\bin\Release %out_path%\MediaMux\ /Y
XCOPY mediaMux\bin\Debug\help %out_path%\MediaMux\help\ /Y /E
XCOPY mediaMux\bin\Debug\language %out_path%\MediaMux\language\ /Y /E
XCOPY mediaMux\bin\Debug\ffmpeg %out_path%\MediaMux\ffmpeg\ /Y /E
del %out_path%\MediaMux\CodeListCfg.json
del %out_path%\MediaMux\ConfigFile.json
del %out_path%\MediaMux\Newtonsoft.Json.pdb
del %out_path%\MediaMux\Newtonsoft.Json.xml
del %out_path%\MediaMux\concat.txt

XCOPY mediaMux\bin\x86\Release %out_path%\MediaMux32\ /Y /E
XCOPY mediaMux\bin\Debug\help %out_path%\MediaMux32\help\ /Y /E
XCOPY mediaMux\bin\Debug\language %out_path%\MediaMux32\language\ /Y /E
del %out_path%\MediaMux32\CodeListCfg.json
del %out_path%\MediaMux32\ConfigFile.json
del %out_path%\MediaMux32\Newtonsoft.Json.pdb
del %out_path%\MediaMux32\Newtonsoft.Json.xml
del %out_path%\MediaMux\concat.txt

del %out_path%\MediaMux64.rar
del %out_path%\MediaMux32.rar
cd %out_path%
%win_rar% a -r %out_path%\MediaMux64.rar .\MediaMux\*.*
%win_rar% a -r %out_path%\MediaMux32.rar .\MediaMux32\*.*

pause