[SCE CONFIDENTIAL DOCUMENT]
PSP(TM) GMO Converter version 1.51
                   Copyright (C) 2011 Sony Computer Entertainment Inc.
                                                   All Rights Reserved
======================================================================
This package includes a converter, viewer, and documents necessary 
for handling 3D model data for PSP.

[gmoconv ver 1.51]
gmoconv is a 3D model converter, which converts common 3D model data
into 3D model data for PSP.

[gmoview ver 1.51]
gmoview is a 3D model viewer, which allows the 3D model data for PSP, 
created by the gmoconv, to be viewed and checked on a PC.

----------------------------------------------------------------------
Contents of This Package
----------------------------------------------------------------------
devkit/
|---tool
|     +--- gmoconv
|          |--- Readme_Gmoconv-English.txt : This file
|          |--- GmoConv.exe                : Converter
|          |--- GmoView.exe                : Viewer
|          |--- GmoConv.cfg                : Setting file
|          |--- GmoConv.def                : User definition file
|          |--- GxoTool.dll                : Basic module of converter
|          |--- data/                      : Background data of viewer
|          |--- ImportXSI.dll              : Importing module of XSI ( Crosswalk 5.1 )
|          |--- ImportXSI_F363.dll         : Importing module of XSI ( XSIFtk 3.6.3 )
|          |--- ImportFBX.dll              : Importing module of FBX ( FBXSDK 2009.1 )
|          |--- ImportFBX_2005.dll         : Importing module of FBX ( FBXSDK 2005.12a )
|          |--- lib/                       : Advancing module of converter
|          |--- msvcp71.dll
|          |--- msvcr71.dll
|          |--- Crosswalk_5.1.32.dll       : Autodesk(R) Crosswalk(R) 5.1 dll
|          |--- test/                      : test data
|          +--- src
|                |--- GmoConv
|                +--- GmoView
+---document
     |---format            : Data format documents
     |    |---GMO_Format-Overview-English.pdf
     |    +---GMO_Format-Reference-English.pdf
     +---tool             : Overview documents
           |---GMO-Converter-English.pdf
           +---GMO-Graphics_Designers_Guideline-English.pdf

----------------------------------------------------------------------
Notes
----------------------------------------------------------------------
< pdf files >
- The documents included in this package can be viewed with 
  Adobe Acrobat 5.0 or higher, or with Adobe Acrobat Reader 5.0 or higher.
  The latest Adobe Reader(former Adobe Acrobat Reader) can be 
  downloaded from the Adobe website.  

< Usage of Crosswalk >
- To use the Crosswalk, it is required to install the Visual C++ 2008 
  Runtime depending on the environment you use.
  Should you install the Visual C++ 2008 Runtime, please download the 
  following package (VC++ 2008 SP1 Redistributable Package).

  http://www.microsoft.com/downloads/details.aspx?familyid=a5c84275-3b97-4ab7-a40d-3802b2af5fc2&displaylang=en

  Note:
  The above URL has been confirmed as active as of 2011/09/26.
  Note that the page may be moved or the content may be updated 
  after that date.

----------------------------------------------------------------------
How to use
----------------------------------------------------------------------
- gmoconv ver1.51

  usage:
    gmoconv <input files(*.xsi,*.gms)> [options]

  options:
    -interact     : input additional options
    -models       : merge models into 1 file
    -motions      : merge motions into 1 file
    -textures     : merge textures into 1 file
    -prompt       : prompt always
    -warning      : prompt on warning
    -error        : prompt on error
    -viewer       : boot the GmoView.exe when process ends
    -o <filename> : specify output file name
    -s <scale>    : scale geometry data
    -t <scale>    : scale motion length
    -r <fps>      : specify motion frame rate
    -O, -O2       : optimize
    -S            : output text format
    -H            : output header file
    -Z            : use compact vertex format
    -Q            : use sequential vertex primitive

