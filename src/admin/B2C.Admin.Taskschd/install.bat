set path=%CD%
%windir%\system32\sc.exe create B2C.Admin.Taskschd.8092 binpath= "%path%\B2C.Admin.Taskschd.exe src"
pause