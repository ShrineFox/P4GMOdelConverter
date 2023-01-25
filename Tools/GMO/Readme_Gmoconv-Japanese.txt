[SCE CONFIDENTIAL DOCUMENT]
PSP(TM) GMO Converter version 1.51
                   Copyright (C) 2011 Sony Computer Entertainment Inc.
                                                   All Rights Reserved
======================================================================
このパッケージは、"PSP" 用 3D モデルデータを扱うために必要なコンバータ、
ビューア、ドキュメントが含まれています。

[gmoconv ver 1.51]
gmoconv は 3D モデルコンバータで、一般的な 3D モデルデータを "PSP" 用 
3D モデルデータに変換します。

[gmoview ver 1.51]
gmoview は 3D モデルビューアで、gmoconv で作成した "PSP" 用 3D モデ
ルデータを PC 上で確認することができます。

----------------------------------------------------------------------
パッケージ構成
----------------------------------------------------------------------
devkit/
|---tool
|     +--- gmoconv
|          |--- Readme_Gmoconv-Japanese.txt  :このファイル
|          |--- GmoConv.exe                  :コンバータ
|          |--- GmoView.exe                  :ビューア
|          |--- GmoConv.cfg                  :設定ファイル
|          |--- GmoConv.def                  :ユーザー定義ファイル
|          |--- GxoTool.dll                  :コンバータ基本モジュール
|          |--- data/                        :ビューア背景データ
|          |--- ImportXSI.dll                :XSI インポータモジュール ( Crosswalk 5.1 )
|          |--- ImportXSI_F363.dll           :XSI インポータモジュール ( XSIFtk 3.6.3 )
|          |--- ImportFBX.dll                :FBX インポータモジュール ( FBXSDK 2009.1 )
|          |--- ImportFBX_2005.dll           :FBX インポータモジュール ( FBXSDK 2005.12a )
|          |--- lib/                         :コンバータ拡張モジュール
|          |--- msvcp71.dll
|          |--- msvcr71.dll
|          |--- Crosswalk_5.1.32.dll         :Autodesk(R) Crosswalk(R) 5.1 dll
|          |--- test/                        :テストデータ
|          +--- src
|                |--- GmoConv
|                +--- GmoView
+---document
     |---format            :データフォーマットドキュメント
     |    |---GMO_Format-Overview-Japanese.pdf
     |    +---GMO_Format-Reference-Japanese.pdf
     +---tool             :概要ドキュメント
           |---GMO-Converter-Japanese.pdf
           +---GMO-Graphics_Designers_Guideline-Japanese.pdf

----------------------------------------------------------------------
注意事項
----------------------------------------------------------------------
< pdf ファイルについて >
- このパッケージに含まれるドキュメントは Adobe Acrobat 5.0以上、
  Adobe Acrobat Reader 5.0 以上でご覧いただけます。
  最新版 Adobe Reader(旧名Adobe Acrobat Reader) につきましては Adobe 
  のホームページよりダウンロード可能です。

< Crosswalk の使用について >
- 環境によりVisual C++ 2008 ランタイムのインストールが必要な場合があります。
  Visual C++ 2008 ランタイムが必要な場合には、以下のパッケージ
 （VC++ 2008 SP1 Redistributable Package）をダウンロードしてください。

  http://www.microsoft.com/downloads/details.aspx?familyid=a5c84275-3b97-4ab7-a40d-3802b2af5fc2&displaylang=en

  注記：
  上記の参照先URLについて、2011/09/26 時点で参照できることを確認しています。
  その後ページが移動したり内容が変更されている可能性がありますので
  ご注意ください。

----------------------------------------------------------------------
使用方法
----------------------------------------------------------------------
- gmoconv ver1.51

  usage:
    gmoconv <入力ファイル(*.xsi,*.gms)> [オプション]

  options:
    -interact     : 追加オプションをキー入力
    -models       : 複数の「モデル」として１つのファイルに結合
    -motions      : 複数の「モーション」として１つのファイルに結合
    -textures     : 複数の「テクスチャ」として１つのファイルに結合
    -prompt       : つねにキー入力待ちをおこなう
    -warning      : 警告時にキー入力待ちをおこなう
    -error        : エラー時にキー入力待ちをおこなう
    -viewer       : 終了時にGmoView.exeを起動する
    -o <filename> : 出力ファイル名を指定する
    -s <scale>    : 形状データをスケーリングする
    -t <scale>    : モーションの長さをスケーリングする
    -r <fps>      : モーションのフレームレートを指定する
    -O, -O2       : 最適化をおこなう
    -S            : テキスト形式を出力する
    -H            : ヘッダファイルを出力する
    -Z            : サイズの小さい頂点フォーマットを使用する
    -Q            : 非インデクス頂点プリミティブを使用する

