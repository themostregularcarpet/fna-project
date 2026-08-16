echo off
cd fna-game

dotnet publish -c Release -r win-x64 /p:NativeAotOptimizationLevel=Max

cd ..

echo compilation succeded