$xmlPath = "C:\Program Files\Paint.NET\PaintDotNet.Effects.Core.xml"

if (-not (Test-Path $xmlPath)) {
    Write-Host "Paint.NET XML documentation file not found at $xmlPath" -ForegroundColor Red
    Exit
}

try {
    [xml]$xml = Get-Content $xmlPath
    $className = "PaintDotNet.Effects.EffectEnvironmentParameters"

    Write-Host "==========================================================" -ForegroundColor Cyan
    Write-Host "Documented Members of EffectEnvironmentParameters (XML Docs):" -ForegroundColor Green
    Write-Host "==========================================================" -ForegroundColor Cyan

    $members = $xml.doc.members.member | Where-Object { $_.name -like "*:$className.*" }
    
    if ($members) {
        foreach ($m in $members) {
            $nameType = $m.name.Substring(0, 2)
            $prefix = $nameType + $className + "."
            $memberName = $m.name.Replace($prefix, "")
            
            $typeStr = switch ($nameType) {
                "M:" { "Method" }
                "P:" { "Property" }
                "F:" { "Field" }
                Default { "Member" }
            }

            Write-Host (" - " + $typeStr + ": " + $memberName) -ForegroundColor White
            if ($m.summary) {
                $summary = ([string]$m.summary.InnerText).Trim() -replace '\s+', ' '
                Write-Host "   Description: $summary" -ForegroundColor Gray
            }
            Write-Host ""
        }
    } else {
        Write-Host "No documented members found for $className." -ForegroundColor Yellow
    }
} catch {
    Write-Host "Error parsing XML documentation: $_" -ForegroundColor Red
}

