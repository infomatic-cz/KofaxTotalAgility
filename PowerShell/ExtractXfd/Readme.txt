Transfer file as text (binary) and reconstruct it in target environment. Useful if file copy&paste is not possible.
https://stackoverflow.com/questions/24468983/how-to-transfer-files-on-different-computers-through-clipboard

Encode file to binary
certutil.exe -encode file.zip filezip.txt

Decode binar to file
certutil.exe -decode filezip.txt file.zip