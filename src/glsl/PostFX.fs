varying vec2 vUv;
uniform sampler2D tInput;
uniform float noiseAmount;
uniform float noiseSpeed;
uniform float vignetteAmount;
uniform float vignetteFallOff;
uniform float invertRatio;
uniform float bwRatio;
uniform float mirrorX;
uniform float mirrorY;
uniform float time;
uniform float boost;
uniform float boostReduction;
uniform float multiplyHorizontal;
uniform float multiplyVertical;
uniform vec2 resolution;

float random(vec2 n, float offset ){ return .5 - fract(sin(dot(n.xy + vec2( offset, 0. ), vec2(12.9898, 78.233)))* 43758.5453);}

void main() {
	vec2 uv = vUv;

	//mirror
	if(mirrorX>0.){ uv.x = .5-abs(vUv.x-.5); }
	if(mirrorY>0.){ uv.y = .5-abs(vUv.y-.5); }

	// Multiply
	uv.x *= multiplyHorizontal;
	uv.y *= multiplyVertical;

	uv = mod(uv,1.);

	vec4 color = texture2D( tInput, uv );

	// greyscale
	vec3 luma = vec3( .299, 0.587, 0.114 );
	color = mix(color,vec4( vec3( dot( color.rgb, luma ) ), color.a ), bwRatio);

	//noise
	color += vec4( vec3( noiseAmount * random( uv, .00001 * noiseSpeed * time ) ), 1. );

	//Vignette
	float dist = distance(vUv, vec2(0.5, 0.5));
	color.rgb *= smoothstep(0.8, vignetteFallOff * 0.799, dist * (vignetteAmount + vignetteFallOff));

	// BOOST
	color.rgb += max(0.,boost - dist * boostReduction)*.3;



	//invert
	color.rgb = mix(color.rgb, (1. - color.rgb),invertRatio);
	gl_FragColor = color;
}
