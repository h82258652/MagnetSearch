///////////////////////////////////////////////////////////////////////////////
// TOOLS / ADDINS
///////////////////////////////////////////////////////////////////////////////

#tool nuget:?package=vswhere

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var msBuildPath = GetFiles(VSWhereLatest() + "/**/MSBuild.exe").FirstOrDefault();

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var verbosity = Argument("verbosity", Verbosity.Minimal);

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var solution = "./MagnetSearch.sln";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .ContinueOnError()
    .Does(() =>
{
    CleanDirectories("./src/*/bin");
    CleanDirectories("./src/*/AppPackages");
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solution);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
        // Use MSBuild
        var settings = new MSBuildSettings
        {
            ToolPath = msBuildPath
        }
        .SetConfiguration(configuration)
        .SetPlatformTarget(PlatformTarget.x86)
        .SetVerbosity(verbosity)
        .WithProperty("AppxBundle", "Always")
        .WithProperty("AppxBundlePlatforms", "x86|x64|arm")
        .WithProperty("UseDotNetNativeToolchain", "false");
        MSBuild(solution, settings);
    }
    else
    {
        // Use XBuild
        XBuild(solution, configurator =>
            configurator.SetConfiguration(configuration)
                .SetVerbosity(verbosity));
    }
});

Task("CopyMsixBundle")
    .IsDependentOn("Build")
    .Does(() =>
{
    var msixbundle = GetFiles("./src/*/AppPackages/*/*.msixbundle").First();
    CreateDirectory("./app");
    CopyFile(msixbundle, "./app/MagnetSearch.msixbundle");
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("CopyMsixBundle");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);