@echo off
setlocal enabledelayedexpansion

cd src

cd PlayGermany.Server
dotnet build

cd ..
cd PlayGermany.Client
CMD /C "npm install"
CMD /C "npm run clean"
CMD /C "npm run build"

cd ..
cd ..