- gmoview ver1.51

  usage:
    gmoview <入力ファイル(*.gmo)> [options]

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
Release 1.51 の変更点
----------------------------------------------------------------------
<GmoConv.exe>
- 頂点ウェイトが省略されたデータを入力できるよう、
  CompVertexWeightプロシージャの動作を修正しました。

- 以下のプロシージャを追加しました
    RemoveSingleWeight

- 以下の直接オプションを追加しました
    remove_single_weight

<GmoView.exe>
- 頂点ウェイトが省略されたデータを表示できるようにしました。

<GMO-Converter-Japanese.pdf>
- 以下の直接オプションの記述を追加しました。
    remove_single_weight

- 以下のプロシージャの記述を追加しました。
    RemoveSingleWeight

----------------------------------------------------------------------
Release 1.50 の変更点
----------------------------------------------------------------------
<GmoConv.exe>
- 設定ファイルを修正しました。最適化オプション -O/-O2 が指定されたとき、
  頂点の統一もおこなうようにしました ( unify_vertex = on )

- 複数モーションをマージする時に、ターゲット名を比較するようにしました。
  ターゲット名が異なる場合は、番号をもとにターゲット名を補正します。

- 複数モーションをマージする時に、ベースポーズを比較するようにしました。
  ベースポーズが異なる場合は、追加のアニメーションカーブを出力します。

- バッチファイル内で複数ファイルをマージできない不具合を修正しました。

- ポリゴンエッジ情報を出力する時に、複数のメッシュを含むパートが
  正しく変換されない不具合を修正しました。

- 頂点ウェイトを削減する時に、スキニングとモーフィングを併用する
  メッシュが正しく変換されない不具合を修正しました。

- GMO バイナリ形式を出力する時に、精度不足のために重複してしまう
  16ビットアニメーションのキーフレームを削除するようにしました。

- FBX ファイル読み込み時に、Maya ASCII ファイルを併用する場合に
  24/25/30/48/50/60 以外のフレームレートを使用できるようにしました。

- XSI ファイル読み込み時に、SoftImage のカスタムパラメータで
  レンダステートなどを設定できるようにしました。

- XSI ファイル読み込み時に、複数のテクスチャプロジェクションが
  設定されたテクスチャが正しく変換されない不具合を修正しました。

- XSI モジュールを CrossWalk 5.1 でビルドするようにしました。
  従来のモジュールは ImportXSI_F363.dll に改名しました。

- 以下の直接オプションを追加しました
    match_motion_target
    match_motion_basepose
    xsi_check_custom_param
    xsi_custom_param_transform
    xsi_custom_param_material
    xsi_custom_param_userdata

<GmoView.exe>
- ビューモードで、モデルの頂点数と頂点形式を表示するようにしました。

- テクスチャステート ( TexFunc TexFilter TexWrap ) を反映できるようにしました。
  互換性のため、デフォルトではこのパラメータが反映されないようになっています。
  反映するには "F" キーを押すか、"-TEXST" オプション を指定してください。

<GMO-Converter-Japanese.pdf>
- XSI ファイルのカスタムパラメータの記述を追加しました。

- 以下の直接オプションの記述を追加しました。
    match_motion_target
    match_motion_basepose
    xsi_check_custom_param
    xsi_custom_param_transform
    xsi_custom_param_material
    xsi_custom_param_userdata

----------------------------------------------------------------------
Release 1.47 の変更点
----------------------------------------------------------------------
<GmoConv.exe>
- バッチファイルを入力ファイルとして指定できるようにしました。

- ポリゴンエッジ情報を出力するオプションを追加しました ( rebuild_faceedge2 )。

- 頂点ウェイトを最適化するオプションを追加しました ( blend_cost_mesh )。

  メッシュ分割のコストを（例えば "300"など）数値で指定することで
  頂点ウェイトが適切に削減され、GPU 負荷が軽減されます。

- 出力オプションによりブロック出力がオフの場合に、名前参照のために
  空のブロックを出力するオプションを追加しました ( output_anchor )。

- FBX モジュールを FBXSDK 2010.2 でビルドできるように修正しました。

- FBX モジュールが Maya の Rigid Bind に対応しました。

- XSI モジュールが以下のパラメータに対応しました。
    dotXSI 5.0 形式のピボットポジション
    dotXSI 3.5 形式のマテリアルアニメーション
    dotXSI 3.0 形式のトランスペアレンシアニメーション

- 以下のプロシージャを追加しました。
    RebuildFaceEdge2

- 以下の直接オプションを追加しました
    batch_extension
    rebuild_faceedge2
    blend_cost_mesh
    blend_cost_matrix
    blend_cost_weight
    blend_cost_strip
    output_anchor