- gmoview ver1.51

  usage:
    gmoview <input files(*.gmo)> [options]

  options:
    -fg <code>     : foreground color ( hex-code )
    -bg <code>     : background color ( hex-code )
    -multi         : enable multi model
    -colx2         : enable color double
    -texst         : enable texture state
    -nomip         : disable mipmap

  help:
    ESCAPE KEY    : QUIT PROGRAM
    SPACE KEY     : LOAD NEXT MODEL
    BACKSPACE KEY : LOAD PREV MODEL
    RETURN KEY    : RESET POSITION
    TAB KEY       : PAUSE ANIMATION
    1,2,3 .. KEY  : CHANGE CONTROL MODE
    C,V KEY       : CHANGE SUBDIVISION

    MOUSE-L BUTTON : ROTATE
    MOUSE-R BUTTON : MOVE
    Z,X KEY       : ZOOM
    +,- KEY       : RESIZE

    F1 KEY    : LIGHTING
    F2 KEY    : TEXTURE
    F3 KEY    : ALPHA BLEND
    F4 KEY    : BACKSIDE CULLING
    F5 KEY    : GRID DISPLAY
    F6 KEY    : BOX DISPLAY
    F7 KEY    : WIRE DISPLAY
    F8 KEY    : BACKGROUND DISPLAY
    A KEY     : MULTI MODEL MODE
    S KEY     : SUBFRAME INTERP MODE
    D KEY     : COLOR DOUBLE MODE
    F KEY     : TEXTURE STATE MODE

----------------------------------------------------------------------
Changes in Release 1.51
----------------------------------------------------------------------
<GmoConv.exe>
- CompVertexWeight procedure operation has been modified 
  in order for vertex weight to enter the omitted data. 

- The following procedure has been added.
    RemoveSingleWeight

- The following direct option has been added. 
    remove_single_weight

<GmoView.exe>
- Made it possible to display the data where vertex weight was omitted.

<GMO-Converter-English.pdf>
- Description of the following direct option has been added.
    remove_single_weight

- Description of the following procedure has been added. 
    RemoveSingleWeight

----------------------------------------------------------------------
Changes in Release 1.50
----------------------------------------------------------------------
<GmoConv.exe>
- The configuration file has been modified. When the optimization option
  -O/-O2 is specified, it now unifies the vertices ( unify_vertex = on ).

- When merging multiple motions, it now compares the target names.
  When different target names are used, they will be corrected based on 
  the number.
  
- When merging multiple motions, it now compares the base poses.
  When different base poses are used, an additional animation curve will 
  be output.

- It was unable to merge multiple files within a batch file. 
  This problem has been fixed.

- When outputting polygon edge data, the part that contained multiple 
  meshes was not converted correctly. This problem has been fixed.

- When deleting the vertex weight, the mesh that was using skinning and 
  morphing was not converted correctly. This problem has been fixed.

- When outputting a GMO binary format, it now deletes the key frame of
  16 bit animation that overlaps due to lack of accuracy.

- When reading an FBX file together with a Maya ASCII file, the frame 
  rate of other than 24/25/30/48/50/60 can now be used.

- When reading an XSI file, it is now possible to set the rendering 
  state or such via the custom parameters of SoftImage.

- When reading an XSI file, the texture for which multiple texture 
  projections were set was not converted correctly. 
  This problem has been fixed.

- The XSI module is now built with the CrossWalk 5.1.
  The existing module has been renamed as ImportXSI_F363.dll.

- The following direct options have been added.
    match_motion_target
    match_motion_basepose
    xsi_check_custom_param
    xsi_custom_param_transform
    xsi_custom_param_material
    xsi_custom_param_userdata

<GmoView.exe>
- The number of vertices and the vertex format of a model are now 
  displayed in view mode.

- It is now possible to refrect the texture state (TexFunc TexFilter TexWrap).
  For keeping the compatibility, this parameter is not reflected by default.
  To reflect it, press the "F" key or specify the "-TEXST" option.

<GMO-Converter-English.pdf>
- A description on the custom parameters of the XSI file has been added.

- Description on the following direct options has been added.
    match_motion_target
    match_motion_basepose
    xsi_check_custom_param
    xsi_custom_param_transform
    xsi_custom_param_material
    xsi_custom_param_userdata

----------------------------------------------------------------------
Changes in Release 1.47
----------------------------------------------------------------------
<GmoConv.exe>
- It is now possible to specify a batch file as an input file.

- An option to output polygon edge information has been added
  ( rebuild_faceedge2 ).

- An option to optimize a vertex weight has been added
  ( blend_cost_mesh ).

  By specifying the cost of mesh division as a value(for example "300"), 
  the vertex weight is decreased appropriately and the GPU load can be reduced.

- An option has been added to output an empty block for referring to a name 
  when the block output in the output option is set to OFF( output_anchor ).

- It is now possible to build the FBX module by FBXSDK 2010.2.

