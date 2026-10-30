#!/usr/bin/env bash
set -e

## install .NET
pushd /tmp
wget https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh
chmod u+x /tmp/dotnet-install.sh
/tmp/dotnet-install.sh --channel 10.0
popd

export DOTNET_ROOT="/opt/buildhome/.dotnet"
export PATH="$DOTNET_ROOT:$PATH"

## 이제 dotnet 명령어가 정상적으로 인식됩니다.
dotnet run --launch-profile "cloudflare" --project ./Website.csproj