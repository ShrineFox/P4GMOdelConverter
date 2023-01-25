[SCE CONFIDENTIAL DOCUMENT]
PSP(TM) GMO Converter version 1.47
                   Copyright (C) 2010 Sony Computer Entertainment Inc.
                                                   All Rights Reserved
======================================================================
본 패키지에는 PSP™용 3D 모델 데이터를 취급하기 위해 필요한 컨버터,
뷰어, 문서가 포함되어 있습니다.

[gmoconv ver 1.47]
gmoconv는 3D 모델 컨버터로, 일반적인 3D 모델 데이터를 PSP™용
3D 모델 데이터로 변환합니다.

[gmoview ver 1.47]
gmoview는 3D 모델 뷰어로, gmoconv로 작성한 PSP™용 3D 모델
데이터를 PC 상에서 확인할 수 있습니다.

----------------------------------------------------------------------
패키지 구성
----------------------------------------------------------------------
devkit/
|---tool
|     +--- gmoconv
|          |--- Readme_Gmoconv-Korean.txt    :본 파일
|          |--- GmoConv.exe                  :컨버터
|          |--- GmoView.exe                  :뷰어
|          |--- GmoConv.cfg                  :설정 파일
|          |--- GmoConv.def                  :사용자 정의 파일
|          |--- GxoTool.dll                  :컨버터 기본 모듈
|          |--- data/                        :뷰어 배경 데이터
|          |--- ImportXSI.dll                :XSI 임포터 모듈
|          |--- ImportXSI_CW33.dll           :XSI 임포터 모듈
|          |--- ImportXSI_CW41.dll           :XSI 임포터 모듈
|          |--- ImportFBX.dll                :FBX 임포터 모듈
|          |--- ImportFBX_2005.dll           :FBX 임포터 모듈
|          |--- lib/                         :컨버터 확장 모듈
|          |--- msvcp71.dll
|          |--- msvcr71.dll
|          |--- XSIFtk.dll                   :SOFTIMAGE(R)|XSI(R) FTK 3.6.3 dll
|          |--- test/                        :테스트 데이터
|          +--- src
|                |--- GmoConv
|                +--- GmoView
+---document
     |---format           :데이터 포맷 문서
     |    |---GMO_Format-Overview-Korean.pdf
     |    +---GMO_Format-Reference-Korean.pdf
     +---tool             :개요 문서
           |---GMO-Converter-Korean.pdf
           +---GMO-Graphics_Designers_Guideline-Korean.pdf

----------------------------------------------------------------------
주의 사항
----------------------------------------------------------------------
< pdf 파일에 대하여 >
- 이 패키지에 포함된 문서는 Adobe Acrobat 5.0이상,
  Adobe Acrobat Reader 5.0이상을 사용하여 보실 수 있습니다.
  Adobe Reader(구 Adobe Acrobat Reader) 최신판은 Adobe 홈페이지에서
  다운로드 하실 수 있습니다.

----------------------------------------------------------------------
사용 방법
----------------------------------------------------------------------
- gmoconv ver1.47

  usage:
    gmoconv <입력 파일(*.xsi,*.gms)> [옵션]

  options:
    -interact     : 추가 옵션을 키 입력
    -models       : 복수의 「모델」로서 하나의 파일로 결합
    -motions      : 복수의 「모션」으로서 하나의 파일로 결합
    -textures     : 복수의 「텍스처」로서 하나의 파일로 결합
    -prompt       : 항상 키 입력 대기 수행
    -warning      : 경고 시에 키 입력 대기 수행
    -error        : 에러 시에 키 입력 대기 수행
    -viewer       : 종료 시에 GmoView.exe 실행
    -o <filename> : 출력 파일명 지정
    -s <scale>    : 형상 데이터 스케일링
    -t <scale>    : 모션의 길이 스케일링
    -r <fps>      : 모션의 프레임레이트 지정
    -O, -O2       : 최적화 수행
    -S            : 텍스트 형식 출력
    -H            : 헤더 파일 출력
    -Z            : 크기가 작은 정점 포맷 사용
    -Q            : 비인덱스 정점 프리미티브 사용

- gmoview ver1.47

  usage:
    gmoview <입력 파일(*.gmo)> [options]

  options:
    -fg <code>     : foreground color ( hex-code )
    -bg <code>     : background color ( hex-code )
    -multi         : enable multi model
    -colx2         : enable color double
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
    Z,X KEY        : ZOOM
    +,- KEY        : RESIZE

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

----------------------------------------------------------------------
Release 1.47의 변경 사항
----------------------------------------------------------------------
<GmoConv.exe>
- 일괄처리(batch) 파일을 입력 파일로서 지정할 수 있도록 했습니다.

- 폴리곤 엣지 정보를 출력하는 옵션을 추가했습니다( rebuild_faceedge2 ).

- 정점 웨이트를 최적화하는 옵션을 추가했습니다( blend_cost_mesh ).

  메시 분할의 코스트를(예를 들면 "300" 등) 수치로 지정함으로써
  정점 웨이트가 적절히 삭감되고 GPU 부하가 경감됩니다.

