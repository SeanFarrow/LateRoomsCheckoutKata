using System.Text;

//Variables that determine the configuration and target to run by default. These can be specified at the commandline for CI purposes.
var target = Argument("target", "Default");
var configuration = Argument<string>("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
var solution = File("LateRoomsCheckoutKata.sln");
var srcPath =Directory("./src");
var testsPath =Directory("./tests");
var nugetPackagesPath =Directory("./packages");

Task("__Clean")
    .Does(() =>
{
        Information("Cleaning the NuGet packages and any binary files inthe source folder.");
var cleaningDirectories =new DirectoryPathCollection(new DirectoryPath[] {nugetPackagesPath}, PathComparer.Default); 
//Clean the source directories.
string dirsGlob =srcPath;
dirsGlob =dirsGlob+"/**/bin/";
dirsGlob =dirsGlob +configuration;
var dirsToClean =GetDirectories(dirsGlob);
cleaningDirectories.Add(dirsToClean);
CleanDirectories(cleaningDirectories);
});

Task("__RestoreNugetPackages")
    .Does(() =>
{
        Information("Restoring NuGet Packages for {0}", solution);
        NuGetRestore(solution);
});

Task("__BuildSolution")
    .Does(() =>
{
        Information("Building {0}", solution);

        MSBuild(solution, settings =>
            settings
                .SetConfiguration(configuration)
                .WithProperty("TreatWarningsAsErrors", "false")
                .UseToolVersion(MSBuildToolVersion.NET46)
                .SetVerbosity(Verbosity.Minimal)
                .SetNodeReuse(false));
});

Task("__RunTests")
    .Does(() =>
{
        Information("Running tests for the {0} solution.", solution);
StringBuilder testFilesGlob =new StringBuilder(testsPath);
testFilesGlob =testFilesGlob.Append("/**/bin/");
testFilesGlob =testFilesGlob.Append(configuration);
testFilesGlob =testFilesGlob.Append("/*.Tests*.dll");
var allTestFiles =GetFiles(testFilesGlob.ToString());
NUnit3(allTestFiles);
});

Task("Build")
    .IsDependentOn("__Clean")
    .IsDependentOn("__RestoreNugetPackages")
    .IsDependentOn("__BuildSolution")
    .IsDependentOn("__RunTests");

Task("Default")
.IsDependentOn("Build");

RunTarget(target);