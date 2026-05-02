@echo off
set OUTPUT=estrutura.txt

echo Gerando estrutura de pastas...

tree /F /A | findstr /V "node_modules .git dist build" > %OUTPUT%

echo Estrutura salva em %OUTPUT%
pause