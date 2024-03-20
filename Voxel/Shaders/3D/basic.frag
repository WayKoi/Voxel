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

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    int distance;
};

in vec3 FragPos;
in vec4 Colour;
in vec3 Normal;

uniform Globallight global;
uniform 
uniform vec3 viewPos;

void main()
{
    // ambient
    vec3 ambient = global.ambient * vec3(Colour.x, Colour.y, Colour.z);

    // diffuse
    float diff = max(dot(Normal, global.direction), 0.0);
    vec3 diffuse = global.diffuse * diff * vec3(Colour.x, Colour.y, Colour.z);

    // Specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-global.direction, Normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 16);
    vec3 specular = global.specular * spec * vec3(Colour.x, Colour.y, Colour.z);

    vec3 result = ambient + diffuse + specular;
    FragColor = vec4(result, 1.0);
}