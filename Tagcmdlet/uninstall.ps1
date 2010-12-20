remove-pssnapin tagsnapin
set-alias installutil $env:windir\Microsoft.NET\Framework\v2.0.50727\installutil
installutil /uninstall .\Tagcmdlet.dll | out-null

echo "Snapin uninstalled successfully"
echo "If you've got the line `"Add-pssnapin tagsnapin`" in your profile"
echo "you should delete it."
echo ""
