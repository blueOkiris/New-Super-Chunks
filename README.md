# New Super Chunks

## Description

A remake of a game demo I made called Super Chunks in my new game Engine, Eksedra Engine

The old [Super Chunks](https://github.com/blueOkiris/Super-Chunks)

That project is essentially a remake of an even older game I built in GameMaker 8.1 back in the day. It's ironic because I've based *this* game engine off of Gamemaker 8.1, so I've now come full circle.

The original [Chunks](https://chunksgame.webs.com/)

## Build and Run

### Windows

In command line run `dotnet publish -c Release -r win-x64 /p:PublishSingleFile=true & move .\bin\Release\netcoreapp3.1\win-x64\publish\New\ Super\ Chunks.exe .\`

Then there will be an executable 'New Super Chunks.exe'

### Linux (x64)

In a terminal run `export PublishSingleFile=true && dotnet publish -c Release -r linux-x64 && mv ./bin/Release/netcoreapp3.1/linux-x64/publish/New\ Super\ Chunks ./`

Then there will be a binary called 'New Super Chunks'

### Linux (Raspberry Pi)

Assuming you have properly installed dotnet core 3.1 ARM for Raspberry Pi,

Like Linux x64 `export PublishSingleFile=true && dotnet publish -c Release -r linux-arm && mv ./bin/Release/netcoreapp3.1/linux-arm/publish/New\ Super\ Chunks ./`

## Credits

Game Engine - Dylan Turner

Lead Programmer - Dylan Turner

Sound Effects - LittleRobotSoundFactory from https://www.freesound.org/people/LittleRobotSoundFactory/packs/16681

Test Background Music - Dylan Turner

Sprites - Dylan Turner

Pixeled font - OmegaPC777 from https://www.dafont.com/pixeled.font
