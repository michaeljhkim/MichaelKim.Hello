// app/page.tsx
'use client';  // Make sure to add this line to indicate this is a client-side component

import Image from "next/image";
import { useEffect, useState } from 'react';

export default function Home() {
	useEffect(() => {
		const apiUrl = process.env.HELLO_API; // Default to local API if not set

		// Make sure to use the correct URL for your backend
		/*
		fetch(`${apiUrl}/helloworld`)
			.then((res) => res.text())
			.then((data) => console.log("Data from backend:", data))
			.catch((err) => console.error("Error fetching data:", err));
		*/
		fetch("/api/helloworld")
			.then((res) => res.text())
			.then((data) => console.log("Data from backend:", data))
			.catch((err) => console.error("Error fetching data:", err));
	}, []);

	console.log("API URL from env:", process.env.HELLO_API);

	return <h1>Hello from React!</h1>;
}
