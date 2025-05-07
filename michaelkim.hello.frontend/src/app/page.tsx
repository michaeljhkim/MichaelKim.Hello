// app/page.tsx
'use client';  // Make sure to add this line to indicate this is a client-side component

import Image from "next/image";
import { useEffect, useState } from 'react';

export default function Home() {
	useEffect(() => {
		fetch("/api/helloworld")
			.then((res) => res.text())
			.then((data) => console.log("Data from backend:", data))
			.catch((err) => console.error("Error fetching data:", err));
	}, []);
	
	/*
	useEffect(() => {
		fetch("/api/hello_information2")
			.then((res) => res.text())
			.then((data) => console.log("Data from backend:", data))
			.catch((err) => console.error("Error fetching data:", err));
	}, []);
	*/
	
	console.log("API URL from env:", process.env.HELLO_API);

	return <h1>Hello from React!</h1>;
}
