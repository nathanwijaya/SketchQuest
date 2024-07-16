# SketchQuest

SketchQuest is a speed drawing game where you'll attempt to replicate given images within 60 seconds or less. After each level, your drawing is scored with a custom algorithm by comparing it to the original image. You can only move on to the next level by meeting the score requirement for your chosen difficulty!

![Screenshot of a SketchQuest game, where the player is attempting to replicate an image of an apple](/assets/screenshots/Game.png?raw=true)

## About

SketchQuest was created as a High Distinction custom project for COS20007 Object Oriented Programming. This project was my first attempt at designing and developing my own program using object-oriented principles.

SketchQuest is written in C# using the .NET framework and SplashKit library.

## Getting Started

### Prerequisites

1. .NET 7.0 SDK or later

Ensure that you have the .NET SDK installed. You can download and install the .NET SDK from [here](https://dotnet.microsoft.com/en-us/download). Follow the installation instructions for your operating system.

2. SplashKit

Ensure you have SplashKit installed and properly set up on your system. Follow the instructions on the [SplashKit website](https://splashkit.io/installation/) to install SplashKit for your operating system.

### Installation

1. **Clone the Repository**

   ```bash
   git clone https://github.com/nathanwijaya/sketchquest
   cd SketchQuest
   ```

2. **Build the Project**

   To build the project with SplashKit Manager, use the following command:

   ```bash
   skm dotnet build
   ```

   SketchQuest uses NuGet packages for dependencies. Building the project should implictly restore the required dependencies. If needed, you can do this manually by running the following command in the project directory:

   ```bash
   skm dotnet restore
   ```

> [!IMPORTANT]  
> Make sure that you build and run the project with SplashKit Manager by using the `skm` command. Otherwise, the project may not build or run correctly.

> [!TIP]  
> If you're using MacOS and the project fails to build, please read 'Building on MacOS' in the Troubleshooting section below.

3. **Run the Project**

   After building the project, you can run SketchQuest with this command:

   ```bash
   skm dotnet run
   ```

## Usage

### How to Play

1. **Main Menu**

   When you start SketchQuest, you'll be greeted with the main menu. Here, you can choose to start a new game or view the gallery. You can select the difficulty level for the game, which will determine the score requirement for each level and the time limit for drawing.

   > Easy: 50/100 score requirement, 60 seconds time limit  
   > Medium: 60/100 score requirement, 50 seconds time limit  
   > Hard: 75/100 score requirement, 40 seconds time limit

   ![Screenshot of the SketchQuest main menu](/assets/screenshots/MainMenu.png?raw=true)

2. **Gameplay**

   In the game, you will be presented with an image that you need to replicate. You have up to 60 seconds to draw the image as accurately as possible. You can use your mouse to draw on the canvas, select different colours and pen sizes, enable/disable the grid, and erase mistakes.

   ![Screenshot of a SketchQuest game, where the player is attempting to replicate an image of an apple](/assets/screenshots/Game.png?raw=true)

3. **Level Complete**

   After the time runs out, your drawing will be scored based on how closely it matches the original image. If you meet the score requirement for your chosen difficulty, you will move on to the next level. Highscores will be saved for each difficulty level. You can also choose to save your drawing to the gallery, where you can view past drawings and their scores.

   ![Screenshot of the level complete screen](/assets/screenshots/LevelComplete.png?raw=true)

### Adding Custom Levels

You can add custom level images to the game by following these steps:

1. Place your level images in the `assets/level_images` directory.
2. Update the GameManager constructor in `Managers/GameManager.cs` to include the custom level images.

Each Level object in the GameManager constructor should be initialised with the name of the image file (without the file extension).

> [!NOTE]  
> The image for the first level should be initialised in the first Level object, the image for the second level should be initialised in the second Level object, and so on.

For example, if you had custom level images named `pear.png`, `orange.png`, and `banana.png`, you would update the GameManager constructor as follows:

```csharp
    private GameManager()
    {
        _state = new MainMenu();
        _levels = new List<Level>
        {
            new Level("pear"),
            new Level("orange"),
            new Level("banana")
        };
    }
```

> [!IMPORTANT]  
> Level images must be in PNG format. They are also recommended to have a size of 500 x 500 pixels - images with different dimensions may not be displayed correctly on the canvas.

## Troubleshooting

### Building on MacOS

If the project fails to build on MacOS, you may need to edit the `.csproj` file to include the following environment variable configuration for SplashKit:

```xml
<EnvironmentVariables>
  <Variable name="DYLD_LIBRARY_PATH" value="/Users/USERNAME/.splashkit/lib/macos" />
</EnvironmentVariables>
```

Change `USERNAME` to your MacOS username and ensure that the path matches your local SplashKit library path.

The full `.csproj` file should look something like this:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net7.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <PropertyGroup>
    <StartAction>Project</StartAction>
    <StartWorkingDirectory>.</StartWorkingDirectory>
    <ExternalConsole>true</ExternalConsole>
    <EnvironmentVariables>
      <Variable name="DYLD_LIBRARY_PATH" value="/Users/USERNAME/.splashkit/lib/macos" />
    </EnvironmentVariables>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="SkiaSharp" Version="2.88.8" />
    <PackageReference Include="System.Text.Json" Version="8.0.3" />
  </ItemGroup>
</Project>
```

## Acknowledgements

- Default level images from Openclipart (https://openclipart.org)
- Main menu and game background music by AdhesiveWombat
  (https://soundcloud.com/adhesivewombat)
- Gallery background image from Adobe Stock (https://stock.adobe.com)
- Sound effects from Pixabay (https://pixabay.com)
- Pixellari font by Zacchary Dempsey-Plante (https://github.com/zedseven/Pixellari)
- Roboto font by Christian Robertson (https://fonts.google.com/specimen/Roboto)

## Screenshots

![Screenshot of the SketchQuest main menu](/assets/screenshots/MainMenu.png?raw=true)

![Screenshot of a SketchQuest game, where the player is attempting to replicate an image of an apple](/assets/screenshots/Game.png?raw=true)

![Screenshot of the level complete screen](/assets/screenshots/LevelComplete.png?raw=true)

![Screenshot of the SketchQuest gallery](/assets/screenshots/Gallery.png?raw=true)

![Screenshot of the gallery detail view, showing a past drawing and its metadata](/assets/screenshots/GalleryDetail.png?raw=true)

## License

This project is licensed under the terms specified in the [LICENSE](LICENSE) file.
