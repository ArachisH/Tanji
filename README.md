# Tanji
<p align="center">
  <img src="https://user-images.githubusercontent.com/24996684/35466253-f228fa38-02b6-11e8-8ec6-7d3f57caf2e2.png" alt="Tanji Logo" width="128"/>
</p>

[![Build](https://github.com/ArachisH/Tanji/actions/workflows/build.yaml/badge.svg?branch=develop)](https://github.com/ArachisH/Tanji/actions/workflows/build.yaml)
![GitHub Tag](https://img.shields.io/github/v/tag/ArachisH/Tanji?label=Latest%20Version)
![GitHub Total Downloads](https://img.shields.io/github/downloads/ArachisH/Tanji/total?label=Total%20Downloads)
![GitHub License](https://img.shields.io/github/license/ArachisH/Tanji?label=License)

Tanji is a Habbo protocol analyzer that intercepts and decrypts the traffic between the Habbo client and its servers. It provides developers and enthusiasts with tooling to inspect, manipulate, and understand the game's network communication.

## Features
- **Packet Logger** | Monitor incoming and outgoing packets in real time.
- **Packet Injection** | Send custom packets to the client or server.
- **Packet Filtering** | Block or suppress specific packets.
- **Packet Replacing** | Modify packets on the fly before they reach their destination.
- **Extension Support** | Build and load custom extensions to automate or extend functionality.
- **Tunneling Support** | Forward game traffic through an external proxy for anonymity or region bypassing.
- **Session Persistence** | Connect to a remotely hosted instance to resume an active intercepted session at any time, even after closing the application.

## Platform Support
| Platform   | Status        |
|------------|---------------|
| Flash      | Supported     |
| Unity      | Planned       |
| Shockwave  | Planned       |

## Requirements
- Windows (x64)
- [.NET 10 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

## Getting Started
Download the latest release from the [Releases](https://github.com/ArachisH/Tanji/releases) page, extract the archive, and run `Tanji.exe`. No installation or administrator rights required.

## License
Distributed under the GNU GPLv3 License. See [`LICENSE`](LICENSE) for more information.