- The FBX module has supported Maya's Rigid Bind.

- The XSI module has supported the following parameters:
    Pivot position in the dotXSI 5.0 format
    Material animation in the dotXSI 3.5 format
    Transparent animation in the dotXSI 3.0 format

- The following procedure has been added:
    RebuildFaceEdge2

- The following direct options have been added:
    batch_extension
    rebuild_faceedge2
    blend_cost_mesh
    blend_cost_matrix
    blend_cost_weight
    blend_cost_strip
    output_anchor

<GmoView.exe>
- GPU load related information such as the number of vertex weights is now 
  displayed.
  To view the information, press the "4" key.
  (Note that the value of the GPU load is a rough estimation.)

<GMO-Converter-English.pdf>
- A description on how to use the batch file has been added.

- A description on the convertible parameter has been added.

- A description on the XSI file conversion has been added. 

- Descriptions on the following direct options have been added and modified:
    batch_extension
    rebuild_faceedge
    rebuild_faceedge2
    blend_cost_mesh
    blend_cost_matrix
    blend_cost_weight
    blend_cost_strip
    output_anchor

- Descriptions on the following procedures have been added and modified:
    LimitVertexBlend
    RebuildFaceEdge
    RebuildFaceEdge2

<GMO_Format-Reference-English.pdf>
- Descriptions on the following commands have been added:
    EdgeFlags
    EdgeFaces

----------------------------------------------------------------------
Changes in Release 1.46
----------------------------------------------------------------------
<GmoConv.exe>
- When reading an FBX file, the pivot was not read. This problem has 
  been fixed.

- When reading an FBX file, if a material has a transparent texture, 
  it now ignores the transparent color of the material.

- When reading an FBX file, bone attributes of Maya ASCII can be now set
  as an extra attribute.

- If the GIM converter is placed in a folder next to that of the GMO 
  converter, image files are now automatically converted into the GIM 
  format.
  Please refer to "2 How to Use the GMO Converter -> Textures" 
  of the GMO Converter document for more details.

- The following options have been added:
    image_extension2
    image_converter
    fbx_filter_transp
    fbx_maya_notes_transform

<GMO-Converter-English.pdf>
- The description on the texture conversion has been modified.

- The description on the FBX file conversion has been modified.

- Descriptions on the following direct options have been added and 
  modified:
    image_extension
    image_extension2
    image_converter
    fbx_filter_transp
    fbx_maya_notes_transform

----------------------------------------------------------------------
Changes in Release 1.45
----------------------------------------------------------------------
<GmoConv.exe>
- When contact points shared multiple sides, the triangle strip was 
  not converted correctly. This problem has been fixed.

- A 16 bit animation could not be read from the GMO binary format 
  correctly. This problem has been fixed. 

- "only" can be now specified as an output option for when outputting 
  a specific block(output_xxxx).

- An option has been added to avoid a situation where a single mesh refers
  to multiple vertex blocks(unify_mesh, unweld_mesh).

- A rendering state and such can now be set using an additional attribute 
  of Maya ASCII when reading an FBX file(fbx_maya_notes_material).

- An FBX module that is built using FBXSDK 2009.1 has been included
  (ImportFBX.dll). By using this module, the NURBS surface conversion 
  can be improved slightly. The existing module has been renamed as 
  ImportFBX_2005.dll.

- An XSI module that is built using CrossWalk 3.3 has been included
  (ImportXSI_CW33.dll). By using this module, the dotXSI 6.0 format conversion 
  can be improved slightly. Note that some features are not supported such as 
  the skeleton or the instance.

- Test data such as the FBX/XSI files have been included(test/).

- The following direct option values have been added:
    unify_mesh          arrays
    output_bone         only
    output_part         only
    output_material     only
    output_texture      only
    output_motion       only
    format_color        default
    format_index        default
    format_keyframe     default
    format_vertex       default
    format_normal       default
    format_vcolor       default
    format_tcoord       default
    format_weight       default
    fbx_default_vcolor  off / on

- The following direct options have been added:
    unweld_mesh
    fbx_output_root
    fbx_maya_notes_material
    fbx_maya_notes_userdata

- The following direct option has been removed:
    fbx_maya_notes_prefix

<GmoView.exe>
- Files except the GMO file are now converted for display.

- The characters of the menus are now displayed as a texture image.

<GMO-Converter-English.pdf>
- A description on the FBX file conversion has been added.