- 출력 옵션에 의해 블록 출력이 off인 경우에 이름 참조를 위해
  빈 블록을 출력하는 옵션을 추가했습니다( output_anchor ).

- FBX 모듈을 FBXSDK 2010.2로 빌드할 수 있도록 수정했습니다.

- FBX 모듈이 Maya의 Rigid Bind에 대응했습니다.

- XSI 모듈이 다음 패러미터에 대응했습니다.
    dotXSI 5.0 형식의 피벗 포지션
    dotXSI 3.5 형식의 머티리얼 애니메이션
    dotXSI 3.0 형식의 투명(transparency) 애니메이션

- 다음 프로시저를 추가했습니다.
    RebuildFaceEdge2

- 다음 직접 옵션을 추가했습니다.
    batch_extension
    rebuild_faceedge2
    blend_cost_mesh
    blend_cost_matrix
    blend_cost_weight
    blend_cost_strip
    output_anchor

<GmoView.exe>
- 정점 웨이트 수 등 GPU 부하에 관련된 정보를 표시하도록 했습니다.
  확인하려면 "4" 키를 눌러 정보 페이지를 표시해 주십시오.
  (단, GPU 부하의 수치는 어림값입니다.)

<GMO-Converter-Korean.pdf>
- 일괄처리(batch) 파일의 사용에 대한 기재를 추가했습니다.

- 변환 가능한 패러미터에 대한 기재를 추가했습니다.

- XSI 파일의 변환에 대한 기재를 추가했습니다.

- 다음 직접 옵션의 기재를 추가/수정했습니다.
    batch_extension
    rebuild_faceedge
    rebuild_faceedge2
    blend_cost_mesh
    blend_cost_matrix
    blend_cost_weight
    blend_cost_strip
    output_anchor

- 다음 프로시저의 기재를 추가/수정했습니다.
    LimitVertexBlend
    RebuildFaceEdge
    RebuildFaceEdge2

<GMO_Format-Reference-Korean.pdf>
- 다음 명령어의 기재를 추가했습니다.
    EdgeFlags
    EdgeFaces

----------------------------------------------------------------------
Release 1.46의 변경 사항
----------------------------------------------------------------------
<GmoConv.exe>
- FBX 파일 읽기 시에 피벗이 읽히지 않는 버그를 수정했습니다.

- FBX 파일 읽기 시에 머티리얼의 투명도 텍스처가 있을 때,
  머티리얼의 투명도 컬러를 무시하도록 했습니다.

- FBX 파일 읽기 시에 Maya ASCII의 추가 속성에서 본(bone) 속성을
  설정할 수 있게 했습니다.

- GIM 컨버터가 GMO 컨버터의 옆 폴더에 저장되어 있으면,
  이미지 파일을 자동으로 GIM 형식으로 변환하도록 했습니다.
  자세한 내용은 GMO 컨버터 문서의 「2. 사용방법 -> 텍스처에 대하여」를
  참조해 주십시오.

- 아래의 직접 옵션을 추가했습니다.
    image_extension2
    image_converter
    fbx_filter_transp
    fbx_maya_notes_transform

<GMO-Converter-Korean.pdf>
- 텍스처의 변환에 대한 기재를 수정했습니다.

- FBX 파일의 변환에 대한 기재를 수정했습니다.

- 아래의 직접 옵션의 기재를 추가/수정했습니다.
    image_extension
    image_extension2
    image_converter
    fbx_filter_transp
    fbx_maya_notes_transform

----------------------------------------------------------------------
Release 1.45의 변경 사항
----------------------------------------------------------------------
<GmoConv.exe>
- 접속면이 복수 변을 공유하는 경우에 삼각형 스트립이 올바르게 변환되지
  않는 버그를 수정했습니다.

- GMO 바이너리 형식으로부터 16비트 애니메이션을 올바르게 읽을 수 없는
  버그를 수정했습니다.

- 특정 블록만을 출력하는 경우의 편의를 위해 출력 옵션의 값에
  "only"를 지정할 수 있게 했습니다(output_xxxx).

- 하나의 메시가 복수의 정점 블록을 참조하는 것을 피하기 위한
  옵션을 추가했습니다(unify_mesh, unweld_mesh).

- FBX 파일 읽기 시에 Maya ASCII의 추가 속성에서
  렌더링 상태 등을 설정할 수 있게 했습니다(fbx_maya_notes_material).

- FBXSDK 2009.1로 빌드한 FBX 모듈을 포함시켰습니다(ImportFBX.dll).
  이것을 사용하면 NURBS 서피스의 변환이 약간 개선됩니다.
  기존의 모듈은 ImportFBX_2005.dll로 개명되었습니다.

- CrossWalk 3.3으로 빌드한 XSI 모듈을 포함시켰습니다(ImportXSI_CW33.dll).
  이것을 사용하면 dotXSI 6.0 형식의 변환이 약간 개선됩니다.
  단, 스켈레톤이나 인스턴스 등 일부 미대응인 기능도 있습니다.

- FBX/XSI 파일 등의 테스트 데이터를 포함시켰습니다(test/).