<GmoView.exe>
- 頂点ウェイト数など GPU 負荷に関連する情報を表示するようにしました。
  確認するには "4" キーを押して情報ページを表示してください。
  （ただし GPU 負荷の数値は概算です）

<GMO-Converter-Japanese.pdf>
- バッチファイルの使用についての記述を追加しました。

- 変換可能なパラメータについての記述を追加しました。

- XSI ファイルの変換についての記述を追加しました。

- 以下の直接オプションの記述を追加／修正しました。
    batch_extension
    rebuild_faceedge
    rebuild_faceedge2
    blend_cost_mesh
    blend_cost_matrix
    blend_cost_weight
    blend_cost_strip
    output_anchor

- 以下のプロシージャの記述を追加／修正しました。
    LimitVertexBlend
    RebuildFaceEdge
    RebuildFaceEdge2

<GMO_Format-Reference-Japanese.pdf>
- 以下のコマンドの記述を追加しました。
    EdgeFlags
    EdgeFaces

----------------------------------------------------------------------
Release 1.46 の変更点
----------------------------------------------------------------------
<GmoConv.exe>
- FBX ファイル読み込み時に、ピボットが読み込まれない不具合を修正しました。

- FBX ファイル読み込み時に、マテリアルの透明度テクスチャがあるとき、
  マテリアルの透明度カラーを無視するようにしました。

- FBX ファイル読み込み時に、Maya ASCII の追加アトリビュートで
  ボーン属性を設定できるようにしました。

- GIM コンバータが GMO コンバータの隣のフォルダに格納されていれば、
  画像ファイルを自動的に GIM 形式にコンバートするようにしました。
  詳細は、GMO コンバータドキュメント内の「2.使い方 -> テクスチャについて」を
  参照ください。

- 以下の直接オプションを追加しました
    image_extension2
    image_converter
    fbx_filter_transp
    fbx_maya_notes_transform

<GMO-Converter-Japanese.pdf>
- テクスチャの変換についての記述を修正しました。

- FBX ファイルの変換についての記述を修正しました。

- 以下の直接オプションの記述を追加／修正しました。
    image_extension
    image_extension2
    image_converter
    fbx_filter_transp
    fbx_maya_notes_transform

----------------------------------------------------------------------
Release 1.45 の変更点
----------------------------------------------------------------------
<GmoConv.exe>
- 接続面が複数辺を共有する場合に三角形ストリップが正しく変換されない
  不具合を修正しました。

- GMO バイナリ形式から16ビットアニメーションが正しく読み込まれない
  不具合を修正しました。

- 特定ブロックだけを出力する場合の便宜のため、出力オプションの値に
  "only" を指定できるようにしました(output_xxxx)。

- 一つのメッシュが複数の頂点ブロックを参照するのを回避するための
  オプションを追加しました(unify_mesh, unweld_mesh)。

- FBX ファイル読み込み時に、Maya ASCII の追加アトリビュートで
  レンダステートなどを設定できるようにしました(fbx_maya_notes_material)。

- FBXSDK 2009.1 でビルドした FBX モジュールを同梱しました(ImportFBX.dll)。
  これを使用することで NURBS サーフェスの変換が若干改善します。
  従来のモジュールは ImportFBX_2005.dll に改名しました。

- CrossWalk 3.3 でビルドした XSI モジュールを同梱しました(ImportXSI_CW33.dll)。
  これを使用することで dotXSI 6.0 形式の変換が若干改善します。
  ただしスケルトンやインスタンス等、一部未対応の機能もあります。

- FBX/XSI ファイル等のテストデータを同梱しました (test/)

- 以下の直接オプションの値を追加しました。
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

- 以下の直接オプションを追加しました。
    unweld_mesh
    fbx_output_root
    fbx_maya_notes_material
    fbx_maya_notes_userdata

- 以下の直接オプションを廃止しました。
    fbx_maya_notes_prefix

<GmoView.exe>
- GMO 以外のファイルをコンバートして表示するようにしました。

- メニューの文字をテクスチャ画像で表示するようにしました。

<GMO-Converter-Japanese.pdf>
- FBX ファイルの変換についての説明を追加しました。

- 以下の直接オプションの記述を追加／修正しました。
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

- 以下のプロシージャの記述を追加／修正しました。
    UnifyMesh
    UnweldMesh

----------------------------------------------------------------------
Release 1.44 の変更点
----------------------------------------------------------------------
<GmoConv.exe>
- テキスト形式で Animate コマンドの引数に INDEX0 OFFSET0 を指定できる
  ようにしました。

- 影響ボーン数が８を超えるスキンメッシュからエッジメッシュを生成した
  場合、アニメーションが正しく再生されない不具合を修正しました。

- XSI 読み込み時に、アニメーションの開始終了時刻が XYZ 各要素で異なる
  場合、誤った接線ハンドルが追加されることがある不具合を修正しました。

