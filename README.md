# KillerDex

A Dead by Daylight match tracker for Windows.

## About

KillerDex is a Windows Forms application that allows you to track your Dead by Daylight matches. Keep a record of the killers you face, the maps you play on, and the allies you team up with.

## Features

- Track match history with detailed information
- Manage allies (friends) database
- View dashboard statistics (best ally, most faced killer, etc.)
- Multi-language support (English / Italian)

## Requirements

- Windows 10 or later
- .NET Framework 4.8

## Project Structure

```
KillerDex/
├── solution/              # Source code (Visual Studio solution)
│   ├── KillerDex.Core/
│   ├── KillerDex.Infrastructure/
│   └── KillerDex.WinForms/
├── portable_win_app/      # Compiled portable application
├── scripts/               # Build and utility scripts
├── LICENSE
└── README.md
```

## Portable Version

To build the portable Windows application:

1. Open PowerShell in the repository root
2. Run the build script:
   ```powershell
   .\scripts\build-portable.ps1
   ```
3. The portable application will be created in `portable_win_app/`
4. Run `KillerDex.exe` directly from that folder - no installation required

## Building from Source

1. Clone the repository
2. Open `solution/KillerDex.sln` in Visual Studio 2022
3. Build the solution (Ctrl+Shift+B)
4. Run the application (F5)

## Status

**Work in Progress** - This project is currently under active development. Updates may be infrequent as I balance work and university studies.

## Usage

*Coming soon*

## Screenshots

*Coming soon*

## License

**All Rights Reserved** - This software is proprietary. See the [LICENSE](LICENSE) file for details.

For licensing inquiries: stangherlin.enrico@gmail.com

## Author

Enrico Stangherlin

---

Made with love for the Dead by Daylight community
