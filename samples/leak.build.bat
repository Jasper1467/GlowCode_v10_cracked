REM debug crt in a dll
rmdir /s/q debug-dll
md debug-dll
cl leak.cpp -Zi -MDd -link -out:debug-dll\leak.exe user32.lib

REM release crt in a dll
rmdir /s/q rel-dll
md rel-dll
cl leak.cpp -Zi -O2 -MD -link -out:rel-dll\leak.exe user32.lib

REM debug crt static linked
rmdir /s/q debug-static
md debug-static
cl leak.cpp -Zi -MTd -link -out:debug-static\leak.exe user32.lib

REM release crt static linked
rmdir /s/q rel-static
md rel-static
cl leak.cpp -Zi -O2 -MT -link -out:rel-static\leak.exe user32.lib