- FBX 読み込み時に、Maya ASCII からテクスチャアニメーションを取得する
  場合、Offset/Repeat アニメーションが存在しなければ、
  TranslateFrame/Coverageアニメーションを取得するようにしました。

- 解決されない参照をもつコマンドを削除するプロシージャを追加しました。
  この機能を使用するには "--remove_unresolved on" を指定してください。

- 最新 SDK でビルドした XSI モジュールを同梱しました。
  これを利用するには GmoConv.cfg を修正してください。
  ( load "ImportXSI" -> load "ImportXSI_CW32" )

<GMO-Converter-Japanese.pdf>
- 以下の直接オプションの記述を追加しました。
  remove_unresolved

- 以下のプロシージャの記述を追加しました。
  RemoveUnresolved

----------------------------------------------------------------------
Release 1.43 の変更点
----------------------------------------------------------------------
<GmoConv.exe>
- VC++9 でビルドできるよう修正しました。

- キーフレーム削減後に不要な関数カーブを削除できるようにしました。
  この機能を有効にするには "--epsikon_static 0.0" を指定してください。

- FBX 読み込み時にフレームレートを FBX から取得できるようにしました。
  この機能を有効にするには "--fbx_time_mode on" を指定してください。

- テキスト形式内部でコンバートオプションを指定できるようにしました。
  ConvertOption コマンドを使用してください。

- FBX 読み込み時にアニメーションの最後のフレームが出力されない不具合を
  修正しました。

- FBX 読み込み時に Maya ASCII から以下のパラメータが取得できない不具合を
  修正しました。
  - texture animation ( Repeat UV, Offset )
  - extrapolation ( Pre/Post Infinity )

- 以下の直接オプションを追加しました。
  --epsilon_static
  --fbx_time_mode
  --source_define
  --source_option
  --output_define
  --output_option

- 最新 SDK でビルドした XSI/FBX モジュールを同梱しました。
  - ImportXSI_CW26.dll ( Crosswalk 2.6 )
  - ImportFBX_2009.dll ( FBXSDK 2009.1 )
  これらを利用するには GmoConv.cfg を編集してください。
  - load "ImportXSI_CW26"
  - load "ImportFBX_2009"

<ドキュメントファイル>
<GMO-Converter-Japanese.pdf>
- 以下の直接オプションの記述を追加しました。
  epsilon_static
  fbx_time_mode
  source_define
  source_option
  output_define
  output_option

- 以下のプロシージャの記述を修正しました。
  RebuildKeyFrame

<GMO_Format-Reference-Japanese.pdf>
- 以下の定義コマンドの記述を追加しました。
  ConvertOption

----------------------------------------------------------------------
Release 1.42 の変更点
----------------------------------------------------------------------
<GmoConv.exe>
- VC++8 でビルドできるよう修正しました。

- マテリアル統合時にアニメーションされたマテリアルを統合しないように
  しました。

- FBX 読み込み時にマテリアル名とテクスチャ名をブロック名に反映するように
  しました。

- FBX 読み込み時に頂点カラーがゼロならば 0xffffffff として出力するように
  しました。

- FBX 読み込み時に Maya ASCII から以下のパラメータを取得するようにしました。
  - frame rate
  - frame loop
  - material animation ( Color, Transp, etc )
  - texture animation ( Repeat UV, Offset )
  - extrapolation ( Pre/Post Infinity )
  - transform notes
  - material notes

- 以下の直接オプションを追加しました。
  --fbx_use_material_name
  --fbx_use_texture_name
  --fbx_default_vcolor
  --fbx_maya_notes_prefix

<GmoView.exe>
- マテリアルの初期アルファ値がゼロの時、透明度アニメーションが機能しない
  不具合を修正しました。

----------------------------------------------------------------------
Release 1.41 の変更点
----------------------------------------------------------------------
<GmoConv.exe>
- 未定義の直接オプションが指定されると警告します。

- FBX のインポート時に Maya ASCII をチェックします。
  テクスチャアニメーションをそちらから取得します。

- 以下の直接オプションを追加しました。
  check_direct_option
  fbx_check_maya_ascii

----------------------------------------------------------------------
Release 1.40 の変更点
----------------------------------------------------------------------
<GmoConv.exe>
- GMP ファイルを出力できるようになりました。GMP ファイルは
  Graphics Engine のパケットを格納したファイル形式です。
  使用法はサンプルプログラム model6 を参照してください。

- GMP ファイルにテクスチャイメージを格納できるようになりました。
  画像ファイルが GIM 形式のときテクスチャパケットが生成されます。

- GMP ファイルに複数のモーションを格納できるようになりました。
  ルートオブジェクトの子としてモーションオブジェクトが生成されます。

