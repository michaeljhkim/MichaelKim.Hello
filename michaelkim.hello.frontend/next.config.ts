import type { NextConfig } from "next";

const backendUrl =
	process.env.services__MKapiservice__http__0  || // would have to define this in some .env file or something
	process.env.services__MKapiservice__https__0 ||
	'http://localhost:5431' || 						// htpp is preferred over https (security reasons)
	'https://localhost:7448';
	

if (!backendUrl) {
	console.warn("WARNING: MKapiservice backend URL not found in environment variables.");
}


const nextConfig: NextConfig = {
	webpack: (config, { isServer }) => config,

	async rewrites() {
		if (!backendUrl) {
			throw new Error("Cannot create rewrite rule: MKapiservice URL is undefined.");
		}
		return [
			{
				source: '/api/:path*',
				destination: `${backendUrl}/:path*`,
			},
		];
	},

	env: {
		HELLO_API: backendUrl ?? "", // Provide fallback to avoid "undefined"
	},
};
export default nextConfig;