- 다음의 직접 옵션의 값을 추가했습니다.
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

- 다음의 직접 옵션을 추가했습니다.
    unweld_mesh
    fbx_output_root
    fbx_maya_notes_material
    fbx_maya_notes_userdata

- 다음의 직접 옵션을 폐지했습니다.
    fbx_maya_notes_prefix

<GmoView.exe>
- GMO 이외의 파일을 변환하여 표시하도록 했습니다.

- 메뉴의 문자를 텍스처 이미지로 표시하도록 했습니다.

<GMO-Converter-Korean.pdf>
- FBX 파일의 변환에 대한 설명을 추가했습니다.

- 다음의 직접 옵션의 기재를 추가/수정했습니다.
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

- 다음 프로시저의 기재를 추가/수정했습니다.
    UnifyMesh
    UnweldMesh

----------------------------------------------------------------------
Release 1.44의 변경 사항
----------------------------------------------------------------------
<GmoConv.exe>
- 텍스트 형식으로 Animate 명령어의 패러미터에 INDEX0 OFFSET0를 지정할 수
  있도록 하였습니다.

- 영향 본(bone) 수가 8을 넘는 스킨 메시에서 엣지 메시를 생성한 경우,
  애니메이션이 올바르게 재생되지 않는 버그를 수정하였습니다.

- XSI 읽기 시에 애니메이션의 시작/종료 시각이 XYZ 각 요소에서 다른 경우,
  잘못된 접선 핸들이 추가되는 경우의 버그를 수정하였습니다.

- FBX 읽기 시에 Maya ASCII에서 텍스처 애니메이션을 취득하는 경우에
  Offset/Repeat 애니메이션이 존재하지 않으면, TranslateFrame/Coverage
  애니메이션을 취득하도록 하였습니다.

- 해결되지 않은 참조를 가지는 명령어를 삭제하는 프로시저를 추가하였습니다.
  이 기능을 사용하려면, "--remove_unresolved on"을 지정해 주십시오.

- 최신 SDK로 빌드한 XSI 모듈을 동봉하였습니다.
  이것을 이용하려면, GmoConv.cfg를 수정해 주십시오.
  ( load "ImportXSI" -> load "ImportXSI_CW32" )

<GMO-Converter-Korean.pdf>
- 다음의 직접 옵션에 대한 내용을 추가했습니다.
  remove_unresolved

- 다음의 프로시저를 추가하였습니다. 
  RemoveUnresolved

----------------------------------------------------------------------
Release 1.43 의 변경 사항
----------------------------------------------------------------------
<GmoConv.exe>
- VC++9에서 빌드할 수 있도록 수정했습니다.

- 키 프레임 삭감 후에 불필요한 함수 커브를 삭제할 수 있도록 했습니다.
  이 기능을 활성화하시려면 "--epsikon_static 0.0"을 지정해 주십시오.

- FBX 읽기 시에 프레임 레이트를 FBX에서 취득할 수 있도록 했습니다.
  이 기능을 활성화하시려면 "--fbx_time_mode on"을 지정해 주십시오.

- 텍스트 형식 내부에서 변환 옵션을 지정할 수 있도록 했습니다.
  ConvertOption 커맨드를 사용해 주십시오.

- FBX 읽기 시에 애니메이션의 마지막 프레임이 출력되지 않는 문제를
  수정했습니다.

- FBX 읽기 시에 Maya ASCII에서 다음의 패러미터가 취득되지 않는 문제를
  수정했습니다.
  - texture animation ( Repeat UV, Offset )
  - extrapolation ( Pre/Post Infinity )

- 다음의 직접 옵션을 추가했습니다.
  --epsilon_static
  --fbx_time_mode
  --source_define
  --source_option
  --output_define
  --output_option

- 최신 SDK에서 빌드한 XSI/FBX 모듈을 포함했습니다.
  - ImportXSI_CW26.dll ( Crosswalk 2.6 )
  - ImportFBX_2009.dll ( FBXSDK 2009.1 )
  이들을 이용하시려면 GmoConv.cfg를 편집해 주십시오.
  - load "ImportXSI_CW26"
  - load "ImportFBX_2009"

<문서 파일>
<GMO-Converter-Korean.pdf>
- 다음의 직접 옵션에 대한 내용을 추가했습니다.
  epsilon_static
  fbx_time_mode
  source_define
  source_option
  output_define
  output_option

- 다음의 프로시저에 대한 내용을 수정했습니다.
  RebuildKeyFrame

<GMO_Format-Reference-Korean.pdf>
- 다음의 정의 커맨드에 대한 내용을 추가했습니다.
  ConvertOption

----------------------------------------------------------------------
Release 1.42의 변경 사항
----------------------------------------------------------------------
<GmoConv.exe>
- VC++8에서 빌드할 수 있도록 수정했습니다.

- 머티리얼 통합시에 애니메이션된 머티리얼을 통합하지 않도록 했습니다.

- FBX 읽기 시에 머티리얼명과 텍스처명을 블록명에 반영하도록 했습니다.

- FBX 읽기 시에 정점 컬러가 0이면 0xffffffff로 출력하도록 했습니다.

