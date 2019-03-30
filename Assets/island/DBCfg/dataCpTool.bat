@ECHO OFF
set listFile=list.tmp
del "%listFile%" /q 1>nul 2>nul
dir *.cs /a /b >> "%listFile%"
FOR /f "tokens=*" %%a IN ('more "%listFile%"') DO (ANSI2UTF8.vbs "%%a")
del "%listFile%" /q 1>nul 2>nul
set curPath = %cd%
move /y *.cs %curPath%
PAUSE