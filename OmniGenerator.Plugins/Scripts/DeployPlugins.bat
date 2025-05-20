set projectname=%1
set solutiondir=%2
set outdir=%3

set sourcedir=%solutiondir%%projectname%\%outdir%\
set targetdir=%solutiondir%OmniGenerator.Cli\%outdir%\Plugins\%projectname%\

if exist %targetdir% (
	rmdir  %targetdir%
)

mkdir %targetdir%

copy %sourcedir%\*.dll %targetdir%
copy %sourcedir%\*.deps.json %targetdir%
copy %sourcedir%\*.pdb %targetdir%