- FBX 읽기 시에 Maya ASCII에서 다음의 패러미터를 취득하도록 했습니다.
  - frame rate
  - frame loop
  - material animation ( Color, Transp, etc )
  - texture animation ( Repeat UV, Offset )
  - extrapolation ( Pre/Post Infinity )
  - transform notes
  - material notes

- 다음의 직접 옵션을 추가했습니다.
  --fbx_use_material_name
  --fbx_use_texture_name
  --fbx_default_vcolor
  --fbx_maya_notes_prefix

<GmoView.exe>
- 머티리얼의 초기 알파값이 0일 때, 투명도 애니메이션이 기능하지 않는
  버그를 수정했습니다.

----------------------------------------------------------------------
Release 1.41의 변경 사항
----------------------------------------------------------------------
<GmoConv.exe>
- 정의되지 않은 직접 옵션이 지정되면 경고합니다.

- FBX의 임포트 시에 Maya ASCII를 체크합니다.
  텍스처 애니메이션을 거기서 취득합니다.

- 다음의 직접 옵션을 추가했습니다.
  check_direct_option
  fbx_check_maya_ascii

----------------------------------------------------------------------
Release 1.40의 변경 사항
----------------------------------------------------------------------
<GmoConv.exe>
- GMP 파일을 출력할 수 있게 되었습니다. GMP 파일은 Graphics Engine
  패킷을 저장한 파일 형식입니다.
  사용법은 샘플 프로그램 model6을 참조해 주십시오.

- GMP 파일에 텍스처 이미지를 저장할 수 있게 되었습니다.
  이미지 파일이 GIM 형식이면 텍스처 패킷이 생성됩니다.

- GMP 파일에 복수의 모션을 저장할 수 있게 되었습니다.
  루트 오브젝트의 자식으로서 모션 오브젝트가 생성됩니다.

- LINUX 상에서 확장 모듈이 제대로 동작하지 않는 버그를 수정했습니다.

- 내부에서 사용하는 라이브러리에 namespace를 부가했습니다.
  내부에서 사용하는 라이브러리의 버전을 업데이트했습니다.
  따라서 이전 확장 모듈은 재컴파일해야만 합니다.

----------------------------------------------------------------------
Release 1.30의 변경 사항
----------------------------------------------------------------------
<GmoConv.exe>
- CompFCurveChannel 프로시저를 수정하였습니다. 
  부분 요소 애니메이션이 올바르게 변환되지 않는 문제를 수정하였습니다. 

- RebuildBoundingPoints 프로시저를 수정하였습니다. 
  계산 오차로 인해 NaN이 출력되는 문제를 수정하였습니다. 
  모드에 "obb", "aabb" 를 지정할 수 있게 되었습니다. 

- GXP 파일을 출력할 수 있게 되었습니다. 
  GXP 파일은 Graphics Engine의 패킷을 저정한 파일 형식입니다. 
  자세한 내용은 샘플 프로그램 model6를 참조하여 주십시오. 

- 다음의 커맨드를 추가하였습니다. 
  MeshType
  MeshLevel
  TexGen
  TexMatrix

- 다음의 RenderState 모드를 추가하였습니다. 
  FLIP_FACE
  FLIP_NORMAL

- 다음의 옵션을 추가하였습니다. 
  -levels

- 다음의 직접 옵션을 추가하였습니다. 
  rebuild_line
  rebuild_linestrip
  rebuild_faceedge

- 다음의 프로시저를 추가하였습니다. 
  RebuildLine
  RebuildLineStrip
  RebuildFaceEdge

<GmoView.exe>
- 메시 마스크에 대응하게 되었습니다. 

- 텍스처 좌표 생성에 대응하게 되었습니다. 

- RenderState 모드 FLIP_FACE FLIP_NORMAL을 추가하였습니다. 

- RenderState 커맨드를 메시 블록에서 사용할 수 있게 되었습니다. 


----------------------------------------------------------------------
Release 1.20의 변경 사항
----------------------------------------------------------------------
<GmoConv.exe>
- ImportXSI 모듈을 수정하였습니다. 
  VC++.NET로 빌드할 수 있습니다. 

- ImportFBX 모듈을 추가하였습니다. 
  FBX 파일을 임포트 할 수 있습니다. 

- RebuildTriStrip 프로시저를 수정하였습니다. 
  스트립화 처리를 개선하였습니다. 

- RebuildRectPatch 프로시저를 수정하였습니다. 
  BSpline을 RectPatch로 변환하였습니다. 

- UnifyMesh 프로시저를 추가하였습니다.
  같은 속성을 가지는 메시를 결합합니다. 

- UnifyArrays 프로시저를 추가하였습니다. 
  같은 포맷을 가진 정점 데이터를 결합합니다. 

- 다음의 직접 옵션을 추가하였습니다. 
  fbx_filter_fcurve
  rebuild_rectpatch
  unify_mesh
  unify_arrays

- LimitVertexBlend 프로시저를 호출하는 곳을 이동하였습니다. 