- Descriptions on the following direct options have been added and modified.
    unify_mesh
    unweld_mesh
    output_bone
    output_part
    output_material
    output_texture
    output_motion
    format_fname
    format_color
    format_index
    format_keyframe
    format_vertex
    format_normal
    format_vcolor
    format_tcoord
    format_weight
    fbx_output_root
    fbx_default_vcolor
    fbx_maya_notes_material
    fbx_maya_notes_userdata
    fbx_maya_notes_prefix

-  Descriptions on the following procedures have been added and modified.
    UnifyMesh
    UnweldMesh

----------------------------------------------------------------------
Changes in Release 1.44
----------------------------------------------------------------------
<GmoConv.exe>
- INDEX0 OFFSET0 can now be specified to the "Animate" command argument 
  in the text format.

- When an edge mesh was generated based on the skin mesh whose number of 
  affected bones exceed 8, the animation was not played back correctly.
  This problem has been fixed.

- When reading XSI, if the start/end time of the animation differed 
  among each XYZ elements, a wrong tangent handle could be added. This 
  problem has been fixed. 

- When attempting to obtain a texture animation from the Maya ASCII at 
  the time of FBX read, if an Offset/Repeat animation does not exists, 
  it now obtains a TranslateFrame/Coverage animation. 

- A procedure has been added to delete a command that has an unresolvable 
  reference.To use this feature, please specify "--remove_unresolved on".

- An XSI module that is built using the latest SDK has been contained.
  To use this module, please modify GmoConv.cfg.
  ( load "ImportXSI" -> load "ImportXSI_CW32" )

<GMO-Converter-English.pdf>
- Descriptions on the following direct options have been added.
  remove_unresolved

- The following procedures have been added:
  RemoveUnresolved

----------------------------------------------------------------------
Changes in Release 1.43
----------------------------------------------------------------------
<GmoConv.exe>
- It has been fixed so that it can be built using VC++9.

- It has been enabled to delete an unnecessary function curve after a 
  key frame is deleted. Please specify "--epsikon_static 0.0" to enable 
  this feature.

- It has been enabled to obtain the frame rate from the FBX when reading 
  the FBX. Please specify "--fbx_time_mode on" to enable this feature.

- It has been enabled to specify a conversion option within the text 
  format. Please use the ConvertOption command to specify the option.

- The following bug has been fixed.
  The last frame of the animation was not output when reading the FBX.

- The following bug has been fixed.
  The following parameters were not obtained from the Maya ASCII when 
  reading the FBX.
  - texture animation ( Repeat UV, Offset )
  - extrapolation ( Pre/Post Infinity )

- The following direct options have been added.
  --epsilon_static
  --fbx_time_mode
  --source_define
  --source_option
  --output_define
  --output_option

- The XSI/FBX modules that are built using the latest SDK have been 
  included.
  - ImportXSI_CW26.dll ( Crosswalk 2.6 )
  - ImportFBX_2009.dll ( FBXSDK 2009.1 )
  Please edit GmoConv.cfg to use the modules.
  - load "ImportXSI_CW26"
  - load "ImportFBX_2009"

<Document File>
<GMO-Converter-English.pdf>
- Descriptions on the following direct options have been added.
  epsilon_static
  fbx_time_mode
  source_define
  source_option
  output_define
  output_option

- The description on the following procedure has been modified.
  RebuildKeyFrame

<GMO_Format-Reference-English.pdf>
- A description on the following definition command has been added.
  ConvertOption

----------------------------------------------------------------------
Changes in Release 1.42
----------------------------------------------------------------------
<GmoConv.exe>
- It is now possible to built it using VC++8.

- When integrating materials, animated materials are now excluded.

- When reading FBX, the material and texture names are now reflected 
  to the block name.

- If the vertex color is 0 when reading FBX, it is now output as 0xffffffff. 

- When reading FBX, the following parameters are obtained from Maya ASCII:
  - frame rate
  - frame loop
  - material animation ( Color, Transp, etc )
  - texture animation ( Repeat UV, Offset )
  - extrapolation ( Pre/Post Infinity )
  - transform notes
  - material notes

- The following direct options have been added:
  --fbx_use_material_name
  --fbx_use_texture_name
  --fbx_default_vcolor
  --fbx_maya_notes_prefix

<GmoView.exe>
- When the initial alpha value of the material was 0, the transparency animation did not work. This problem has been fixed. 

