dotnet publish --self-contained -r win8-x64

cd bin\Debug\netcoreapp3.0\win8-x64\publish

cf push Kinvitech.FileManager -b binary_buildpack -s windows -c .\Kinvitech.FileManager.exe --no-route -u process

cd ..\..\..\