<GmoView.exe>
- 행의 순서가 반전된 BMP 파일을 사용할 수 있게 하였습니다. 

----------------------------------------------------------------------
Release 1.11의 변경 사항
----------------------------------------------------------------------
<GmoConv.exe>
- 축퇴 삼각형(degenerate triangle)을 포함하는 메시가 올바르게 
  스트립화되지 않는 문제를 수정하였습니다.

<GmoView.exe>
- 외부 텍스처가 올바르게 표시되지 않는 문제를 수정하였습니다

----------------------------------------------------------------------
Release 1.10의 변경 사항
----------------------------------------------------------------------
<GmoConv.exe>
- 바이너리 파일 출력시에 블록명이 출력되지 않는 문제를
  수정하였습니다.

- 바이너리 파일 입력시에 패딩 내용이 불안정해지는 문제를
  수정하였습니다.

- 바이너리 파일 입력시에 모핑이 올바르게 입력되지 않는 문제를
  수정하였습니다.

- dotXSI 파일의 인벨럽(envelope)을 가지는 인스턴스에 대응하였습니다.

- 오브젝트 컬링에 대응하였습니다.(BoundingPoints 커맨드)

- Ｚ 정렬 모드 정보에 대응하였습니다. (BoneState 커맨드)

- 16bit 애니메이션에 대응하였습니다.

- 정수 정점 데이터의 오프셋을 지정할 수 있도록 하였습니다.

- 바운딩 박스의 대상을 지정할 수 있도록 하였습니다.

- 다음의 데이터 정의를 추가하였습니다.
    BoundingPoints
    BoneState

- 다음의 직접 옵션을 추가하고 수정하였습니다.
    rebuild_bounding
    rebuild_bounding_points
    format_keyframe
    offset_vertex
    offset_tcoord

- SortByTransparency 프로시저의 동작을 변경하였습니다.
  본을 정렬하지 않고 반투명 본에 BoneState 커맨드를 추가하였습니다.

- freeze_patchuv 직후 옵션의 설정 파일 값을 변경하였습니다.
  디폴트에서 곡면 프리미티브에 텍스처 좌표를 추가하였습니다.

<GmoView.exe>
- DXT1 1bit alpha mode의 판정 조건 오류를 수정하였습니다.

- 내부 라이브러리를 업데이트 하였습니다.(devkit 2.0.0과 동일한 것)

- 16bit 애니메이션 데이터에 대응하였습니다.

- 모션의 추가 로드에 대응하였습니다.
  (컨트롤 키 + 파일 드롭)

- 모션의 일시 정지에 대응하였습니다.(Tab 키)

< 문서 파일 >
- 3D_Format-Overview에서 GMO_Format-Overview로 파일명을
  변경하였습니다.

- 3D_Format-Command_Reference 및 3D_Format-Block_Reference를 
  결합하여 하나의 파일로 구성하고 파일명을 GMO_Format-Reference로
  변경하였습니다.

----------------------------------------------------------------------
Release 1.00의 변경 사항
----------------------------------------------------------------------
<GmoConv.exe>
- 정수 정점 데이터 사용시에 법선 벡터의 방향이 비틀리지 않도록
  VertexOffset의 스케일 값 계산을 수정하였습니다.

- dotXSI 파일의 변환으로 하위 계층을 가진 노드의 인스턴스에
  대응하였습니다.

- 설정 파일의 서식을 변경하였습니다.

- 사용법 파일(gmoconv.usg)을 설정 파일(gmoconv.cfg)에 결합하였습니다.

- 바이너리 형식(*.GMO)의 입력에 대응하였습니다.

- -Q 옵션을 추가하였습니다. 비인덱스 정점 프리미티브를 사용합니다.

- format_index 직접 옵션에서 독립 정점화 하지 않도록 하였습니다.
  비인덱스 정점 프리미티브를 사용하려면 unweld_vertex 직접 옵션을 
  병용하든지, -Q 옵션을 사용하시기 바랍니다.

- sort_block、sort_command 직접 옵션을 폐지하였습니다.

- sort_type 직접 옵션을 추가하고, 기정값을 on으로 하였습니다.

- last_weight 직접 옵션을 폐지하였습니다.

- sort_transparency 직접 옵션에 블록 지정을 추가하였습니다.

- 다음의 직접 옵션을 추가하였습니다.
    output_directory
    unify_vertex
    unweld_vertex
    remove_unused
    align_image
    xsi_ignore_prefix

- 소스 코드 파일을 추가하였습니다.

<GmoView.exe>
- 매트릭스 모드를 추가하였습니다. 데이터 크기와 정점 등을 표시합니다.

- 컬러 더블의 에뮬레이션에 대응하였습니다. (-cd 옵션)

- 애니메이션의 외부삽입에 대응하였습니다. 단 REPEAT EXTEND 모드는
  미대응입니다.

- FrameRepeat 커맨드에 대응하였습니다.

- 데이터 폴더 명을 "gmoview"에서 "data" 로 변경하였습니다.

