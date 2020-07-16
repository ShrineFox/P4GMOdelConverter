# P4GMOdelConverter
A tool for creating working P4G custom models

![](https://i.imgur.com/FX2zuV2.png)

## How to Use
1. Extract the MODEL_DATA from your P4G AMD/PAC file using [Amicitia](https://amicitia.github.io/post/amicitia).
2. Give it the .GMO file extension.
3. Open the .GMO with P4GMOdelConverter and it'll generate 2 .mds files (hopefully)
   Note: Animations from the GMO will be included in the .mds files if the box is checked.
4. Open the generated ``_p4g.mds`` to create a new .GMO that should work in P4G.


For more use cases, see [this guide on importing custom models](https://shrinefox.com/guides/2020/06/18/wip-importing-custom-models-in-p3p-p4g/).

## Features
## Convert GMO/FBX model to MDS text file
 - Outputs P4G-compatible MDS with meshes split up into thier own "parts"
 - (Optional) Convert FBX directly to GMO before MDS (may improve compatibility)
 - (Optional) Exlude animations or textures/materials from P4G MDS (for testing geometry)
 - (Optional) Loads animations from GMO
 - (Optional) Renames underscores to spaces in bone names (except "_Bone")
 
 Improves compatibility with GMO animations when converting from FBX 
 - (Optional) Rename the bone weapons attach to for battle models
 - (Optional) Extract TM2 textures from GMO (for the MDS to reference when converting back to GMO)
 ## Convert MDS text file to GMO model
 - Outputs GMO rebuilt from input MDS
 - Updates MDS file before converting with new animation order
 - (Optional) Run generated GMO through TGE's tool to fix compatibility with P4G PC
 - (Optional) Automatically view generated GMO in new GMOView window
 
 Might not work with P4G PC "fixed" models
## Re-order MDS animations on the fly
 - Move animations up and down by name in animation list
 - Update last generated/opened MDS files with new animation order
 - Export current animation set to MDS file
 - Load animation set from MDS file
