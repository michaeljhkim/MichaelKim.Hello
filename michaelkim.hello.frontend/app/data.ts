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

type ProjectInfo = {
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
		description: 'A Vulkan game engine developed using the Entity Component structure. Includes Scripting, GUI Editing, and reflection-based serialization.',
		link:  '#project-links',
		media: '/videos/yurrgoht_engine_demo.mp4',
		id: 'project1',
	},
	{
		name: 'PT-ImGen',
		description: 'Accurate path-tracing algorithm, generating standard resolution images.',
		link:  '#project-links',
		media: '/images/ray_trace_demo.png',
		id: 'project2',
	}
]

export const WORK_EXPERIENCE: WorkExperience[] = [
	{
		company: 'Bevy Foundation',
		title: 'Quality Assurance (Volunteer)',
		start: '2024',
		end: 'Present',
		link: 'https://bevyengine.org/foundation/',
		id: 'work1',
	}
]

export const PROJECT_INFO: ProjectInfo[] = [
	{
		title: 'MichaelKim.Hello Github',
		description: 'Source code for this Web Application',
		link: 'https://github.com/michaeljhkim/MichaelKim.Hello',
		uid: 'blog-1'
	}
]

type PinnedRepo = {
	name: string;
	description: string;
	link: string;
	uuid: string;
};

// This function retrieves the github pinned repos, scrapped from personal github page
export function getPinnedRepos(endpointName: string) {
	const [data, setData] = useState<PinnedRepo[]>([]);
	const [loading, setLoading] = useState(true);
	const [error, setError] = useState<Error | null>(null);

  	useEffect(() => {
		fetch(`${process.env.HELLO_API}/${endpointName}`)
			.then((res) => {
				if (!res.ok) throw new Error("Network response was not ok");
				return res.json();
			})
			.then((json: PinnedRepo[]) => {
				console.log("Data from backend (PostgreSQL):", json);
				setData(json);
			})
			.catch((err) => {
				console.error("Error fetching data:", err);
				setError(err);
			})
			.finally(() => setLoading(false));
	}, []);

	return { data, loading, error };
}

// This functions retrieves the data from a specified endpoint
export function getData(endpoint_name: string) {
	const [data, setData] = useState<string>("Loading...");
	
  	useEffect(() => {
		fetch(`${process.env.HELLO_API}/${endpoint_name}`)
			.then((res) => res.text())
			.then((text) => {
				console.log("Data from backend (postgresql):", text);
				setData(text);
			})
			.catch((err) => console.error("Error fetching data:", err));
	}, []);

	return data;
}

export const SOCIAL_LINKS: string[] = ['Github', 'LinkedIn']
