# P4GMOdelConverter
A tool for creating working P4G custom models
![](https://i.imgur.com/joWxiJ6.png)
## How to Use
1. Extract the MODEL_DATA from your P4G AMD/PAC file using [Amicitia](https://amicitia.github.io/post/amicitia).
2. Give it the .GMO file extension.
3. Open the .GMO with [P4GMOdelConverter](https://github.com/ShrineFox/P4GMOdelConverter/releases) and it'll generate 2 .mds files (hopefully)
   Note: Animations from the GMO will be included in the .mds files if the box is checked.
4. Open the generated ``_p4g.mds`` to create a new .GMO that should work in P4G.

# Features as of [Release 1.5.1](https://github.com/ShrineFox/P4GMOdelConverter/releases)
## Convert GMO/FBX model to MDS text file
- Output P4G-compatible MDS with meshes split up into their own "parts"
- (Optional) Convert FBX directly to GMO before MDS (may improve compatibility)
- (Optional) Exclude animations or textures/materials from P4G MDS (for testing geometry)
- (Optional) Extract TM2 texture files from GMO (for MDS to reference when rebuilding GMO)
- (Optional) Load animations from GMO for re-ordering/renaming/rebuilding
- (Optional) Rename underscores to spaces in bone names (except "_Bone")
- (Optional) Specify the bone weapons attach to for battle models
## Convert MDS text file to GMO model
- Auto-Convert non-TM2 textures referenced in MDS to TM2
- Output GMO rebuilt from input MDS
- Updates MDS file before converting with new animation names/order
- (Optional) Run generated GMO through TGE's tool to fix compatibility with P4G PC
- (Optional) Automatically view generated GMO in new GMOView window
## Re-order MDS animations on the fly
- Move animations up and down by name in animation list
- Update last generated/opened MDS files with new animation order
- Export current animation set to MDS file
- Load animation set from MDS file
- Choose a preset to auto-rename animations for P3P/P4G protags, party members and some bosses

## Latest Update (v1.5.1)
- Added auto-rename presets
- Corrected player_body_Bone name not being fixed
## Known Issues
- Card disappears for P4G party members
- Meshes weighted to Bip01 Head_Bone often don't work for yet unknown reasons
