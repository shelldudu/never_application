set path=%CD%
%windir%\system32\sc.exe create B2C.Message.Api.23301 binpath= "%path%\B2C.Message.Api.exe src"
pause