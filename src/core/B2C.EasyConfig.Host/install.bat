set path=%CD%
%windir%\system32\sc.exe create B2C.EasyConfig.Host.10010 binpath= "%path%\B2C.EasyConfig.Host.exe src"
pause