- 소스 코드 파일을 추가하였습니다.
  본 버전은 소스 코드 공개 때문에 다시 작성되었고, 
  곡면 표시에 미대응, 광선 OFF 시에 컬러가 반영되지 않는 제한이
  있습니다.
  GmoView1X.exe는 이를 빌드한 바이너리입니다.

----------------------------------------------------------------------
Release 0.9.0의 변경 사항
----------------------------------------------------------------------
<GmoConv.exe>
- 정수 정점 데이터의 계산 방법을 버림에서 반올림으로 변경하였습니다.

- 정수 정점 웨이트의 총합 오차를 보정하도록 변경하였습니다.

- 컨트롤 키를 누르면서 입력 파일을 끌어다 놓으면 콘솔에 프롬프트를
  표시하여 추가 옵션을 입력할 수 있도록 변경하였습니다.

- Scale3 커맨드에 대응하였습니다. (청크 형태 = 0x00e1)
  이 커맨드는 Maya에서 "Segment Scale Compensate"를 지정한 스케일링에
  해당합니다.
  ( 단, optimize_bone 옵션에 의한 본(bone) 최적화는 Scale3 커맨드에
  대응하지 않았습니다.)

- 정점 웨이트의 수가 1인 모델 데이터를 GMS 텍스트 형식으로 변환한 다음 
  다시 한 번 변환한 경우 제대로 변환되지 않는 문제를 수정하였습니다.

- last_weight 직접 옵션을 추가하였습니다.
  이 옵션은 GMS 텍스트 형식으로 마지막 정점 웨이트의 출력을 전환합니다.
  (어떠한 지정으로 출력된 GMS 텍스트 형식도 모두 읽을 수 있습니다.)
  GMO 바이너리 형식에서는 항상 마지막 정점 웨이트를 출력합니다.

- GMS 텍스트 형식에서 마지막 정점 웨이트를 디폴트로 출력하도록
  변경하였습니다.
  이전 버전과 동일한 동작으로 만들기 위해서는 gmoconv.cfg 파일의 
  last_weight 직접 옵션값을 off로 변경하여 주십시오.

<GmoView.exe>
- Scale3 커맨드에 대응하였습니다.

<GMO-Converter>
- 옵션 설정 방법에 대한 내용을 변경하였습니다.

- 출력 옵션에 last_weight에 대한 내용을 추가하였습니다.

----------------------------------------------------------------------
Release 0.6.5의 변경 사항
----------------------------------------------------------------------
< GmoConv.exe >
- 입력 파일로서 2D 이미지 데이터를 지정하였을 때 텍스처에만 해당하는
  모델로 변환하도록 변경하였습니다. 

- 복수의 「텍스처」로서 하나의 파일에 결합하기 위한 직접 옵션값 및
  간접 옵션을 추가하였습니다.
    --merge_mode texture
    -textures

- 다음과 같은 옵션을 추가하였습니다.
    --format_fname     : FileName 커맨드의 형식 지정

< GmoView.exe >
- 128 바이트로 정렬된 형식의 TM2 파일이 올바르게 표시되지 않는 문제를 
  수정하였습니다.

- 텍스처에 이미지 파일이 포함되지 않을 때의 파일 검색에서 확장자가 
  .gim .tm2 .tga .bmp인 파일을 우선하도록 하였습니다.

<GMO-Converter>
- 아래의 옵션에 대한 내용을 추가하였습니다.
    -textures
    -O2

- merge_mode 옵션에 값 texture에 대한 내용을 추가하였습니다.

- 아래의 출력 옵션에 대한 내용을 추가하였습니다.
    format_index
    format_fname

----------------------------------------------------------------------
변경 이력
----------------------------------------------------------------------
( Release 0.6.0 version )
< GmoConv.exe >
-다음과 같은 출력 옵션을 추가하였습니다.
 --format_index		:정점 인덱스 형식 지정

< GmoView.exe >
- 텍스처에 이미지 파일이 포함되지 않을 때 모델과 동일한 폴더의 이미지 
  파일을 로드하도록 변경하였습니다. 

-인덱스 드로잉 프리미티브가 아닌 것을 표시할 수 있도록 변경하였습니다.

-S3TC 압축 형식의 텍스처를 표시할 수 있도록 변경하였습니다.

-Backspace 키로 이전 모델을 로드하도록 변경하였습니다.

( Release 0.2.4 version )
< GmoConv.exe >
- 실행시 및 GMS 파일 변환시 XSIFtk.dll 파일을 링크하지 않도록
  변경하였습니다.

< GmoView.exe >
- 폴리곤에 대한 반사 매핑(reflection mapping)이 제대로 표시되지 않는
  문제를 수정하였습니다.

- 고속 모드의 텍스처를 표시할 수 있도록 변경하였습니다.

- 다음의 옵션을 추가하였습니다.
    -fg <code>     : foreground color ( hex-code )
    -bg <code>     : background color ( hex-code )

( Release 0.2.3 version )
< GmoConv.exe >
- dotXSI 입력시 함수 곡선 취급 정보가 올바르게 변환되지 않는 문제를
  수정하였습니다.

- 정점 웨이트 수가 64 이상일 때 올바르게 변환되지 않는 문제를
  수정하였습니다.

