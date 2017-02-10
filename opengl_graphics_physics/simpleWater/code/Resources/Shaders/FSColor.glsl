#version 330 core

in vec2 UV;
in vec3 Position_worldspace;
in vec3 Normal_worldspace;
in vec3 EyeDirection_worldspace;
in vec3 LightDirection_worldspace;

layout(location = 0) out vec4 color;

uniform sampler2D myTextureSampler;

uniform vec4 MaterialProperties;
uniform vec3 MaterialColor;

uniform vec2 screenSize;

uniform float lightPower;
uniform vec3 lightColor;

void main(){

	vec2 TexCoord = gl_FragCoord.xy / screenSize;
	// Material properties
	vec3 MaterialDiffuseColor = texture2D(myTextureSampler, UV).rgb + MaterialColor;

	vec3 normal = normalize( Normal_worldspace );

	// Direction of the light (from the fragment to the light)
	vec3 lightDirection = normalize(LightDirection_worldspace);

	float diffuseFactor = clamp(dot(normal, lightDirection), 0, 1);

	// Eye vector (towards the camera)
	vec3 vertexToCamera = normalize(EyeDirection_worldspace);
	// Direction in which the triangle reflects the light
	vec3 reflectedLightDir = reflect(-lightDirection, normal);

	float specularFactor = clamp(dot(vertexToCamera, reflectedLightDir), 0, 1);

	float Ambient = MaterialProperties.x; //ambient intensity
	float Diffuse = MaterialProperties.y * diffuseFactor; //diffuse intensity
	float Specular = MaterialProperties.z * pow(specularFactor, MaterialProperties.w); //specular intensity, specular shininess

	float totalLight = (Ambient + Diffuse + Specular);

	color = vec4(MaterialDiffuseColor * lightColor * lightPower * totalLight, 0.1f);
}
