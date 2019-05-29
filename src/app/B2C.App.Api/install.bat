set path=%CD%
%windir%\system32\sc.exe create B2C.App.Api.8081 binpath= "%path%\B2C.App.Api.exe src"
pause