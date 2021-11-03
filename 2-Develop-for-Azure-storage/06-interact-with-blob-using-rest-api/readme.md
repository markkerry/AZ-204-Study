# Interact With Blob Service REST API

The following can be completed from a Linux machine or Azure Cloud Shell.

```bash
ssh azureuser@publicip
```

Create variables for the request

```bash
request_date=$(TZ=GMT date "+%a, %d %h %Y %H:%M:%S %Z")
storage_service_version="2015-04-05"
storage_account="storage account name"
access_key="storage account access key"
resource="/${storage_account}/records/data"
request_method="GET"
```

Create a temporary environment variable containing the list of headers that need to be signed:

```bash
headers="x-ms-date:$request_date\nx-ms-version:$storage_service_version"
```

Create a variable for the full string that will be signed. This must be formatted correctly hence the number or new line escape sequences (`\n`):

```bash
string_to_sign="${request_method}\n\n\n\n\n\n\n\n\n\n\n\n${headers}\n${resource}"
```

Create a variable that contains the access key decoded and converted to hex:

```bash
hex_key="$(echo -n $access_key | base64 -d -w0 | xxd -p -c256)"
```

Generate the encrypted signature:

```bash
signature=$(printf "$string_to_sign" | openssl dgst -sha256 -mac HMAC -macopt "hexkey:$hex_key" -binary | base64 -w0)
```

Create the authorisation header:

```bash
authorization_header="SharedKey $storage_account:$signature"
```

Download the Blob data to a local file:

```bash
curl -H "x-ms-date:$request_date" \
  -H "x-ms-version:$storage_service_version" \
  -H "Authorization: $authorization_header" \
  "https://${storage_account}.blob.core.windows.net/records/data" > data.csv
```

Edit the contents of the csv ready to be uploaded again. Generate a new authorization header. Notice this time the method is `PUT`:

```bash
request_date=$(TZ=GMT date "+%a, %d %h %Y %H:%M:%S %Z")
request_method="PUT"
content_type="text/plain"
file_length=$(wc -m < data.csv)
headers="x-ms-blob-type:BlockBlob\nx-ms-date:$request_date\nx-ms-version:$storage_service_version"
string_to_sign="${request_method}\n\n\n$file_length\n\n$content_type\n\n\n\n\n\n\n${headers}\n${resource}"
hex_key="$(echo -n $access_key | base64 -d -w0 | xxd -p -c256)"
signature=$(printf "$string_to_sign" | openssl dgst -sha256 -mac HMAC -macopt "hexkey:$hex_key" -binary |  base64 -w0)
authorization_header="SharedKey $storage_account:$signature"
```

Upload the modified data to the Blob.

```bash
curl -X PUT -H "x-ms-blob-type:BlockBlob" \
  -H "x-ms-date:$request_date" \
  -H "x-ms-version:$storage_service_version" \
  -H "Authorization: $authorization_header" \
  -H "Content-Length:$file_length" \
  -H "Content-Type:$content_type" \
  --data-binary "@data.csv" \
  "https://${storage_account}.blob.core.windows.net/records/data"
```
