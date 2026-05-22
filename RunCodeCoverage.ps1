# Script pour générer et afficher la couverture de code

Write-Host "?? Exécution des tests avec couverture de code..." -ForegroundColor Cyan

# Nettoyer les anciens résultats
if (Test-Path ".\TestResults") {
    Remove-Item ".\TestResults" -Recurse -Force
}
if (Test-Path ".\CoverageReport") {
    Remove-Item ".\CoverageReport" -Recurse -Force
}

# Exécuter les tests avec couverture
dotnet test --collect:"XPlat Code Coverage" --results-directory:./TestResults

if ($LASTEXITCODE -eq 0) {
    Write-Host "`n?? Génération du rapport de couverture..." -ForegroundColor Cyan
    
    # Générer le rapport
    reportgenerator `
        -reports:".\TestResults\**\coverage.cobertura.xml" `
        -targetdir:".\CoverageReport" `
        -reporttypes:"Html;HtmlSummary;Badges;TextSummary;MarkdownSummaryGithub"
    
    Write-Host "`n? Rapport généré avec succès!" -ForegroundColor Green
    Write-Host "`n?? Résumé de la couverture:" -ForegroundColor Yellow
    Get-Content .\CoverageReport\Summary.txt
    
    Write-Host "`n?? Ouverture du rapport détaillé dans le navigateur..." -ForegroundColor Cyan
    Start-Process .\CoverageReport\index.html
} else {
    Write-Host "`n? Les tests ont échoué. Consultez les erreurs ci-dessus." -ForegroundColor Red
    exit 1
}
