[![Build status](https://ci.appveyor.com/api/projects/status/08cu616f6j9m7n8r?svg=true)](https://ci.appveyor.com/project/LaylConway/subterran)

Subterran is an open source 3D game engine written in C#.

### Getting Started

You can download the latest Subterran release from GitHub [as binary from releases](https://github.com/LaylConway/Subterran/releases), or as source by cloning the git repository:

```Shell
git clone https://github.com/LaylConway/Subterran.git
```

Latest development builds can be downloaded [from AppVeyor](https://ci.appveyor.com/project/LaylConway/subterran/build/artifacts).

[No NuGet packages are available yet.](https://github.com/LaylConway/Subterran/issues/1)

The best way to get started with Subterran is to download *Subterran Libraries* from AppVeyor and look at the [example projects](https://github.com/LaylConway/Subterran/tree/develop/Examples).
Unfortunately there is no [Getting Started](https://github.com/LaylConway/Subterran/wiki/Getting-Started) page on how to create your first game with subterran yet.

### OpenTK in Subterran

This project uses OpenTK both as generic math library and for OpenGL rendering.
OpenTK non-math related classes should only be used within the `Subterran.OpenTK` project to allow porting to different libraries.