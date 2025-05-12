import { useEffect, useState } from 'react'
import React from "react";

type Project = {
	name: string
	description: string
	link: string
	video: string
	id: string
}

type WorkExperience = {
	company: string
	title: string
	start: string
	end: string
	link: string
	id: string
}

type BlogPost = {
	title: string
	description: string
	link: string
	uid: string
}

type SocialLink = {
	label: string
	link: string
}

interface LoopingVideoProps {
  src: string;
  width?: string;
  height?: string;
  className?: string;
}


export const PROJECTS: Project[] = [
	{
		name: 'Yurrgoht Game Engine',
		description:
			'A Vulkan game engine developed using the Entity Component structure. Includes Scripting, GUI Editing, and reflection-based serialization.',
		link: 'https://github.com/michaeljhkim/YURRGOHT_ENGINE',
		video: '/videos/yurrgoht_engine_demo.mp4',
		id: 'project1',
	},
	{
		name: 'HelixWatt (video is placeholder)',
		description: '',
		link: 'https://github.com/michaeljhkim/HelixWatt',
		video:
			'/videos/yurrgoht_engine_demo.mp4',
		id: 'project2',
	},
]

export const WORK_EXPERIENCE: WorkExperience[] = [
	{
		company: 'Reglazed Studio',
		title: 'CEO',
		start: '2024',
		end: 'Present',
		link: 'https://ibelick.com',
		id: 'work1',
	},
	{
		company: 'Freelance',
		title: 'Design Engineer',
		start: '2022',
		end: '2024',
		link: 'https://ibelick.com',
		id: 'work2',
	},
	{
		company: 'Freelance',
		title: 'Front-end Developer',
		start: '2017',
		end: 'Present',
		link: 'https://ibelick.com',
		id: 'work3',
	},
]

export const BLOG_POSTS: BlogPost[] = [
	{
		title: 'Exploring the Intersection of Design, AI, and Design Engineering',
		description: 'How AI is changing the way we design',
		link: '/blog/exploring-the-intersection-of-design-ai-and-design-engineering',
		uid: 'blog-1',
	},
	{
		title: 'Why I left my job to start my own company',
		description:
			'A deep dive into my decision to leave my job and start my own company',
		link: '/blog/exploring-the-intersection-of-design-ai-and-design-engineering',
		uid: 'blog-2',
	},
	{
		title: 'What I learned from my first year of freelancing',
		description:
			'A look back at my first year of freelancing and what I learned',
		link: '/blog/exploring-the-intersection-of-design-ai-and-design-engineering',
		uid: 'blog-3',
	},
]

// This functions retreievs the data from a specified endpoint
export function getData(endpoint_name: string) {
	const [data, setData] = useState<string>("Loading...");
	fetch("/api/" + endpoint_name)
		.then((res) => res.text())
		.then((text) => {
			console.log("Data from backend (postgresql):", text);
			setData(text);
		})
		.catch((err) => console.error("Error fetching data:", err));
	return data;
}

export const SOCIAL_LINKS: string[] = ['Github', 'LinkedIn']
