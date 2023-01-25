[SCE CONFIDENTIAL DOCUMENT]
PSP(TM) GIM Converter version 1.50
                   Copyright (C) 2010 Sony Computer Entertainment Inc.
                                                   All Rights Reserved
======================================================================
This package includes the converter, viewer, and documents necessary 
for handling the 2D image data for PSP.  

[gimconv ver 1.50]
gimconv is a 2D image converter. It converts common 2D image data into 
2D image data for PSP.  

[gimview ver 1.50]
gimview is a 2D image viewer.  It allows the 2D image data for PSP, 
created by gimconv, to be viewed and confirmed on a PC.  

----------------------------------------------------------------------
Notes
----------------------------------------------------------------------
- The documents included in this package can be viewed with 
  Adobe Acrobat 5.0 or later, or with Adobe Acrobat Reader 5.0 or later.
  The latest Adobe Reader (formerly Adobe Acrobat Reader) can be 
  downloaded from the Adobe home page.

- When executing this tool, the following software must be installed 
  in the computer.
  DirectX 9.0b or later

----------------------------------------------------------------------
Contents of This Package
----------------------------------------------------------------------
devkit/
|--- tool
|     +--- gimconv
|           |--- Readme_Gimconv-English.txt   : This file
|           |--- GimConv.exe                   : Converter
|           |--- GimView.exe                   : Viewer
|           |--- GimConv.cfg                   : Setting file
|           |--- GxoTool.dll                   : Converter basic module
|           |--- lib/                          : Converter expand module
|           |--- data/                         : Viewer background data
|           |--- msvcp71.dll
|           |--- msvcr71.dll
|           +--- src
|                 |--- GimConv
|                 +--- GimView
+--- document
      |---format             : Data format document
      |   +---GIM_Format-Overview-English.pdf
      +---tool              : Overview document
            +---GIM-Converter-English.pdf

----------------------------------------------------------------------
How to use
----------------------------------------------------------------------
- gimconv ver 1.50

  usage:
    gimconv <input files(*.bmp,*.tga,*.tm2,*.dds,*.avi,*.gim,*.png)> [options]

  options:
    -interact     : input additional options
    -pictures     : merge files as pictures
    -frames       : merge files as frames
    -levels       : merge files as levels
    -prompt       : prompt always
    -warning      : prompt on warning
    -error        : prompt on error
    -viewer       : start gimview.exe when process ends
    -o <filename> : specify output file name
    -s <w,h>      : resize image data
    -S            : output text format
    -P            : resize image data to a power of two
    -N            : output in normal pixel storage format
    -F            : output in faster pixel storage format

- gimview ver 1.50
 
  usage:
    gimview <input files(*.gim)> [options]

  options:
    -fg <code>     : foreground color ( hex-code )
    -bg <code>     : background color ( hex-code )
    -nomip         : disable mipmap

  help:
    ESCAPE KEY    : QUIT PROGRAM
    SPACE KEY     : LOAD NEXT MODEL
    BACKSPACE KEY : LOAD PREV MODEL
    RETURN KEY    : RESET POSITION
    TAB KEY       : PAUSE ANIMATION
    1,2,3 .. KEY  : CHANGE CONTROL MODE

    MOUSE-L BUTTON : ROTATE
    MOUSE-R BUTTON : MOVE
    Z,X KEY       : ZOOM
    C,V KEY       : ROLL
    +,- KEY       : RESIZE

    F1 KEY        : TOGGLE BLEND
    F2 KEY        : TOGGLE FUNC
    F3 KEY        : TOGGLE FILTER
    F4 KEY        : TOGGLE WRAP
    F5 KEY        : TOGGLE BG
    F6 KEY        : TOGGLE FG
    F7 KEY        : TOGGLE GRID
    F8 KEY        : TOGGLE BUFFER
    F9 KEY        : SHOW FILE INFO

----------------------------------------------------------------------
Changes in Release 1.50
----------------------------------------------------------------------
<GimConv.exe>
- A batch file can now be specified as an input file.

