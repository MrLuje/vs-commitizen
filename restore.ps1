.paket/paket.exe install

# Fixup weird binding redirects on System.Runtime
@(".\ViewTest\App.config", ".\vs-commitizen.Tests\App.config", ".\vs-commitizen\App.config") `
| %{ 
    $currentFile = $_
    (Get-Content $currentFile) `
    -replace '(?sm)<bindingRedirect oldVersion="0.0.0.0-65535.65535.65535.65535" newVersion="(4.1.1.1)"', '<bindingRedirect oldVersion="0.0.0.0-65535.65535.65535.65535" newVersion="4.0.0.0"' `
    | Set-Content $currentFile
}