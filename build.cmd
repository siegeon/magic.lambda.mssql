
set version=%1
set key=%2

cd %~dp0
dotnet build magic.lambda.mssql/magic.lambda.mssql.csproj --configuration Release --source https://api.nuget.org/v3/index.json
dotnet nuget push magic.lambda.mssql/bin/Release/magic.lambda.mssql.%version%.nupkg -k %key% -s https://api.nuget.org/v3/index.json
