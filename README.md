# PluralVideos
 
Command line tools for decrypting Pluralsight videos downloaded from the Pluralsight Offline Player or downloading decrypted videos

## Prerequisites

- This tools requires .Net Core 3.1

## Installing
- Download the latest binary from [here](https://github.com/Damurka/PluralVideos/releases).

## Usage

### DownloadVideos
To use this tool you need to have downloaded the Pluralsight App and logged in 
with valid credentials. It use the pluralsight app credentials

You can only download videos that you can view
 ```diff
Flags
	--out        Required. Output folder path
	--course     Required. Course to download
	--module     Video clip to download
	--clip       Video clip to download
	--list       List course without downloading
	--db         Database file path
	--timeout    (Default: 15) Timeout period for video download in seconds
	--help       Display this help screen.
```
 
The ```--course``` flag is the string on the url table of content. The Url ***/library/courses/linq-fundamentals-csharp-6/table-of-contents*** the course flag would be ***linq-fundamentals-csharp-6***

To download a module or a single video you need to get the course id or the clip id. To get these ids run the ```--list``` flag which will list the all modules with their ids

**Examples:**

- ***List course content***
```diff
DownloadVideos --out <OutputPath> --course linq-fundamentals-csharp-6 --list
```
- ***Download a full course***
```diff
DownloadVideos --out <OutputPath> --course linq-fundamentals-csharp-6
```
- ***Download a single video***
```diff
DownloadVideos --out <OutputPath> --course linq-fundamentals-csharp-6 --clip 97619f0d-5618-4a53-8dc8-08fa981883fc
```
- ***Download a single module***
```diff
DownloadVideos --out <OutputPath> --course linq-fundamentals-csharp-6 --module 97619f0d-5618-4a53-8dc8-08fa981883fc
```
	
### DecryptVideos
Decrypts video already downloaded by pluralsight app
```diff
Flags
	--out        Required. Output folder path
	--db         Database file path
	--course     Course folder path
	--trans      Create subtitle file along with the video
	--rm         Remove encrypted folder after decryption
	--help       Display this help screen.
```
The database flag ```--db``` defaults to the default location where the pluralsight store the video so is the courses folder ```--course```.

To remove videos after decrypting use ```--rm``` flag. To include transcript use ```--trans```

**Examples:**

- ***Decrypt videos from the default location with transcript***
```diff
DecryptVideos --out <OutputPath> --trans
```

**Notes:**
- Do not  remove the course from the Pluralsight Offine Player before decrypting. You can add ```---delete``` checkbox to remove the course after the course decrypted.
- Some courses don't have subtitles.

## Author

- Dodoma

## Copyright ©

- This software is freeware and open source and is only intended for personal or educational use
``` diff
-  Pluralsight Terms of Use does not allow downloading/storing of the video. https://www.pluralsight.com/terms
``` 
