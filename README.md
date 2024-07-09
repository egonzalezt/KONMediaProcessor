# KONMediaProcessor

[![KONMediaProcessor](https://img.shields.io/nuget/vpre/KONMediaProcessor.svg?cacheSeconds=3600&label=KONMediaProcessor%20nuget)](https://www.nuget.org/packages/KONMediaProcessor)

<p align="center"><a target="_blank"><img src="./icon.png" width="200"></a></p>

KONMediaProcessor is a lightweight media processing library designed to simplify common tasks related to video and image processing in .NET applications. It provides easy-to-use interfaces for transcoding videos, retrieving video information, and obtaining image metadata, serving as a wrapper for the powerful FFmpeg multimedia framework.

## Purpose

The purpose of KONMediaProcessor is to streamline media processing workflows in .NET projects, reducing development time and complexity. It offers a set of intuitive interfaces and classes for performing tasks such as video transcoding, video information retrieval, and image metadata extraction, leveraging the capabilities of FFmpeg.

## Features

* **Video Transcoding**: Easily transcode videos from one format to another, and adjust parameters such as video and audio codecs, bitrate, and resolution.
* **Video Information Retrieval**: Retrieve detailed information about videos, including duration, resolution, frame rate, and audio format.
* **Image Metadata Extraction**: Obtain metadata from images, such as dimensions, color space, and file format.

## Why "KON"?

The name "KON" is a reference to the popular anime series "K-ON!". In the series, the main characters form a school band and share a passion for music and creativity. Similarly, KONMediaProcessor aims to empower developers to create multimedia applications with ease and creativity.

## Requisites

KONMediaProcessor needs `FFmpeg` and `FFprobe` to work, you can download them from the official [FFmpeg website](https://ffmpeg.org/)

## Configure

To start using the library, add the following to your `ServiceCollection`:

```csharp
services.AddKONMediaProcessor();
```

### Locate FFpeg and FFprobe

There are two ways to locate the application the automatic way and the manual way, using the automatic way the library will locate the application using `where` or `which` to locate the path, if the path is not possible to locate the application will throw `FFmpegNotFoundException`

Using the manual way you need to use the class `FFmpegConfig` where you manually set the location of FFmpeg and FFmprobe applications

```csharp
FFmpegConfig.SetFFmpegLocation("/usr/bin/ffmpeg");
FFmpegConfig.SetFFprobeLocation("/usr/bin/ffprobe");
```

## Contributing
Contributions are welcome! If you find any issues or have suggestions for improvements, please open an issue or submit a pull request on GitHub.

## License
This project is licensed under the [Apache 2.0 license](./LICENSE).