----------------------------------------------------------------------
Changes in Release 1.41
----------------------------------------------------------------------
<GmoConv.exe>
- A warning is displayed when an undefined direct option is specified.

- When importing FBX, the Maya ASCII is checked from where the texture 
  animation is obtained. 

- The following direct options have been added:
  check_direct_option
  fbx_check_maya_ascii

----------------------------------------------------------------------
Changes in Release 1.40
----------------------------------------------------------------------
<GmoConv.exe>
- A GMP file can now be output.
  The GMP file is a file format that contains a Graphics Engine packet.
  Please refer to the sample program model6 for the usage.

- The GMP file now contains a texture image.
  When the image file is in a form of GIM, a texture packet is generated.

- The GMP file now contains multiple motions.
  A motion object is generated as a child of a root object.

- The extensible module did not work correctly on LINUX. 
  This problem has been fixed.

- namespace has been added to the internal library.
  The version of the internal library has been upgraded.
  Accordingly, you need to recompile the old extensible modules.

----------------------------------------------------------------------
Changes in Release 1.30
----------------------------------------------------------------------
<GmoConv.exe>
- CompFCurveChannel procedure has been modified to fix the problem
  in converting subchannel annimations.

- RebuildBoundingPoints procedure has been modified to fix a problem
  that "NaN" is output due to the computation error. 
  The procedure now supports "obb" and "aabb" mode.

- The program now supports GXP file output.
  GXP is a file format which contains packets for Graphics Engine.
  For the details, please refer to the sample program "model6".

- The following commands have been added:
  MeshType
  MeshLevel
  TexGen
  TexMatrix

- The following RenderState modes have been added:
  FLIP_FACE
  FLIP_NORMAL

- The following option has been added:
  -levels

- The following direct options have been added:
  rebuild_line
  rebuild_linestrip
  rebuild_faceedge

- The following procedures have been added:
  RebuildLine
  RebuildLineStrip
  RebuildFaceEdge

<GmoView.exe>
- The support for mesh masks has been added.

- The support for texture coordinates creation has been added.

- A RenderState mode, FLIP_FACE FLIP_NORMAL, has been added.

- RenderState command can be now used for mesh blocks.

----------------------------------------------------------------------
Changes in Release 1.20
----------------------------------------------------------------------
<GmoConv.exe>
- ImportXSI has been modified and now you can build it with VC++.NET.

- ImportFBX module has been modified and now you can import
  the FBX file.

- RebuildTriStrip procedure has been modified in order to enhance
  strip processings.

- RebuildRectPatch procedure has been added to convert BSpline 
  to RectPatch.

- UnifyMesh procedure has been added so that the converter could 
  combine with mesh which has the same attribute.

- UnifyArrays procedure has been added so that the converter could 
  combine with the vertex data of the same format.

- Direct options have been added as follows:
  fbx_filter_fcurve
  rebuild_rectpatch
  unify_mesh
  unify_arrays

- The calling point of LimitVertexBlend procedure has been moved. 

<GmoView.exe>
- A modification has been made so that you can use the BMP file 
  whose lines are in reversed order.


----------------------------------------------------------------------
Changes in Release 1.11
----------------------------------------------------------------------
<GmoConv.exe>
- Mesh which contains degenerate triangles was not stripped correctly.
  This problem has been fixed.

<GmoView.exe>
- External texture was not displayed correctly.
  This problem has been fixed.

----------------------------------------------------------------------
Changes in Release 1.10
----------------------------------------------------------------------
<GmoConv.exe>
- When outputting a binary file, block name was not output.
  This problem has been fixed.

- When entering a binary file, the contents of padding was inconsistent.
  This problem has been fixed.

- When entering a binary file, morphing was not entered correctly.
  This problem has been fixed.

- A modification has been made so that an instance with envelope of dotXSI
  file could be supported.

- Object culling has now been supported(BoundingPoints command).

- Z sort mode has now been support(BoneState command).

- 16bit animation has now been support.

- A modification has been made so that you can specify the offset of 
  integer vertex data.

- A modification has been made so that you can specify the target
  of bounding box.

- Data definitions have been added as below:
    BoundingPoints
    BoneState

- Direct options have been added/modified as below:
    rebuild_bounding
    rebuild_bounding_points
    format_keyframe
    offset_vertex
    offset_tcoord

- Behavior of the SortByTransparency procedure has been modified
  so that bones would not be sorted and the - SortByTransparency command
  would be added to semi-transparent bones.

