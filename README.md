# RetroGame

RetroGame is an online co-op Bullet Hell game.

## Server Side

All server code is written in C# with .NET 5

The server side is split into 3 parts:
- Auth server
- Game server
- Networking library

### Auth Server

The Auth server is a web API written with ASP.NET.

It's purpose is to handle authentication related work to reduce the load on the Game server.

All data is stored in a MongoDB database, passwords are hashed with BCrypt.

### Game Server

The Game server is the main server where all game related work takes place.

This includes lobbies and the game itself.

### Networking Library

The Networking library is, as it's name suggests, a library that contains networking related code.

All client and server packets as well as (de)serialization is done in this library.

The library is used both in the Game server and the Game client.

## Client Side

The client side is split into 2 parts:
- Game engine
- Client

### Game engine

The Game engine is written in C++. It contains the code for rendering and physics.

Rendering is done using OpenGL.

### Client

The Client is where the game is played.

The client is written in C#. The Game engine code is called via interop.

Most of the networking is handled in the Networking library, thus, the client only has to handle on rendering.