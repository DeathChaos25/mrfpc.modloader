# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/mrfpc.modloader/*" -Force -Recurse
dotnet publish "./mrfpc.modloader.csproj" -c Release -o "$env:RELOADEDIIMODS/mrfpc.modloader" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location