- The value of the configuration file of the freeze_patchuv direct option
  has been modified so as to add a texture coordinate to a curved 
  primitive by default.

<GmoView.exe>
- The judgment condition of the DXT1 1bit alpha mode was wrong.
  This problem has been fixed.

- Internal library has been updated(in the same way as devkit 2.0.0).

- 16bit animation data has now been support.

- Additional load of motion has now been supported.
  (Control key + Drop file)

- Pause of motion has now been supported(Tab key).

< Document File >
- The file name has been modified from 3D_Format-Overview to
  GMO_Format-Overview.

- The 3D_Format-Command_Reference and the 3D_Format-Block_Reference 
  have been combined to create 1 file, named GMO_Format-Reference.


----------------------------------------------------------------------
Changes in Release 1.00
----------------------------------------------------------------------
<GmoConv.exe>
- Calculation of the VertexOffset scale value has been modified to avoid
  inappropriate sense of normal vector when using the integer vertex data.

- A modification has been made so as to support the instance of the node
  which has more directories below when converting the dotXSI file.

- The format of the setting file has been modified.

- The usage file(gmoconv.usg) has been merged with the setting file
  (gmoconv.cfg).

- A modification has been made to support binary format inputting(*.GMO).

- The -Q option has been added, which uses sequential vertex primitive.

- The format_index direct option has been modified so as not perform
  a process to create an independent vertex, separating from a shared
  vertex among multiple polygons.
  For using the sequential vertex primitive, please also use the
  unweld_vertex direct option or use the -Q option.

- The sort_block and the sort_command direct options have been abolished.

- The sort_type direct option has been added, which is ON by default.

- The last_weight direct option has been abolished.

- Block specification has been added to the sort_transparency
  direct option.

- The following direct options have been added:
    output_directory
    unify_vertex
    unweld_vertex
    remove_unused
    align_image
    xsi_ignore_prefix

- A source code file has been added.

<GmoView.exe>
- Metrics mode, which displays the data size or vertex numbers,
  has been added.

- The color-double emulation has been supported(-cd option).

- Extrapolation of animation has been supported, however the REPEAT EXTEND
  mode is not supported.

- The FrameRepeat command has been supported.

- The data folder name "gmoview" has been changed to "data".

- A referential source code file has been added.
  The code of this version is the modified one to be opened, therefore it
  has some restrictions as not to display curved surface and not to render
  colors when the light source is OFF.

  A binary of GmoView1X.exe is to be created by building this.


----------------------------------------------------------------------
Changes in Release 0.9.0
----------------------------------------------------------------------
<GmoConv.exe>
- In calculations of integer vertex data, values are now rounded off
  to the nearest integer instead of being rounded down.

- Errors in the sum of integer vertex weights are now compensated.

- Pressing the Control key while drag-and-dropping an input file now 
  displays a prompt in the console where additional options may be input.

- Scale3 command is now supported. (Chunk Type = 0x00e1)
  This command is the equivalent of scaling in Maya with "Segment Scale 
  Compensate" specified.
  (However, bone optimization using the optimize_bone option does not
  support the Scale3 command.)

- After converting model data whose number of vertex weights is 1 to 
  GMS text format, further conversion of this data did not produce proper 
  results.  This problem has been corrected.

- The direct option last_weight has been added.
  This option is the switch for outputting the last vertex weight in GMS 
  text format.  (GMS text output in either state can be read.  Note that
  the last vertex weight is always output in GMO binary format.)

- The last vertex weight is now the default output in GMS text format.
  To return to the behavior of previous versions, change the value of the 
  last_weight direct option (in the gmoconv.cfg file) to off.

<GmoView.exe>
- Scale3 command is now supported.

<GMO-Converter>
- Explanation of setting options has been changed.

- Description of last_weight has been added to Output Options.

----------------------------------------------------------------------
Changes in Release 0.6.5
----------------------------------------------------------------------
< GmoConv.exe >
- A change has been made so that the result of conversion will be 
  a model with only textures when 2D image data is specified as 
  an input file.  

- Direct and indirect options for merging files as textures have been 
  added.  
    --merge_mode texture
    -textures

- The following option has been added.  
    --format_fname     : specify the format of the FileName command

< GmoView.exe >
- A TM2 file which is aligned on a 128-byte boundary was not displayed 
  correctly.  This problem has been fixed.  

