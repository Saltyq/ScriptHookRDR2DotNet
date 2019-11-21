# Set the working directory to SDK
Set-Location "sdk/"
# Download the file with Invoke-WebRequest
$source = "http://dev-c.com/files/ScriptHookRDR2_SDK_1.0.1207.73.zip"
Invoke-WebRequest $source -OutFile "sdk.rar"
# Extract the contents of the file in the current directory
7z x sdk.rar
# Return to the location where we started
Set-Location "..\"
