#!/usr/bin/env bash
set -e

## install .NET
pushd /tmp
wget https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh
chmod u+x /tmp/dotnet-install.sh
/tmp/dotnet-install.sh --channel 10.0

## run the project to build the static files. Ensure launch profile ASPNETCORE_ENVIRONMENT is set to Release!
dotnet run --launch-profile "netlify" --project ./BlazorStaticWebsite/BlazorStaticWebsite.csproj
