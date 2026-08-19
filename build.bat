@echo off
cd TexturePacker
dotnet run

move "atlas.png" "..\fna-game\Content\Graphics"
move "atlas_data.json" "..\fna-game\Content\Graphics"

cd ..

cd fna-game

dotnet run

cd ..

echo build was successful