@echo off
cd TexturePacker
dotnet run

cd ..

move "C:\Users\carpet\Documents\GitHub\game-project\TexturePacker\atlas.png" "C:\Users\carpet\Documents\GitHub\game-project\fna-game\Content\Graphics"
move "C:\Users\carpet\Documents\GitHub\game-project\TexturePacker\atlas_json.json" "C:\Users\carpet\Documents\GitHub\game-project\fna-game\Content\Graphics"

cd fna-game

dotnet run

cd ..

echo build was successful