[SCE CONFIDENTIAL DOCUMENT]
PSP(TM) GMO Converter version 1.51
                   Copyright (C) 2011 Sony Computer Entertainment Inc.
                                                   All Rights Reserved
======================================================================
���̃p�b�P�[�W�́A"PSP" �p 3D ���f���f�[�^���������߂ɕK�v�ȃR���o�[�^�A
�r���[�A�A�h�L�������g���܂܂�Ă��܂��B

[gmoconv ver 1.51]
gmoconv �� 3D ���f���R���o�[�^�ŁA��ʓI�� 3D ���f���f�[�^�� "PSP" �p 
3D ���f���f�[�^�ɕϊ����܂��B

[gmoview ver 1.51]
gmoview �� 3D ���f���r���[�A�ŁAgmoconv �ō쐬���� "PSP" �p 3D ���f
���f�[�^�� PC ��Ŋm�F���邱�Ƃ��ł��܂��B

----------------------------------------------------------------------
�p�b�P�[�W�\��
----------------------------------------------------------------------
devkit/
|---tool
|     +--- gmoconv
|          |--- Readme_Gmoconv-Japanese.txt  :���̃t�@�C��
|          |--- GmoConv.exe                  :�R���o�[�^
|          |--- GmoView.exe                  :�r���[�A
|          |--- GmoConv.cfg                  :�ݒ�t�@�C��
|          |--- GmoConv.def                  :���[�U�[��`�t�@�C��
|          |--- GxoTool.dll                  :�R���o�[�^��{���W���[��
|          |--- data/                        :�r���[�A�w�i�f�[�^
|          |--- ImportXSI.dll                :XSI �C���|�[�^���W���[�� ( Crosswalk 5.1 )
|          |--- ImportXSI_F363.dll           :XSI �C���|�[�^���W���[�� ( XSIFtk 3.6.3 )
|          |--- ImportFBX.dll                :FBX �C���|�[�^���W���[�� ( FBXSDK 2009.1 )
|          |--- ImportFBX_2005.dll           :FBX �C���|�[�^���W���[�� ( FBXSDK 2005.12a )
|          |--- lib/                         :�R���o�[�^�g�����W���[��
|          |--- msvcp71.dll
|          |--- msvcr71.dll
|          |--- Crosswalk_5.1.32.dll         :Autodesk(R) Crosswalk(R) 5.1 dll
|          |--- test/                        :�e�X�g�f�[�^
|          +--- src
|                |--- GmoConv
|                +--- GmoView
+---document
     |---format            :�f�[�^�t�H�[�}�b�g�h�L�������g
     |    |---GMO_Format-Overview-Japanese.pdf
     |    +---GMO_Format-Reference-Japanese.pdf
     +---tool             :�T�v�h�L�������g
           |---GMO-Converter-Japanese.pdf
           +---GMO-Graphics_Designers_Guideline-Japanese.pdf

----------------------------------------------------------------------
���ӎ���
----------------------------------------------------------------------
< pdf �t�@�C���ɂ��� >
- ���̃p�b�P�[�W�Ɋ܂܂��h�L�������g�� Adobe Acrobat 5.0�ȏ�A
  Adobe Acrobat Reader 5.0 �ȏ�ł������������܂��B
  �ŐV�� Adobe Reader(����Adobe Acrobat Reader) �ɂ��܂��Ă� Adobe 
  �̃z�[���y�[�W���_�E�����[�h�\�ł��B

< Crosswalk �̎g�p�ɂ��� >
- ���ɂ��Visual C++ 2008 �����^�C���̃C���X�g�[�����K�v�ȏꍇ������܂��B
  Visual C++ 2008 �����^�C�����K�v�ȏꍇ�ɂ́A�ȉ��̃p�b�P�[�W
 �iVC++ 2008 SP1 Redistributable Package�j���_�E�����[�h���Ă��������B

  http://www.microsoft.com/downloads/details.aspx?familyid=a5c84275-3b97-4ab7-a40d-3802b2af5fc2&displaylang=en

  ���L�F
  ��L�̎Q�Ɛ�URL�ɂ��āA2011/09/26 ���_�ŎQ�Ƃł��邱�Ƃ��m�F���Ă��܂��B
  ���̌�y�[�W���ړ���������e���ύX����Ă���\��������܂��̂�
  �����ӂ��������B

----------------------------------------------------------------------
�g�p���@
----------------------------------------------------------------------
- gmoconv ver1.51

  usage:
    gmoconv <���̓t�@�C��(*.xsi,*.gms)> [�I�v�V����]

  options:
    -interact     : �ǉ��I�v�V�������L�[����
    -models       : �����́u���f���v�Ƃ��ĂP�̃t�@�C���Ɍ���
    -motions      : �����́u���[�V�����v�Ƃ��ĂP�̃t�@�C���Ɍ���
    -textures     : �����́u�e�N�X�`���v�Ƃ��ĂP�̃t�@�C���Ɍ���
    -prompt       : �˂ɃL�[���͑҂��������Ȃ�
    -warning      : �x�����ɃL�[���͑҂��������Ȃ�
    -error        : �G���[���ɃL�[���͑҂��������Ȃ�
    -viewer       : �I������GmoView.exe���N������
    -o <filename> : �o�̓t�@�C�������w�肷��
    -s <scale>    : �`��f�[�^���X�P�[�����O����
    -t <scale>    : ���[�V�����̒������X�P�[�����O����
    -r <fps>      : ���[�V�����̃t���[�����[�g���w�肷��
    -O, -O2       : �œK���������Ȃ�
    -S            : �e�L�X�g�`�����o�͂���
    -H            : �w�b�_�t�@�C�����o�͂���
    -Z            : �T�C�Y�̏��������_�t�H�[�}�b�g���g�p����
    -Q            : ��C���f�N�X���_�v���~�e�B�u���g�p����

- gmoview ver1.51

  usage:
    gmoview <���̓t�@�C��(*.gmo)> [options]

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
Release 1.51 �̕ύX�_
----------------------------------------------------------------------
<GmoConv.exe>
- ���_�E�F�C�g���ȗ����ꂽ�f�[�^����͂ł���悤�A
  CompVertexWeight�v���V�[�W���̓�����C�����܂����B

- �ȉ��̃v���V�[�W����ǉ����܂���
    RemoveSingleWeight

- �ȉ��̒��ڃI�v�V������ǉ����܂���
    remove_single_weight

<GmoView.exe>
- ���_�E�F�C�g���ȗ����ꂽ�f�[�^��\���ł���悤�ɂ��܂����B

<GMO-Converter-Japanese.pdf>
- �ȉ��̒��ڃI�v�V�����̋L�q��ǉ����܂����B
    remove_single_weight

- �ȉ��̃v���V�[�W���̋L�q��ǉ����܂����B
    RemoveSingleWeight

----------------------------------------------------------------------
Release 1.50 �̕ύX�_
----------------------------------------------------------------------
<GmoConv.exe>
- �ݒ�t�@�C�����C�����܂����B�œK���I�v�V���� -O/-O2 ���w�肳�ꂽ�Ƃ��A
  ���_�̓���������Ȃ��悤�ɂ��܂��� ( unify_vertex = on )

- �������[�V�������}�[�W���鎞�ɁA�^�[�Q�b�g�����r����悤�ɂ��܂����B
  �^�[�Q�b�g�����قȂ�ꍇ�́A�ԍ������ƂɃ^�[�Q�b�g����␳���܂��B

- �������[�V�������}�[�W���鎞�ɁA�x�[�X�|�[�Y���r����悤�ɂ��܂����B
  �x�[�X�|�[�Y���قȂ�ꍇ�́A�ǉ��̃A�j���[�V�����J�[�u���o�͂��܂��B

- �o�b�`�t�@�C�����ŕ����t�@�C�����}�[�W�ł��Ȃ��s����C�����܂����B

- �|���S���G�b�W�����o�͂��鎞�ɁA�����̃��b�V�����܂ރp�[�g��
  �������ϊ�����Ȃ��s����C�����܂����B

- ���_�E�F�C�g���팸���鎞�ɁA�X�L�j���O�ƃ��[�t�B���O�𕹗p����
  ���b�V�����������ϊ�����Ȃ��s����C�����܂����B

- GMO �o�C�i���`�����o�͂��鎞�ɁA���x�s���̂��߂ɏd�����Ă��܂�
  16�r�b�g�A�j���[�V�����̃L�[�t���[�����폜����悤�ɂ��܂����B

- FBX �t�@�C���ǂݍ��ݎ��ɁAMaya ASCII �t�@�C���𕹗p����ꍇ��
  24/25/30/48/50/60 �ȊO�̃t���[�����[�g���g�p�ł���悤�ɂ��܂����B

- XSI �t�@�C���ǂݍ��ݎ��ɁASoftImage �̃J�X�^���p�����[�^��
  �����_�X�e�[�g�Ȃǂ�ݒ�ł���悤�ɂ��܂����B

- XSI �t�@�C���ǂݍ��ݎ��ɁA�����̃e�N�X�`���v���W�F�N�V������
  �ݒ肳�ꂽ�e�N�X�`�����������ϊ�����Ȃ��s����C�����܂����B

- XSI ���W���[���� CrossWalk 5.1 �Ńr���h����悤�ɂ��܂����B
  �]���̃��W���[���� ImportXSI_F363.dll �ɉ������܂����B

- �ȉ��̒��ڃI�v�V������ǉ����܂���
    match_motion_target
    match_motion_basepose
    xsi_check_custom_param
    xsi_custom_param_transform
    xsi_custom_param_material
    xsi_custom_param_userdata

<GmoView.exe>
- �r���[���[�h�ŁA���f���̒��_���ƒ��_�`����\������悤�ɂ��܂����B

- �e�N�X�`���X�e�[�g ( TexFunc TexFilter TexWrap ) �𔽉f�ł���悤�ɂ��܂����B
  �݊����̂��߁A�f�t�H���g�ł͂��̃p�����[�^�����f����Ȃ��悤�ɂȂ��Ă��܂��B
  ���f����ɂ� "F" �L�[���������A"-TEXST" �I�v�V���� ���w�肵�Ă��������B

<GMO-Converter-Japanese.pdf>
- XSI �t�@�C���̃J�X�^���p�����[�^�̋L�q��ǉ����܂����B

- �ȉ��̒��ڃI�v�V�����̋L�q��ǉ����܂����B
    match_motion_target
    match_motion_basepose
    xsi_check_custom_param
    xsi_custom_param_transform
    xsi_custom_param_material
    xsi_custom_param_userdata

----------------------------------------------------------------------
Release 1.47 �̕ύX�_
----------------------------------------------------------------------
<GmoConv.exe>
- �o�b�`�t�@�C������̓t�@�C���Ƃ��Ďw��ł���悤�ɂ��܂����B

- �|���S���G�b�W�����o�͂���I�v�V������ǉ����܂��� ( rebuild_faceedge2 )�B

- ���_�E�F�C�g���œK������I�v�V������ǉ����܂��� ( blend_cost_mesh )�B

  ���b�V�������̃R�X�g���i�Ⴆ�� "300"�Ȃǁj���l�Ŏw�肷�邱�Ƃ�
  ���_�E�F�C�g���K�؂ɍ팸����AGPU ���ׂ��y������܂��B

- �o�̓I�v�V�����ɂ��u���b�N�o�͂��I�t�̏ꍇ�ɁA���O�Q�Ƃ̂��߂�
  ��̃u���b�N���o�͂���I�v�V������ǉ����܂��� ( output_anchor )�B

- FBX ���W���[���� FBXSDK 2010.2 �Ńr���h�ł���悤�ɏC�����܂����B

- FBX ���W���[���� Maya �� Rigid Bind �ɑΉ����܂����B

- XSI ���W���[�����ȉ��̃p�����[�^�ɑΉ����܂����B
    dotXSI 5.0 �`���̃s�{�b�g�|�W�V����
    dotXSI 3.5 �`���̃}�e���A���A�j���[�V����
    dotXSI 3.0 �`���̃g�����X�y�A�����V�A�j���[�V����

- �ȉ��̃v���V�[�W����ǉ����܂����B
    RebuildFaceEdge2

- �ȉ��̒��ڃI�v�V������ǉ����܂���
    batch_extension
    rebuild_faceedge2
    blend_cost_mesh
    blend_cost_matrix
    blend_cost_weight
    blend_cost_strip
    output_anchor

<GmoView.exe>
- ���_�E�F�C�g���Ȃ� GPU ���ׂɊ֘A�������\������悤�ɂ��܂����B
  �m�F����ɂ� "4" �L�[�������ď��y�[�W��\�����Ă��������B
  �i������ GPU ���ׂ̐��l�͊T�Z�ł��j

<GMO-Converter-Japanese.pdf>
- �o�b�`�t�@�C���̎g�p�ɂ��Ă̋L�q��ǉ����܂����B

- �ϊ��\�ȃp�����[�^�ɂ��Ă̋L�q��ǉ����܂����B

- XSI �t�@�C���̕ϊ��ɂ��Ă̋L�q��ǉ����܂����B

- �ȉ��̒��ڃI�v�V�����̋L�q��ǉ��^�C�����܂����B
    batch_extension
    rebuild_faceedge
    rebuild_faceedge2
    blend_cost_mesh
    blend_cost_matrix
    blend_cost_weight
    blend_cost_strip
    output_anchor

- �ȉ��̃v���V�[�W���̋L�q��ǉ��^�C�����܂����B
    LimitVertexBlend
    RebuildFaceEdge
    RebuildFaceEdge2

<GMO_Format-Reference-Japanese.pdf>
- �ȉ��̃R�}���h�̋L�q��ǉ����܂����B
    EdgeFlags
    EdgeFaces

----------------------------------------------------------------------
Release 1.46 �̕ύX�_
----------------------------------------------------------------------
<GmoConv.exe>
- FBX �t�@�C���ǂݍ��ݎ��ɁA�s�{�b�g���ǂݍ��܂�Ȃ��s����C�����܂����B

- FBX �t�@�C���ǂݍ��ݎ��ɁA�}�e���A���̓����x�e�N�X�`��������Ƃ��A
  �}�e���A���̓����x�J���[�𖳎�����悤�ɂ��܂����B

- FBX �t�@�C���ǂݍ��ݎ��ɁAMaya ASCII �̒ǉ��A�g���r���[�g��
  �{�[��������ݒ�ł���悤�ɂ��܂����B

- GIM �R���o�[�^�� GMO �R���o�[�^�ׂ̗̃t�H���_�Ɋi�[����Ă���΁A
  �摜�t�@�C���������I�� GIM �`���ɃR���o�[�g����悤�ɂ��܂����B
  �ڍׂ́AGMO �R���o�[�^�h�L�������g���́u2.�g���� -> �e�N�X�`���ɂ��āv��
  �Q�Ƃ��������B

- �ȉ��̒��ڃI�v�V������ǉ����܂���
    image_extension2
    image_converter
    fbx_filter_transp
    fbx_maya_notes_transform

<GMO-Converter-Japanese.pdf>
- �e�N�X�`���̕ϊ��ɂ��Ă̋L�q���C�����܂����B

- FBX �t�@�C���̕ϊ��ɂ��Ă̋L�q���C�����܂����B

- �ȉ��̒��ڃI�v�V�����̋L�q��ǉ��^�C�����܂����B
    image_extension
    image_extension2
    image_converter
    fbx_filter_transp
    fbx_maya_notes_transform

----------------------------------------------------------------------
Release 1.45 �̕ύX�_
----------------------------------------------------------------------
<GmoConv.exe>
- �ڑ��ʂ������ӂ����L����ꍇ�ɎO�p�`�X�g���b�v���������ϊ�����Ȃ�
  �s����C�����܂����B

- GMO �o�C�i���`������16�r�b�g�A�j���[�V�������������ǂݍ��܂�Ȃ�
  �s����C�����܂����B

- ����u���b�N�������o�͂���ꍇ�̕֋X�̂��߁A�o�̓I�v�V�����̒l��
  "only" ���w��ł���悤�ɂ��܂���(output_xxxx)�B

- ��̃��b�V���������̒��_�u���b�N���Q�Ƃ���̂�������邽�߂�
  �I�v�V������ǉ����܂���(unify_mesh, unweld_mesh)�B

- FBX �t�@�C���ǂݍ��ݎ��ɁAMaya ASCII �̒ǉ��A�g���r���[�g��
  �����_�X�e�[�g�Ȃǂ�ݒ�ł���悤�ɂ��܂���(fbx_maya_notes_material)�B

- FBXSDK 2009.1 �Ńr���h���� FBX ���W���[���𓯍����܂���(ImportFBX.dll)�B
  ������g�p���邱�Ƃ� NURBS �T�[�t�F�X�̕ϊ����኱���P���܂��B
  �]���̃��W���[���� ImportFBX_2005.dll �ɉ������܂����B

- CrossWalk 3.3 �Ńr���h���� XSI ���W���[���𓯍����܂���(ImportXSI_CW33.dll)�B
  ������g�p���邱�Ƃ� dotXSI 6.0 �`���̕ϊ����኱���P���܂��B
  �������X�P���g����C���X�^���X���A�ꕔ���Ή��̋@�\������܂��B

- FBX/XSI �t�@�C�����̃e�X�g�f�[�^�𓯍����܂��� (test/)

- �ȉ��̒��ڃI�v�V�����̒l��ǉ����܂����B
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

- �ȉ��̒��ڃI�v�V������ǉ����܂����B
    unweld_mesh
    fbx_output_root
    fbx_maya_notes_material
    fbx_maya_notes_userdata

- �ȉ��̒��ڃI�v�V������p�~���܂����B
    fbx_maya_notes_prefix

<GmoView.exe>
- GMO �ȊO�̃t�@�C�����R���o�[�g���ĕ\������悤�ɂ��܂����B

- ���j���[�̕������e�N�X�`���摜�ŕ\������悤�ɂ��܂����B

<GMO-Converter-Japanese.pdf>
- FBX �t�@�C���̕ϊ��ɂ��Ă̐�����ǉ����܂����B

- �ȉ��̒��ڃI�v�V�����̋L�q��ǉ��^�C�����܂����B
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

- �ȉ��̃v���V�[�W���̋L�q��ǉ��^�C�����܂����B
    UnifyMesh
    UnweldMesh

----------------------------------------------------------------------
Release 1.44 �̕ύX�_
----------------------------------------------------------------------
<GmoConv.exe>
- �e�L�X�g�`���� Animate �R�}���h�̈����� INDEX0 OFFSET0 ���w��ł���
  �悤�ɂ��܂����B

- �e���{�[�������W�𒴂���X�L�����b�V������G�b�W���b�V���𐶐�����
  �ꍇ�A�A�j���[�V�������������Đ�����Ȃ��s����C�����܂����B

- XSI �ǂݍ��ݎ��ɁA�A�j���[�V�����̊J�n�I�������� XYZ �e�v�f�ňقȂ�
  �ꍇ�A������ڐ��n���h�����ǉ�����邱�Ƃ�����s����C�����܂����B

- FBX �ǂݍ��ݎ��ɁAMaya ASCII ����e�N�X�`���A�j���[�V�������擾����
  �ꍇ�AOffset/Repeat �A�j���[�V���������݂��Ȃ���΁A
  TranslateFrame/Coverage�A�j���[�V�������擾����悤�ɂ��܂����B

- ��������Ȃ��Q�Ƃ����R�}���h���폜����v���V�[�W����ǉ����܂����B
  ���̋@�\���g�p����ɂ� "--remove_unresolved on" ���w�肵�Ă��������B

- �ŐV SDK �Ńr���h���� XSI ���W���[���𓯍����܂����B
  ����𗘗p����ɂ� GmoConv.cfg ���C�����Ă��������B
  ( load "ImportXSI" -> load "ImportXSI_CW32" )

<GMO-Converter-Japanese.pdf>
- �ȉ��̒��ڃI�v�V�����̋L�q��ǉ����܂����B
  remove_unresolved

- �ȉ��̃v���V�[�W���̋L�q��ǉ����܂����B
  RemoveUnresolved

----------------------------------------------------------------------
Release 1.43 �̕ύX�_
----------------------------------------------------------------------
<GmoConv.exe>
- VC++9 �Ńr���h�ł���悤�C�����܂����B

- �L�[�t���[���팸��ɕs�v�Ȋ֐��J�[�u���폜�ł���悤�ɂ��܂����B
  ���̋@�\��L���ɂ���ɂ� "--epsikon_static 0.0" ���w�肵�Ă��������B

- FBX �ǂݍ��ݎ��Ƀt���[�����[�g�� FBX ����擾�ł���悤�ɂ��܂����B
  ���̋@�\��L���ɂ���ɂ� "--fbx_time_mode on" ���w�肵�Ă��������B

- �e�L�X�g�`�������ŃR���o�[�g�I�v�V�������w��ł���悤�ɂ��܂����B
  ConvertOption �R�}���h���g�p���Ă��������B

- FBX �ǂݍ��ݎ��ɃA�j���[�V�����̍Ō�̃t���[�����o�͂���Ȃ��s���
  �C�����܂����B

- FBX �ǂݍ��ݎ��� Maya ASCII ����ȉ��̃p�����[�^���擾�ł��Ȃ��s���
  �C�����܂����B
  - texture animation ( Repeat UV, Offset )
  - extrapolation ( Pre/Post Infinity )

- �ȉ��̒��ڃI�v�V������ǉ����܂����B
  --epsilon_static
  --fbx_time_mode
  --source_define
  --source_option
  --output_define
  --output_option

- �ŐV SDK �Ńr���h���� XSI/FBX ���W���[���𓯍����܂����B
  - ImportXSI_CW26.dll ( Crosswalk 2.6 )
  - ImportFBX_2009.dll ( FBXSDK 2009.1 )
  �����𗘗p����ɂ� GmoConv.cfg ��ҏW���Ă��������B
  - load "ImportXSI_CW26"
  - load "ImportFBX_2009"

<�h�L�������g�t�@�C��>
<GMO-Converter-Japanese.pdf>
- �ȉ��̒��ڃI�v�V�����̋L�q��ǉ����܂����B
  epsilon_static
  fbx_time_mode
  source_define
  source_option
  output_define
  output_option

- �ȉ��̃v���V�[�W���̋L�q���C�����܂����B
  RebuildKeyFrame

<GMO_Format-Reference-Japanese.pdf>
- �ȉ��̒�`�R�}���h�̋L�q��ǉ����܂����B
  ConvertOption

----------------------------------------------------------------------
Release 1.42 �̕ύX�_
----------------------------------------------------------------------
<GmoConv.exe>
- VC++8 �Ńr���h�ł���悤�C�����܂����B

- �}�e���A���������ɃA�j���[�V�������ꂽ�}�e���A���𓝍����Ȃ��悤��
  ���܂����B

- FBX �ǂݍ��ݎ��Ƀ}�e���A�����ƃe�N�X�`�������u���b�N���ɔ��f����悤��
  ���܂����B

- FBX �ǂݍ��ݎ��ɒ��_�J���[���[���Ȃ�� 0xffffffff �Ƃ��ďo�͂���悤��
  ���܂����B

- FBX �ǂݍ��ݎ��� Maya ASCII ����ȉ��̃p�����[�^���擾����悤�ɂ��܂����B
  - frame rate
  - frame loop
  - material animation ( Color, Transp, etc )
  - texture animation ( Repeat UV, Offset )
  - extrapolation ( Pre/Post Infinity )
  - transform notes
  - material notes

- �ȉ��̒��ڃI�v�V������ǉ����܂����B
  --fbx_use_material_name
  --fbx_use_texture_name
  --fbx_default_vcolor
  --fbx_maya_notes_prefix

<GmoView.exe>
- �}�e���A���̏����A���t�@�l���[���̎��A�����x�A�j���[�V�������@�\���Ȃ�
  �s����C�����܂����B

----------------------------------------------------------------------
Release 1.41 �̕ύX�_
----------------------------------------------------------------------
<GmoConv.exe>
- ����`�̒��ڃI�v�V�������w�肳���ƌx�����܂��B

- FBX �̃C���|�[�g���� Maya ASCII ���`�F�b�N���܂��B
  �e�N�X�`���A�j���[�V�����������炩��擾���܂��B

- �ȉ��̒��ڃI�v�V������ǉ����܂����B
  check_direct_option
  fbx_check_maya_ascii

----------------------------------------------------------------------
Release 1.40 �̕ύX�_
----------------------------------------------------------------------
<GmoConv.exe>
- GMP �t�@�C�����o�͂ł���悤�ɂȂ�܂����BGMP �t�@�C����
  Graphics Engine �̃p�P�b�g���i�[�����t�@�C���`���ł��B
  �g�p�@�̓T���v���v���O���� model6 ���Q�Ƃ��Ă��������B

- GMP �t�@�C���Ƀe�N�X�`���C���[�W���i�[�ł���悤�ɂȂ�܂����B
  �摜�t�@�C���� GIM �`���̂Ƃ��e�N�X�`���p�P�b�g����������܂��B

- GMP �t�@�C���ɕ����̃��[�V�������i�[�ł���悤�ɂȂ�܂����B
  ���[�g�I�u�W�F�N�g�̎q�Ƃ��ă��[�V�����I�u�W�F�N�g����������܂��B

- LINUX ��Ŋg�����W���[�������������삵�Ȃ��s����C�����܂����B

- �����Ŏg�p���Ă��郉�C�u������ namespace ��t�����܂����B
  �����Ŏg�p���Ă��郉�C�u�����̃o�[�W�������X�V���܂����B
  ���̂��߁A�Â��g�����W���[���͍ăR���p�C�����K�v�ł��B

----------------------------------------------------------------------
Release 1.30 �̕ύX�_
----------------------------------------------------------------------
<GmoConv.exe>
- CompFCurveChannel �v���V�[�W�����C�����܂����B
  �����v�f�A�j���[�V�������������ϊ�����Ȃ��s����C�����܂����B

- RebuildBoundingPoints �v���V�[�W�����C�����܂����B
  �v�Z�덷�ɂ�� NaN ���o�͂����s����C�����܂����B
  ���[�h�Ƃ��� "obb", "aabb" ���w��ł���悤�ɂȂ�܂����B

- GXP �t�@�C�����o�͂ł���悤�ɂȂ�܂����B
  GXP �t�@�C���́AGraphics Engine �̃p�P�b�g���i�[�����t�@�C���`���ł��B
  �ڂ����̓T���v���v���O���� model6 ���Q�Ƃ��Ă��������B

- �ȉ��̃R�}���h��ǉ����܂����B
  MeshType
  MeshLevel
  TexGen
  TexMatrix

- �ȉ��� RenderState ���[�h��ǉ����܂����B
  FLIP_FACE
  FLIP_NORMAL

- �ȉ��̃I�v�V������ǉ����܂����B
  -levels

- �ȉ��̒��ڃI�v�V������ǉ����܂����B
  rebuild_line
  rebuild_linestrip
  rebuild_faceedge

- �ȉ��̃v���V�[�W����ǉ����܂���
  RebuildLine
  RebuildLineStrip
  RebuildFaceEdge

<GmoView.exe>
- ���b�V���}�X�N�ɑΉ����܂����B

- �e�N�X�`�����W�����ɑΉ����܂����B

- RenderState ���[�h FLIP_FACE FLIP_NORMAL ��ǉ����܂����B

- RenderState �R�}���h�����b�V���u���b�N�Ŏg�p�ł���悤�ɂȂ�܂����B


----------------------------------------------------------------------
Release 1.20 �̕ύX�_
----------------------------------------------------------------------
<GmoConv.exe>
- ImportXSI ���W���[�����C�����܂����B
  VC++.NET �Ńr���h�ł��܂��B

- ImportFBX ���W���[����ǉ����܂����B
  FBX �t�@�C�����C���|�[�g�ł��܂��B

- RebuildTriStrip �v���V�[�W�����C�����܂����B
  �X�g���b�v�����������P���܂����B

- RebuildRectPatch �v���V�[�W����ǉ����܂����B
  BSpline �� RectPatch �ɕϊ����܂��B

- UnifyMesh �v���V�[�W����ǉ����܂����B
  �����A�g���r���[�g�������b�V�����������܂��B

- UnifyArrays �v���V�[�W����ǉ����܂����B
  �����t�H�[�}�b�g�������_�f�[�^���������܂��B

- �ȉ��̒��ڃI�v�V������ǉ����܂����B
  fbx_filter_fcurve
  rebuild_rectpatch
  unify_mesh
  unify_arrays

- LimitVertexBlend �v���V�[�W���̌Ăяo���ӏ����ړ����܂����B

<GmoView.exe>
- �s���������]���� BMP �t�@�C�����g�p�ł���悤�ɂ��܂����B

----------------------------------------------------------------------
Release 1.11 �̕ύX�_
----------------------------------------------------------------------
<GmoConv.exe>
- �k�ރg���C�A���O�����܂ރ��b�V�����������X�g���b�v������Ȃ�
  �s����C�����܂����B

<GmoView.exe>
- �O���e�N�X�`�����������\������Ȃ��s����C�����܂����B

----------------------------------------------------------------------
Release 1.10 �̕ύX�_
----------------------------------------------------------------------
<GmoConv.exe>
- �o�C�i���t�@�C���o�͎��Ƀu���b�N�����o�͂���Ȃ��s���
  �C�����܂����B

- �o�C�i���t�@�C�����͎��Ƀp�f�B���O�̓��e���s��ɂȂ�s���
  �C�����܂����B

- �o�C�i���t�@�C�����͎��Ƀ��[�t�B���O�����������͂���Ȃ��s���
  �C�����܂����B

- dotXSI �t�@�C���̃G���x���[�v�����C���X�^���X�ɑΉ����܂����B

- �I�u�W�F�N�g�J�����O�ɑΉ����܂����B( BoundingPoints �R�}���h )

- �y�\�[�g���[�h���ɑΉ����܂����B( BoneState �R�}���h )

- 16bit �A�j���[�V�����ɑΉ����܂����B

- �������_�f�[�^�̃I�t�Z�b�g���w��ł���悤�ɂ��܂����B

- �o�E���f�B���O�{�b�N�X�̑Ώۂ��w��ł���悤�ɂ��܂����B

- �ȉ��̃f�[�^��`��ǉ����܂����B
    BoundingPoints
    BoneState

- �ȉ��̒��ڃI�v�V������ǉ��A�C�����܂����B
    rebuild_bounding
    rebuild_bounding_points
    format_keyframe
    offset_vertex
    offset_tcoord

- SortByTransparency �v���V�[�W���̓����ύX���܂����B
  �{�[���̃\�[�g���s�킸�A�������{�[���� BoneState �R�}���h��ǉ����܂��B

- freeze_patchuv ���ڃI�v�V�����̐ݒ�t�@�C���̒l��ύX���܂����B
  �f�t�H���g�ŋȖʃv���~�e�B�u�Ƀe�N�X�`�����W��ǉ����܂��B

<GmoView.exe>
- DXT1 1bit alpha mode �̔�������̌����C�����܂����B

- �����̃��C�u�������X�V���܂����B�idevkit 2.0.0 �Ɠ������́j

- 16bit �A�j���[�V�����f�[�^�ɑΉ����܂����B

- ���[�V�����̒ǉ����[�h�ɑΉ����܂����B
 �i�R���g���[���L�[�{�t�@�C���h���b�v�j

- ���[�V�����̈ꎞ��~�Ή����܂����B�iTab �L�[�j

< �h�L�������g�t�@�C�� >
- 3D_Format-Overview ���� GMO_Format-Overview �Ƀt�@�C������
  �ύX���܂����B

- 3D_Format-Command_Reference �y�� 3D_Format-Block_Reference ��
  ��������1�̃t�@�C���ɂ��A�t�@�C������ GMO_Format-Reference ��
  �ύX���܂����B

----------------------------------------------------------------------
Release 1.00 �̕ύX�_
----------------------------------------------------------------------
<GmoConv.exe>
- �������_�f�[�^�g�p���ɖ@���x�N�g���̕������c�܂Ȃ��悤 VertexOffset ��
  �X�P�[���l�̌v�Z���C�����܂����B

- dotXSI �t�@�C���̕ϊ��ŉ��ʊK�w�����m�[�h�̃C���X�^���X�ɑΉ����܂����B

- �ݒ�t�@�C���̏�����ύX���܂����B

- �g�p�@�t�@�C��(gmoconv.usg)��ݒ�t�@�C��(gmoconv.cfg)�ɓ������܂����B

- �o�C�i���`��(*.GMO)�̓��͂ɑΉ����܂����B

- -Q �I�v�V������ǉ����܂����B��C���f�N�X���_�v���~�e�B�u���g�p���܂��B

- format_index ���ڃI�v�V�����œƗ����_���������Ȃ�Ȃ��悤�ɂ��܂����B
  ��C���f�N�X���_�v���~�e�B�u���g�p����ɂ� unweld_vertex ���ڃI�v�V������
  ���p���邩�A-Q �I�v�V�������g�p���Ă��������B

- sort_block�Asort_command ���ڃI�v�V������p�~���܂����B

- sort_type ���ڃI�v�V������ǉ����A����l�� on �ɂ��܂����B

- last_weight ���ڃI�v�V������p�~���܂����B

- sort_transparency ���ڃI�v�V�����Ƀu���b�N�w���ǉ����܂����B

- �ȉ��̒��ڃI�v�V������ǉ����܂���
    output_directory
    unify_vertex
    unweld_vertex
    remove_unused
    align_image
    xsi_ignore_prefix

- �\�[�X�R�[�h�t�@�C����ǉ����܂����B

<GmoView.exe>
- ���g���N�X���[�h��ǉ����܂����B�f�[�^�T�C�Y�Ⓒ�_���Ȃǂ�\�����܂��B

- �J���[�_�u���̃G�~�����[�V�����ɑΉ����܂����B(-cd �I�v�V����)

- �A�j���[�V�����̊O�}�ɑΉ����܂����B������ REPEAT EXTEND ���[�h�͖��Ή��ł��B

- FrameRepeat �R�}���h�ɑΉ����܂����B

- �f�[�^�t�H���_���� "gmoview" ���� "data" �ɕύX���܂����B

- �\�[�X�R�[�h�t�@�C����ǉ����܂����B
  �{�o�[�W�����̃\�[�X�R�[�h�ł́A�Ȗʕ\���ɖ��Ή��A�y�ь���OFF����
  �J���[�����f����Ȃ��_�̐���������܂��B

----------------------------------------------------------------------
Release 0.9.0 �̕ύX�_
----------------------------------------------------------------------
<GmoConv.exe>
- �������_�f�[�^�̌v�Z���@��؂�̂Ă���l�̌ܓ��ɕύX���܂����B

- �������_�E�F�C�g�̑��a�덷��␳����悤�ɕύX���܂����B

- �R���g���[���L�[�������Ȃ�����̓t�@�C�����h���b�O���h���b�v����ƁA
  �R���\�[���Ƀv�����v�g��\�����A�ǉ��I�v�V��������͂ł���悤��
  �ύX���܂����B

- Scale3 �R�}���h�ɑΉ����܂��� ( �`�����N�^�C�v = 0x00e1 )
  ���̃R�}���h�� Maya �ɂ����� "Segment Scale Compensate"
  ���w�肵���X�P�[�����O�ɑ������܂��B
  ( ������ optimize_bone �I�v�V�����ɂ��{�[���œK����
  Scale3 �R�}���h�ɖ��Ή��ł��B )

- ���_�E�F�C�g�̐����P�̃��f���f�[�^�� GMS �e�L�X�g�`���ɕϊ����Ă���
  ����ɂ�����x�ϊ������ꍇ�ɐ������ϊ�����Ȃ��Ƃ����s����C�����܂����B

- last_weight ���ڃI�v�V������ǉ����܂����B
  ���̃I�v�V������ GMS �e�L�X�g�`���ōŌ�̒��_�E�F�C�g�̏o�͂��X�C�b�`���܂��B
  �i������̎w��ŏo�͂��ꂽ GMS �e�L�X�g�`�����ǂݍ��ނ��Ƃ��ł��܂��B
  �Ȃ� GMO �o�C�i���`���ł͂˂ɍŌ�̒��_�E�F�C�g���o�͂��܂��B�j

- GMS �e�L�X�g�`���ōŌ�̒��_�E�F�C�g���f�t�H���g�ŏo�͂���悤�ɕύX���܂����B
  �ȑO�̃o�[�W�����Ɠ�������ɂ��邽�߂ɂ� gmoconv.cfg �t�@�C����
  last_weight ���ڃI�v�V�����̒l�� off �ɕύX���Ă��������B

<GmoView.exe>
- Scale3 �R�}���h�ɑΉ����܂����B

<GMO-Converter>
- �I�v�V�����̐ݒ���@�̋L�q��ύX���܂����B

- �o�̓I�v�V������last_weight�̋L�q��ǉ����܂����B

----------------------------------------------------------------------
Release 0.6.5 �̕ύX�_
----------------------------------------------------------------------
< GmoConv.exe >
- ���̓t�@�C���Ƃ��ĂQ�c�摜�f�[�^���w�肵���ۂɁA
  �e�N�X�`�������̃��f���ɕϊ�����悤�ɕύX���܂����B

- �����́u�e�N�X�`���v�Ƃ��ĂP�̃t�@�C���Ɍ������邽�߂�
  ���ڃI�v�V�����l�A����ъԐڃI�v�V������ǉ����܂����B
    --merge_mode texture
    -textures

- �ȉ��̃I�v�V������ǉ����܂���
    --format_fname     : FileName �R�}���h�̌`�����w��

< GmoView.exe >
- 128 �o�C�g�A���C�����ꂽ�`���� TM2 �t�@�C�����������\������Ȃ�
  �s����C�����܂����B

- �e�N�X�`���ɉ摜�t�@�C�����܂܂�Ȃ��Ƃ��̃t�@�C�������ɂ�����
  �g���q�� .gim .tm2 .tga .bmp �̃t�@�C����D�悷��悤�ɂ��܂����B

<GMO-Converter>
- �ȉ��̃I�v�V�����̋L�q��ǉ����܂����B
    -textures
    -O2

- merge_mode�I�v�V�����ɁA�ltexture�̋L�q��ǉ����܂����B

- �ȉ��̏o�̓I�v�V�����̋L�q��ǉ����܂����B
    format_index
    format_fname

----------------------------------------------------------------------
�ύX����
----------------------------------------------------------------------
( Release 0.6.0 version )
< GmoConv.exe >
- �ȉ��̏o�̓I�v�V������ǉ����܂���
  --format_index       :���_�C���f�N�X�`�����w��

< GmoView.exe >
- �e�N�X�`���ɉ摜�t�@�C�����܂܂�Ȃ��Ƃ��A���f���Ɠ����t�H���_��
  �摜�t�@�C�������[�h����悤�ɕύX���܂����B

- ��C���f�N�X�`��v���~�e�B�u��\���ł���悤�ɕύX���܂����B

- S3TC ���k�`���̃e�N�X�`����\���ł���悤�ɕύX���܂����B

- Backspace �L�[�őO�̃��f�������[�h����悤�ɕύX���܂����B

( Release 0.2.4 version )
< GmoConv.exe >
- �N�����A�y�� GMS �t�@�C���̃R���o�[�g���� XSIFtk.dll �t�@�C����
  �����N���Ȃ��悤�ɕύX���܂����B

< GmoView.exe >
- �|���S���ɑ΂��郊�t���N�V�����}�b�s���O���������\������Ȃ��Ƃ���
  �s����C�����܂����B

- �������[�h�̃e�N�X�`����\���ł���悤�ɕύX���܂����B

- �ȉ��̃I�v�V������ǉ����܂����B
    -fg <code>     : foreground color ( hex-code )
    -bg <code>     : background color ( hex-code )

( Release 0.2.3 version )
< GmoConv.exe >
- dotXSI �̓��͎��Ɋ֐��J�[�u�̃n���h����񂪐������ϊ�����Ȃ�
  �Ƃ����s����C�����܂����B

- ���_�E�F�C�g���� 64 �ȏ�̎��ɐ������ϊ�����Ȃ��Ƃ����s���
  �C�����܂����B

< GmoView.exe >
- �p�b�`�̐���_���k�ނ��Ă��鎞�A�����������v�Z����Ȃ��Ƃ���
  �s����C�����܂����B

- HERMITE CUBIC ��ԃA�j���[�V�����ɂ����āA�L�[�t���[���̎�����
  �֐��J�[�u���������v�Z����Ȃ��s����C�����܂����B

( Release 0.2.2 version )
- �ȉ��̃h�L�������g��ǉ����܂����B
   doc/tools/GMO-Converter-Japanese.pdf
   doc/tools/GMO-Graphics_Designers_Guideline-Japanese.pdf
   doc/format/3D_Format-Overview-Japanese.pdf
   doc/format/3D_Format-Command_Reference-Japanese.pdf
   doc/format/3D_Format-Block_Reference-Japanese.pdf

< GmoConv.exe >
- �ȉ��̃u���b�N�A�R�}���h��ǉ����܂����B
  BlindBlock   :�u���C���h�f�[�^�u���b�N ( chunk-id 0x000f )
  BlindData    :�u���C���h�f�[�^�R�}���h ( chunk-id 0x00f1 )

- �ȉ��̒�`�R�}���h��ǉ����܂����B
  DefineEnum    :�񋓒萔�̒�`
  DefineBlock   :�u���b�N��`
  DefineCommand :�R�}���h��`

- RenderState �R�}���h�Ɉȉ��̃p�����[�^��ǉ����܂����B
  CULL_FACE     :���ʃJ�����O ( state-id 0x0003 )
  DEPTH_TEST    :�f�v�X�e�X�g ( state-id 0x0004 )
  DEPTH_MASK    :�f�v�X�������� ( state-id 0x0005 )
  ALPHA_TEST    :�A���t�@�e�X�g ( state-id 0x0006 )
  ALPHA_MASK    :�A���t�@�������� ( state-id 0x0007 )

- �ȉ��̍œK���I�v�V������ǉ����܂���
  --optimize_bone      :�s�v�ȃ{�[�����폜
  --sort_transparency  :�`��f�[�^�𓧖��x�Ń\�[�g
  --freeze_patchuv     :�p�b�`�̂t�u���W���t���[�Y
  --freeze_texcrop     :�e�N�X�`���N���b�v���t���[�Y
  --epsilon_translate  :�ʒu�̋��e�덷
  --epsilon_rotate     :��]�̋��e�덷
  --epsilon_scale      :�g��̋��e�덷
  --epsilon_misc       :���̑��̋��e�덷

- �œK���I�v�V���� --frame_loop�A�y�� --frame_rate �Ɉȉ��̂悤��
  �l��ǉ����܂����B

  --frame_loop default ( FrameLoop ��ύX���Ȃ� )
  --frame_rate default ( FrameRate ��ύX���Ȃ� )

- GMS �e�L�X�g�`���t�@�C���̒��_�E�F�C�g�̍ő吔�� 15 ���� 255 
  �ɑ������܂����B

- GMS �e�L�X�g�`���t�@�C���̓��͎��Ƀo�b�N�X���b�V�����܂ރe�N�X�`��
  �t�@�C�����𐳂��������ł��Ȃ��Ƃ����s����C�����܂����B

- GMS �e�L�X�g�`���t�@�C���̓��͎��� Shift-JIS �R�[�h���܂ރe�N�X�`��
  �t�@�C�����𐳂��������ł��Ȃ��Ƃ����s����C�����܂����B

- �}�e���A���𓝍����鎞�� RenderState ���r����悤�ɕύX���܂����B

- ���[�t�`��̐��� 8 �𒴂���ꍇ�A1 �`�󂸂A�������`���ŏo��
  ����悤�ɕύX���܂����B

- dotXSI �̓��͎��Ƀ��[�v���� NurbsSurface �𗼒[�̐���_��
  ���L�����ɏo�͂���悤�ɏC�����܂����B

- dotXSI �̓��͎��ɁA���[�t�B���O�̃x�[�X�`��̃e�N�X�`�����W
  ����ђ��_�J���[���ۑ�����Ȃ��Ƃ����s����C�����܂����B

- dotXSI 3.6 �̃R���X�^���g�V�F�C�h�w��ɑΉ����܂����B

- dotXSI 3.6 �ŕ����̃e�N�X�`�����W�Z�b�g���쐬���ꂽ�ꍇ�ɁA
  �e�N�X�`�����W���������ϊ��ł��Ȃ��Ƃ����s����C�����܂����B

< GmoView.exe >
- �f�o�b�O�\�����[�h��ǉ����܂����B
  �ڍׂ́A��L�g�p���@�ɂ�����w���v���Q�Ƃ��������B

- �ȉ��� RenderState �R�}���h�̃p�����[�^�ɑΉ����܂����B
  CULL_FACE   ���ʃJ�����O
  DEPTH_TEST  �f�v�X�e�X�g
  DEPTH_MASK  �f�v�X��������
  ALPHA_TEST  �A���t�@�e�X�g
  ALPHA_MASK  �A���t�@��������

- 1�`�󂸂A�������`���ŏo�͂��ꂽ���[�t�B���O���f�����Đ��ł���
  �悤�ɕύX���܂����B

- �E�B���h�E�T�C�Y�� +/- �L�[�ŃT�C�Y�ύX�ł���悤�ɕύX���܂����B

- 16bit 256entry CSM1 CLUT �`���� TM2 �t�@�C�������[�h�����ۂ�
  �ُ�I������Ƃ����s����C�����܂����B

<GMO-Converter>
- �u1.�͂��߂Ɂv�́u�C���X�g�[���v�ɂ����āA�r���A�[�w�i�f�[�^�ɂ��Ă�
  �L�q��ǉ����܂����B

- �u3.�I�v�V�����v�́u���ڃI�v�V�����v�ɂ����āA�ȉ��̍œK���I�v�V������
  �ldefault�ɂ��Ă̋L�q��ǉ����܂����B
  frame_loop
  frame_rate

- �u3.�I�v�V�����v�́u���ڃI�v�V�����v�ɂ����āA�œK���I�v�V������
  �ȉ��̃I�v�V�����ɂ��Ă̋L�q��ǉ����܂����B
    optimize_bone
    sort_transparency
    freeze_patchuv
    freeze_texcrop
    epsilon_translate
    epsilon_rotate
    epsilon_scale
    epsilon_misc

<3D_Format-Overview>
- �u2.�f�[�^�\���v�́u�R�}���h�v����сu�o�C�i���`���v�ɂ����āA�f�[�^�^
  �̉���Ɉȉ��̋L�q��ǉ����܂����B
    u_char
    u_short
    u_int

<3D_Format-Block_Reference>
- BlindBlock�u���b�N�ɂ��Ă̋L�q��ǉ����܂����B

- File�u���b�N�́u�R�}���h�v�ɂ����āA�ȉ��̃R�}���h�ɂ��Ă̋L�q��
  �ǉ����܂����B
    DefineEnum
    DefineBlock
    DefineCommand

- Model�u���b�N�́u�R�}���h�v�ɂ����āAVertexOffset�̋L�q��ǉ����܂����B

- Bone�u���b�N�́u�R�}���h�v�ɂ����āAMorphIndex�̋L�q��ǉ����܂����B

- Arrays�u���b�N�́u�f�[�^�`���v�ɂ����āA���_�J���[�ɂ��Ă̋L�q��
  �ǉ����܂����B

- FCurve�u���b�N�́u�����v�ɂ����āA�ȉ��̒萔�̋L�q��ǉ����܂����B
  �܂��u�f�[�^�`���v�ɂ����āAHERMITE��Ԃ����CUBIC��Ԃ̋L�q��
  �ǉ����܂����B
    HERMITE
    CUBIC

<3D_Format-Command_Reference>
- �ȉ��̒�`�R�}���h�ɂ��Ă̋L�q��ǉ����܂����B
    DefineEnum
    DefineBlock
    DefineCommand

- �ȉ��̋��ʃR�}���h�ɂ��Ă̋L�q��ǉ����܂����B
    VertexOffset
    MorphIndex

- BlindData�R�}���h�ɂ��Ă̋L�q��ǉ����܂����B

- MorphWeights�́u����v�ɂ����āAMorphIndex�ɂ��Ă̋L�q��ǉ�
  ���܂����B

- RenderState�́u�����v�ɂ����āA�`��X�e�[�g�̒萔���ȉ��̂悤��
  �ύX���܂����B
  �폜:    ENABLE_LIGHTING
           ENABLE_FOG
  �ǉ�:    LIGHTING
           FOG
           CULL_FACE
           DEPTH_TEST
           DEPTH_MASK
           ALPHA_TEST
           ALPHA_MASK

- Animate�́u����v�ɂ����āA����ΏۃR�}���h�̋L�q����TexFactor��
  �폜���A�ȉ��̃R�}���h�̋L�q��ǉ����܂����B
    MorphIndex
    Ambient

- �ȉ��̃R�}���h�́u�����v�ɂ����āA�C���f�N�X�̌^��short����u_short��
  �ύX���܂����B
    DrawArrays
    DrawParticle
    DrawBSpline
    DrawRectMesh
    DrawRectPatch

- SetTexture�́u����v�ɂ����āA���ӎ������폜���܂����B

- BlendFunc�́u�����v�ɂ����āA���[�h�̒萔MIX���폜���A�ȉ��̒萔��
  �L�q��ǉ����܂����B
    ADD
    SUB
    REV

- BlendFunc�́u�����v�ɂ����āA�W���̉���Ɉȉ��̒萔�̋L�q��ǉ����܂����B
    ZERO
    ONE

----------------------------------------------------------------------
�g�p�����E����
----------------------------------------------------------------------
���̃\�t�g�E�F�A�̎g�p�����A�g�p�����͋M�ЂƓ���(������Ѓ\�j�[�E
�R���s���[�^�G���^�e�C�������g)�Ƃ̊Ԃɒ�������Ă���_��ɏ����܂��B

----------------------------------------------------------------------
���W�Ɋւ��钍�ӏ���
----------------------------------------------------------------------
"PSP" �͊�����Ѓ\�j�[�E�R���s���[�^�G���^�e�C�������g�̏��W�ł��B

�p�b�P�[�W���̖{�����ɋL�ڂ���Ă����Ж��A���i���A�T�[�r�X���́A��ʂ�
�e�Ђ̏��W�܂��͓o�^���W�ł��B
�Ȃ��A�p�b�P�[�W���̖{������ (R)�A(TM)�A(SM)�}�[�N�͖��L���Ă��Ȃ��ꍇ��
����܂��B

This software contains Autodesk(R) Crosswalk code developed by 
Autodesk, Inc.
(C) 2009 Autodesk, Inc. ALL RIGHTS RESERVED.
Autodesk, Crosswalk, FBX, Maya, Softimage and 3ds Max are registered 
trademarks or trademarks of Autodesk, Inc., and/or its subsidiaries and/or 
affiliates in the USA and/or other countries.

----------------------------------------------------------------------
���쌠�ɂ���
----------------------------------------------------------------------
�{�\�t�g�E�F�A�̒��쌠�́A������Ѓ\�j�[��R���s���[�^�G���^�e�C�������g
�ɋA�����Ă��܂��B