- A change has been made so that files with the extensions .gim, .tm2, 
  .tga, and .bmp are given a higher priority in a file search of when 
  image files are not included in textures.  

<gmo-Converter>
- Descriptions of the following options have been added.
    -textures
    -O2

- A description of the value texture has been added to the merge_mode 
  options. 

- Descriptions of the following output options have been added.
    format_index
    format_fname

----------------------------------------------------------------------
History of Changes
----------------------------------------------------------------------
( Release 0.6.0 version )
< GmoConv.exe >
- The following output option has been added.  
  --format_index       : Specify vertex index format

< GmoView.exe >
- A change has been made so that an image file in a folder same 
  as that of a model is loaded when there is no image file in a 
  texture. 

- A change has been made so that a sequential drawing primitive 
  can be shown.  

- A change has been made so that a texture of S3TC compression 
  format can be shown.  

- A change has been made so that a previous model is loaded 
  with the Backspace key.  

( Release 0.2.4 version )
< GmoConv.exe >
- A change has been made so that the XSIFtk.dll file will not be linked 
  on startup and when converting a GMS file.  

< GmoView.exe >
- The reflection mapping of a polygon was not displayed correctly.  
  This problem has been fixed.  

- A change has been made so that a texture in high-speed mode can be 
  displayed.  

- The following options have been added.  
    -fg <code>     : foreground color ( hex-code )
    -bg <code>     : background color ( hex-code )

( Release 0.2.3 version )
< GmoConv.exe >
- The handle information of a function curve was not converted properly 
  when inputting dotXSI.  This problem has been fixed.  

- Data was not converted properly when the number of vertex weights was
  64 or more.  This problem has been fixed.  

< GmoView.exe >
- Light source was not calculated properly when the control point of 
  a patch was degenerated.  This problem has been fixed.  

- With the HERMITE CUBIC interpolation animation, a function curve was 
  not calculated properly at the time set within the key frame.  
  This problem has been fixed.  

( Release 0.2.2 version )
- The following documents have been added.  
   doc/tools/GMO-Converter-English.pdf
   doc/tools/GMO-Graphics_Designers_Guideline-English.pdf
   doc/format/3D_Format-Overview-English.pdf
   doc/format/3D_Format-Command_Reference-English.pdf
   doc/format/3D_Format-Block_Reference-English.pdf

< GmoConv.exe >
- The following block and command have been added.  
  BlindBlock   : Blind data block ( chunk-id 0x000f )
  BlindData    : Blind data command ( chunk-id 0x00f1 )

- The following definition commands have been added.  
  DefineEnum    : Enumerated Constant Definition
  DefineBlock   : Block Definition
  DefineCommand : Command Definition

- The following parameters have been added to the RenderState command.  
  CULL_FACE     : Backside culling ( state-id 0x0003 )
  DEPTH_TEST    : Depth test ( state-id 0x0004 )
  DEPTH_MASK    : Depth writing ( state-id 0x0005 )
  ALPHA_TEST    : Alpha test ( state-id 0x0006 )
  ALPHA_MASK    : Alpha writing ( state-id 0x0007 )

- The following optimization options have been added.  
  --optimize_bone      : Delete unnecessary bones
  --sort_transparency  : Sort drawing data by transparency
  --freeze_patchuv     : Freeze patch UV coordinate
  --freeze_texcrop     : Freeze texture crop
  --epsilon_translate  : Position tolerance
  --epsilon_rotate     : Rotation tolerance
  --epsilon_scale      : Enlargement tolerance
  --epsilon_misc       : Other tolerances

- The following values have been added to the optimization options 
  --frame_loop and --frame_rate.  

  --frame_loop default ( Do not change loop range )
  --frame_rate default ( Do not change playback speed )

- The maximum number of vertex weights of a GMS text format file 
  has been increased from 15 to 255.  

- When inputting a GMS text format file, a texture file name including 
  a backward slash mark could not be processed correctly.  
  This problem has been fixed.  

- When inputting a GMS text format file, a texture file name including 
  a Shift-JIS code could not be processed correctly.  
  This problem has been fixed.  

- A change has been made to compare RenderState commands when 
  integrating materials.  

- A change has been made to output data in a state where morph shapes 
  are consecutive on a one-by-one basis when the number of morph shapes 
  exceeds 8.  

- A modification has been made to output data without control points of 
  both ends of NurbsSurface being overlapped if the NurbsSurface is 
  looped when inputting dotXSI.  

