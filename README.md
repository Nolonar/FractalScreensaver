# FractalScreensaver
A screensaver that draws fractals

| Project   | Build status            | Latest release |
| --------- | ----------------------- | -------------- |
| Fractal   | ![.NET Core Desktop][1] | [Binaries][2]  |

## Requirements
- Windows 10 64 bit

## How to use
1. Put `Fractal.scr` where you want to keep it.
2. Right-click on `Fractal.scr` and select "Install". Do not delete `Fractal.scr`, or the screensaver will no longer work.

### Settings
#### General
**Type**: The type of fractal to draw. Can be either "Tree" or "Snowflake".

| Tree       | Snowflake       |
| ---------- | --------------- |
| [<img src="md_type_tree.png" alt="Tree" width="400">][4] | [<img src="md_type_snowflake.png" alt="Snowflake" width="400">][5] |

**Edges**: Snowflakes only. The number of edges the snowflake will have. If **Random** is enabled, edge count will be chosen at random every time.

**Iterations**: How many times the lines will be fractured.

#### Appearance
**Rainbow**: If enabled, each line will have its own color. If disabled, the entire fractal will have the same color.

**Keep in Viewport**: If enabled, the fractal will be centered and scaled down to be fully visible on screen. If disabled, the fractal may only be partially visible.

**Minimum/Maximum bump length**: How far the bump can reach when the fractal is fractured. The length is chosen at random for each fractal. A value of 1 means the bump will have the same length as the line it broke off from, 2 means the bump will be twice as high. Can be negative.

#### Timing
**Iteration delay**: How long (in milliseconds) the fractal will be visible before it is fractured again.

**Next fractal delay**: How long (in milliseconds) the fractal will be visible after it has reached its last iteration, and before the next fractal is started.

#### Others
**Save fractals**: If enabled, will save an image of each fractal, after they have reached their final iteration.


  [1]: https://github.com/Nolonar/FractalScreensaver/workflows/.NET%20Core%20Desktop/badge.svg
  [2]: https://github.com/Nolonar/FractalScreensaver/releases/latest/download/Fractal.scr
  [3]: https://dotnet.microsoft.com/download/dotnet-core/thank-you/runtime-desktop-3.1.6-windows-x64-installer
  [4]: md_type_tree.png
  [5]: md_type_snowflake.png
