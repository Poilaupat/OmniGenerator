# Script pour générer la couverture de code par projet spécifique

param(
    [Parameter(Mandatory=$false)]
    [string]$ProjectName = ""
)

$projects = @(
    "OmniGenerator.Lib",
    "OmniGenerator.Cli",
    "OmniGenerator.Plugins",
    "OmniGenerator.Plugins.Tessi"
)

Write-Host "?? Analyse de couverture de code par projet" -ForegroundColor Cyan
Write-Host "==========================================`n" -ForegroundColor Cyan

if ($ProjectName -eq "") {
    Write-Host "Projets disponibles:" -ForegroundColor Yellow
    for ($i = 0; $i -lt $projects.Length; $i++) {
        Write-Host "  $($i + 1). $($projects[$i])"
    }
    Write-Host "`nPour analyser un projet spécifique, utilisez:" -ForegroundColor Yellow
    Write-Host "  .\RunCodeCoverageByProject.ps1 -ProjectName 'OmniGenerator.Lib'`n" -ForegroundColor Gray
}

# Nettoyer les anciens résultats
if (Test-Path ".\TestResults") {
    Remove-Item ".\TestResults" -Recurse -Force
}
if (Test-Path ".\CoverageReport") {
    Remove-Item ".\CoverageReport" -Recurse -Force
}

Write-Host "?? Exécution des tests avec couverture de code...`n" -ForegroundColor Cyan

# Exécuter les tests
dotnet test --collect:"XPlat Code Coverage" --results-directory:./TestResults

if ($LASTEXITCODE -eq 0) {
    Write-Host "`n?? Génération du rapport...`n" -ForegroundColor Cyan
    
    reportgenerator `
        -reports:".\TestResults\**\coverage.cobertura.xml" `
        -targetdir:".\CoverageReport" `
        -reporttypes:"Html;TextSummary"
    
    Write-Host "`n? Analyse complète de la couverture par projet:`n" -ForegroundColor Green
    
    # Lire et afficher le résumé
    $summaryContent = Get-Content .\CoverageReport\Summary.txt -Raw
    
    foreach ($project in $projects) {
        Write-Host "????????????????????????????????????????" -ForegroundColor DarkGray
        Write-Host "?? $project" -ForegroundColor Cyan
        Write-Host "????????????????????????????????????????" -ForegroundColor DarkGray
        
        # Extraire les lignes pour ce projet
        $lines = $summaryContent -split "`n" | Where-Object { $_ -match "^\s+$project\." -or $_ -match "^$project\s" }
        
        if ($lines) {
            foreach ($line in $lines) {
                # Colorer selon le pourcentage
                if ($line -match "(\d+\.?\d*)%") {
                    $percentage = [double]$Matches[1]
                    if ($percentage -ge 80) {
                        Write-Host $line -ForegroundColor Green
                    } elseif ($percentage -ge 50) {
                        Write-Host $line -ForegroundColor Yellow
                    } else {
                        Write-Host $line -ForegroundColor Red
                    }
                } else {
                    Write-Host $line
                }
            }
        } else {
            Write-Host "  Aucune donnée de couverture trouvée" -ForegroundColor DarkGray
        }
        Write-Host ""
    }
    
    Write-Host "?? Ouvrir le rapport détaillé ? (O/N)" -ForegroundColor Yellow
    $response = Read-Host
    if ($response -eq "O" -or $response -eq "o") {
        Start-Process .\CoverageReport\index.html
    }
} else {
    Write-Host "`n? Les tests ont échoué." -ForegroundColor Red
    exit 1
}
