import type { NextConfig } from "next";

const backendUrl =
	'https://michaelkim-hello-backend-apiservice-f8dpawfbgsfhh7db.canadacentral-01.azurewebsites.net';
	

if (!backendUrl) {
	console.warn("WARNING: MKapiservice backend URL not found in environment variables.");
}


const nextConfig: NextConfig = {
	webpack: (config, { isServer }) => config,
	output: "export",

	/*
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
	*/

	env: {
		HELLO_API: backendUrl ?? "", // Provide fallback to avoid "undefined"
	},
};
export default nextConfig;
