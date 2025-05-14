import { useEffect, useState } from 'react'
import React from "react";

type Project = {
	name: string
	description: string
	link: string
	media: string
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

// Media can be an image or video
export const PROJECTS: Project[] = [
	{
		name: 'Yurrgoht Game Engine',
		description:
			'A Vulkan game engine developed using the Entity Component structure. Includes Scripting, GUI Editing, and reflection-based serialization.',
		link: 'https://github.com/michaeljhkim/YURRGOHT_ENGINE',
		media: '/videos/yurrgoht_engine_demo.mp4',
		id: 'project1',
	},
	{
		name: 'HelixWatt (video is placeholder)',
		description: 'Database Application that process energy consumption data to predict future energy consumption.',
		link: 'https://github.com/michaeljhkim/HelixWatt',
		media:
			'/videos/yurrgoht_engine_demo.mp4',
		id: 'project2',
	},
	{
		name: 'PT-Gen',
		description: 'Accurate path-tracing algorithm, generating standard resolution images',
		link: 'https://github.com/michaeljhkim/PT-ImGen',
		media:
			'/images/ray_trace_demo.png',
		id: 'project3',
	}
]

export const WORK_EXPERIENCE: WorkExperience[] = [
	{
		company: 'Bevy Foundation',
		title: 'Volunteer Contributer',
		start: '2024',
		end: 'Present',
		link: 'https://bevyengine.org/foundation/',
		id: 'work1',
	}
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
	console.log(`${process.env.HELLO_API}/${endpoint_name}`);
	
	fetch(`${process.env.HELLO_API}/${endpoint_name}`)
		.then((res) => res.text())
		.then((text) => {
			console.log("Data from backend (postgresql):", text);
			setData(text);
		})
		.catch((err) => console.error("Error fetching data:", err));
	return data;
}

export const SOCIAL_LINKS: string[] = ['Github', 'LinkedIn']
