#Requires -Version 5.1
<#
.SYNOPSIS
    Builds KillerDex solution and creates a portable Windows application.

.DESCRIPTION
    This script compiles the KillerDex solution in Release mode and copies
    the necessary files to the portable_win_app folder, excluding debug files.

.EXAMPLE
    .\build-portable.ps1
#>

[CmdletBinding()]
param()

$ErrorActionPreference = "Stop"

# Paths
$RepoRoot = Split-Path -Parent $PSScriptRoot
$SolutionPath = Join-Path $RepoRoot "solution\KillerDex.sln"
$OutputPath = Join-Path $RepoRoot "solution\KillerDex.WinForms\bin\Release"
$PortablePath = Join-Path $RepoRoot "portable_win_app"

# Colors for output
function Write-Step { param($Message) Write-Host "`n[$([char]0x25B6)] $Message" -ForegroundColor Cyan }
function Write-Success { param($Message) Write-Host "    $([char]0x2714) $Message" -ForegroundColor Green }
function Write-Error { param($Message) Write-Host "    $([char]0x2718) $Message" -ForegroundColor Red }

Write-Host ""
Write-Host "============================================" -ForegroundColor Yellow
Write-Host "       KillerDex Portable Build Script      " -ForegroundColor Yellow
Write-Host "============================================" -ForegroundColor Yellow

# Find MSBuild
Write-Step "Searching for MSBuild..."

$MSBuildPaths = @(
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\*\MSBuild\Current\Bin\MSBuild.exe"
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2022\*\MSBuild\Current\Bin\MSBuild.exe"
    "${env:ProgramFiles}\Microsoft Visual Studio\2019\*\MSBuild\Current\Bin\MSBuild.exe"
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\*\MSBuild\Current\Bin\MSBuild.exe"
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
)

$MSBuild = $null
foreach ($pattern in $MSBuildPaths) {
    $found = Get-ChildItem -Path $pattern -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($found) {
        $MSBuild = $found.FullName
        break
    }
}

if (-not $MSBuild) {
    Write-Error "MSBuild not found. Please install Visual Studio 2019/2022 or Build Tools."
    exit 1
}

Write-Success "Found: $MSBuild"

# Verify solution exists
Write-Step "Verifying solution..."
if (-not (Test-Path $SolutionPath)) {
    Write-Error "Solution not found: $SolutionPath"
    exit 1
}
Write-Success "Solution found"

# Clean portable folder
Write-Step "Cleaning portable_win_app folder..."
if (Test-Path $PortablePath) {
    Get-ChildItem -Path $PortablePath -Recurse | Remove-Item -Recurse -Force
}
else {
    New-Item -ItemType Directory -Path $PortablePath | Out-Null
}
Write-Success "Folder cleaned"

# Build solution
Write-Step "Building solution in Release mode..."
$buildArgs = @(
    $SolutionPath
    "/p:Configuration=Release"
    "/p:Platform=Any CPU"
    "/t:Rebuild"
    "/v:minimal"
    "/nologo"
)

& $MSBuild $buildArgs
if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed with exit code $LASTEXITCODE"
    exit 1
}
Write-Success "Build completed successfully"

# Copy files to portable folder
Write-Step "Copying files to portable_win_app..."

# Files to include (patterns)
$IncludePatterns = @("*.exe", "*.dll", "*.config")

# Files to exclude (patterns)
$ExcludePatterns = @("*.pdb", "*.xml")

# Copy main files
foreach ($pattern in $IncludePatterns) {
    Get-ChildItem -Path $OutputPath -Filter $pattern | ForEach-Object {
        $shouldExclude = $false
        foreach ($exclude in $ExcludePatterns) {
            if ($_.Name -like $exclude) {
                $shouldExclude = $true
                break
            }
        }
        if (-not $shouldExclude) {
            Copy-Item -Path $_.FullName -Destination $PortablePath
            Write-Success $_.Name
        }
    }
}

# Copy localization folders (e.g., it/, en/)
Get-ChildItem -Path $OutputPath -Directory | Where-Object {
    $_.Name -match "^[a-z]{2}(-[A-Z]{2})?$"
} | ForEach-Object {
    $destDir = Join-Path $PortablePath $_.Name
    Copy-Item -Path $_.FullName -Destination $destDir -Recurse
    Write-Success "$($_.Name)/ (localization)"
}

# Summary
Write-Host ""
Write-Host "============================================" -ForegroundColor Green
Write-Host "           Build Complete!                  " -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Green
Write-Host ""
Write-Host "Output folder: $PortablePath" -ForegroundColor White
Write-Host ""

# List final contents
Write-Host "Contents:" -ForegroundColor Cyan
Get-ChildItem -Path $PortablePath -Recurse | ForEach-Object {
    $relativePath = $_.FullName.Substring($PortablePath.Length + 1)
    if ($_.PSIsContainer) {
        Write-Host "    [DIR]  $relativePath" -ForegroundColor Yellow
    }
    else {
        $size = "{0:N0} KB" -f ($_.Length / 1KB)
        Write-Host "    [FILE] $relativePath ($size)" -ForegroundColor White
    }
}

Write-Host ""
