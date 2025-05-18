'use client'
import { useEffect, useState } from 'react'
import { TextEffect } from '@/components/ui/text-effect'
import Link from 'next/link'
import { getData } from './data'

export function Header() {
	return (
		<header className="mb-8 flex items-center justify-between">
			<div>
				<Link href="/" className="font-medium text-black dark:text-white text-3xl">
					{getData("first_name") + " " + getData("last_name")}
				</Link>
				<TextEffect as="p" preset="fade" per="char" className="text-zinc-600 dark:text-zinc-500" delay={0.5}>
					3rd Year Computer Science
				</TextEffect>
			</div>
		</header>
	)
}
