param(
    [switch]$Build,
    [switch]$NoPause
)

# Project paths
$ProjectDir = $PSScriptRoot
$OverrideConfigDir = Join-Path $ProjectDir "JsonConfigs.Override"
$DefaultConfigDir = Join-Path $ProjectDir "JsonConfigs"

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "       BananaBridge Paint.NET AI Plugin Suite Installer   " -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan

# 1. Check for configuration override before using defaults
$ActiveConfigDir = $null
$ConfigMode = "Default"

if ((Test-Path $OverrideConfigDir) -and ((Get-ChildItem -Path $OverrideConfigDir -Filter "*.json" -ErrorAction SilentlyContinue).Count -gt 0)) {
    $ActiveConfigDir = $OverrideConfigDir
    $ConfigMode = "Override"
    $jsonCount = (Get-ChildItem -Path $OverrideConfigDir -Filter "*.json").Count
    Write-Host "[CONFIG] Config override folder detected: '$OverrideConfigDir'" -ForegroundColor Green
    Write-Host "         Active mode: OVERRIDE ($jsonCount JSON files found)" -ForegroundColor Green
    Write-Host "         Using custom override configurations instead of default JsonConfigs content." -ForegroundColor Cyan
} else {
    $ActiveConfigDir = $DefaultConfigDir
    $ConfigMode = "Default"
    $jsonCount = (Get-ChildItem -Path $DefaultConfigDir -Filter "*.json" -ErrorAction SilentlyContinue).Count
    Write-Host "[CONFIG] No override configuration folder found at '$OverrideConfigDir'." -ForegroundColor Yellow
    Write-Host "         Active mode: DEFAULT content ($jsonCount JSON files in '$DefaultConfigDir')" -ForegroundColor Gray
}

# 2. Build if requested or if output DLL is missing
$BuildOutputDebugDir = Join-Path $ProjectDir "BananaBridge\bin\Debug\net9.0-windows\win-x64"
$BuildOutputReleaseDir = Join-Path $ProjectDir "BananaBridge\bin\Release\net9.0-windows\win-x64"

$needsBuild = $Build -or (-not (Test-Path (Join-Path $BuildOutputReleaseDir "BananaBridge.dll")) -and -not (Test-Path (Join-Path $BuildOutputDebugDir "BananaBridge.dll")))

if ($needsBuild) {
    Write-Host "`n[BUILD] Compiling BananaBridge solution with active configurations ($ConfigMode)..." -ForegroundColor Yellow
    & dotnet build "$ProjectDir\BananaBridge.sln" -c Release
    if ($LASTEXITCODE -ne 0) {
        Write-Host "[ERROR] Build failed! Please review the compiler errors above." -ForegroundColor Red
        if (-not $NoPause) { Read-Host "Press Enter to exit" }
        Exit
    }
}

# Choose newest build
$SourceDir = $null
if ((Test-Path $BuildOutputReleaseDir) -and (Test-Path $BuildOutputDebugDir)) {
    $releaseTime = (Get-Item (Join-Path $BuildOutputReleaseDir "BananaBridge.dll")).LastWriteTime
    $debugTime = (Get-Item (Join-Path $BuildOutputDebugDir "BananaBridge.dll")).LastWriteTime
    if ($releaseTime -gt $debugTime) {
        $SourceDir = $BuildOutputReleaseDir
        $config = "Release"
    } else {
        $SourceDir = $BuildOutputDebugDir
        $config = "Debug"
    }
} elseif (Test-Path $BuildOutputReleaseDir) {
    $SourceDir = $BuildOutputReleaseDir
    $config = "Release"
} elseif (Test-Path $BuildOutputDebugDir) {
    $SourceDir = $BuildOutputDebugDir
    $config = "Debug"
} else {
    Write-Host "[ERROR] No compiled DLL found in bin output folders." -ForegroundColor Red
    Write-Host "Please build the project first by running: dotnet build" -ForegroundColor Yellow
    if (-not $NoPause) { Read-Host "Press Enter to exit" }
    Exit
}

# 3. Resolve destination Paint.NET Effects directories
$UserPdnDir = Join-Path $env:USERPROFILE "Documents\paint.net App Files\Effects"
$MachinePdnDir = "C:\Program Files\paint.net\Effects"

$TargetDirs = @()

if (Test-Path $UserPdnDir) {
    $TargetDirs += $UserPdnDir
}

# Check if running as Admin to write to Program Files
$isAdmin = ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
if (Test-Path $MachinePdnDir) {
    if ($isAdmin) {
        $TargetDirs += $MachinePdnDir
    } elseif ($TargetDirs.Count -eq 0) {
        # Need elevation if user folder doesn't exist
        Write-Host "Admin privileges required to write to $MachinePdnDir. Relaunching..." -ForegroundColor Yellow
        Start-Process powershell -ArgumentList "-NoProfile -ExecutionPolicy Bypass -File `"$PSCommandPath`"" -Verb RunAs
        Exit
    }
}

if ($TargetDirs.Count -eq 0) {
    # If neither exists, create UserPdnDir (Paint.NET standard location)
    Write-Host "Creating user Effects folder at: $UserPdnDir" -ForegroundColor Cyan
    New-Item -ItemType Directory -Path $UserPdnDir -Force | Out-Null
    $TargetDirs += $UserPdnDir
}

# 4. Copy assemblies and active configuration files
Write-Host "`n[DEPLOY] Installing $config build to target directory(s):" -ForegroundColor Cyan

foreach ($destDir in $TargetDirs) {
    Write-Host "  -> Destination: $destDir" -ForegroundColor Cyan
    try {
        if (-not (Test-Path $destDir)) {
            New-Item -ItemType Directory -Path $destDir -Force | Out-Null
        }

        # Copy Core & UI DLLs
        Copy-Item -Path (Join-Path $SourceDir "BananaBridge.dll") -Destination $destDir -Force -ErrorAction Stop
        Copy-Item -Path (Join-Path $SourceDir "BananaBridge.UI.dll") -Destination $destDir -Force -ErrorAction Stop
        Write-Host "     [OK] Copied BananaBridge.dll and BananaBridge.UI.dll" -ForegroundColor Green

        # Deploy active JSON configurations to Effects\JsonConfigs\
        $destConfigDir = Join-Path $destDir "JsonConfigs"
        if (-not (Test-Path $destConfigDir)) {
            New-Item -ItemType Directory -Path $destConfigDir -Force | Out-Null
        }
        Copy-Item -Path (Join-Path $ActiveConfigDir "*.json") -Destination $destConfigDir -Force -ErrorAction Stop
        Write-Host "     [OK] Deployed $ConfigMode JSON configurations ($jsonCount files) to: $destConfigDir" -ForegroundColor Green

    } catch {
        Write-Host "     [ERROR] Failed to deploy into ${destDir}: $_" -ForegroundColor Red
    }
}

# Check running Paint.NET process
$pdnProcess = Get-Process -Name "paint.net" -ErrorAction SilentlyContinue
if ($pdnProcess) {
    Write-Host "`n[NOTICE] Paint.NET is currently running. Please restart Paint.NET for changes to take effect." -ForegroundColor Yellow
} else {
    Write-Host "`n[SUCCESS] Installation complete! Launch Paint.NET to use the AI plugins." -ForegroundColor Green
}

if (-not $NoPause) {
    Read-Host "`nPress Enter to exit"
}
