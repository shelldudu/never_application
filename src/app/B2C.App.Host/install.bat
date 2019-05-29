set path=%CD%
%windir%\system32\sc.exe create B2C.App.Host.8080 binpath= "%path%\B2C.App.Host.exe src"
pause