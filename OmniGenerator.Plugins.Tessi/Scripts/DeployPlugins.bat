set projectname=%1
set solutiondir=%2
set outdir=%3

set sourcedll=%solutiondir%%projectname%\%outdir%%projectname%.dll
set sourcedeps=%solutiondir%%projectname%\%outdir%%projectname%.deps.json
set sourcepdb=%solutiondir%%projectname%\%outdir%%projectname%.pdb
set targetdir=%solutiondir%OmniGenerator.Cli\%outdir%\Plugins

if not exist %targetdir% (
	mkdir %targetdir%
)

copy %sourcedll% %targetdir%
copy %sourcedeps% %targetdir%

if exist %sourcepdb% (
	copy %sourcepdb% %targetdir%
)