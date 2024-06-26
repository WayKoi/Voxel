#version 330 core
out vec4 FragColor;

struct Globallight {
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

struct PointLight {
    vec3 position;

    vec3 diffuse;
    vec3 specular;

    float distance;
};

in vec3 FragPos;
in vec4 Colour;
in vec3 Normal;

uniform Globallight global;
uniform PointLight lights[100];
uniform int lightCount;

uniform vec3 viewPos;

uniform float farPlane;
uniform float FogDensity;
uniform vec3 FogColour;
uniform float FogStart;
uniform float FogEnd;

void main()
{
    float specularShine = 1;

    // global light
    // ambient
    vec3 ambient = global.ambient * vec3(Colour.x, Colour.y, Colour.z);

    // diffuse
    float diff = max(dot(Normal, global.direction), 0.0);
    vec3 diffuse = global.diffuse * diff * vec3(Colour.x, Colour.y, Colour.z);

    // Specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-global.direction, Normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), specularShine);
    vec3 specular = global.specular * spec * vec3(Colour.x, Colour.y, Colour.z);

    vec3 result = ambient + diffuse + specular;

    for (int i = 0; i < lightCount; i++) {
        float dist = distance(FragPos, lights[i].position);

        if (dist < lights[i].distance) {
            // diffuse
            vec3 lightdir = normalize(lights[i].position - FragPos);
            diff = max(dot(Normal, lightdir), 0.0);
            diffuse = lights[i].diffuse * diff * vec3(Colour.x, Colour.y, Colour.z);

            // specular
            reflectDir = reflect(-lightdir, Normal);
            spec = pow(max(dot(viewDir, reflectDir), 0.0), specularShine);
            specular = lights[i].specular * spec * vec3(Colour.x, Colour.y, Colour.z);

            float amount = (lights[i].distance - dist) / lights[i].distance;
            result = result + (diffuse + specular) * amount;
        }
    }

    // Fog calculations
    float end = farPlane * FogEnd;
    float FogStartDistance = farPlane * FogStart;
    float camDist = length(FragPos - viewPos);
    float distRatio = 4.0 * max(camDist - FogStartDistance, 0) / (end - FogStartDistance);
    float FogFactor = exp(-distRatio * FogDensity);

    FragColor = mix(vec4(FogColour, 1.0), vec4(result, Colour.w), FogFactor);
}