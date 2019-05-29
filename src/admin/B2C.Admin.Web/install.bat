set path=%CD%
%windir%\system32\sc.exe create B2C.Admin.Web.8090 binpath= "%path%\B2C.Admin.Web.exe src"
pause