set-alias installutil $env:windir\Microsoft.NET\Framework\v2.0.50727\installutil
installutil .\Tagcmdlet.dll | out-null
add-pssnapin tagsnapIn

if (test-path $profile)
{
    "`n# to add Get-Tags & Set-Tags commands`nAdd-pssnapin tagsnapin" | out-file $profile -encoding Default -append
    echo ""
}
else
{
    echo "We have detected no profile."
    echo "To be able to use Get-Tags & Set-Tags command, please create one"
    echo "and add the following line in the file : "
    echo "Add-pssnapin tagsnapin"
    echo ""
}

echo "Snapin installed successfully"
echo "You are now able to use Get-Tags & Set-Tags commands."
echo "man Get-Tags && man Set-Tags to obtain help."
echo ""
