#version 420

uniform sampler2D diffuseSampler;
uniform sampler2D positionSampler;
uniform sampler2D normalsSampler;
uniform sampler2D metDiffIntShinSpecIntSampler;

layout(std140, binding = 1) uniform LBVars
{
	vec3 lightInvDir;
	float shadowTransitionSize;
	float outerCutOff;
	float innerCutOff;
	float lightRadius;
	float lightPower;
	vec3 lightColor;
	float ambient;
	mat4 depthBiasMVP;
	mat4 MVP;
	vec3 lightPosition;
	float constant;
	float linear;
	float exponential;
};

layout(std140, binding = 2) uniform CBVars
{
	vec2 screenSize;
	float near;
	float far;
	vec3 cameraPos;
};

// Ouput data
layout(location = 0) out vec3 color;

void main()
{
	vec2 TexCoord = gl_FragCoord.xy / screenSize;
	vec3 WorldPos = texture(positionSampler, TexCoord).xyz;
	vec3 MaterialDiffuseColor = texture(diffuseSampler, TexCoord).xyz;
	vec3 Normal_worldSpace = texture(normalsSampler, TexCoord).xyz;
	// Material properties
	vec4 MatPropertiesMetDiffShinSpec = texture(metDiffIntShinSpecIntSampler, TexCoord);

	// Vector that goes from the vertex to the camera, in world space.
	vec3 EyeDirection_worldSpace = cameraPos - WorldPos;

	// Normal of the computed fragment, in camera space
	vec3 n = normalize(Normal_worldSpace);
	// Direction of the light (from the vertex to the light)
	vec3 l = normalize(lightInvDir);
	// Cosine of the angle between the normal and the light direction, 
	// clamped above 0
	//  - light is at the vertical of the triangle -> 1
	//  - light is perpendicular to the triangle -> 0
	//  - light is behind the triangle -> 0
	float cosTheta = clamp(dot(n, l), 0, 1);

	// Eye vector (towards the camera)
	vec3 E = normalize(EyeDirection_worldSpace);
	// Direction in which the triangle reflects the light
	vec3 R = reflect(-l, n);
	// Cosine of the angle between the Eye vector and the Reflect vector,
	// clamped to 0
	//  - Looking into the reflection -> 1
	//  - Looking elsewhere -> < 1
	float cosAlpha = clamp(dot(E, R), 0, 1);

	//float visibility = ShadowContribution(ShadowCoord.xy, ShadowCoord.z);
	//float depth = texture( shadowMapSampler, ShadowCoord.xy).r;
	//float bias = 0.0005f;

	//float visibility = 1.0;

	//if ( depth  <  ShadowCoord.z - bias){
	//	visibility = 0.5f;
	//}
	//vec3 Ambient = lightColor * lightPower * MaterialAmbientIntensity;
	//vec3 Diffuse = lightColor * lightPower * MaterialDiffuseIntensityValue * cosTheta;
	//vec3 SpecularColor = MaterialSpecularColor * lightColor * lightPower * pow(cosAlpha, shininess);
	//specular sent to shader should be only the specular strength but we can try to specify color as well to fake translucent paint effect
	//these are only light calculations
	//we add the texture color later

	float Metallic = MatPropertiesMetDiffShinSpec.x;
	float Diffuse = MatPropertiesMetDiffShinSpec.y * cosTheta;
	float Specular = MatPropertiesMetDiffShinSpec.z * pow(cosAlpha, MatPropertiesMetDiffShinSpec.w);
	vec3 SpecularColor = mix(vec3(1.0), MaterialDiffuseColor, Metallic); //roughness parameter and reflection map will help with black metallic objects 

	//directional only
	color = lightColor * lightPower * (MaterialDiffuseColor * (ambient + Diffuse) + SpecularColor * Specular);

		// Ambient : simulates indirect lighting
		//MaterialAmbientColor * MaterialDiffuseIntensityValue +
		// Diffuse : "color" of the object
		//MaterialDiffuseColor * lightColor * lightPower * cosTheta * MaterialDiffuseIntensityValue +
		// Specular : reflective highlight, like a mirror
		//MaterialSpecularColor * lightColor * lightPower * pow(cosAlpha, shininess);
}