- LINUX 上で拡張モジュールが正しく動作しない不具合を修正しました。

- 内部で使用しているライブラリに namespace を付加しました。
  内部で使用しているライブラリのバージョンを更新しました。
  そのため、古い拡張モジュールは再コンパイルが必要です。

----------------------------------------------------------------------
Release 1.30 の変更点
----------------------------------------------------------------------
<GmoConv.exe>
- CompFCurveChannel プロシージャを修正しました。
  部分要素アニメーションが正しく変換されない不具合を修正しました。

- RebuildBoundingPoints プロシージャを修正しました。
  計算誤差により NaN が出力される不具合を修正しました。
  モードとして "obb", "aabb" を指定できるようになりました。

- GXP ファイルを出力できるようになりました。
  GXP ファイルは、Graphics Engine のパケットを格納したファイル形式です。
  詳しくはサンプルプログラム model6 を参照してください。

- 以下のコマンドを追加しました。
  MeshType
  MeshLevel
  TexGen
  TexMatrix

- 以下の RenderState モードを追加しました。
  FLIP_FACE
  FLIP_NORMAL

- 以下のオプションを追加しました。
  -levels

- 以下の直接オプションを追加しました。
  rebuild_line
  rebuild_linestrip
  rebuild_faceedge

- 以下のプロシージャを追加しました
  RebuildLine
  RebuildLineStrip
  RebuildFaceEdge

<GmoView.exe>
- メッシュマスクに対応しました。

- テクスチャ座標生成に対応しました。

- RenderState モード FLIP_FACE FLIP_NORMAL を追加しました。

- RenderState コマンドをメッシュブロックで使用できるようになりました。


----------------------------------------------------------------------
Release 1.20 の変更点
----------------------------------------------------------------------
<GmoConv.exe>
- ImportXSI モジュールを修正しました。
  VC++.NET でビルドできます。

- ImportFBX モジュールを追加しました。
  FBX ファイルをインポートできます。

- RebuildTriStrip プロシージャを修正しました。
  ストリップ化処理を改善しました。

- RebuildRectPatch プロシージャを追加しました。
  BSpline を RectPatch に変換します。

- UnifyMesh プロシージャを追加しました。
  同じアトリビュートをもつメッシュを結合します。

- UnifyArrays プロシージャを追加しました。
  同じフォーマットをもつ頂点データを結合します。

- 以下の直接オプションを追加しました。
  fbx_filter_fcurve
  rebuild_rectpatch
  unify_mesh
  unify_arrays

- LimitVertexBlend プロシージャの呼び出し箇所を移動しました。

<GmoView.exe>
- 行順序が反転した BMP ファイルを使用できるようにしました。

----------------------------------------------------------------------
Release 1.11 の変更点
----------------------------------------------------------------------
<GmoConv.exe>
- 縮退トライアングルを含むメッシュが正しくストリップ化されない
  不具合を修正しました。

<GmoView.exe>
- 外部テクスチャが正しく表示されない不具合を修正しました。

----------------------------------------------------------------------
Release 1.10 の変更点
----------------------------------------------------------------------
<GmoConv.exe>
- バイナリファイル出力時にブロック名が出力されない不具合を
  修正しました。

- バイナリファイル入力時にパディングの内容が不定になる不具合を
  修正しました。

- バイナリファイル入力時にモーフィングが正しく入力されない不具合を
  修正しました。

- dotXSI ファイルのエンベロープを持つインスタンスに対応しました。

- オブジェクトカリングに対応しました。( BoundingPoints コマンド )

- Ｚソートモード情報に対応しました。( BoneState コマンド )

- 16bit アニメーションに対応しました。

- 整数頂点データのオフセットを指定できるようにしました。

- バウンディングボックスの対象を指定できるようにしました。

- 以下のデータ定義を追加しました。
    BoundingPoints
    BoneState

- 以下の直接オプションを追加、修正しました。
    rebuild_bounding
    rebuild_bounding_points
    format_keyframe
    offset_vertex
    offset_tcoord

- SortByTransparency プロシージャの動作を変更しました。
  ボーンのソートを行わず、半透明ボーンに BoneState コマンドを追加します。

- freeze_patchuv 直接オプションの設定ファイルの値を変更しました。
  デフォルトで曲面プリミティブにテクスチャ座標を追加します。

<GmoView.exe>
- DXT1 1bit alpha mode の判定条件の誤りを修正しました。

- 内部のライブラリを更新しました。（devkit 2.0.0 と同じもの）

- 16bit アニメーションデータに対応しました。

- モーションの追加ロードに対応しました。
 （コントロールキー＋ファイルドロップ）

- モーションの一時停止対応しました。（Tab キー）

< ドキュメントファイル >
- 3D_Format-Overview から GMO_Format-Overview にファイル名を
  変更しました。

