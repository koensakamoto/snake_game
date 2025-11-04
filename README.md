# Snake Game

A networked multiplayer Snake game implementation with web-based UI and MySQL database integration.

## Overview

This project consists of a networked Snake game with a WebAssembly-based GUI client and a custom web server. The game features real-time multiplayer gameplay, game state persistence, and player statistics tracking.

## Demo


https://github.com/user-attachments/assets/29d3150b-fdc3-42bc-a97b-4a4476e63585




## Project Structure

```
snake_game/
├── SnakeHandout/
│   ├── GUI/                    # Blazor WebAssembly GUI
│   │   ├── GUI/               # Server-side application
│   │   └── GUI.Client/        # Client-side Blazor WebAssembly
│   ├── WebServer/             # Game web server
│   └── Snake.sln              # Main solution file
└── NetworkingLibrary/         # Custom networking library
    ├── Networking/            # Core networking implementation
    ├── ChatClient/            # Example chat client
    └── ChatServer/            # Example chat server
```

## Technology Stack

- **.NET 8.0** - Core framework
- **Blazor WebAssembly** - Client-side UI
- **ASP.NET Core** - Web server
- **MySQL** - Database for game and player data
- **Custom TCP Networking** - Real-time game communication

## Features

- Multiplayer networked Snake gameplay
- Real-time game state synchronization
- Player statistics and game history tracking
- Web-based UI accessible from any browser
- MySQL database integration for persistent data
- Custom networking layer for client-server communication
- XML-based game data serialization
- Content-length header implementation for HTTP communication

## Prerequisites

- .NET 8.0 SDK or later
- MySQL Server
- Visual Studio 2022 (recommended) or any compatible IDE

## Building the Project

### Using Visual Studio

1. Open `SnakeHandout/Snake.sln` in Visual Studio
2. Restore NuGet packages (should happen automatically)
3. Build the solution (Ctrl+Shift+B)

### Using .NET CLI

```bash
cd SnakeHandout
dotnet restore
dotnet build
```

## Running the Application

### Start the Web Server

```bash
cd SnakeHandout/WebServer
dotnet run
```

### Start the GUI Application

```bash
cd SnakeHandout/GUI/GUI
dotnet run
```

The web application should open in your default browser automatically.

## Database Setup

1. Install MySQL Server
2. Create a database for the game
3. Configure connection string in the appropriate configuration file
4. The application will handle table creation on first run

## Development

### Projects

- **GUI** - Main Blazor server application
- **GUI.Client** - Blazor WebAssembly client
- **WebServer** - Game server handling network connections and game logic
- **Networking** - Reusable networking library for TCP communication

### Key Components

- `Server.cs` - TCP server implementation for handling client connections
- `NetworkConnectionWebServer.cs` - Network connection wrapper for web server
- `WebServer.cs` - Main web server logic and HTTP request handling

## Recent Updates

- Implemented async database calls for improved performance
- Added game end time tracking
- Implemented XML serialization for game data
- Enhanced UI polish
- Added Content-Length header support for HTTP responses
- Integrated web server with game database
