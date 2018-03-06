param
(
    [Parameter(Position=0, Mandatory=1, ValueFromPipeline=$true)]
    [string]$version
)

Write-Host "Updating VSIX manifest version... " -ForegroundColor Cyan -NoNewline

$manifest_file = (Get-ChildItem "source.extension.vsixmanifest" -Recurse)[0]

[xml]$vsix_manifest = Get-Content $manifest_file.FullName

$ns = New-Object System.Xml.XmlNamespaceManager $vsix_manifest.NameTable
$ns.AddNamespace("ns", $vsix_manifest.DocumentElement.NamespaceURI) | Out-Null

$version_attribute = "0.0.1"

if ($vsix_manifest.SelectSingleNode("//ns:Identity", $ns)) # VS2012 format
{
    $version_attribute = $vsix_manifest.SelectSingleNode("//ns:Identity", $ns).Attributes["Version"]
}
elseif ($vsix_manifest.SelectSingleNode("//ns:Version", $ns)) # VS2010 format
{
    $version_attribute = $vsix_manifest.SelectSingleNode("//ns:Version", $ns)
}

$version_attribute.InnerText = $version
$vsix_manifest.Save($manifest_file) | Out-Null

$version | Write-Host -ForegroundColor Green

$version