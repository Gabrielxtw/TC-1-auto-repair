<#
.SYNOPSIS
    Runs a SonarQube analysis of the TC1.RepairShop solution.

.PARAMETER Token
    SonarQube authentication token (generate it in SonarQube under
    My Account > Security > Generate Tokens).

.PARAMETER HostUrl
    SonarQube server URL. Defaults to http://localhost:9000.

.EXAMPLE
    ./scripts/sonar-analyze.ps1 -Token squ_xxxxxxxxxxxxxxxxxxxx
#>
param(
    [Parameter(Mandatory = $true)]
    [string]$Token,

    [string]$HostUrl = "http://localhost:9000",

    [string]$ProjectKey = "tc1-repairshop"
)

$ErrorActionPreference = "Stop"
$repoRoot = Resolve-Path "$PSScriptRoot/.."
Set-Location $repoRoot

if (-not (dotnet tool list -g | Select-String "dotnet-sonarscanner")) {
    Write-Host "Installing dotnet-sonarscanner global tool..." -ForegroundColor Cyan
    dotnet tool install --global dotnet-sonarscanner
}

Write-Host "Starting SonarQube analysis..." -ForegroundColor Cyan
dotnet sonarscanner begin `
    /k:"$ProjectKey" `
    /d:sonar.host.url="$HostUrl" `
    /d:sonar.token="$Token" `
    /d:sonar.cs.opencover.reportsPaths="coverage/**/coverage.opencover.xml" `
    /d:sonar.exclusions="**/bin/**,**/obj/**,**/Migrations/**" `
    /d:sonar.test.exclusions="**/bin/**,**/obj/**"

Write-Host "Building solution..." -ForegroundColor Cyan
dotnet build TC1.RepairShop.slnx --configuration Release

Write-Host "Running tests with coverage..." -ForegroundColor Cyan
$previousErrorActionPreference = $ErrorActionPreference
$ErrorActionPreference = "Continue"
& dotnet test TC1.RepairShop.slnx `
    --configuration Release `
    --settings coverlet.runsettings `
    --results-directory coverage
$ErrorActionPreference = $previousErrorActionPreference
if ($LASTEXITCODE -ne 0) {
    Write-Host "Some tests failed (exit code $LASTEXITCODE) - continuing so the Sonar analysis still gets published." -ForegroundColor Yellow
}

Write-Host "Finishing SonarQube analysis..." -ForegroundColor Cyan
dotnet sonarscanner end /d:sonar.token="$Token"

Write-Host "Done. Open $HostUrl/dashboard?id=$ProjectKey to see the results." -ForegroundColor Green