< GmoView.exe >
- 패치의 제어점이 축퇴되었을 때 광원이 올바르게 계산되지 
  않는 문제를 수정하였습니다.

- HERMITE CUBIC 보간 애니메이션에서 키 프레임 시각에서 함수 곡선이
  올바르게 계산되지 않는 문제를 수정하였습니다.

( Release 0.2.2 version )
- 다음 문서를 추가하였습니다
   doc/tools/GMO-Converter-Korean.pdf
   doc/tools/GMO-Graphics_Designers_Guideline-Korean.pdf
   doc/format/3D_Format-Overview-Korean.pdf
   doc/format/3D_Format-Command_Reference-Korean.pdf
   doc/format/3D_Format-Block_Reference-Korean.pdf

< GmoConv.exe >
- 다음의 블록, 커맨드를 추가하였습니다.
  BlindBlock   :블라인드 데이터 블록 ( chunk-id 0x000f )
  BlindData    :블라인드 데이터 커맨드 ( chunk-id 0x00f1 )

- 다음의 정의 커맨드를 추가하였습니다.
  DefineEnum    :열거 상수 정의
  DefineBlock   :블록 정의
  DefineCommand :커맨드 정의

- RenderState 커맨드에 다음과 같은 패러미터를 추가하였습니다.
  CULL_FACE     :뒷면 컬링 ( state-id 0x0003 )
  DEPTH_TEST    :깊이 테스트 ( state-id 0x0004 )
  DEPTH_MASK    :깊이 쓰기 ( state-id 0x0005 )
  ALPHA_TEST    :알파 테스트 ( state-id 0x0006 )
  ALPHA_MASK    :알파 쓰기 ( state-id 0x0007 )

- 다음의 최적화 옵션을 추가하였습니다.
  --optimize_bone      :불필요한 본 삭제
  --sort_transparency  :드로잉 데이터를 투명도로 분류
  --freeze_patchuv     :패치의 UV 좌표 계산
  --freeze_texcrop     :텍스처 크롭 반영후 삭제
  --epsilon_translate  :위치의 허용 오차
  --epsilon_rotate     :회전의 허용 오차
  --epsilon_scale      :확대의 허용 오차
  --epsilon_misc       :기타 허용 오차

- 최적화 옵션, --frame_loop 및 --frame_rate에 다음과 같은 값을 
  추가하였습니다.

  --frame_loop default ( FrameLoop를 변경하지 않음 )
  --frame_rate default ( FrameRate를 변경하지 않음 )

- GMS 텍스트 형식 파일의 정점 웨이트의 최대 개수를 15에서 255로 
  증가시켰습니다.

- GMS 텍스트 형식 파일 입력시 백 슬래시를 포함하는 텍스처 파일명을 
  올바르게 처리할 수 없는 문제를 수정하였습니다.

- GMS 텍스트 형식 파일 입력시 Shift-JIS 코드를 포함하는 텍스처
  파일명을 올바르게 처리할 수 없는 문제를 수정하였습니다.

- 마테리얼(material)을 통합할 때 RenderState를 비교하도록
  변경하였습니다.

- 모프 형상의 개수가 8을 초과하는 경우, 하나의 형상씩 연속된 형식으로 
  출력하도록 변경하였습니다.

- dotXSI 입력시 반복한 NurbsSurface를 양 끝의 제어점을 공유하지 않고 
  출력하도록 변경하였습니다.

- dotXSI 입력시 모핑의 베이스 형상의 텍스처 좌표 및 정점 컬러가
  저장되지 않는 문제를 수정하였습니다.

- dotXSI 3.6의 상수 쉐이드(constant shade) 지정에 대응하였습니다.

- dotXSI 3.6에서 복수의 텍스처 좌표 세트가 작성된 경우 텍스처 좌표가
  올바르게 변경되지 않는 문제를 수정하였습니다.

< GmoView.exe >
- 디버그 표시 모드를 추가하였습니다.
  자세한 내용은 위 사용 방법의 도움말을 참조하시기 바랍니다.

- 다음과 같은 RenderState 커맨드의 패러미터에 대응하였습니다.
  CULL_FACE   뒷면 컬링
  DEPTH_TEST  깊이 테스트
  DEPTH_MASK  깊이 쓰기
  ALPHA_TEST  알파 테스트
  ALPHA_MASK  알파 쓰기

- 하나의 형상씩 연속된 형식으로 출력된 모핑 모델을 재생할 수 있도록
  변경하였습니다.

- 창 크기를 +/- 키로 조정할 수 있도록 변경하였습니다.

- 16bit 256entry CSM1 CLUT 형식의 TM2 파일을 로드하였을 때 이상
  종료되는 문제를 수정하였습니다.

<GMO-Converter>
- 「1.들어가기 전에」의 「설치」에서 뷰어 배경 데이터에 대한 내용을
  추가하였습니다.

- 「3.옵션」의 「직접 옵션」에서 다음의 최적화 옵션에 값 default에 대한
  내용을 추가하였습니다. 
  frame_loop
  frame_rate