- 3D_Format-Command_Reference 及び 3D_Format-Block_Reference を
  結合して1つのファイルにし、ファイル名を GMO_Format-Reference に
  変更しました。

----------------------------------------------------------------------
Release 1.00 の変更点
----------------------------------------------------------------------
<GmoConv.exe>
- 整数頂点データ使用時に法線ベクトルの方向が歪まないよう VertexOffset の
  スケール値の計算を修正しました。

- dotXSI ファイルの変換で下位階層をもつノードのインスタンスに対応しました。

- 設定ファイルの書式を変更しました。

- 使用法ファイル(gmoconv.usg)を設定ファイル(gmoconv.cfg)に統合しました。

- バイナリ形式(*.GMO)の入力に対応しました。

- -Q オプションを追加しました。非インデクス頂点プリミティブを使用します。

- format_index 直接オプションで独立頂点化をおこなわないようにしました。
  非インデクス頂点プリミティブを使用するには unweld_vertex 直接オプションを
  併用するか、-Q オプションを使用してください。

- sort_block、sort_command 直接オプションを廃止しました。

- sort_type 直接オプションを追加し、既定値を on にしました。

- last_weight 直接オプションを廃止しました。

- sort_transparency 直接オプションにブロック指定を追加しました。

- 以下の直接オプションを追加しました
    output_directory
    unify_vertex
    unweld_vertex
    remove_unused
    align_image
    xsi_ignore_prefix

- ソースコードファイルを追加しました。

<GmoView.exe>
- メトリクスモードを追加しました。データサイズや頂点数などを表示します。

- カラーダブルのエミュレーションに対応しました。(-cd オプション)

- アニメーションの外挿に対応しました。ただし REPEAT EXTEND モードは未対応です。

- FrameRepeat コマンドに対応しました。

- データフォルダ名を "gmoview" から "data" に変更しました。

- ソースコードファイルを追加しました。
  本バージョンのソースコードでは、曲面表示に未対応、及び光源OFF時に
  カラーが反映されない点の制限があります。

----------------------------------------------------------------------
Release 0.9.0 の変更点
----------------------------------------------------------------------
<GmoConv.exe>
- 整数頂点データの計算方法を切り捨てから四捨五入に変更しました。

- 整数頂点ウェイトの総和誤差を補正するように変更しました。

- コントロールキーを押しながら入力ファイルをドラッグ＆ドロップすると、
  コンソールにプロンプトを表示し、追加オプションを入力できるように
  変更しました。

- Scale3 コマンドに対応しました ( チャンクタイプ = 0x00e1 )
  このコマンドは Maya において "Segment Scale Compensate"
  を指定したスケーリングに相当します。
  ( ただし optimize_bone オプションによるボーン最適化は
  Scale3 コマンドに未対応です。 )

- 頂点ウェイトの数が１のモデルデータを GMS テキスト形式に変換してから
  さらにもう一度変換した場合に正しく変換されないという不具合を修正しました。

- last_weight 直接オプションを追加しました。
  このオプションは GMS テキスト形式で最後の頂点ウェイトの出力をスイッチします。
  （いずれの指定で出力された GMS テキスト形式も読み込むことができます。
  なお GMO バイナリ形式ではつねに最後の頂点ウェイトを出力します。）

- GMS テキスト形式で最後の頂点ウェイトをデフォルトで出力するように変更しました。
  以前のバージョンと同じ動作にするためには gmoconv.cfg ファイルの
  last_weight 直接オプションの値を off に変更してください。

<GmoView.exe>
- Scale3 コマンドに対応しました。

<GMO-Converter>
- オプションの設定方法の記述を変更しました。

- 出力オプションにlast_weightの記述を追加しました。

----------------------------------------------------------------------
Release 0.6.5 の変更点
----------------------------------------------------------------------
< GmoConv.exe >
- 入力ファイルとして２Ｄ画像データを指定した際に、
  テクスチャだけのモデルに変換するように変更しました。

- 複数の「テクスチャ」として１つのファイルに結合するための
  直接オプション値、および間接オプションを追加しました。
    --merge_mode texture
    -textures

- 以下のオプションを追加しました
    --format_fname     : FileName コマンドの形式を指定

< GmoView.exe >
- 128 バイトアラインされた形式の TM2 ファイルが正しく表示されない
  不具合を修正しました。

- テクスチャに画像ファイルが含まれないときのファイル検索において
  拡張子が .gim .tm2 .tga .bmp のファイルを優先するようにしました。

<GMO-Converter>
- 以下のオプションの記述を追加しました。
    -textures
    -O2

- merge_modeオプションに、値textureの記述を追加しました。

- 以下の出力オプションの記述を追加しました。
    format_index
    format_fname