- When inputting dotXSI, the texture coordinate and vertex color which 
  are the morphing base shapes were not saved.  This problem has been 
  fixed.  

- The constant shade specification of dotXSI 3.6 has been supported.  

- When creating multiple sets of texture coordinate by dotXSI 3.6, 
  texture coordinate could not be converted correctly.  This problem has 
  been fixed.  

< GmoView.exe >
- A debug display mode has been added.  
  For details, please refer to the "help" in the above "How to use".  

- The following RenderState command parameters have been supported.  
  CULL_FACE   Backside culling
  DEPTH_TEST  Depth test
  DEPTH_MASK  Depth writing
  ALPHA_TEST  Alpha test
  ALPHA_MASK  Alpha writing

- A change has been made so that the data output in a state where morph 
  shapes are consecutive on a one-by-one basis can be played back.  

- A change has been made so that the window size can be changed by 
  the +/- key.  

- When a TM2 file having a 16bit 256entry CSM1 CLUT format was loaded, 
  an abnormal termination occurred.  This problem has been fixed.  

<GMO-Converter>
- In "Installation" of "1. Introduction", a description of viewer 
  background data has been added.  

- In "Direct Options" of "3. Options", a description of the value default
  has been added to the following optimization options.  
  frame_loop
  frame_rate

- In "Direct Options" of "3. Options", descriptions of the following 
  options have been added to the optimization options.  
    optimize_bone
    sort_transparency
    freeze_patchuv
    freeze_texcrop
    epsilon_translate
    epsilon_rotate
    epsilon_scale
    epsilon_misc

<3D_Format-Overview>
- In "Commands" and "Binary Format" of "2. Data Structure", descriptions 
  of the following data types have been added.  
    u_char
    u_short
    u_int

<3D_Format-Block_Reference>
- A description of the block BlindBlock has been added.  

- In "Commands" of the block File, descriptions of the following 
  commands have been added.  
    DefineEnum
    DefineBlock
    DefineCommand

- In "Commands" of the block Model, a description of VertexOffset has 
  been added.  

- In "Commands" of the block Bone, a description of MorphIndex has 
  been added.  

- In "Data Format" of the block Arrays, a description of vertex color 
  has been added.  

- With the block FCurve, descriptions of the following constants have 
  been added in "Arguments" and descriptions of HERMITE interpolation 
  and CUBIC interpolation have been added in "Data Format".  
    HERMITE
    CUBIC

<3D_Format-Command_Reference>
- Descriptions of the following definition commands have been added.  
    DefineEnum
    DefineBlock
    DefineCommand

- Descriptions of the following common commands have been added.  
    VertexOffset
    MorphIndex

- A description of the command BlindData has been added.  

- In "Description" of MorphWeights, a description of MorphIndex has 
  been added.  

- In "Arguments" of RenderState, the drawing state constants have been 
  changed as follows.  
  Deleted:    ENABLE_LIGHTING
              ENABLE_FOG
  Added:      LIGHTING
              FOG
              CULL_FACE
              DEPTH_TEST
              DEPTH_MASK
              ALPHA_TEST
              ALPHA_MASK

- In "Description" of Animate, TexFactor has been deleted from 
  the description of commands to be operated and descriptions of 
  the following commands have been added.  
    MorphIndex
    Ambient

- In "Arguments" of the following commands, the type of index has been 
  changed from short to u_short.  
    DrawArrays
    DrawParticle
    DrawBSpline
    DrawRectMesh
    DrawRectPatch

- In "Description" of SetTexture, the notes have been deleted.  

- In "Arguments" of BlendFunc, the mode constant MIX has been deleted 
  and descriptions of the following constants have been added.  
    ADD
    SUB
    REV

- In "Arguments" of BlendFunc, descriptions of the following constants 
  have been added to the description of factor.  
    ZERO
    ONE

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

This software contains Autodesk(R) Crosswalk code developed by 
Autodesk, Inc.
(C) 2009 Autodesk, Inc. ALL RIGHTS RESERVED.
Autodesk, Crosswalk, FBX, Maya, Softimage and 3ds Max are registered 
trademarks or trademarks of Autodesk, Inc., and/or its subsidiaries and/or 
affiliates in the USA and/or other countries.

----------------------------------------------------------------------
Copyrights
----------------------------------------------------------------------
The copyright of this software belongs to Sony Computer Entertainment
Inc.  