- 「3.옵션」의 「직접 옵션」에서 최적화 옵션에 다음 옵션들에 대한
  내용을 추가하였습니다.
    optimize_bone
    sort_transparency
    freeze_patchuv
    freeze_texcrop
    epsilon_translate
    epsilon_rotate
    epsilon_scale
    epsilon_misc

<3D_Format-Overview>
- 「2.데이터 구조」의 「커맨드」 및 「바이너리 형식」에서 데이터 자료형
  설명에 다음의 내용을 추가하였습니다.
    u_char
    u_short
    u_int

<3D_Format-Block_Reference>
- BlindBlock 블록에 대한 내용을 추가하였습니다.

- File 블록의 「커맨드」에서 다음 커맨드에 대한 내용을 추가하였습니다.
    DefineEnum
    DefineBlock
    DefineCommand

- Model 블록의 「커맨드」에서 VertexOffset에 대한 내용을 추가하였습니다.

- Bone 블록의 「커맨드」에서 MorphIndex에 대한 내용을 추가하였습니다.

- Arrays 블록의 「데이터 형식」에서 정점 컬러에 대한 내용을
  추가하였습니다.
 
- FCurve 블록의 「패러미터」에서 다음 상수에 대한 내용을 추가하였습니다.
  또, 「데이터 형식」에서 HERMITE 보간 및 CUBIC 보간에 대한 내용을
  추가하였습니다.
    HERMITE
    CUBIC

<3D_Format-Command_Reference>
- 다음의 정의 커맨드에 대한 내용을 추가하였습니다.
    DefineEnum
    DefineBlock
    DefineCommand

- 다음의 공통 커맨드에 대한 내용을 추가하였습니다.
    VertexOffset
    MorphIndex

- BlindData 커맨드에 대한 내용을 추가하였습니다.

- MorphWeights의 「해설」에서 MorphIndex에 대한 내용을 추가하였습니다.

- RenderState의 「패러미터」에서 드로잉 상태의 상수를 다음과 같이
  변경하였습니다.
  삭제:    ENABLE_LIGHTING
           ENABLE_FOG
  추가:    LIGHTING
           FOG
           CULL_FACE
           DEPTH_TEST
           DEPTH_MASK
           ALPHA_TEST
           ALPHA_MASK

- Animate의 「해설」에서 조작 대상 커맨드의 내용에서 TexFactor를
  삭제하고 다음의 커맨드에 대한 내용을 추가하였습니다.
    MorphIndex
    Ambient

- 다음 커맨드의 「패러미터」에서 인덱스의 자료형을 short에서 u_short로
  변경하였습니다.
    DrawArrays
    DrawParticle
    DrawBSpline
    DrawRectMesh
    DrawRectPatch

- SetTexture의 「해설」에서 주의 사항을 삭제하였습니다.

- BlendFunc의 「패러미터」에서 모드의 상수 MIX를 삭제하고 다음 상수에
  대한 내용을 추가하였습니다.
    ADD
    SUB
    REV

- BlendFunc의 「패러미터」에서 계수의 해설에 다음 상수의 내용을
  추가하였습니다.
    ZERO
    ONE

----------------------------------------------------------------------
사용 허락 및 제한
----------------------------------------------------------------------
본 소프트웨어의 사용 허락 및 사용 제한 등은 귀사와 당사(Sony Computer 
Entertainment Inc.) 간에 체결되어 있는 계약에 준합니다.

----------------------------------------------------------------------
상표에 관한 주의 사항
----------------------------------------------------------------------
"PSP" is a trademark of Sony Computer Entertainment Inc.  

All other product and company names mentioned herein, with or without 
the registered trademark symbol (R) , trademark symbol (TM) 
or service mark symbol (SM) , are generally trademarks and/or registered 
trademarks of their respective owners. 

SOFTIMAGE(R)|XSI(R) FTK

SOFTIMAGE, Avid, XSI and dotXSI are either registered trademarks 
or trademarks of Avid Technology, Inc. in the United States and/or 
other countries.


Autodesk(R) Crosswalk(R)

Autodesk, Crosswalk, and Softimage are registered trademarks or 
trademarks of Autodesk, Inc., and/or its subsidiaries and/or 
affiliates in the USA and/or other countries. All other brand names, 
product names, or trademarks belong to their respective holders. 

THIS SOFTWARE CONTAINS AUTODESK(R) CROSSWALK CODE 
DEVELOPED BY AUTODESK, INC.
(c) 2009 AUTODESK, INC.  ALL RIGHTS RESERVED.


Autodesk(R) FBX(R) SDK

Autodesk and FBX are registered trademarks or trademarks of 
Autodesk, Inc., in the USA and/or other countries.
All other brand names, product names, or trademarks belong to
their respective holders. 
(c) 2009 Autodesk, Inc. All rights reserved.

This software contains Autodesk(R) FBX(R) code developed by Autodesk, Inc.
Copyright 2009 Autodesk, Inc.  All rights, reserved.

----------------------------------------------------------------------
저작권에 대하여
----------------------------------------------------------------------
본 소프트웨어의 저작권은 Sony Computer Entertainment Inc.에 있습니다.
