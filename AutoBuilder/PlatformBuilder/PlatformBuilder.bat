@Echo off
echo [---!PLATFORM! builder---]
echo COMPRESSION	: !COMPRESSION!
echo.
echo Begin	: %time%
echo Building...
set CONFIG=./PlatformBuilder/!PLATFORM!path.txt
for /f "tokens=1,2 delims=$$" %%a in (%CONFIG%) do (
	if "%%a"=="BUILDPATH" (
		set BUILDPATH=%%b
	)
	if "%%a"=="LOGPATH" (
		set LOGPATH=%%b
	)
)
echo "%PROJECTPATH%" 
"%EDITORPATH%" -quit -batchmode -projectPath "%PROJECTPATH%" -executeMethod AutoBuilder.BuildGame "$%BUILDPATH%" "$%LOGPATH%" "$!PLATFORM!" "$!COMPRESSION!"  
echo Finish building!
echo End	: %time%
echo.