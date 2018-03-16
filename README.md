# RgbaComposer
Small command line tool which allows to combine RGBA PNG image from different channels of other images

## Usage

```
RgbComposer.exe <destination_file_path> <source_image_pipe>[<source_image_pipe>...]
<source_image_pipe> ::= <input_channels>-"<source_file_path>":<output_channels>
<input_channels> ::= <channels>
<output_channels> ::= <channels>
<channels> ::= <channel>[<channel>...]
<channel> ::= r|g|b|a
```

## Example

```
RgbComposer.exe destination.png rg-"source1.tga":rg ba-"source2.tga":r
```

Genarates a PNG image from 3 other images:

- RG channel values are taken from RG channels of source1.tga
- BA channel values are taken from R channel of source2.tga