- The following direct option has been added.
    batch_extension

<GimView.exe>
- The menu strings are now displayed using a texture image.

<GIM-Converter-English.pdf>
- A description on the usage of a batch file has been added.

- The following direct option has been added.
    batch_extension

----------------------------------------------------------------------
Changes in Release 1.43
----------------------------------------------------------------------
<GimConv.exe>
- It is now possible to built it using VC++9.

- It now comes with the PNG I/O module.

<GIM Converter Overview Document>
- A description on PNG has been added.

----------------------------------------------------------------------
Changes in Release 1.42
----------------------------------------------------------------------
<GimConv.exe>
- It is now possible to built it using VC++8.

- When reading TGA, the number of palettes were unchanged as 256 although 
  it was in the 4 bit index mode. 
  This problem has been fixed. Also, it now does not sort the palettes 
  if not necessary. 

----------------------------------------------------------------------
Changes in Release 1.41
----------------------------------------------------------------------
<GimConv.exe>
- A warning is displayed when an undefined direct option is specified.

- The following direct options have been added:
  check_direct_option
  unify_level

- The following procedure has been added:
  UnifyLevel

----------------------------------------------------------------------
Changes in Release 1.40
----------------------------------------------------------------------
<GimConv.exe>
- A GIP file can now be output.
  The GIP file is a file format that contains a Graphics Engine packet.
  Please refer to the sample program mode16 for the usage.

- The extensible module did not work correctly on LINUX. 
  This problem has been fixed.

- namespace has been added to the internal library.
  The version of the internal library has been upgraded.
  Accordingly, you need to recompile the old extensible modules.

----------------------------------------------------------------------
Changes in Release 1.30
----------------------------------------------------------------------
<GimConv.exe>
- A BMP file which uses run-length mode 2 can now be loaded correctly.

- The program has been modified so that, when accepting an index TGA 
  file, 4 bit index mode is used when the number of color used is 
  equal to or less than 16.

- Filter scripts can be set with two different options. The script
  specified with filter_script is executed before the merging is 
  performed. The script specified with filter_script2 is executed 
  after the merging is performed.

- The program now supports GXP file output.
  GXP is a file format which contains packets for Graphics Engine.
  For the details, please refer to the sample program "model6".

<gimview.exe>
- The BMP which uses run-length mode 2 can be drawn correctly.

----------------------------------------------------------------------
Changes in Release 1.20
----------------------------------------------------------------------
<GimConv.exe>
- A modification has been made so that you can import BMP file
  whose lines are in reversed order.

- A modification has been made so that you can export the 
  AVI/BMP/TGA/TM2/DDS file.

<gimview.exe>
- A modification has been made so that you can use the BMP file
  whose lines are in reversed order.

----------------------------------------------------------------------
Changes in Release 1.10
----------------------------------------------------------------------
<GimConv.exe>
- The Judgement condition of DXT1 1bit alpha mode was wrong.
  This problem has been fixed.

- The internal library has been updated(in the same way as GmoConv 1.10).

- -images -palettes options have been added.

<GimView.exe>
- The Judgement condition of DXT1 1bit alpha mode was wrong.
  This problem has been fixed.

- The internal library has been updated(in the same way as devkit 2.0.0).

- Pause of animation has now been supported(Tab key).

< Document File >
- The file name has been modified from 2D_Format-Overview 
  to GIM_Format-Overview.

----------------------------------------------------------------------
Changes in Release 1.00
----------------------------------------------------------------------
<gimconv>
- DDS file of 32bit color was not properly converted.  
  This problem has been modified.

- MBP file of 1bit color has been supported.(plane mask is set.)

- The format of the configuration file has been modified.

- The Usage file has been integrated with the configuration file.

- Input/Output of text format(*.GIS) has been supported.(-S option)

- Integration with planes and sequences has been supported.
  (-planes,-sequences options)

- The way to specify the pixel format has been modified.
    image_format, palette_format -> Added
    format_color, format_index   -> Abolished

