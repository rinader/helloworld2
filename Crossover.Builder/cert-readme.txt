Importing an Existing Certificate
    - Run mmc.exe.
   - Go to File-> Add/Remove Snap-In
    -   Choose the Certificates snap-in.
    - Select Computer Account
    - Navigate to: Certificates (Local Computer)\Personal\Certificates
    - Right click the Certificates folder and choose All Tasks -> Import.
    - Follow the wizard instructions to select the certificate.
Once imported, then re-run the command from an Administrator command prompt, e.g.:

netsh http add urlacl url=https://+:9443/ user=Everyone
netsh http add sslcert ipport=0.0.0.0:9443 certhash=bac253c28151201b986858f6363e50f41f606f17 appid={a721f02d-33fc-4308-9b47-5958a57ff59d}
