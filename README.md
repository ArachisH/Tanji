<p align="center">
  <img src="https://user-images.githubusercontent.com/24996684/35466253-f228fa38-02b6-11e8-8ec6-7d3f57caf2e2.png" alt="Tanji" width="256"/>
</p>

<p align="center">
	<img alt="Build (Main)" src="https://github.com/ArachisH/Tanji/actions/workflows/build-main.yaml/badge.svg">
	<img alt="Build (Develop)" src="https://github.com/ArachisH/Tanji/actions/workflows/build-develop.yaml/badge.svg">
	<img alt="GitHub Tag" src="https://img.shields.io/github/v/tag/ArachisH/Tanji?label=Latest%20Version">
	<img alt="GitHub Total Downloads" src="https://img.shields.io/github/downloads/ArachisH/Tanji/total?label=Total%20Downloads">
	<img alt="GitHub License" src="https://img.shields.io/github/license/ArachisH/Tanji?label=License">
</p>

Tanji is a Habbo protocol analyzer that intercepts and decrypts the traffic between the Habbo client and its servers. It provides developers and enthusiasts with tooling to inspect, manipulate, and understand the game's network communication.

## Features
- **Packet Logger** | Monitor incoming and outgoing packets in real time.
- **Packet Injection** | Send custom packets to the client or server.
- **Packet Filtering** | Block or suppress specific packets.
- **Packet Replacing** | Modify packets on the fly before they reach their destination.
- **Extension Support** | Build and load custom extensions to automate or extend functionality.
- **Tunneling Support** | Forward game traffic through an external proxy for anonymity or region bypassing.
- **Session Persistence** | Connect to a remotely hosted instance to resume an active intercepted session at any time, even after closing the application.

## Previews
# Packet Logger
<img src="https://github.com/user-attachments/assets/00e1eeec-0c3f-42bd-9327-a882ffdbd12c" width="500" height="500">

## Supported Clients
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

## Related/Referenced Projects
- **[Eavesdrop](https://github.com/ArachisH/Eavesdrop)** - HTTP(S) proxy server for Windows that intercepts and modifies local web traffic.
- **[Flazzy](https://github.com/ArachisH/Flazzy)** - .NET library for [dis]assembling Shockwave Flash binaries with AS3 decompilation and deobfuscation support.
- **[Shockky](https://github.com/PaulusParssinen/Shockky)** - .NET library for [dis]assembling Adobe Shockwave and Director files.
- **[Wazzy](https://github.com/ArachisH/Wazzy)** - .NET library for [dis]assembling WebAssembly binaries.

## License
Distributed under the GNU GPLv3 License. See [`LICENSE`](LICENSE) for more information.