- The following direct options have been renamed.
    output_info     -> update_fileinfo
    import_userdata -> update_userdata

- The following direct options have been added.
    output_object
    output_script
    output_directory
    script_extension
    check_limit
    limit_image_width
    limit_image_height
    limit_level_count
    limit_height_count
    output_sequence

- The source code files have been added.

<gimview>
- MULTI CLUT image of 32bit color was not properly displayed.
  This problem has been modified.

- The option -nomip has been added to prohibit mipmap.
  Please specify this option when a texture which contains mipmap 
  is not displayed at all.

- A source code file has been added.

----------------------------------------------------------------------
Changes in Release 0.9.0
----------------------------------------------------------------------
<GimConv.exe>
- Pressing the Control key at boot now displays a prompt where additional 
  options may be input.

- FileInfo chunk is now supported.  (Chunk Type = 0x00ff)
  This chunk is used to record data such as the creation tool, creation 
  date/time, filename, and user name.

- Direct options output_info/output_image/output_palette have been added.
  These options are switches for the chunk output to the output file.

<GimView.exe>
- Pressing the F10 key now displays the FileInfo chunk contents.

<GIM-Converter>
- The Functional Overview diagram has been changed.

- Explanation of setting options has been changed.

- Description of the following output options has been added.
    output_info
    output_image
    output_palette

----------------------------------------------------------------------
Changes in Release 0.6.5
----------------------------------------------------------------------
< GimConv.exe >
- A TM2 file which is aligned on a 128-byte boundary was not converted 
  correctly.  This problem has been fixed.  

< GimView.exe >
- A TM2 file which is aligned on a 128-byte boundary was not displayed 
  correctly.  This problem has been fixed.  

----------------------------------------------------------------------
History of Changes
----------------------------------------------------------------------
( Release 0.6.0 version )
< GimConv.exe >
- DDS format has been supported as an input file.  
  (However, the size of an image cannot be changed.)

< GimView.exe >
- A change has been made so that a picture of S3TC compression format 
  can be shown.  

- A change has been made so that a previous image is loaded with 
  the Backspace key.  

( Release 0.2.4 version )
< GimView.exe >
- A change has been made so that a picture in high-speed mode can be 
  displayed.  

- The following options have been added.  
    -fg <code>     : foreground color ( hex-code )
    -bg <code>     : background color ( hex-code )

( Release 0.2.2 version )
- The following documents have been added.  
   doc/format/2D_Format-Overview-English.pdf
   doc/tools/GIM-Converter-English.pdf

< GimConv.exe >
- The following conversion options and output options have been added. 
  --pixel_alpha    : Change the alpha value
  --format_color   : Change the color pixel format
  --format_index   : Change the index pixel format
  --import_userdata: Import user data file

- When a TM2 file having a 16bit 256entry CSM1 CLUT format was loaded, 
  an abnormal termination occurred.  This problem has been fixed.  

- A change has been made to import user data when inputting a TM2 file. 

- A change has been made to use the setting files gimconv.cfg and 
  gimconv.usg.  

< GimView.exe >
- A change has been made so that the window size can be changed by 
  the +/- key.  

- When a TM2 file having a 16bit 256entry CSM1 CLUT format was loaded, 
  an abnormal termination occurred.  This problem has been fixed.  

----------------------------------------------------------------------
Permission and Restrictions on Use
----------------------------------------------------------------------
The permission, restrictions, etc. on using this software conform to
the contract concluded between your company and our company (Sony
Computer Entertainment Inc).  

----------------------------------------------------------------------
Note on Trademarks
----------------------------------------------------------------------
"PSP" is a trademark of Sony Computer Entertainment Inc.  

All other product and company names mentioned herein, with or without 
the registered trademark symbol (R) , trademark symbol (TM) or 
service mark symbol (SM) , are generally trademarks and/or registered 
trademarks of their respective owners.

----------------------------------------------------------------------
Copyrights
----------------------------------------------------------------------
The copyright of this software belongs to Sony Computer Entertainment
Inc.  
