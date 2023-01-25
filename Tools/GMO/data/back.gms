.GMS 1.00

Model "back" {
	Bone "bone-0" {
		DrawPart "part-0"
	}
	Part "part-0" {
		Mesh "mesh-0" {
			SetMaterial "material-0"
			DrawArrays "arrays-0" TRIANGLE_STRIP 4 1 0 1 2 3
		}
		Arrays "arrays-0" VERTEX|TEXCOORD 4 1 0 {
			-30000.000000 15000.000000 -10000.000000 0.000000 0.000000
			-30000.000000 -15000.000000 -10000.000000 0.000000 1.000000
			30000.000000 15000.000000 -10000.000000 1.000000 0.000000
			30000.000000 -15000.000000 -10000.000000 1.000000 1.000000
		}
	}
	Material "material-0" {
		RenderState LIGHTING 0
		RenderState DEPTH_TEST 0
		RenderState DEPTH_MASK 0
		Layer "layer-0" {
			SetTexture "texture-0"
		}
	}
	Texture "texture-0" {
		FileName "back.bmp"
	}
	Motion "motion-0" {
		FrameLoop 0.000000 20.000000
		FrameRate 1.000000
		Animate "Material::material-0" TexCrop 0 "fcurve-0"
		FCurve "fcurve-0" LINEAR 4 2 0 {
			0.000000 0.000000 0.000000 3.000000 3.000000
			20.000000 1.000000 -1.000000 3.000000 3.000000
		}
	}
}