----------------------------------------------------------------------
変更履歴
----------------------------------------------------------------------
( Release 0.6.0 version )
< GmoConv.exe >
- 以下の出力オプションを追加しました
  --format_index       :頂点インデクス形式を指定

< GmoView.exe >
- テクスチャに画像ファイルが含まれないとき、モデルと同じフォルダの
  画像ファイルをロードするように変更しました。

- 非インデクス描画プリミティブを表示できるように変更しました。

- S3TC 圧縮形式のテクスチャを表示できるように変更しました。

- Backspace キーで前のモデルをロードするように変更しました。

( Release 0.2.4 version )
< GmoConv.exe >
- 起動時、及び GMS ファイルのコンバート時に XSIFtk.dll ファイルを
  リンクしないように変更しました。

< GmoView.exe >
- ポリゴンに対するリフレクションマッピングが正しく表示されないという
  不具合を修正しました。

- 高速モードのテクスチャを表示できるように変更しました。

- 以下のオプションを追加しました。
    -fg <code>     : foreground color ( hex-code )
    -bg <code>     : background color ( hex-code )

( Release 0.2.3 version )
< GmoConv.exe >
- dotXSI の入力時に関数カーブのハンドル情報が正しく変換されない
  という不具合を修正しました。

- 頂点ウェイト数が 64 以上の時に正しく変換されないという不具合を
  修正しました。

< GmoView.exe >
- パッチの制御点が縮退している時、正しく光源計算されないという
  不具合を修正しました。

- HERMITE CUBIC 補間アニメーションにおいて、キーフレームの時刻で
  関数カーブが正しく計算されない不具合を修正しました。

( Release 0.2.2 version )
- 以下のドキュメントを追加しました。
   doc/tools/GMO-Converter-Japanese.pdf
   doc/tools/GMO-Graphics_Designers_Guideline-Japanese.pdf
   doc/format/3D_Format-Overview-Japanese.pdf
   doc/format/3D_Format-Command_Reference-Japanese.pdf
   doc/format/3D_Format-Block_Reference-Japanese.pdf

< GmoConv.exe >
- 以下のブロック、コマンドを追加しました。
  BlindBlock   :ブラインドデータブロック ( chunk-id 0x000f )
  BlindData    :ブラインドデータコマンド ( chunk-id 0x00f1 )

- 以下の定義コマンドを追加しました。
  DefineEnum    :列挙定数の定義
  DefineBlock   :ブロック定義
  DefineCommand :コマンド定義

- RenderState コマンドに以下のパラメータを追加しました。
  CULL_FACE     :裏面カリング ( state-id 0x0003 )
  DEPTH_TEST    :デプステスト ( state-id 0x0004 )
  DEPTH_MASK    :デプス書き込み ( state-id 0x0005 )
  ALPHA_TEST    :アルファテスト ( state-id 0x0006 )
  ALPHA_MASK    :アルファ書き込み ( state-id 0x0007 )

- 以下の最適化オプションを追加しました
  --optimize_bone      :不要なボーンを削除
  --sort_transparency  :描画データを透明度でソート
  --freeze_patchuv     :パッチのＵＶ座標をフリーズ
  --freeze_texcrop     :テクスチャクロップをフリーズ
  --epsilon_translate  :位置の許容誤差
  --epsilon_rotate     :回転の許容誤差
  --epsilon_scale      :拡大の許容誤差
  --epsilon_misc       :その他の許容誤差

- 最適化オプション --frame_loop、及び --frame_rate に以下のような
  値を追加しました。

  --frame_loop default ( FrameLoop を変更しない )
  --frame_rate default ( FrameRate を変更しない )

- GMS テキスト形式ファイルの頂点ウェイトの最大数を 15 から 255 
  に増加しました。

- GMS テキスト形式ファイルの入力時にバックスラッシュを含むテクスチャ
  ファイル名を正しく処理できないという不具合を修正しました。

- GMS テキスト形式ファイルの入力時に Shift-JIS コードを含むテクスチャ
  ファイル名を正しく処理できないという不具合を修正しました。

- マテリアルを統合する時に RenderState を比較するように変更しました。

- モーフ形状の数が 8 を超える場合、1 形状ずつ連続した形式で出力
  するように変更しました。

- dotXSI の入力時にループした NurbsSurface を両端の制御点を
  共有せずに出力するように修正しました。

- dotXSI の入力時に、モーフィングのベース形状のテクスチャ座標
  および頂点カラーが保存されないという不具合を修正しました。

- dotXSI 3.6 のコンスタントシェイド指定に対応しました。

- dotXSI 3.6 で複数のテクスチャ座標セットが作成された場合に、
  テクスチャ座標が正しく変換できないという不具合を修正しました。

< GmoView.exe >
- デバッグ表示モードを追加しました。
  詳細は、上記使用方法におけるヘルプを参照ください。

