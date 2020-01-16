
set version=%1
set key=%2

cd %~dp0

dotnet build magic.lambda.mysql/magic.lambda.mysql.csproj --configuration Release --source https://api.nuget.org/v3/index.json
dotnet nuget push magic.lambda.mysql/bin/Release/magic.lambda.mysql.%version%.nupkg -k %key% -s https://api.nuget.org/v3/index.json
