# Paths
$Root = $PSScriptRoot
$OpenSSL = "openssl"  # Ensure OpenSSL is in PATH

function Generate-DevCert {
    param (
        [string]$Name,
        [string]$Subject,
        [string]$OutDir
    )

    Write-Host "`n🔐 Generating $Name certificate..."

    # Create self-signed cert in Windows cert store
    $cert = New-SelfSignedCertificate `
        -CertStoreLocation "Cert:\CurrentUser\My" `
        -DnsName $Subject `
        -FriendlyName "$Name Dev Cert" `
        -NotAfter (Get-Date).AddYears(1) `
        -KeyExportPolicy Exportable

    # Export as .pfx (temporary)
    $pfxPath = "$env:TEMP\$Name-dev.pfx"
    $pfxPassword = ConvertTo-SecureString -String "devpass" -AsPlainText -Force
    Export-PfxCertificate -Cert $cert -FilePath $pfxPath -Password $pfxPassword

    # Output folder
    $fullOut = Join-Path $Root $OutDir
    New-Item -ItemType Directory -Path $fullOut -Force | Out-Null

    # Convert to .crt and .key
    & $OpenSSL pkcs12 -in $pfxPath -clcerts -nokeys -nodes -passin pass:devpass | Out-File "$fullOut/$Name.crt"
    & $OpenSSL pkcs12 -in $pfxPath -nocerts -nodes -passin pass:devpass | Out-File "$fullOut/$Name.key"

    Remove-Item $pfxPath -Force
    Write-Host "✅ $Name cert saved to $fullOut"
}

Generate-DevCert -Name "promtail" -Subject "localhost" -OutDir "promtail"
Generate-DevCert -Name "grafana"  -Subject "localhost" -OutDir "grafana"

