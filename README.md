Hey, it's the source code of my game i've been passionately working on for quite a little bit of time! For this task i'm using FNA game framework for all of the meat of the game, Ogmo Editor for creating levels, Ableton for all of the music, and VS Code to write code.

For now it has something i like to call CBS (Component-Based System), it's like ECS (Entity-Component System), but a bit more simplified in favor of ease to use, TexturePacker that packs all of the textures into a texture atlas. Also, this project is 100% NativeAOT-compatible, every needed library is being statically linked (FNA3D, FAudio, SDL3), every asset is baked into a single .exe file through ./build.bat (for debugging) and ./publish.bat (for releasing). It has it's own simple Scene system, Logical resolution system, and a couple of other nice things!

My TODO's are:
- Cutscene system.
- Text rendering/Dialogue system.
- A bit more advanced scene system.
- Some other useful components.
- Expanded tilemap functionality.
