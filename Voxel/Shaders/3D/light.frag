#version 330 core
out vec4 FragColor;

in vec3 FragPos;

uniform vec3 lightColour;

uniform vec3 viewPos;
uniform float farPlane;
uniform float FogDensity;
uniform vec3 FogColour;
uniform float FogStart;
uniform float FogEnd;

void main()
{
    // Fog calculations
    float end = farPlane * FogEnd;
    float FogStartDistance = farPlane * FogStart;
    float camDist = length(FragPos - viewPos);
    float distRatio = 4.0 * max(camDist - FogStartDistance, 0) / (end - FogStartDistance);
    float FogFactor = exp(-distRatio * FogDensity);

    FragColor = mix(vec4(FogColour, 1.0), vec4(lightColour, 1), FogFactor);
}