#version 330 core
out vec4 FragColor;

in vec3 FragPos;

uniform vec3 lightColour;

void main()
{
    FragColor = vec4(lightColour, 1.0);
}