- 以下の RenderState コマンドのパラメータに対応しました。
  CULL_FACE   裏面カリング
  DEPTH_TEST  デプステスト
  DEPTH_MASK  デプス書き込み
  ALPHA_TEST  アルファテスト
  ALPHA_MASK  アルファ書き込み

- 1形状ずつ連続した形式で出力されたモーフィングモデルを再生できる
  ように変更しました。

- ウィンドウサイズを +/- キーでサイズ変更できるように変更しました。

- 16bit 256entry CSM1 CLUT 形式の TM2 ファイルをロードした際に
  異常終了するという不具合を修正しました。

<GMO-Converter>
- 「1.はじめに」の「インストール」において、ビュアー背景データについての
  記述を追加しました。

- 「3.オプション」の「直接オプション」において、以下の最適化オプションに
  値defaultについての記述を追加しました。
  frame_loop
  frame_rate

- 「3.オプション」の「直接オプション」において、最適化オプションに
  以下のオプションについての記述を追加しました。
    optimize_bone
    sort_transparency
    freeze_patchuv
    freeze_texcrop
    epsilon_translate
    epsilon_rotate
    epsilon_scale
    epsilon_misc

<3D_Format-Overview>
- 「2.データ構造」の「コマンド」および「バイナリ形式」において、データ型
  の解説に以下の記述を追加しました。
    u_char
    u_short
    u_int

<3D_Format-Block_Reference>
- BlindBlockブロックについての記述を追加しました。

- Fileブロックの「コマンド」において、以下のコマンドについての記述を
  追加しました。
    DefineEnum
    DefineBlock
    DefineCommand

- Modelブロックの「コマンド」において、VertexOffsetの記述を追加しました。

- Boneブロックの「コマンド」において、MorphIndexの記述を追加しました。

- Arraysブロックの「データ形式」において、頂点カラーについての記述を
  追加しました。

- FCurveブロックの「引数」において、以下の定数の記述を追加しました。
  また「データ形式」において、HERMITE補間およびCUBIC補間の記述を
  追加しました。
    HERMITE
    CUBIC

<3D_Format-Command_Reference>
- 以下の定義コマンドについての記述を追加しました。
    DefineEnum
    DefineBlock
    DefineCommand

- 以下の共通コマンドについての記述を追加しました。
    VertexOffset
    MorphIndex

- BlindDataコマンドについての記述を追加しました。

- MorphWeightsの「解説」において、MorphIndexについての記述を追加
  しました。

- RenderStateの「引数」において、描画ステートの定数を以下のように
  変更しました。
  削除:    ENABLE_LIGHTING
           ENABLE_FOG
  追加:    LIGHTING
           FOG
           CULL_FACE
           DEPTH_TEST
           DEPTH_MASK
           ALPHA_TEST
           ALPHA_MASK

- Animateの「解説」において、操作対象コマンドの記述からTexFactorを
  削除し、以下のコマンドの記述を追加しました。
    MorphIndex
    Ambient

- 以下のコマンドの「引数」において、インデクスの型をshortからu_shortに
  変更しました。
    DrawArrays
    DrawParticle
    DrawBSpline
    DrawRectMesh
    DrawRectPatch

- SetTextureの「解説」において、注意事項を削除しました。

- BlendFuncの「引数」において、モードの定数MIXを削除し、以下の定数の
  記述を追加しました。
    ADD
    SUB
    REV

- BlendFuncの「引数」において、係数の解説に以下の定数の記述を追加しました。
    ZERO
    ONE

----------------------------------------------------------------------
使用許諾・制限
----------------------------------------------------------------------
このソフトウェアの使用許諾、使用制限は貴社と当社(株式会社ソニー・
コンピュータエンタテインメント)との間に締結されている契約に準じます。

----------------------------------------------------------------------
商標に関する注意書き
----------------------------------------------------------------------
"PSP" は株式会社ソニー・コンピュータエンタテインメントの商標です。

パッケージ内の本文中に記載されている会社名、製品名、サービス名は、一般に
各社の商標または登録商標です。
なお、パッケージ内の本文中に (R)、(TM)、(SM)マークは明記していない場合が
あります。

This software contains Autodesk(R) Crosswalk code developed by 
Autodesk, Inc.
(C) 2009 Autodesk, Inc. ALL RIGHTS RESERVED.
Autodesk, Crosswalk, FBX, Maya, Softimage and 3ds Max are registered 
trademarks or trademarks of Autodesk, Inc., and/or its subsidiaries and/or 
affiliates in the USA and/or other countries.

----------------------------------------------------------------------
著作権について
----------------------------------------------------------------------
本ソフトウェアの著作権は、株式会社ソニー･コンピュータエンタテインメント
に帰属しています。

