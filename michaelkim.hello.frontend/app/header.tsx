'use client'
import { useEffect, useState } from 'react'
import { TextEffect } from '@/components/ui/text-effect'
import Link from 'next/link'

export function Header() {
	const [firstName, setFirstName] = useState<string>("Loading...");
	const [lastName, setLastName] = useState<string>("Loading...");
	fetch("/api/first_name")
		.then((res) => res.text())
		.then((data) => {
			console.log("Data from backend (postgresql):", data);
			setFirstName(data);
		})
		.catch((err) => console.error("Error fetching data:", err));

	fetch("/api/last_name")
		.then((res) => res.text())
		.then((data) => {
			console.log("Data from backend (postgresql):", data);
			setLastName(data);
		})
		.catch((err) => console.error("Error fetching data:", err));



	return (
		<header className="mb-8 flex items-center justify-between">
			<div>
				<Link href="/" className="font-medium text-black dark:text-white">
					{firstName + " " + lastName}
				</Link>
				<TextEffect
					as="p"
					preset="fade"
					per="char"
					className="text-zinc-600 dark:text-zinc-500"
					delay={0.5}
				>
					3rd Year Computer Science
				</TextEffect>
			</div>
		</header>
	)
}
