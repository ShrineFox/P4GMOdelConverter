# P4GMOdelConverter
A tool for creating working P4G custom models

![](https://i.imgur.com/yvqOOBH.png)

Converts FBX and GMO models to MDS (an editable text format) and back.

Also corrects issues that prevent custom models from rendering correctly in P4G.

You can also optionally dump TM2 textures and quickly re-order animations.

## Usage
1. Extract the MODEL_DATA from your P4G AMD/PAC file using [Amicitia](https://amicitia.github.io/post/amicitia).
2. Give it the .GMO file extension.
3. Open the .GMO with P4GMOdelConverter and it'll generate 2 .mds files (hopefully)
   Note: Animations from the GMO will be included in the .mds files if the box is checked.
4. Open the generated ``_p4g.mds`` to create a new .GMO that should work in P4G.

For more instructions, see [this guide](https://shrinefox.com/guides/2020/06/18/wip-importing-custom-models-in-p3p-p4g/).
