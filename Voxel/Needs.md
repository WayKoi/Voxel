### Basic Needs

- Chunk Drawing
- Block Culling

### Wants

- 2D UI system
- Some way of drawing voxel objects that have cubes smaller than the terrain


### Ideas

- Make the chunks use bitwise operators for meshing
	- maybe dont use meshing, see if that is laggy or fine with very large worlds
- Make cubes have types but the colours of each can be slightly different than eachother
	- basically make types be more descriptive
	- Also include a material, like the specular, diffuse and ambient of the cube
- Make the meshing not care about cube type by meshing them together and using a texture or multiple for colour / lightihg calculations
- Make chunks able to scale
	- World as well
- Add ambient occlusion for cubes
- Use bitwise operators for block face culling
- make transparent voxels work
- Allow for loading of voxel files
	- Make own standard
	- Make tool for creating the files