.\packages\OpenCover.4.5.3723\OpenCover.Console.exe -register:user -target:"packages\xunit.runners.2.0.0-rc3-build2880\tools\xunit.console.exe" "-targetargs:Subterran.Tests\bin\Debug\Subterran.Tests.dll -noshadow -appveyor" -filter:"+[Subterran*]*" -output:coverage.xml