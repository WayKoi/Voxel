# Overview

This is a Voxel Engine built in C# with OpenTK (The C# Wrapper of OpenGL)

# Features

This engine allows you to build voxel worlds one cube at a time.
It also has support for multi coloured lighting.

![A large cube with lots of different coloured lights surrounding it](screenshots/big-cube.png)

The world also allows you to generate terrain using your own functions

![A very hilly world with lights scattered about](screenshots/terrain.png)

You can also populate the world with structures, using the custom structure file.

This is an example of a structure file for a simple lamp post.

```
# This is the template for the structure files
# it starts with the cube type definitions
# static types look like this

# def static <R> <G> <B> <alpha> <shiny>
def static 0 0 0 1 1

# def light <R> <G> <B> <distance>
def light 1 0.98 0.956 30

# then the actual cubes will be defined afterwards
# c <index of type from defs above> <x> <y> <z>

c 0 0 0 0
c 0 0 1 0
c 0 0 2 0
c 0 0 3 0
c 0 0 4 0
c 0 0 5 0
c 0 0 6 0
c 0 1 6 0
c 0 0 6 1
c 0 -1 6 0
c 0 0 6 -1

# to add a point light to the structure you would do this
# l <index> <X> <Y> <Z>
# Note: the x y z coordinates can be float with lights

l 0 0 7 0
```

And here is a world with lots of them scattered about

![the same hilly world with lots of lamp posts scattered around](screenshots/structure.png)

You can also change the colour of the sky and fog in this world to give it a different feel.

![The same world as before but with pink fog and sky](screenshots/fog.PNG)

This world will also load as the game runs an once a chunk is out of view range it will be culled out.

![A gif showing the world loading chunks when the engine starts](screenshots/loading.gif)

# Optimizations

This world does cull all voxel faces that are obscured using bitwise operators

![A view under the terrain shows that the voxels under the surface are not drawn](screenshots/cull.png)
